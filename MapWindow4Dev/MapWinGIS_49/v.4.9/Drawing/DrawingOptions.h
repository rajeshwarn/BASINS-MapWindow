/**************************************************************************************
 * File name: DrawingOptions.h
 *
 * Project: MapWindow Open Source (MapWinGis ActiveX control) 
 * Description: Declaration of CDrawingOptionsEx
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
 // Sergei Leschinski (lsu) 25 june 2010 - created the file.

#pragma once
#include "MapWinGis.h"
#include <gdiplus.h>
#include "enumerations.h"

class CDrawingOptionsEx
{
public:
	
	CDrawingOptionsEx& operator=(const CDrawingOptionsEx& opt);

public:	
	//OLE_COLOR selectionColor;
	//unsigned short selectionTransparency;
	//bool useSelectionColor;
	
	bool visible;
	bool fillVisible;
	bool linesVisible;
	bool fillBgTransparent;

	double scaleX;
	double scaleY;
	double rotation;
	//double pictureRotation;
	double fillGradientRotation;

	// colors
	OLE_COLOR fillColor;
	OLE_COLOR fillColor2;
	OLE_COLOR lineColor;
	OLE_COLOR pointColor;
	OLE_COLOR fillBgColor;
	
	// stipples
	tkDashStyle lineStipple;
	tkGDIPlusHatchStyle fillHatchStyle;

	float lineWidth;
	float pointSize;	
	float fillTransparency;			// used without stipples (GDI+)	 // 0 - 255
	float lineTransparency;
	
	int pointNumSides;		// number of sides for a single shape
	float pointShapeRatio;	// shape ratio for simple shape
	
	tkFillType fillType;
	tkPointSymbolType pointSymbolType;
	tkPointShapeType pointShapeType;
	tkVectorDrawingMode drawingMode;
	tkGradientBounds fillGradientBounds;
	tkGradientType fillGradientType;
	
	VARIANT_BOOL verticesVisible;
	VARIANT_BOOL verticesFillVisible;
	OLE_COLOR verticesColor;
	tkVertexType verticesType;
	int verticesSize;

	IImage* picture;
	unsigned char pointCharcter;
	CString fontName;
	CString tag;
	
	ILinePattern* linePattern;
	bool useLinePattern;

	Gdiplus::Brush* brushPlus;
	Gdiplus::Pen* penPlus;
	Gdiplus::Bitmap* bitmapPlus;
	Gdiplus::ImageAttributes* imgAttributes;
	bool m_needDeleteBitmapPlus;

	CPen* pen;
	CPen* penOld;
	CBrush* brush;
	CBrush* brushOld;
	
	#pragma region Constructor
	// constructor
	CDrawingOptionsEx::CDrawingOptionsEx(void)
	{
		m_needDeleteBitmapPlus = false;
		//selectionColor = RGB(255, 255, 0);
		//selectionTransparency = 180;
				
		drawingMode = vdmGDIMixed;

		visible = true;
		fillVisible = true;
		linesVisible = true;
		fillBgTransparent = true;
		
		fillColor = RGB(255,0,0);
		fillColor2 = RGB(200,200,200);
		lineColor = RGB(0,0,0);
		fillBgColor = RGB(0,120,0);
		pointColor = RGB(0,0,0);

		lineWidth = 1.0;
		pointSize = 8.0;
		fillTransparency = 255;
		lineTransparency = 255;
		rotation = 0.0;
		fillGradientRotation = 0.0;

		lineStipple = dsSolid;
		fillHatchStyle = hsHorizontal;
		pointSymbolType = ptSymbolStandard;
		fillGradientType = gtLinear;
		fillType = ftStandard;
		pointShapeType = ptShapeRegular;
		fillGradientBounds = gbPerShape;

		picture = NULL;
		pointCharcter = 'a';

		scaleX = 1.0;
		scaleY = 1.0;

		pointNumSides = 4;
		pointShapeRatio = 0.5;

		fillGradientRotation = 0.0;
		
		fontName = "Times New Roman";
		tag = "";
		
		verticesVisible = VARIANT_FALSE;
		verticesFillVisible = VARIANT_TRUE;
		verticesColor = RGB(0, 0, 255);
		verticesType = vtSquare;
		verticesSize = 6;

		linePattern = NULL;
		useLinePattern = false;

		penPlus = NULL;
		brushPlus = NULL;
		bitmapPlus = NULL;
		imgAttributes = NULL;

		pen = new CPen();
		brush = new CBrush();
		penOld = NULL;
		brushOld = NULL;
	}
	
	// Constructor 2
	CDrawingOptionsEx::CDrawingOptionsEx(const CDrawingOptionsEx& opt)
	{
		this->picture = NULL;
		this->linePattern = NULL;

		*this = opt;
		picture = NULL;
	}
	#pragma endregion
	
	#pragma region Destructor
	CDrawingOptionsEx::~CDrawingOptionsEx()
	{
		if ( picture )	picture->Release();
		if ( linePattern )	linePattern->Release();
		
		this->ReleaseGdiPlusBrush();
		//this->ReleaseGDIPlusPen();	// TODO: exception here ?!

		if (pen) 	delete pen;
		if (brush)	delete brush;

		if ( brushPlus )	delete brushPlus;
		if ( penPlus )		delete penPlus;
		
		if ( imgAttributes) delete imgAttributes;

		this->ReleaseGdiPlusBitmap();
		
	}
	#pragma endregion
	
	#pragma region Functions

	bool CanUseLinePattern();
	void DrawGraphicPath(Gdiplus::Graphics* graphics, Gdiplus::GraphicsPath* path);
	void FillGraphicsPath(Gdiplus::Graphics* graphics, Gdiplus::GraphicsPath* path, Gdiplus::RectF& bounds);

	void InitGdiPlusBrushAndPen(Gdiplus::RectF* bounds = NULL);		// bounds for gradient
	void InitGdiPlusBrush(Gdiplus::RectF* bounds = NULL);
	void InitGdiPlusPen();
	void InitGdiPlusPicture();

	void ReleaseGdiPlusBrushAndPen();
	void ReleaseGdiPlusBrush();
	void ReleaseGdiPlusPen();
	void ReleaseGdiPlusBitmap();

	// GDI drawing
	void InitGdiBrushAndPen(CDC* dc);
	void InitGdiBrushAndPen(CDC* dc, bool selection, OLE_COLOR selectionColor);
	void InitGdiBrush();
	void InitGdiPen();
	void InitGdiBrush(bool selection, OLE_COLOR selectionColor);
	void InitGdiPen(bool selection, OLE_COLOR selectionColor);
	void InitGdiVerticesPen(CDC* dc);
	
	void ReleaseGdiBrushAndPen(CDC* dc);
	void ReleaseGdiVerticesPen();

	// utilities
	Gdiplus::Bitmap* ImageToGdiPlusBitmap(IImage* img);
	CPoint* CDrawingOptionsEx::GetVertex();
	void DrawPointSymbol(Gdiplus::Graphics& g, CDC* dc, Gdiplus::Point* points, float* angles, int count);
	int LineStippleToGDIPenStyle(tkDashStyle lineStipple);
	Gdiplus::GraphicsPath* get_FontCharacterPath(CDC* dc, bool previewDrawing);
	#pragma endregion
};
