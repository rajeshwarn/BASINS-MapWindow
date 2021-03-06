/**************************************************************************************
 * File name: ShapefileDrawing.cpp
 *
 * Project: MapWindow Open Source (MapWinGis ActiveX control) 
 * Description: Implementation of CShapefileDrawer
 *
 **************************************************************************************
 * The contents of this file are subject to the Mozilla Public License Version 1.1
 * (the "License"); you may not use this file except in compliance with 
 * the License. You may obtain a copy of the License at http://www.mozilla.org/mpl/ 
 * See the License for the specific language governing rights and limitations
 * under the License.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 ************************************************************************************** 
 * Contributor(s): 
 * (Open source contributors should list themselves and their modifications here). */
 // Sergei Leschinski (lsu) 25 june 2010 - created the file

#include "stdafx.h"
#include "ShapefileDrawing.h"
#include "LineDrawing.h"

#include "LinePattern.h"
#include "TableClass.h"
#include "ShapefileReader.h"
#include "Shape.h"
#include "GeometryOperations.h"
#include "macros.h"
#include "PointSymbols.h"

// MEMO: there are several formats to hold shape data while drawing
// there are to switches: regular/edit mode; and fast/slow mode
// regular-disk: PolygonData  (pointers to the positions of the memory from fread)
// regular-fast: IShapeData   (CShapeData class)
// edit-COM: IShapeData		  (CShapeWrapperCOM class)
// edit-fast: IShapeData	  (CShapeWrapper class)

// Drawing details:
// complex polylines will be drawn through GDIPlus only as 

using namespace Gdiplus;

enum tkDrawingShape
{
	pshPixel = 0,
	pshEllipse = 1,
	pshPolygon = 2,
	pshPicture = 3,
	pshCharacter = 4,
};

#pragma region MainDrawing
//*******************************************************************
//	Draw()				          
//*******************************************************************
void CShapefileDrawer::Draw(const CRect & rcBounds, IShapefile* sf, FILE* file)
{
	#ifdef USE_TIMER
		CTimer tmr;
		tmr.Init("c:\\mw_output.txt");
		tmr.Start();
	#endif

	if( sf == NULL )
	{	
		// TODO: return this error in some way
		// ErrorMessage(sf, tkINTERFACE_NOT_SUPPORTED);
		return;
	}
	_shapefile = reinterpret_cast<CShapefile*>(sf);
	
	#ifdef USE_TIMER
	tmr.PrintTime("Before bounds");
	#endif
	// -------------------------------------------------------
	//	 check bounds
	// -------------------------------------------------------
	double zMin, zMax;
	IExtents * box=NULL;
	sf->get_Extents(&box);
	box->GetBounds(&_xMin,&_yMin,&zMin,&_xMax,&_yMax,&zMax);
	box->Release();
	box=NULL;

	if ( _xMin>_extents->right || _xMax<_extents->left || _yMin>_extents->top || _yMax<_extents->bottom )
		return;

	#ifdef USE_TIMER
	tmr.PrintTime("After bounds");
	#endif

	// --------------------------------------------------------
	//	 reading shapefile properties
	// --------------------------------------------------------
	long _numShapes;
	
	VARIANT_BOOL _useQTree;
	VARIANT_BOOL _useSpatialIndex;
	VARIANT_BOOL _hasSpatilaIndex;
	tkSelectionAppearance selectionAppearance;
	
	_shapefile->get_SelectionAppearance(&selectionAppearance);
	_shapefile->get_FastMode(&_fastMode);
	_shapefile->get_ShapefileType(&_shptype);
	_shapefile->get_NumShapes(&_numShapes);
	_shapefile->get_EditingShapes(&_isEditing);
	_shapefile->get_UseQTree(&_useQTree);
	_shapefile->get_UseSpatialIndex(&_useSpatialIndex);
	_shapefile->get_HasSpatialIndex(&_hasSpatilaIndex);
	_useSpatialIndex = (_useSpatialIndex && _hasSpatilaIndex);
		
	// get 2D type for not checking it afterwards
	if ( _shptype == SHP_POINTM || _shptype == SHP_POINTZ )	_shptype = SHP_POINT;
	else if ( _shptype == SHP_POLYLINEM || _shptype == SHP_POLYLINEZ )	_shptype = SHP_POLYLINE;
	else if ( _shptype == SHP_POLYGONM || _shptype == SHP_POLYGONZ )	_shptype = SHP_POLYGON;
	else if ( _shptype == SHP_MULTIPOINTM || _shptype == SHP_MULTIPOINTZ )	_shptype = SHP_MULTIPOINT;
	
	// clearing the pathes	
	_vertexPathes.clear();

	#ifdef USE_TIMER
	tmr.PrintTime("Before reading drawing options");
	#endif
	// --------------------------------------------------------
	//	 acquiring drawing options
	// --------------------------------------------------------
	// default options
	IShapeDrawingOptions* iDefOpt = NULL;
	_shapefile->get_DefaultDrawingOptions(&iDefOpt);
	CDrawingOptionsEx* defaultOptions = ((CShapeDrawingOptions*)iDefOpt)->get_UnderlyingOptions();
	iDefOpt->Release(); iDefOpt= NULL;
	
	// selection options
	CDrawingOptionsEx* selectionOptions = NULL;
	if (selectionAppearance == saDrawingOptions)
	{
		IShapeDrawingOptions* iSelOpt = NULL;
		_shapefile->get_SelectionDrawingOptions(&iSelOpt);
		selectionOptions = ((CShapeDrawingOptions*)iSelOpt)->get_UnderlyingOptions();
		iSelOpt->Release(); iSelOpt = NULL;
	}
	else
	{
		// a default options will be used with drawing transparent selection on top of it
		OLE_COLOR color;
		unsigned char transp;
		selectionOptions = defaultOptions;
		_shapefile->get_SelectionColor(&color);
		_shapefile->get_SelectionTransparency(&transp);
		m_selectionColor = color;
		m_selectionTransparency = transp;
	}
	
	// categories
	IShapefileCategories* icategories = NULL;
	_shapefile->get_Categories(&icategories);
	CShapefileCategories* categories = (CShapefileCategories*)icategories;
	icategories->Release();
	ASSERT(categories != NULL);
	
	#ifdef USE_TIMER
		tmr.PrintTime("Before reading drawing options");
	#endif

	// --------------------------------------------------------
	//	 Settings dc/graphics options
	// --------------------------------------------------------
	ASSERT(_sfReader != NULL);

	int* qtreeResult;					// results of quad tree selection
	vector<long>* selectResult = NULL;	// results of spatial index selection
	int offset;							// position (number) of a shape in the shapefile		

	// --------------------------------------------------------
	//		Reading from disk
	// --------------------------------------------------------
	if( !_isEditing)
	{		
		if (file == NULL )
		{
			perror("Shapefile doesn't exist");
			return;
		}
		else
		{
			// retrieving the name
			BSTR fname;
			sf->get_Filename(&fname);
			int b_strlen = wcslen(fname);
			char * sFilename = new char[(b_strlen+1)<<1];
			int sFilenamelast = WideCharToMultiByte(CP_ACP,0,fname,b_strlen,sFilename,b_strlen<<1,0,0);
			::SysFreeString(fname);
			sFilename[sFilenamelast] = 0;
			
			// reading index
			_sfReader = new CShapefileReader();
			if (!_sfReader->ReadShapefileIndex(sFilename, file))
			{
				delete[] sFilename;
				delete _sfReader; _sfReader = NULL;
				return;
				// TODO: Add error handling
			}

			// ---------------------------------------------------------
			//	extracting shapes from spatial index
			// ---------------------------------------------------------
			if (_useSpatialIndex)
			{
				selectResult = SelectShapesFromSpatialIndex(sFilename, _extents);
				if (!selectResult)
				{
					_useSpatialIndex = VARIANT_FALSE;
				}
				else
				{
					_numShapes = selectResult->size();
					sort(selectResult->begin(), selectResult->end());
				}
			}
			delete[] sFilename;
		}
	}
	#ifdef USE_TIMER
		tmr.PrintTime("After reading shapeindex");	
	#endif

	long numCategories;
	categories->get_Count(&numCategories);
	std::vector<vector<int>> categoryIndices;
	std::vector<vector<int>> categorySelIndices;  // used for selectionAppearance == saSelectionColor only
	categoryIndices.resize(numCategories + 2);	// +1 = default options; +2 = selection options
	categorySelIndices.resize(numCategories + 1); // +1 = default options; 
	
	// --------------------------------------------------------------
	//	 Analyzing visibility expression
	// --------------------------------------------------------------
	std::vector<long> arr;
	CString err;
	bool useAll = true;
	
	BSTR expr;
	_shapefile->get_VisibilityExpression(&expr);

	if (SysStringLen(expr) > 0)
	{
		ITable* tbl = NULL;
		_shapefile->get_Table(&tbl);
		
		USES_CONVERSION;
		if (((CTableClass*)tbl)->Query_(OLE2CA(expr), arr, err))
		{
			useAll = false;
		}
	}

	// --------------------------------------------------------------
	//		Extracting shapes using quad tree							
	// --------------------------------------------------------------
	if(_useQTree)
	{
		int shapesCount;
		IExtents * bBox = NULL;

		CoCreateInstance( CLSID_Extents, NULL, CLSCTX_INPROC_SERVER, IID_IExtents, (void**)&bBox );
		bBox->SetBounds(_extents->left,_extents->bottom,0,_extents->right, _extents->top,0);
		((CShapefile*)sf)->QuickQueryInEditMode(bBox,&qtreeResult,&shapesCount);
		if (qtreeResult == NULL) 
		{
			bBox->Release(); bBox=NULL;
			goto cleaning; 
		}

		_numShapes = (long)shapesCount;
	}
	
	// --------------------------------------------------------------------
	//  nullify the screen size of all shapes
	//  the size is used to choose whether to draw labels and charts or not
	// --------------------------------------------------------------------
	_shapeData = ((CShapefile*)_shapefile)->get_ShapeVector();
	if (_shptype == SHP_POLYGON || _shptype == SHP_POLYLINE)
	{
		int size = _shapeData->size();
		for (int i = 0; i < size; i++)
		{
			(*_shapeData)[i]->size = 0;
			(*_shapeData)[i]->isVisible = false;
		}
	}
	
	// --------------------------------------------------------------
	//	 Building lists of shape indices for each category
	// --------------------------------------------------------------
	unsigned int k = 0; // position in arr of visible shapes
	for(int i = 0; i < (int)_numShapes; i++ )
	{
		if(_useQTree)
		{
			offset = qtreeResult[i];
		}
		else
		{
			if (!_isEditing && _useSpatialIndex)	
			{
				offset = (*selectResult)[i] - 1;	
			}
			else
			{
				offset = i;
			}
		}

		// does it comply with visiblity expression
		if (!useAll)
		{
			// searching the index
			bool stop = false;
			while(arr[k] < offset)
			{
				k++;
				if (k >= arr.size())
				{
					stop = true;
					break;
				}
			}
			
			// there can't be any visible shapes
			if (stop)
			{
				break;
			}

			// missing this shape, because it complies with the expression
			if (arr[k] > offset)
			{
				continue;
			}
		}
		
		// marking shape as visible; it may still fall out of extents but it is inefficient to test it here
		(*_shapeData)[offset]->isVisible = true;
		
		
		bool selected = (*_shapeData)[offset]->selected;
		if (selected)
		{
			if (selectionAppearance == saDrawingOptions)
			{
				categoryIndices[numCategories + 1].push_back(offset);	// selection options
			}
			else
			{
				long catIndex = (*_shapeData)[offset]->category;
				if (catIndex < 0 || catIndex >= numCategories)
				{
					categorySelIndices[numCategories].push_back(offset);	// default options
				}
				else
				{
					categorySelIndices[catIndex].push_back(offset);		// category
				}
			}
		}
		else
		{
			long catIndex = (*_shapeData)[offset]->category;
			
			if (catIndex < 0 || catIndex >= numCategories)
			{
				categoryIndices[numCategories].push_back(offset);	// default options
			}
			else
			{
				categoryIndices[catIndex].push_back(offset);		// category
				// TODO: check whether the drawing options are different from default
			}
		}
	}

	// -----------------------------------------------------
	//	Drawing
	// -----------------------------------------------------
	CDrawingOptionsEx* options = NULL;
	
	#ifdef USE_TIMER
		tmr.PrintTime("Before reading data");
	#endif
	
	// in some cases selected objects should be drawn first
	bool selectionFirst = false;
	if (_shptype == SHP_POINT || _shptype == SHP_MULTIPOINT)
	{
		tkCollisionMode mode;
		sf->get_CollisionMode(&mode);
		if (mode != AllowCollisions)
		{
			selectionFirst = true;
		}
	}
	
	// drawing selection at the bottom
	if (selectionFirst)
	{
		if (selectionAppearance == saSelectionColor)
		{
			for(int i = categorySelIndices.size() - 1; i >= 0 ; i--)
			{
				if (i == numCategories)				
				{
					options = defaultOptions;
				}
				else if (i < numCategories)
				{
					options = ((CShapefileCategories*)categories)->get_UnderlyingOptions(i);
				}
				
				std::vector<int>* indices = &categorySelIndices[i];
				this->DrawCategory(options, indices, true);
			}
		}
		else
		{
			options = selectionOptions;
			std::vector<int>* indices = &categoryIndices[numCategories + 1];	
			this->DrawCategory(options, indices, false);	// false: options set in the selection itself so no need to draw it on top
		}
	}
	
	// drawing unselected shapes
	for(int i = categoryIndices.size() - 2; i >= 0 ; i--)
	{
		if (i == numCategories)				
		{
			options = defaultOptions;
		}
		else if (i < numCategories)
		{
			options = ((CShapefileCategories*)categories)->get_UnderlyingOptions(i);
		}
		else
		{
			ASSERT(true);
		}
		
		std::vector<int>* indices = &categoryIndices[i];
		this->DrawCategory(options, indices, false);
	}

	// drawing selection at the bottom
	if (!selectionFirst)
	{
		if (selectionAppearance == saSelectionColor)
		{
			for(int i = categorySelIndices.size() - 1; i >= 0 ; i--)
			{
				if (i == numCategories)				
				{
					options = defaultOptions;
				}
				else if (i < numCategories)
				{
					options = ((CShapefileCategories*)categories)->get_UnderlyingOptions(i);
				}
				
				std::vector<int>* indices = &categorySelIndices[i];
				this->DrawCategory(options, indices, true);
			}
		}
		else
		{
			options = selectionOptions;
			std::vector<int>* indices = &categoryIndices[numCategories + 1];	
			this->DrawCategory(options, indices, false);	// false: options set in the selection itself so no need to draw it on top
		}
	}

	// drawing the vertices
	for (unsigned int i = 0; i <_vertexPathes.size(); i++)
	{
		DrawVertices(_vertexPathes[i].path, _vertexPathes[i].options);
	}
	
	// clearing pathes
	for (unsigned int i = 0; i <_vertexPathes.size(); i++)
	{
		delete _vertexPathes[i].path;
	}

	#ifdef USE_TIMER
		tmr.PrintTime("After drawing");	
		tmr.Stop();
	#endif
	
// ------------------------------------------
//  final cleaning
// ------------------------------------------
cleaning:
	if (!_isEditing)
	{
		delete _sfReader;
		_sfReader = NULL;
	}
	
	if(_useQTree) 
	{
		delete[] qtreeResult;
	}
	else
	{
		if (_useSpatialIndex && selectResult)
		{
			selectResult->clear();
			delete selectResult;
			selectResult = NULL;
		}
	}
}

// ********************************************************
//		DrawCategory()
// ********************************************************
void CShapefileDrawer::DrawCategory(CDrawingOptionsEx* options, std::vector<int>* indices, bool drawSelection)
{
	if (indices->size() == 0)
		return;
	
	if ((!options->visible) || (!options->fillVisible && !options->linesVisible))
	{
		return;
	}
	
	// ----------------------------------------------------
	// auto selecting the fastest drawing mode 
	// for the current set of options
	// ----------------------------------------------------
	if (options->lineWidth == 1.0f && options->lineTransparency == 255 && options->linesVisible)
	{
		options->drawingMode = vdmGDIMixed;
	}
	else
	{
		options->drawingMode = vdmGDIPlus;
	}

	if (options->pointSymbolType == ptSymbolFontCharacter || options->pointSymbolType == ptSymbolPicture)
	{
		options->drawingMode = vdmGDIPlus;
	}
	
	// circles look more neat in GDI+
	if (options->pointSymbolType == ptSymbolStandard && options->pointShapeType == ptShapeCircle)
	{
		options->drawingMode = vdmGDIPlus;
	}

	if (_forceGdiplus)
	{
		options->drawingMode = vdmGDIPlus;
	}
	
	if (_dc)
	{
		_dc->SetBkColor(options->fillBgColor);
		if ( options->fillBgTransparent )
		{
			_dc->SetBkMode(TRANSPARENT);
		}
		else
		{
			_dc->SetBkMode(OPAQUE);
		}
	}
	
	// ------------------------------------------------
	// perform drawing
	// ------------------------------------------------
	if ( _shptype == SHP_POINT )
	{
		this->DrawPointCategory(options, indices, drawSelection);
	}
	else if ( _shptype == SHP_POLYLINE || _shptype == SHP_POLYGON )
	{
		if (_shptype == SHP_POLYLINE && options->useLinePattern && options->CanUseLinePattern())
		{
			this->DrawLinePatternCategory(options, indices, drawSelection);
		}
		else
		{
			if ( options->drawingMode == vdmGDIMixed && _shptype == SHP_POLYLINE )
			{
				this->DrawLineCategoryGDI( options, indices, drawSelection );	// only lines are drawn here
			}
			else
			{
				this->DrawPolyCategory( options, indices, drawSelection );
			}
		}
	}
}
#pragma endregion

#pragma region DrawPointCategory
// *************************************************************
//		DrawPointsCategory()
// *************************************************************
void CShapefileDrawer::DrawPointCategory( CDrawingOptionsEx* options, std::vector<int>* indices, bool drawSelection)
{
	ASSERT(_dc == NULL);
	
	IShapeWrapper* shp = NULL;
	tkDrawingShape pntShape;
	
	GraphicsPath* path = NULL;
	float* data = NULL;
	Bitmap* bmPixel = NULL;
	int numPoints = 0;
	int size = int(options->pointSize/2.0);
	OLE_COLOR pixelColor;
	
	// creating a symbol to draw
	if (options->pointSymbolType == ptSymbolStandard || (options->pointSymbolType == ptSymbolPicture && options->picture == NULL))
	{
		// receiving coodinates to define shape of symbol
		if ( options->pointSize <= 1.0 )
		{
			pntShape = pshPixel;
			if ( options->linesVisible )
				pixelColor = options->lineColor;
			else
				pixelColor = options->fillColor;
			
			if (options->drawingMode == vdmGDIMixed || options->drawingMode == vdmGDI)
			{
				m_hdc = _graphics->GetHDC();
				_dc = CDC::FromHandle(m_hdc);
			}
		}
		else if ( options->pointShapeType == ptShapeCircle )
		{
			pntShape = pshEllipse;
		}
		else
		{
			options->drawingMode = vdmGDIPlus;	// GDI drawing will lead to rounding coordinates to intergers, and distortions as a result
			pntShape = pshPolygon;
			data = get_SimplePointShape(options->pointShapeType, options->pointSize, options->rotation, options->pointNumSides, options->pointShapeRatio, &numPoints);
			if (!data) 
				return;
		}
		
		// GDI+ is used at least partially
		if (options->drawingMode == vdmGDIPlus || options->drawingMode == vdmGDIMixed)
		{
			if ( pntShape == pshEllipse )
			{
				options->InitGdiPlusBrush(&RectF(Gdiplus::REAL(-size), Gdiplus::REAL(-size), Gdiplus::REAL(options->pointSize), Gdiplus::REAL(options->pointSize))) ;
				pntShape = pshEllipse;
				path = new GraphicsPath();
				path->StartFigure();
				path->AddEllipse((Gdiplus::REAL)(-options->pointSize/2.0), (Gdiplus::REAL)(-options->pointSize/2.0), options->pointSize, options->pointSize);
				path->CloseFigure();
			}
			else if ( pntShape == pshPolygon )
			{
				options->InitGdiPlusBrush(&RectF(Gdiplus::REAL(-size), Gdiplus::REAL(-size), Gdiplus::REAL(options->pointSize), Gdiplus::REAL(options->pointSize))) ;
				path = new GraphicsPath();
				path->StartFigure();
				path->AddLines(reinterpret_cast<Gdiplus::PointF*>(data), numPoints);
				path->CloseFigure();
			}
			else if ( pntShape == pshPixel && options->drawingMode == vdmGDIPlus)
			{
				bmPixel = new Bitmap( 1,1, _graphics);
				long alpha = ((long)options->fillTransparency)<<24;
				bmPixel->SetPixel( 0, 0, Color(alpha | BGR_TO_RGB(pixelColor)));
			}
		}

		// drawing with GDI
		if (options->drawingMode == vdmGDI || options->drawingMode == vdmGDIMixed)
		{
			m_hdc = _graphics->GetHDC();
			_dc = CDC::FromHandle(m_hdc);
			options->InitGdiBrushAndPen(_dc);
		}
	}
	else if ( options->pointSymbolType == ptSymbolPicture )
	{
		options->InitGdiPlusPicture();
		if (!options->bitmapPlus)
			return;
		pntShape = pshPicture;
	}
	else if ( options->pointSymbolType == ptSymbolFontCharacter )
	{
		options->InitGdiPlusBrush(&RectF((Gdiplus::REAL)-size, (Gdiplus::REAL)-size, (Gdiplus::REAL)options->pointSize, (Gdiplus::REAL)options->pointSize)) ;
		
		m_hdc = _graphics->GetHDC();
		_dc = CDC::FromHandle(m_hdc);
		path = options->get_FontCharacterPath(_dc, false);
		_graphics->ReleaseHDC(m_hdc);
		_dc = NULL;

		Gdiplus::Matrix mtx;
		_graphics->GetTransform(&mtx);
		mtx.Rotate((Gdiplus::REAL)options->rotation);
		path->Transform(&mtx);
		pntShape = pshPolygon;
	}

	VARIANT_BOOL fastMode;
	_shapefile->get_FastMode(&fastMode);

	double x = 0, y = 0;
	VARIANT_BOOL vbretval;
	int shapeIndex;

	tkCollisionMode collisionMode;
	_shapefile->get_CollisionMode(&collisionMode);

	for (int j = 0; j < (int)indices->size(); j++)
	{
		shapeIndex = (*indices)[j];
		
		// ------------------------------------------------------
		//	 Reading point data
		// ------------------------------------------------------
		if (! _isEditing)
		{
			char* data = _sfReader->ReadShapeData(shapeIndex);
			if ( data )
			{
				x = *(double*)(data + 4);	// 4 bytes on shape type
				y = *(double*)(data + 12); 
				delete[] data;
			}
			else
				continue;
		}
		else
		{
			if (fastMode)
			{
				shp = _shapefile->get_ShapeWrapper(shapeIndex);
				if (shp != NULL)
				{
					shp->get_PointXY(0, x, y);
				}
				else 
					continue;
			}
			else
			{
				IShape* shape = NULL;
				_shapefile->get_Shape(shapeIndex, &shape);
				if (shape)
				{
					shape->get_XY(0, &x, &y, &vbretval);
					shape->Release();
				}
				else
					continue;
			}
		}
		
		if (!(x > _extents->right || x < _extents->left || y > _extents->top|| y < _extents->bottom))
		{
			// ------------------------------------------------------
			//	 Collision avoidance
			// ------------------------------------------------------
			int xInt = static_cast<int>((x - _extents->left) * _dx);
			int yInt = static_cast<int>((_extents->top - y) * _dy);
			
			
			// preventing point collision
			if (!collisionMode == AllowCollisions)
			{
				CCollisionList* list = collisionMode == LocalList ? &_localCollisionList : _collisionList;
				
				CRect* rect = NULL;
				
				if (options->pointSymbolType == ptSymbolPicture && options->picture != NULL)	
				{
					long width, height;
					options->picture->get_Width(&width);
					options->picture->get_Height(&height);
					int wd = static_cast<int>((double)width * options->scaleX/2.0);
					int ht = static_cast<int>((double)height * options->scaleY/2.0);

					rect = new CRect(xInt - wd, yInt - ht, xInt + wd, yInt + ht);
				}
				else
				{
					rect = new CRect(xInt - int(options->pointSize/2.0), 
											yInt - int(options->pointSize/2.0),
											xInt + int(options->pointSize/2.0), 
											yInt + int(options->pointSize/2.0));
				}
				
				if (list->HaveCollision(*rect) && _avoidCollisions)
				{
					delete rect; 
					continue;
				}
				else
				{
					list->AddRectangle(rect, 0, 0);
					delete rect;
				}
			}

			// ------------------------------------------------------
			//	 Drawing
			// ------------------------------------------------------
			if ( pntShape == pshPixel )
			{
				if (drawSelection)
				{
					_dc->SetPixelV(xInt, yInt, m_selectionColor);
				}
				else
				{
					if ( options->drawingMode == vdmGDIPlus )
					{
						_graphics->DrawImage(bmPixel, xInt, yInt);
					}
					else
					{
						_dc->SetPixelV(xInt, yInt, pixelColor);
					}
				}
				(*_shapeData)[shapeIndex]->size = 1;
			}
			else if (pntShape == pshPicture)
			{
				Gdiplus::Matrix mtx;
				_graphics->GetTransform(&mtx);
				
				long width, height;
				options->picture->get_Width(&width);
				options->picture->get_Height(&height);
				int wd = static_cast<int>((double)width * options->scaleX/2.0);
				int ht = static_cast<int>((double)height * options->scaleY/2.0);
				
				mtx.Translate((float)(xInt), (float)(yInt));
				mtx.Rotate((float)options->rotation);
				mtx.Translate((float)-wd, (float)-ht);
				_graphics->SetTransform(&mtx);
				
				Gdiplus::Rect rect(0, 0, INT(options->bitmapPlus->GetWidth() * options->scaleX), INT(options->bitmapPlus->GetHeight() * options->scaleY));

				if (!drawSelection || m_selectionTransparency <= 255)
				{
					_graphics->DrawImage(options->bitmapPlus, rect, 0, 0, options->bitmapPlus->GetWidth(), options->bitmapPlus->GetHeight(), Gdiplus::UnitPixel, options->imgAttributes);
				}
				
				if (drawSelection)				
				{
					SolidBrush brush(Gdiplus::Color(m_selectionTransparency << 24 | BGR_TO_RGB(m_selectionColor)));
					_graphics->FillRectangle(&brush, rect);
				}

				mtx.Reset();
				_graphics->SetTransform(&mtx);

				(*_shapeData)[shapeIndex]->size = MAX(wd, ht);
			}
			else
			{
				// GDI+ mode
				if ( options->drawingMode == vdmGDIPlus || options->drawingMode == vdmGDIMixed )
				{
					Gdiplus::Matrix mtx;
					mtx.Translate(Gdiplus::REAL(xInt), Gdiplus::REAL(yInt));
					_graphics->SetTransform(&mtx);
					
					if (!drawSelection || m_selectionTransparency < 255)
					{
						// drawing fill
						if ( options->fillVisible )
						{
							_graphics->FillPath(options->brushPlus, path);
						}
						
						// we'll draw outline but it'll be slow
						if ( options->drawingMode == vdmGDIPlus && options->linesVisible)	
						{
							options->DrawGraphicPath(_graphics, path);
						}
					}

					mtx.Reset();
					_graphics->SetTransform(&mtx);
				}
				
				// GDI mode
				if ( options->drawingMode == vdmGDI || options->drawingMode == vdmGDIMixed )
				{
					if (!drawSelection || m_selectionTransparency < 255)
					{
						if ( pntShape == pshPolygon )
						{
							_dc->SetWindowOrg(-xInt , -yInt);
							_dc->Polygon(reinterpret_cast<LPPOINT>(data), numPoints);
							_dc->SetWindowOrg(0 , 0);
						}
						else if ( pntShape = pshEllipse )
						{
							_dc->SetWindowOrg(-xInt , -yInt);
							_dc->Ellipse( -size, -size, size, size);
							_dc->SetWindowOrg(0 , 0);
						}
					}
				}

				// drawing transparent selection
				if (drawSelection)
				{
					Gdiplus::Matrix mtx;
					mtx.Translate(Gdiplus::REAL(xInt), Gdiplus::REAL(yInt));
					_graphics->SetTransform(&mtx);
					
					SolidBrush brush(Gdiplus::Color(m_selectionTransparency << 24 | BGR_TO_RGB(m_selectionColor)));
					_graphics->FillPath(&brush, path);

					mtx.Reset();
					_graphics->SetTransform(&mtx);
				}

				(*_shapeData)[shapeIndex]->size = (int)options->pointSize;
			}
		}
	}
	
	if (_dc)
	{
		_dc->SetWindowOrg(0, 0);
		options->ReleaseGdiBrushAndPen(_dc);
		_graphics->ReleaseHDC(m_hdc);
		_dc = NULL;
	}
	
	// ----------------------------------------------------
	//	  Cleaning
	// ----------------------------------------------------
	if ( bmPixel ) delete bmPixel;
	if ( path )	delete path;
	if ( data )	delete[] data;
	
	options->ReleaseGdiPlusBrush();
	options->ReleaseGdiPlusBitmap();
}
#pragma endregion

#pragma region DrawPolyCategory
// *************************************************************
//		DrawPolygonCategory()
// *************************************************************
// Polygon or polyline category
void CShapefileDrawer::DrawPolyCategory( CDrawingOptionsEx* options, std::vector<int>* indices, bool drawSelection)
{
	ASSERT(_dc == NULL);
	
	if (_shptype == SHP_POLYLINE && options->linesVisible == false)
		return;
	
	double xMin, xMax, yMin, yMax;
	char* data = NULL;

	// ------------------------------------------------------------------------------------------
	// in GDIMixed, GDI will be used for drawing outlines, but only if width is equal to 1.  
	// ------------------------------------------------------------------------------------------
	tkVectorDrawingMode drawingMode = options->drawingMode;
	if ( options->drawingMode == vdmGDIMixed && (options->lineWidth != 1  || !options->linesVisible) ) 	 	
	{
		drawingMode = vdmGDIPlus;
	}
	
	// ------------------------------------------------------------------------------------------------------
	//  GDI+ mode for per-shape gradient is used, as it's time consuming to switch between GDI/GDI+ for each polygon
	//  Obtain dc -> add shapes to the path -> releases dc -> draw GDI+ fill -> Obtain dc -> Draw GDI+ path
	// ------------------------------------------------------------------------------------------------------
	bool perShapeDrawing = options->fillVisible && options->fillType == ftGradient && 
						   options->fillGradientBounds == gbPerShape && _shptype == SHP_POLYGON;
	
	if (perShapeDrawing)
	{
		drawingMode = vdmGDIPlus;
	}

	GraphicsPath* path = new Gdiplus::GraphicsPath(Gdiplus::FillModeWinding);
	
	LONG minDrawingSize;
	_shapefile->get_MinDrawingSize(&minDrawingSize);
	double delta = MIN(minDrawingSize/_dx, minDrawingSize/_dy);

	OLE_COLOR pointColor = options->linesVisible?options->lineColor:options->fillColor;
	_bmpPixel->SetPixel(0,0, Gdiplus::Color(255 << 24 | BGR_TO_RGB(pointColor)));
	
	VARIANT_BOOL fastMode;
	_shapefile->get_FastMode(&fastMode);
	
	// -----------------------------------------------------
	//  Per-shape gradient (GDI+ only)
	// -----------------------------------------------------
	if ( perShapeDrawing )
	{
		for (int j = 0; j < (int)indices->size(); j++)
		{
			GraphicsPath pathFill(Gdiplus::FillModeWinding);
		
			if (this->DrawPolygonGDIPlus((*indices)[j], pathFill , delta, pointColor, drawingMode, xMin, xMax, yMin, yMax))
			{
				// drawing fill
				int xmin = int((xMin - _extents->left)* _dx);
				int ymin = int((_extents->top - yMin) * _dy);
				int xmax = int((xMax - _extents->left)* _dx);
				int ymax = int((_extents->top - yMax) * _dy);
			
				RectF rect( (Gdiplus::REAL) xmin, (Gdiplus::REAL)ymin, (Gdiplus::REAL)xmax - xmin, (Gdiplus::REAL)ymax - ymin);
				if (!drawSelection || m_selectionTransparency < 255)
				{
					options->FillGraphicsPath(_graphics, &pathFill, rect);
				
					// drawing lines
					if ( options->linesVisible && drawingMode == vdmGDIPlus)
					{
						options->DrawGraphicPath(_graphics, &pathFill);
					}
					
					if (options->verticesVisible)
					{
						path->AddPath(&pathFill, FALSE);
					}
				}

				if (drawSelection && m_selectionTransparency > 0)
				{
					SolidBrush brush(Gdiplus::Color(m_selectionTransparency << 24 | BGR_TO_RGB(m_selectionColor)));
					_graphics->FillPath(&brush, &pathFill);
				}
			}
		}
	}
	
	// ----------------------------------------------------------
	//	  Draw as a single path for all shapes
	// ----------------------------------------------------------
	else
	{
		// -------------------------------------------------------------------------
		// opening GDI mode; no calls to the _graphics until handle is released !!!
		// -------------------------------------------------------------------------
		if ( drawingMode == vdmGDIMixed )  
		{
			m_hdc = _graphics->GetHDC();
			_dc = CDC::FromHandle(m_hdc);
			_dc->BeginPath();				// strange, but it affects drawing in GDIPlus
			options->InitGdiBrushAndPen(_dc);
		}
		
		// constructing a path
		for (int j = 0; j < (int)indices->size(); j++)
		{
			this->DrawPolygonGDIPlus((*indices)[j], *path, delta, pointColor, drawingMode, xMin, xMax, yMin, yMax);
		}
		
		if ( drawingMode == vdmGDIMixed )  
			_graphics->ReleaseHDC(m_hdc);
		
		if (!drawSelection || m_selectionTransparency < 255)
		{
			// drawing fill (calculating measures of gradient: whole layer)
			if ( options->fillVisible && _shptype == SHP_POLYGON )
			{
				int xmin = int((_xMin - _extents->left)* _dx);
				int ymin = int((_extents->top - _yMin) * _dy);
				int xmax = int((_xMax - _extents->left)* _dx);
				int ymax = int((_extents->top - _yMax) * _dy);
				
				RectF rect( (Gdiplus::REAL)xmin, (Gdiplus::REAL)ymin, (Gdiplus::REAL)xmax - xmin, (Gdiplus::REAL)ymax - ymin);
				options->FillGraphicsPath(_graphics, path, rect);
			}
			
			// drawing lines
			if ( options->linesVisible )
			{
				if ( drawingMode == vdmGDIPlus )
				{
					options->DrawGraphicPath(_graphics, path);
				}
				else if ( options->drawingMode == vdmGDIMixed )
				{
					m_hdc = _graphics->GetHDC();
					_dc->EndPath();
					_dc->StrokePath();
					_graphics->ReleaseHDC(m_hdc);
				}
			}
		}
		
		m_hdc = _graphics->GetHDC();
		options->ReleaseGdiBrushAndPen(_dc);
		_graphics->ReleaseHDC(m_hdc);
		_dc = NULL;
		
		// selection drawing: GDI+
		if (drawSelection && m_selectionTransparency > 0)
		{
			if (_shptype == SHP_POLYGON)
			{
				SolidBrush brush(Gdiplus::Color(m_selectionTransparency << 24 | BGR_TO_RGB(m_selectionColor)));
				_graphics->FillPath(&brush, path);
			}
			else
			{
				Pen pen(Gdiplus::Color(m_selectionTransparency << 24 | BGR_TO_RGB(m_selectionColor)));
				pen.SetLineJoin(Gdiplus::LineJoinRound);
				_graphics->DrawPath(&pen, path);
			}
		}
	}
	
	options->ReleaseGdiPlusBrush();
	
	if (options->verticesVisible)
	{
		VertexPath vertexPath;
		vertexPath.options = options;
		vertexPath.path = path;
		_vertexPathes.push_back(vertexPath);
	}
	else
	{
		delete path;
	}
}

// *************************************************************
//		DrawPolygonGDIPlus()
// *************************************************************
// Draws a single polygon choosing the correct mode
bool CShapefileDrawer::DrawPolygonGDIPlus(int shapeIndex, Gdiplus::GraphicsPath& path, double minSize, OLE_COLOR pointColor, tkVectorDrawingMode drawingMode,
										 double& xMin, double& xMax, double& yMin, double& yMax)
{
	if (! _isEditing)
	{
		if (_fastMode)
		{
			IShapeData* shpData = _shapefile->get_ShapeData(shapeIndex);
			if (shpData)
			{
				shpData->get_BoundsXY(xMin, xMax, yMin, yMax);
				if (this->WithinVisibleExtents(xMin, xMax, yMin, yMax))
				{
					(*_shapeData)[shapeIndex]->size = (int)(((xMax - xMin) + (yMax - yMin))/2.0*_dx);
					if ((xMax - xMin >= minSize) || (yMax - yMin >= minSize))	// the poly must be larger than a pixel at a current to be drawn
					{
						this->AddPolygonToPath(&path, shpData, drawingMode);
						return true;
					}
					else
					{
						this->DrawPolygonPoint(xMin, xMax, yMin, yMax, pointColor);
						return false;
					}
				}
			}
		}
		else
		{
			char* data = _sfReader->ReadShapeData(shapeIndex);
			if (data)
			{
				double* bounds = (double*)(data + 4);
				xMin = bounds[0]; yMin = bounds[1];	xMax = bounds[2]; yMax = bounds[3];

				bool result = false;
				if (this->WithinVisibleExtents(xMin, xMax, yMin, yMax))
				{
					(*_shapeData)[shapeIndex]->size = (int)(((xMax - xMin) + (yMax - yMin))/2.0*_dx);
					if ((xMax - xMin >= minSize) || (yMax - yMin >= minSize))	// the poly must be larger than a pixel at a current to be drawn
					{
						PolygonData* shapeData = _sfReader->ReadPolygonData(data);
						this->AddPolygonToPath(&path, shapeData, drawingMode);
						delete shapeData;
						result = true;
					}
					else
					{
						this->DrawPolygonPoint(xMin, xMax, yMin, yMax, pointColor);
						result = false;
					}
				}
				delete[] data;
				return result;
			}
		}
	}
	else
	{
		if (_fastMode)
		{
			CShapeWrapper* wrapper = (CShapeWrapper*)_shapefile->get_ShapeWrapper(shapeIndex);
			IShapeData* shpData = (IShapeData*)wrapper;
			if ( shpData )
			{
				shpData->get_BoundsXY(xMin, xMax, yMin, yMax);
				if (this->WithinVisibleExtents(xMin, xMax, yMin, yMax))
				{
					(*_shapeData)[shapeIndex]->size = (int)(((xMax - xMin) + (yMax - yMin))/2.0*_dx);
					if ((xMax - xMin >= minSize) || (yMax - yMin >= minSize))	// the poly must be larger than a pixel at a current to be drawn
					{
						this->AddPolygonToPath(&path, shpData, drawingMode);
						return true;
					}
					else
					{
						this->DrawPolygonPoint(xMin, xMax, yMin, yMax, pointColor);
						return false;
					}
				}
			}
		}
		else
		{
			// regular mode with COM points
			CShapeWrapperCOM* shp = (CShapeWrapperCOM*)((CShapefile*)_shapefile)->get_ShapeWrapper(shapeIndex);
			ASSERT(shp);

			if (shp)
			{
				shp->get_BoundsXY(xMin, xMax, yMin, yMax);

				if (this->WithinVisibleExtents(xMin, xMax, yMin, yMax))
				{
					(*_shapeData)[shapeIndex]->size = (int)(((xMax - xMin) + (yMax - yMin))/2.0*_dx);
					if ((xMax - xMin >= minSize) || (yMax - yMin >= minSize))	// the poly must be larger than a pixel at a current to be drawn
					{
						this->AddPolygonToPath(&path, shp, drawingMode);
						return true;
					}
					else
					{
						this->DrawPolygonPoint(xMin, xMax, yMin, yMax, pointColor);
						return false;
					}
				}
			}
		}
	}
	return false;
}
#pragma endregion

#pragma region DrawPolyCategoryGDI
// ****************************************************************
//		DrawPolygonCategoryGDI()
// ****************************************************************
// GDI drawing
void CShapefileDrawer::DrawLineCategoryGDI(CDrawingOptionsEx* options, std::vector<int>* indices, bool drawSelection)
{
	ASSERT(_dc == NULL);
	
	IShapeWrapper* shp = NULL;
	IShapeData* shpData = NULL;

	double xMin, xMax, yMin, yMax;
	char* data = NULL;
	
	// Entering GDI mode; no calls to _graphics until releasing HDC!!!
	m_hdc = _graphics->GetHDC();
	_dc = CDC::FromHandle(m_hdc);
	_dc->EndPath();
	_dc->SetPolyFillMode(WINDING);
	options->InitGdiBrushAndPen(_dc, drawSelection, m_selectionColor);
	
	Gdiplus::GraphicsPath* path = new Gdiplus::GraphicsPath();
	
	VARIANT_BOOL fastMode;
	_shapefile->get_FastMode(&fastMode);
	
	LONG minDrawingSize;
	_shapefile->get_MinDrawingSize(&minDrawingSize);
	
	double delta = MIN(minDrawingSize/_dx, minDrawingSize/_dy);
	int count = 0;
	
	OLE_COLOR pointColor = options->linesVisible?options->lineColor:options->fillColor;
	_bmpPixel->SetPixel(0,0, Gdiplus::Color(255 << 24 | BGR_TO_RGB(pointColor)));

	for (int j = 0; j < (int)indices->size(); j++)
	{
		int shapeIndex = (*indices)[j];
		if (! _isEditing)
		{
			if (fastMode)
			{
				shpData = _shapefile->get_ShapeData(shapeIndex);
				if (shpData)
				{
					shpData->get_BoundsXY(xMin, xMax, yMin, yMax);
					
					if (WithinVisibleExtents(xMin, xMax, yMin, yMax))
					{
						(*_shapeData)[shapeIndex]->size = (int)(((xMax - xMin) + (yMax - yMin))/2.0*_dx);
						if ((xMax - xMin >= delta) || (yMax - yMin >= delta))	// the poly must be larger than a pixel at a current to be drawn
						{
							this->DrawPolyGDI( shpData, options, *path, options->verticesVisible?true:false);
						}
						else
						{
							this->DrawPolygonPoint(xMin, xMax, yMin, yMax, pointColor);
						}
					}
				}
			}
			else
			{
				char* data = _sfReader->ReadShapeData(shapeIndex);
				if (data)
				{
					double* bounds = (double*)(data + 4);
					xMin = bounds[0]; yMin = bounds[1];	xMax = bounds[2]; yMax = bounds[3];

					if (WithinVisibleExtents(xMin, xMax, yMin, yMax))
					{
						(*_shapeData)[shapeIndex]->size = (int)(((xMax - xMin) + (yMax - yMin))/2.0*_dx);
						if ((xMax - xMin >= delta) || (yMax - yMin >= delta))	// the poly must be larger than a pixel at a current to be drawn
						{
							PolygonData* shapeData = _sfReader->ReadPolygonData(data);
							this->DrawPolyGDI( shapeData, options, *path, options->verticesVisible?true:false);
							delete shapeData;
						}
						else
						{
							this->DrawPolygonPoint(xMin, xMax, yMin, yMax, pointColor);
						}
					}
					delete[] data;
				}
			}
		}
		else
		{
			if (fastMode)
			{
				CShapeWrapper* wrapper = (CShapeWrapper*)_shapefile->get_ShapeWrapper(shapeIndex);
				IShapeData* shpData = (IShapeData*)wrapper;
				
				if ( shpData )
				{
					shpData->get_BoundsXY(xMin, xMax, yMin, yMax);

					if (WithinVisibleExtents(xMin, xMax, yMin, yMax))
					{
						(*_shapeData)[shapeIndex]->size = (int)(((xMax - xMin) + (yMax - yMin))/2.0*_dx);
						if ((xMax - xMin >= delta) || (yMax - yMin >= delta))	// the poly must be larger than a pixel at a current to be drawn
						{
							this->DrawPolyGDI( shpData, options, *path, options->verticesVisible?true:false);
						}
						else
						{
							this->DrawPolygonPoint(xMin, xMax, yMin, yMax, pointColor);
						}
					}
				}
			}
			else
			{
				// regular mode with COM points
				CShapeWrapperCOM* shp = (CShapeWrapperCOM*)((CShapefile*)_shapefile)->get_ShapeWrapper(shapeIndex);
				ASSERT(shp);
			
				if (shp)
				{
					shp->get_BoundsXY(xMin, xMax, yMin, yMax);

					if (WithinVisibleExtents(xMin, xMax, yMin, yMax))
					{
						(*_shapeData)[shapeIndex]->size = (int)(((xMax - xMin) + (yMax - yMin))/2.0*_dx);
						if ((xMax - xMin >= delta) || (yMax - yMin >= delta))	// the poly must be larger than a pixel at a current to be drawn
						{
							this->DrawPolyGDI(shp, options, *path, options->verticesVisible?true:false);
						}
						else
						{
							this->DrawPolygonPoint(xMin, xMax, yMin, yMax, pointColor);
						}
					}
				}
			}
		}
	}
	options->ReleaseGdiBrushAndPen(_dc);
	_graphics->ReleaseHDC(m_hdc);
	_dc = NULL;

	// drawing of vertices
	if (options->verticesVisible)
	{
		VertexPath vertexPath;
		vertexPath.options = options;
		vertexPath.path = path;
		_vertexPathes.push_back(vertexPath);
	}
	else
	{
		delete path;
	}
}
#pragma endregion

#pragma region DrawVertices
// ******************************************************************
//		DrawVertices()
// ******************************************************************
void CShapefileDrawer::DrawVertices(Gdiplus::GraphicsPath* path, CDrawingOptionsEx* options)
{
	if (options->verticesVisible)
	{
		_dc->EndPath();	   // in GDIPlus mode no drawing occurs otherwise
		
		Gdiplus::PointF* points = NULL;
		int count = path->GetPointCount();
		if (count > 0)
		{
			points = new Gdiplus::PointF[count];
			path->GetPathPoints(points, count);
			
			// drawing of vertices
			if (options->verticesVisible)
			{
				options->InitGdiVerticesPen(_dc);

				CPoint* square = options->GetVertex();
				int size = options->verticesSize/2;

				for (int i = 0; i < count; i++)
				{
					int x = (int)-points[i].X;
					int y = (int)-points[i].Y;

					_dc->SetWindowOrg(x, y);
					if (options->verticesType == vtSquare)
					{
						_dc->Polygon(square, 4);
					}
					else
					{
						_dc->Ellipse(-size, -size, size, size);
					}
				}
				_dc->SetWindowOrg(0 , 0);
				delete[] square;
				options->ReleaseGdiBrushAndPen(_dc);
			}
			delete[] points;
		}
	}
}
#pragma endregion

#pragma region MarkerLine

// ******************************************************************
//		DrawLinePatternCategory()
// ******************************************************************
// GDI+ only; no HDC is required
void CShapefileDrawer::DrawLinePatternCategory(CDrawingOptionsEx* options, std::vector<int>* indices, bool drawSelection)
{
	ASSERT(_dc == NULL);
	
	if (_shptype != SHP_POLYLINE )
		return;
	
	double xMin, xMax, yMin, yMax;
	char* data = NULL;

	GraphicsPath* path = new Gdiplus::GraphicsPath(Gdiplus::FillModeWinding);
	
	LONG minDrawingSize;
	_shapefile->get_MinDrawingSize(&minDrawingSize);
	double delta = MIN(minDrawingSize/_dx, minDrawingSize/_dy);

	OLE_COLOR pointColor = options->linesVisible?options->lineColor:options->fillColor;
	_bmpPixel->SetPixel(0,0, Gdiplus::Color(255 << 24 | BGR_TO_RGB(pointColor)));

	// constructing a path
	for (int j = 0; j < (int)indices->size(); j++)
	{
		this->DrawPolygonGDIPlus((*indices)[j], *path, delta, pointColor, vdmGDIPlus, xMin, xMax, yMin, yMax);
	}
	
	// the drawing
	this->DrawPolylinePath(path, options, drawSelection);

	if (options->linePattern && drawSelection)
	{
		int count;
		options->linePattern->get_Count(&count);
		float maxWidth = 0.0f;

		for (int i = 0; i < count; i++)
		{
			ILineSegment* line = NULL;
			options->linePattern->get_Line(i, &line);
			tkLineType style;
			line->get_LineType(&style);
			if (style == lltSimple)
			{
				float width;
				line->get_LineWidth(&width);
				if (width > maxWidth)
				{
					maxWidth = width;
				}
			}
			line->Release();
		}
		
		if (maxWidth > 0.0f)
		{
			Gdiplus::Pen penSelection(Gdiplus::Color(m_selectionTransparency << 24 | BGR_TO_RGB(m_selectionColor)), maxWidth);
			penSelection.SetLineJoin(Gdiplus::LineJoinRound);
			_graphics->DrawPath(&penSelection, path);
		}
	}

	if (options->verticesVisible)
	{
		VertexPath vertexPath;
		vertexPath.options = options;
		vertexPath.path = path;
		_vertexPathes.push_back(vertexPath);
	}
	else
	{
		delete path;
	}
}

// ******************************************************************
//		DrawPolyline()
// ******************************************************************
void CShapefileDrawer::DrawPolylinePath(Gdiplus::GraphicsPath* path, CDrawingOptionsEx* options, bool drawSelection)
{
	if (!options->linePattern)
		return;

	ILinePattern* pattern = options->linePattern;

	int numLines;
	if (options->linePattern->get_Count(&numLines));
	if (numLines == 0)
		return;
	
	ILineSegment* line = NULL;
	
	// path related variables
	bool dataRead = false;
	int pointCount = 0;				// number of points in path
	
	Gdiplus::PointF* points = NULL;
	double ratio;
	
	Gdiplus::PathData* data = new PathData();
	
	BYTE transparency;
	options->linePattern->get_Transparency(&transparency);

	Gdiplus::Pen* penSelection = NULL;
	Gdiplus::SolidBrush* brushSelection = NULL;

	if (drawSelection)
	{
		//penSelection = new Gdiplus::Pen(Gdiplus::Color(m_selectionTransparency << 24 | BGR_TO_RGB(m_selectionColor)));
		brushSelection = new Gdiplus::SolidBrush(Gdiplus::Color(m_selectionTransparency << 24 | BGR_TO_RGB(m_selectionColor)));
	}

	Gdiplus::SmoothingMode mode = _graphics->GetSmoothingMode();
	_graphics->SetSmoothingMode(Gdiplus::SmoothingModeAntiAlias);

	for ( int i = 0; i < numLines; i++)
	{
		pattern->get_Line(i, &line);
		tkLineType type;
		OLE_COLOR color;
		line->get_LineType(&type);
		line->get_Color(&color);

		if (type == lltSimple)
		{
			float width;
			tkDashStyle style;
			line->get_LineWidth(&width);
			line->get_LineStyle(&style);
			
			Gdiplus::Pen* pen = new Gdiplus::Pen(Gdiplus::Color(transparency << 24 | BGR_TO_RGB(color)), width);
			pen->SetLineJoin(Gdiplus::LineJoinRound);
			switch (style)
			{
				case dsSolid:		pen->SetDashStyle(Gdiplus::DashStyleSolid);		break;
				case dsDash:		pen->SetDashStyle(Gdiplus::DashStyleDash);		break;
				case dsDot:			pen->SetDashStyle(Gdiplus::DashStyleDot);		break;
				case dsDashDotDot:	pen->SetDashStyle(Gdiplus::DashStyleDashDotDot);break;
				case dsDashDot:		pen->SetDashStyle(Gdiplus::DashStyleDashDot);	break;
				default:			pen->SetDashStyle(Gdiplus::DashStyleSolid);
			}
			
			_graphics->DrawPath(pen, path);
			delete pen;
		}
		else if (type == lltMarker)
		{
			bool saveAngles = true;
			
			if (!dataRead)
			{
				pointCount = path->GetPointCount();
				if (pointCount > 0)
				{
					path->GetPathData(data);
					pointCount = data->Count;
					//(path.GetPathData(&data) == Gdiplus::Ok)
				}
				dataRead = true;
			}

			if (pointCount > 0)
			{
				// extracting properties
				tkDefaultPointSymbol symbol;
				tkLineLabelOrientation orientation;
				float markerSize, interval, markerOffset;
				OLE_COLOR lineColor;
				
				line->get_Marker(&symbol);
				line->get_MarkerSize(&markerSize);
				line->get_MarkerInterval(&interval);
				line->get_MarkerOffset(&markerOffset);
				line->get_MarkerOutlineColor(&lineColor);
				line->get_MarkerOrientation(&orientation);
				
				//	preparing marker
				int numPoints = 0;
				float* points = get_SimplePointShape(symbol, markerSize, &numPoints);
				int n = 0;

				if (numPoints > 0)
				{
					Gdiplus::SolidBrush* brush = new Gdiplus::SolidBrush(Gdiplus::Color(transparency << 24 | BGR_TO_RGB(color)));
					Gdiplus::Pen* pen = new Gdiplus::Pen(Gdiplus::Color(transparency << 24 | BGR_TO_RGB(lineColor)));
					pen->SetAlignment(Gdiplus::PenAlignmentInset);
				
					double offset = 0;	// set by the marker offset for the start point, and by the unended part of interval for others

					while(n < pointCount)
					{
						if (data->Types[n] == PathPointTypeStart)
						{
							offset = markerOffset;
						}
						else
						{
							CPoint pnt;
							pnt.x = (LONG)data->Points[n - 1].X;
							pnt.y = (LONG)data->Points[n - 1].Y;

							double dx = data->Points[n].X - data->Points[n - 1].X;
							double dy = data->Points[n].Y - data->Points[n - 1].Y;

							double length = sqrt(pow(dx,2.0) + pow(dy,2.0));

							if (length < offset)
							{
								offset -= length;
							}
							else
							{
								int count = (int)((length - offset)/interval) + 1;

								ratio = offset/length;
								Gdiplus::PointF pntStart(Gdiplus::REAL(pnt.x + dx * ratio), Gdiplus::REAL(pnt.y + dy * ratio));
								
								float angle = 0.0f;
								if (orientation == lorParallel )
								{
									angle = (float)(GetPointAngle(dx, dy) / pi * 180.0 - 90.0);
								}
								else if (orientation == lorPerpindicular)
								{
									angle = (float)(GetPointAngle(dx, dy) / pi * 180.0); //+ 90.0);
								}
								
								//int size = (int)markerSize/2;
								SIZE size;
								size.cx = (LONG)markerSize;
								size.cy = (LONG)markerSize;

								for (int j = 0; j < count; j++)
								{
									double ratio = interval/length * (double)j;
									Gdiplus::REAL xPos =(Gdiplus::REAL)(pntStart.X + dx * ratio);
									Gdiplus::REAL yPos =(Gdiplus::REAL)(pntStart.Y + dy * ratio);
									
									/*CPoint pnt((int)xPos, (int)yPos);
									CRect rect(pnt, size);

									if (!_collisionList->HaveCollision(rect))
									{*/
										//_collisionList->AddRectangle(&rect);
										
										_graphics->TranslateTransform(xPos , yPos);
										_graphics->RotateTransform(-angle);

										_graphics->FillPolygon(brush, (Gdiplus::PointF*)points, numPoints);
										_graphics->DrawPolygon(pen, (Gdiplus::PointF*)points, numPoints);

										if (drawSelection)
										{
											_graphics->FillPolygon(brushSelection, (Gdiplus::PointF*)points, numPoints);
										}
									
										_graphics->ResetTransform();
									//}
								}

								offset = interval - ((length - offset) - ((count - 1) * interval));		// the part of interval left to draw
							}
						}

						n++;
					}
					delete[] points;
					delete brush;
					delete pen;
				}
			}
		}
	}

	_graphics->SetSmoothingMode(mode);

	if (penSelection)	delete penSelection;
	if (brushSelection) delete brushSelection;

	delete data;

}
#pragma endregion

#pragma region PolygonToPathGDI+
// ******************************************************************
//		AddPolygonToPath()
// ******************************************************************
// disk version in GDI+
void CShapefileDrawer::AddPolygonToPath(Gdiplus::GraphicsPath* pathFill, PolygonData* shapeData, tkVectorDrawingMode drawingMode)
{
	double* srcPoints = shapeData->points;
	for (int i = 0; i < shapeData->partCount; i++)
	{
		int end;
		if (i == shapeData->partCount - 1)	
		{
			end = shapeData->pointCount - 1;
		}
		else
		{
			end = shapeData->parts[i+1] - 1;
		}
		
		int start = *(shapeData->parts + i);
		int count = end - start + 1;
		
		if ( count > 1)		// if (count > 2)
		{
			int* points = new int[count * 2];
			int k = 0;

			for (int j = start; j <= end; j++)
			{
				int k2 = k * 2;
				points[k2] = (int)((srcPoints[j * 2] - _extents->left) * _dx);
				points[k2 + 1] = (int)((_extents->top - srcPoints[j * 2 + 1]) * _dy);
				
				// we won't write duplicate points
				if (k != 0) 
				{
					if ((points[k2] != points[k2 - 2]) || 
						(points[k2 + 1] != points[k2 - 1]))
						k++;
				}
				else
					k++;
			}
			
			if (k > 1)
			{
				if ( drawingMode == vdmGDIMixed )
				{
					if ( _shptype == SHP_POLYGON )
					{
						_dc->Polygon(reinterpret_cast<POINT*>(points), k);
					}
					else if ( _shptype == SHP_POLYLINE ) 
					{
						_dc->Polyline(reinterpret_cast<POINT*>(points), k);
					}
				}
				
				pathFill->StartFigure();
				pathFill->AddLines(reinterpret_cast<Gdiplus::Point*>(points), k);
			}
			delete[] points; points = NULL;
		}
	}
}

// ******************************************************************
//		AddPolygonToPath()
// ******************************************************************
// Regular in-memory version for GDI+
void CShapefileDrawer::AddPolygonToPath( Gdiplus::GraphicsPath* pathFill, CShapeWrapperCOM* shp, tkVectorDrawingMode drawingMode)
{
	// fast access to points
	std::deque<IPoint*>	allPoints = shp->_allPoints;
	std::deque<long> allParts = shp->_allParts;
	
	int partCount =  allParts.size();
	for(int i = 0; i < partCount; i++)
	{
		long start, end;
		start = shp->get_PartStartPoint(i);
		end = shp->get_PartEndPoint(i);
		int count = end - start + 1;
		
		if (count > 1)			// if (count > 2)
		{
			int* points = new int[count * 2];
			
			int k = 0;
			double x,y;
			for(long j = start; j <= end; j++)
			{
				allPoints[j]->get_X(&x);
				allPoints[j]->get_Y(&y);
				
				int k2 = k * 2;
				points[k2] = (int)((x - _extents->left) * _dx);
				points[k2 + 1] = (int)((_extents->top - y) * _dy);
				
				// we won't write duplicate points
				if (k != 0) 
				{
					if ((points[k2] != points[k2 - 2]) || (points[k2 + 1] != points[k2 - 1]))
						k++;
				}
				else
					k++;
			}

			if (k > 1)
			{
				if ( drawingMode == vdmGDIMixed )
				{
					if ( _shptype == SHP_POLYGON )		 
					{
						_dc->Polygon(reinterpret_cast<POINT*>(points), k);
					}
					else if ( _shptype == SHP_POLYLINE ) 
					{
						_dc->Polyline(reinterpret_cast<POINT*>(points), k);
					}
				}
				
				pathFill->StartFigure();
				pathFill->AddLines(reinterpret_cast<Gdiplus::Point*>(points), k);
			}
			delete[] points; points = NULL;
		}
	}
}

// ******************************************************************
//		AddPolygonToPath()
// ******************************************************************
// Fast in-memory version for GDI+
void CShapefileDrawer::AddPolygonToPath(GraphicsPath* pathFill, IShapeData* shp, tkVectorDrawingMode drawingMode)
{
	int partCount =  shp->get_PartCount();
	for(int i = 0; i < partCount; i++)
	{
		int start = shp->get_PartStartPoint(i);
		int end = shp->get_PartEndPoint(i);
		int count = end - start + 1;
		
		if (count > 1)		// if (count > 2)
		{
			int* points = new int[count * 2];
			double* srcPoints = shp->get_PointsXY();
			
			int k = 0;
			for(int j = start; j <= end; j++)
			{
				int k2 = k * 2;
				points[k2] = (int)((srcPoints[j * 2] - _extents->left) * _dx);
				points[k2 + 1] = (int)((_extents->top - srcPoints[j * 2 + 1]) * _dy);
				
				// we won't write duplicate points
				if (k != 0) 
				{
					if ((points[k2] != points[k2 - 2]) || (points[k2 + 1] != points[k2 - 1]))
						k++;
				}
				else
					k++;
			}

			if (k > 1)
			{
				if ( drawingMode == vdmGDIMixed )
				{
					if ( _shptype == SHP_POLYGON )		 
					{
						_dc->Polygon(reinterpret_cast<POINT*>(points), k);
					}
					else if ( _shptype == SHP_POLYLINE ) 
					{
						_dc->Polyline(reinterpret_cast<POINT*>(points), k);
					}
				}
				
				pathFill->StartFigure();
				pathFill->AddLines(reinterpret_cast<Gdiplus::Point*>(points), k);
			}
			delete[] points; points = NULL;
		}
	}
}
#pragma endregion

#pragma region DrawPolyGDI
// ***************************************************************
//	  DrawPolyGDI()
// ***************************************************************
// Disk version
void CShapefileDrawer::DrawPolyGDI( PolygonData* shapeData, CDrawingOptionsEx* options, Gdiplus::GraphicsPath& path, bool pathIsNeeded )
{
	int* points = new int[shapeData->pointCount * 2];
	int* parts = new int[shapeData->partCount];
	double* srcPoints = shapeData->points;

	int count = 0;
	for (int part = 0; part < shapeData->partCount; part++)
	{
		int end, start;
		if (part == shapeData->partCount - 1)	
		{
			end = shapeData->pointCount;
		}
		else
		{
			end = shapeData->parts[part+1];
		}
		
		start = *(shapeData->parts + part);
		
		int partCount = 0;
		for (int j = start; j < end; j++)
		{
			int k2 = count * 2;
			points[k2] = (int)((srcPoints[j * 2] - _extents->left) * _dx);
			points[k2 + 1] = (int)((_extents->top - srcPoints[j * 2 + 1]) * _dy);
			
			count++;
			partCount++;
		}
			
		parts[part] = partCount;
	}		
	
	if ( _shptype == SHP_POLYLINE )
	{
		_dc->PolyPolyline(reinterpret_cast<POINT*>(points), (DWORD*)parts, shapeData->partCount);
	}
	else if ( _shptype == SHP_POLYGON )
	{
		_dc->PolyPolygon(reinterpret_cast<POINT*>(points), parts, shapeData->partCount);
	}
	
	if (pathIsNeeded)
	{
		path.AddLines(reinterpret_cast<Gdiplus::Point*>(points), count);
	}

	delete[] points;
	delete[] parts; 
}

// ******************************************************************
//		DrawPolyGDI()
// ******************************************************************
// Regular in-memory version
void CShapefileDrawer::DrawPolyGDI( CShapeWrapperCOM* shp, CDrawingOptionsEx* options, Gdiplus::GraphicsPath& path, bool pathIsNeeded )
{
	// fast access to points
	std::deque<IPoint*>	allPoints = shp->_allPoints;
	std::deque<long> allParts = shp->_allParts;

	if (allParts.size() == 0 || allPoints.size() == 0)
		return;
	
	int numParts = allParts.size();
	int* points = new int[allPoints.size() * 2];
	int* parts = new int[numParts];

	int count = 0;
	for(int part = 0; part < numParts; part++)
	{
		long start, end;
		start = shp->get_PartStartPoint(part);
		end = shp->get_PartEndPoint(part);
			
		int partCount = 0;
		double x, y;
		for(int j = start; j <= end; j++)
		{
			allPoints[j]->get_X(&x);
			allPoints[j]->get_Y(&y);

			int k2 = count * 2;
			points[k2] = (int)((x - _extents->left) * _dx);
			points[k2 + 1] = (int)((_extents->top - y) * _dy);

			count++;
			partCount++;
		}
		parts[part] = partCount;
	}

	if ( _shptype == SHP_POLYLINE )
	{
		_dc->PolyPolyline(reinterpret_cast<POINT*>(points), (DWORD*)parts, numParts);
	}
	else if ( _shptype == SHP_POLYGON )
	{
		_dc->PolyPolygon(reinterpret_cast<POINT*>(points), parts, numParts);
	}
	
	if (pathIsNeeded)
	{
		path.AddLines(reinterpret_cast<Gdiplus::Point*>(points), count);
	}

	delete[] points;
	delete[] parts; 
}

// ******************************************************************
//		DrawPolyGDI()
// ******************************************************************
// Fast in-memory version
void CShapefileDrawer::DrawPolyGDI( IShapeData* shp, CDrawingOptionsEx* options, Gdiplus::GraphicsPath& path, bool pathIsNeeded )
{
	if (shp->get_PointCount() == 0 || shp->get_PartCount() == 0)
		return;
	
	int numParts = shp->get_PartCount();
	int* points = new int[shp->get_PointCount() * 2];
	int* parts = new int[numParts];
	double* srcPoints = shp->get_PointsXY();

	int count = 0;
	for(int part = 0; part < numParts; part++)
	{
		int start = shp->get_PartStartPoint(part);
		int end = shp->get_PartEndPoint(part);
			
		int partCount = 0;
		for(int j = start; j <= end; j++)
		{
			int k2 = count * 2;
			points[k2] = (int)((srcPoints[j * 2] - _extents->left) * _dx);
			points[k2 + 1] = (int)((_extents->top - srcPoints[j * 2 + 1]) * _dy);

			count++;
			partCount++;
		}
		parts[part] = partCount;
	}

	if ( _shptype == SHP_POLYLINE )
	{
		_dc->PolyPolyline(reinterpret_cast<POINT*>(points), (DWORD*)parts, numParts);
	}
	else if ( _shptype == SHP_POLYGON )
	{
		_dc->PolyPolygon(reinterpret_cast<POINT*>(points), parts, numParts);
	}
	
	if (pathIsNeeded)
	{
		path.AddLines(reinterpret_cast<Gdiplus::Point*>(points), count);
	}

	delete[] points;
	delete[] parts; 
}

// ******************************************************************
//		DrawPolygonPoint()
// ******************************************************************
// Draws point in plcae of small polygons
inline void CShapefileDrawer::DrawPolygonPoint(double &xMin, double& xMax, double& yMin, double& yMax, OLE_COLOR& pointColor)
{
	int x = (int)(((xMax + xMin)/2 - _extents->left) * _dx);
	int y = (int)((_extents->top - (yMax + yMin)/2) * _dy);

	if (!_dc)
	{
		_graphics->DrawImage(_bmpPixel, x, y);
	}
	else
	{
		_dc->SetPixelV( x, y, pointColor);
	}
}
#pragma endregion

#pragma region Utilities
//********************************************************************
//*		SelectShapesFromSpatialIndex()
//********************************************************************
std::vector<long>* CShapefileDrawer::SelectShapesFromSpatialIndex(char* sFilename, Extent* extents)
{
	string baseName;
	baseName = sFilename;
	baseName = baseName.substr(0,baseName.find_last_of("."));
	
	double lowVals[2], highVals[2];
	lowVals[0] = extents->left;
	lowVals[1] = extents->bottom;
	highVals[0] = extents->right;
	highVals[1] = extents->top;
	
	CIndexSearching *res = new CIndexSearching();	

	if (selectShapesFromIndex((char *)baseName.c_str(), lowVals, highVals, intersection, 100, res) == 0)
	{
		std::vector<long>* selectResult = new std::vector<long>;	
		selectResult->reserve(res->getLength());
		for (int i = 0 ;i < res->getLength(); i++)
		{
			selectResult->push_back((long)res->getValue(i));
		}
		
		delete res;
		return selectResult;
	}
	else
	{
		delete res;
		return NULL;
	}
}

// *************************************************************
//		HavePointCollision()
// *************************************************************
// Checks whether new point overlaps one of the existing points;
// Bounds are treated there as recatngles without rotation
bool CShapefileDrawer::HavePointCollision(CRect* rect)
{
	for(int i=0; i < (int)_pointRectangles.size(); i++)
	{
		CRect* r = _pointRectangles[i];
		if ((rect->right < r->left) || 
			(rect->bottom < r->top)  || 
			(r->right < rect->left) || 
			(r->bottom < rect->top))
			continue;
		else
			return true;
	}
	_pointRectangles.push_back(rect);
	return false;
}
#pragma endregion



//if (i == count - 1)
//							i++;				// we act like we found the following starting point
//
//						// saving the points of previous line
//						if (firstIndex != -1)
//						{
//							// the first point
//							CPoint pnt;
//							pnt.x = (LONG)data.Points[firstIndex].X;
//							pnt.y = (LONG)data.Points[firstIndex].Y;
//							
//							if (pnt.x > 0 && pnt.y > 0)
//							{
//								CRect* rect = new CRect(pnt.x - (int)markerSize, pnt.y - (int)markerSize, pnt.x + (int)markerSize, pnt.y + (int)markerSize);
//								if (!HavePointCollision(rect))
//								{
//									positions.push_back(pnt);
//								
//									if (saveAngles && (firstIndex + 1 < count))
//									{
//										double dx = pnt.x - data.Points[firstIndex + 1].X ;
//										double dy = pnt.y - data.Points[firstIndex + 1].Y;
//										double rotation = GetPointAngle(dx, dy) / pi * 180.0;
//
//										if (options->lineDecorationFlipFirst)
//										{
//											rotation += 180.0;
//										}
//
//										angles.push_back((float)rotation);
//									}
//								}
//								else delete rect;
//							}
//							
//							// the last point
//							if (options->lineDecorationCount > 1)
//							{
//								CPoint pnt;
//								pnt.x = (LONG)data.Points[i - 1].X;
//								pnt.y = (LONG)data.Points[i - 1].Y;
//								
//								if (pnt.x > 0 && pnt.y > 0)
//								{
//									CRect* rect = new CRect(pnt.x - (int)pointSize, pnt.y - (int)pointSize, pnt.x + (int)pointSize, pnt.y + (int)pointSize);
//									if (!HavePointCollision(rect))
//									{
//										positions.push_back(pnt);
//										
//										if (saveAngles)
//										{
//											double dx = data.Points[i - 2].X - pnt.x;
//											double dy = data.Points[i - 2].Y - pnt.y;
//											double rotation = GetPointAngle(dx, dy) / pi * 180.0;
//
//											angles.push_back((float)rotation);
//										}
//									}
//									else delete rect;
//								}
//							}
//							
//							// the other points
//							double interval = length/(options->lineDecorationCount - 1);
//							double limit = interval;
//							double lengthTemp = 0.0;
//							double innerCount = 0; // number of inner points
//
//							for (int j = firstIndex + 1; j < i; j++ )
//							{
//								double delta = sqrt(pow(double(data.Points[j - 1].X - data.Points[j].X),2.0) + pow(double(data.Points[j - 1].Y - data.Points[j].Y),2.0));
//								
//								if (lengthTemp + delta < limit)
//								{
//									// the positions lies outside the segment
//									lengthTemp += delta;
//								}
//								else if (lengthTemp + delta == limit)
//								{
//									// the position lies at the end of segment
//									CPoint pnt;
//									pnt.x = (LONG)data.Points[j].X;
//									pnt.y = (LONG)data.Points[j].Y;
//									lengthTemp += delta;
//									limit += interval;
//									innerCount++;
//									
//									if (pnt.x > 0 && pnt.y > 0)
//									{
//										CRect* rect = new CRect(pnt.x - (int)pointSize, pnt.y - (int)pointSize, pnt.x + (int)pointSize, pnt.y + (int)pointSize);
//										if (!HavePointCollision(rect))
//										{
//											positions.push_back(pnt);
//											if (saveAngles)
//											{
//												double dx = data.Points[j - 1].X - pnt.x;
//												double dy = data.Points[j - 1].Y - pnt.y;
//												double rotation = GetPointAngle(dx, dy) / pi * 180.0;
//												angles.push_back((float)rotation);
//											}
//										}
//										else delete rect;
//									}
//								}
//								else if (lengthTemp + delta > limit)
//								{
//									// the position lies at the middle of segment
//									double ratio = (limit - lengthTemp)/(delta);
//									CPoint pnt;
//									pnt.x = (LONG)(data.Points[j - 1].X + (data.Points[j].X - data.Points[j - 1].X) * ratio);
//									pnt.y = (LONG)(data.Points[j - 1].Y + (data.Points[j].Y - data.Points[j - 1].Y) * ratio);
//									limit += interval;
//									lengthTemp += delta * ratio;
//									innerCount++;
//									
//									if (pnt.x > 0 && pnt.y > 0)
//									{
//										CRect* rect = new CRect(pnt.x - (int)pointSize, pnt.y - (int)pointSize, pnt.x + (int)pointSize, pnt.y + (int)pointSize);
//										if (!HavePointCollision(rect))
//										{
//											positions.push_back(pnt);
//
//											if (saveAngles)
//											{
//												double dx = data.Points[j - 1].X - data.Points[j].X ;
//												double dy = data.Points[j - 1].Y - data.Points[j].Y;
//												double rotation = GetPointAngle(dx, dy) / pi * 180.0;
//												angles.push_back((float)rotation);
//											}
//										}
//										else delete rect;
//									}
//								}
//								
//								if (innerCount + 2 == options->lineDecorationCount)		// first and last points
//								{
//									// all middle points were found, no need to search for the last point									
//									break;
//								}
//							}
//						}
//						
//						// starting new line
//						firstIndex = i;
//						length = 0;
//					}
//					else
//					{
//						length += sqrt(pow(double(data.Points[i - 1].X - data.Points[i].X),2.0) + pow(double(data.Points[i - 1].Y - data.Points[i].Y),2.0));
//					}