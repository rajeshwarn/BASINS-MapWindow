// The old procedures are stored in this file. To access them Map.ShapeDrawingMethod should be set
// to dmStandard or dmNewWithSelection

#pragma once
#include "stdafx.h"
#include "MapWinGis.h"
#include "Map.h"

#include "xtiffio.h"  /* for TIFF */
#include "geotiffio.h" /* for GeoTIFF */
#include "tiff.h"
#include "geotiff.h"
#include "geo_normalize.h"
#include "geovalues.h"
#include "tiffio.h"
#include "tiffiop.h"
#include <fstream>
#include <vector>
#include <atlsafe.h>
#include "IndexSearching.h"

#include "MapPpg.h"
#include "Enumerations.h"

#include "LabelCategory.h"
#include "Labels.h"
#include "Image.h"

#include "ShapefileDrawing.h"
#include "ImageDrawing.h"
#include "LabelDrawing.h"
#include "ChartDrawing.h"

//#include "UtilityFunctions.h"
#include "Projections.h"

using namespace std;

// *************************************************************
//		DrawShapePoint()
// *************************************************************
// there are problems when we use DRAW_POINT macro from DrawShapefileAlt function.
// so I changed to inline function. There must not be decrease in speed if I understand it right,
// and it's nearly impossible to debug such a big macro.
void CMapView::DrawShapePoint(CDC* dc,ShapeLayerInfo* sli,ShapeInfo* si,int pixX, int pixY, int hsize)
{
	if( si->pointType == ptSquare )
	{	
		if( si->pointSize <= 1 )
		{
			dc->SetPixelV(pixX,pixY,si->pointClr);
		}
		else
		{
			dc->FillSolidRect(pixX-hsize,pixY-hsize, (int)si->pointSize, (int)si->pointSize,si->pointClr);
		}
	}
	else if( si->pointType == ptCircle )
	{
		if( si->pointSize <= 1 )
		{
			dc->SetPixel(pixX,pixY,si->pointClr);
		}
		else
		{
			dc->Ellipse(pixX-hsize,pixY-hsize,pixX+hsize,pixY+hsize);
		}
	}
	else if( si->pointType == ptDiamond )
	{	
		CPoint pnts[4];
		pnts[0]=CPoint(pixX,pixY-hsize);
		pnts[1]=CPoint(pixX-hsize,pixY);
		pnts[2]=CPoint(pixX,pixY+hsize);
		pnts[3]=CPoint(pixX+hsize,pixY);
		dc->Polygon(pnts,4);
	}
	else if( si->pointType == ptTriangleUp )
	{	
		CPoint pnts[3];
		pnts[0]=CPoint(pixX,pixY-hsize);
		pnts[1]=CPoint(pixX-hsize,pixY+hsize);
		pnts[2]=CPoint(pixX+hsize,pixY+hsize);
		dc->Polygon(pnts,3);
	}
	else if( si->pointType == ptTriangleDown )
	{	
		CPoint pnts[3];
		pnts[0]=CPoint(pixX,pixY+hsize);
		pnts[1]=CPoint(pixX+hsize,pixY-hsize);
		pnts[2]=CPoint(pixX-hsize,pixY-hsize);
		dc->Polygon(pnts,3);
	}
	else if( si->pointType == ptTriangleLeft )
	{	CPoint pnts[3];
		pnts[0]=CPoint(pixX-hsize,pixY);
		pnts[1]=CPoint(pixX+hsize,pixY+hsize);
		pnts[2]=CPoint(pixX+hsize,pixY-hsize);
		dc->Polygon(pnts,3);
	}
	else if( si->pointType == ptTriangleRight )
	{	CPoint pnts[3];
		pnts[0]=CPoint(pixX+hsize,pixY);
		pnts[1]=CPoint(pixX-hsize,pixY-hsize);
		pnts[2]=CPoint(pixX-hsize,pixY+hsize);
		dc->Polygon(pnts,3);
	}
	else if( si->pointType == ptUserDefined )
	{
		long width = sli->udPointTypeWidth;
		long height = sli->udPointTypeHeight;
		
		if( width<=0 || height<=0 || sli->udPointType==NULL)
		{
			dc->SetPixelV(pixX,pixY,si->pointClr);
		}
		else
		{	
			double left=pixX-width*.5;
			double right=pixX+width*.5;
			double top=pixY+height*.5;
			double bottom=pixY-height*.5;
			
			if( left> m_viewWidth || right < 0 || bottom > m_viewHeight || top < 0 )
				return; //continue;
			
			if( si->pointSize != 1.0 )
			{
				if(si->pointClr == sli->pointClr)
				{
					CDC * stretchDC = new CDC();
					stretchDC->CreateCompatibleDC(dc);
					CBitmap * bmp = new CBitmap();
					CBitmap * oldbmp = NULL;
					long stretchWidth = (long)(sli->udPointTypeWidth * si->pointSize);
					long stretchHeight= (long)(sli->udPointTypeHeight * si->pointSize);
					bmp->CreateDiscardableBitmap(dc,stretchWidth,stretchHeight);
					oldbmp = stretchDC->SelectObject(bmp);
					stretchDC->StretchBlt(0,0,stretchWidth,stretchHeight,sli->udDC,0,0,width,height,SRCCOPY);
					TransparentBlt(dc->m_hDC, pixX - (int)(stretchWidth * 0.5), pixY-(int)(stretchHeight * 0.5), stretchWidth, stretchHeight, 
								  stretchDC->m_hDC, 0, 0, stretchWidth, stretchHeight, sli->udTransColor);
					stretchDC->SelectObject(oldbmp);
					bmp->DeleteObject();
					stretchDC->DeleteDC();
					delete stretchDC;
					delete bmp;
				}
				else
				{
					long r, g, b;
					CDC * stretchDC = new CDC();
					stretchDC->CreateCompatibleDC(dc);
					CBitmap * bmp = new CBitmap();
					CBitmap * oldbmp = NULL;
					long stretchWidth = (long)(sli->udPointTypeWidth * si->pointSize);
					long stretchHeight = (long)(sli->udPointTypeHeight * si->pointSize);
					COLORREF color;
					COLORREF transparent_color = (COLORREF)sli->udTransColor;
					bmp->CreateDiscardableBitmap(dc,stretchWidth,stretchHeight);
					oldbmp = stretchDC->SelectObject(bmp);
					for(int i = 0; i < height; i++)
					{
						for(int j = 0; j < width; j++)
						{
							color = sli->udDC->GetPixel(j,i);
							color = RGB(255-GetRValue(color),255-GetGValue(color),255-GetBValue(color));
							stretchDC->SetPixel(j,i,color);
						}
					}
					r = 255-GetRValue(transparent_color);
					g = 255-GetGValue(transparent_color);
					b = 255-GetBValue(transparent_color);
					stretchDC->StretchBlt(0,0,stretchWidth,stretchHeight,stretchDC,0,0,width,height,SRCCOPY);
					TransparentBlt(dc->m_hDC, pixX- int(stretchWidth * 0.5),pixY - int(stretchHeight * 0.5), stretchWidth, stretchHeight,
								   stretchDC->m_hDC, 0, 0, stretchWidth, stretchHeight, RGB(r,g,b));
					stretchDC->SelectObject(oldbmp);
					bmp->DeleteObject();
					stretchDC->DeleteDC();
					delete stretchDC;
					delete bmp;
				}
			}
			else
			{
				if(si->pointClr == sli->pointClr)
				{
					TransparentBlt(dc->m_hDC, pixX - int(width * 0.5),pixY - int(height * 0.5), width, height, sli->udDC->m_hDC, 0, 0, width, height, sli->udTransColor);
				}
				else
				{
					long r, g, b;
					CDC * selectedDC = new CDC();
					selectedDC->CreateCompatibleDC(dc);
					CBitmap * bmp = new CBitmap();
					CBitmap * oldbmp = NULL;
					COLORREF color;
					COLORREF transparent_color = (COLORREF)sli->udTransColor;
					bmp->CreateDiscardableBitmap(dc,width,height);
					oldbmp = selectedDC->SelectObject(bmp);
					for(int i = 0; i < height; i++)
					{
						for(int j = 0; j < width; j++)
						{
							color = sli->udDC->GetPixel(j,i);
							color = RGB(255-GetRValue(color),255-GetGValue(color),255-GetBValue(color));
							selectedDC->SetPixel(j,i,color);
						}
					}
					r = 255-GetRValue(transparent_color);
					g = 255-GetGValue(transparent_color);
					b = 255-GetBValue(transparent_color);
					TransparentBlt(dc->m_hDC, pixX - int(width * 0.5),pixY- int(height * 0.5), width, height,
								   selectedDC->m_hDC, 0, 0, width, height, RGB(r,g,b));
					selectedDC->SelectObject(oldbmp);
					bmp->DeleteObject();
					selectedDC->DeleteDC();
					delete selectedDC;
					delete bmp;
				}
			}
		}
	}
	else if( si->pointType == ptImageList )
	{
		if( si->ImageListIndex == -1)
			dc->SetPixelV(pixX,pixY,si->pointClr);
		else
		{	
			udPointListItem * iconItem = sli->PointImageList[si->ImageListIndex];
			long width = iconItem->udPointTypeWidth;
			long height = iconItem->udPointTypeHeight;
			if( width<=0 || height<=0 || iconItem->udPointType==NULL)
				dc->SetPixelV(pixX,pixY,si->pointClr);
			else
			{	
				double left=pixX-width*.5;
				double right=pixX+width*.5;
				double top=pixY+height*.5;
				double bottom=pixY-height*.5;
				if( left> m_viewWidth || right < 0 || bottom > m_viewHeight || top < 0 )
					return; //continue;
				
				if( si->pointSize != 1.0 )
				{
					if(si->pointClr == sli->pointClr)
					{
						CDC * stretchDC = new CDC();
						stretchDC->CreateCompatibleDC(dc);
						CBitmap * bmp = new CBitmap();
						CBitmap * oldbmp = NULL;
						long stretchWidth = long(iconItem->udPointTypeWidth * si->pointSize);
						long stretchHeight = long(iconItem->udPointTypeHeight * si->pointSize);
						bmp->CreateDiscardableBitmap(dc,stretchWidth,stretchHeight);
						oldbmp = stretchDC->SelectObject(bmp);
						stretchDC->StretchBlt(0,0,stretchWidth,stretchHeight,iconItem->udDC,0,0,width,height,SRCCOPY);
						TransparentBlt(dc->m_hDC, pixX - int(stretchWidth * 0.5), pixY - int(stretchHeight * 0.5), stretchWidth, stretchHeight, 
									   stretchDC->m_hDC, 0, 0, stretchWidth, stretchHeight, iconItem->udTransColor);
						stretchDC->SelectObject(oldbmp);
						bmp->DeleteObject();
						stretchDC->DeleteDC();
						delete stretchDC;
						delete bmp;
					}else
					{
						long r, g, b;
						CDC * stretchDC = new CDC();
						stretchDC->CreateCompatibleDC(dc);
						CBitmap * bmp = new CBitmap();
						CBitmap * oldbmp = NULL;
						long stretchWidth = long(iconItem->udPointTypeWidth*si->pointSize);
						long stretchHeight = long(iconItem->udPointTypeHeight*si->pointSize);
						COLORREF color;
						COLORREF transparent_color = (COLORREF)iconItem->udTransColor;
						bmp->CreateDiscardableBitmap(dc,stretchWidth,stretchHeight);
						oldbmp = stretchDC->SelectObject(bmp);
						for(int i = 0; i < height; i++)
						{
							for(int j = 0; j < width; j++)
							{
								color = iconItem->udDC->GetPixel(j,i);
								color = RGB(255-GetRValue(color),255-GetGValue(color),255-GetBValue(color));
								stretchDC->SetPixel(j,i,color);
							}
						}
						r = 255-GetRValue(transparent_color);
						g = 255-GetGValue(transparent_color);
						b = 255-GetBValue(transparent_color);
						stretchDC->StretchBlt(0,0,stretchWidth,stretchHeight,stretchDC,0,0,width,height,SRCCOPY);
						TransparentBlt(dc->m_hDC, pixX - int(stretchWidth * 0.5), pixY - int(stretchHeight * 0.5), stretchWidth, stretchHeight,
									   stretchDC->m_hDC, 0, 0, stretchWidth, stretchHeight, RGB(r,g,b));
						stretchDC->SelectObject(oldbmp);
						bmp->DeleteObject();
						stretchDC->DeleteDC();
						delete stretchDC;
						delete bmp;
					}
				}
				else
				{
					if(si->pointClr == sli->pointClr)
					{
						TransparentBlt(dc->m_hDC,pixX-int(width * 0.5),pixY-int(height * 0.5),width,height,
									   iconItem->udDC->m_hDC,0,0,width,height,iconItem->udTransColor);
					}
					else
					{
						long r, g, b;
						CDC * selectedDC = new CDC();
						selectedDC->CreateCompatibleDC(dc);
						CBitmap * bmp = new CBitmap();
						CBitmap * oldbmp = NULL;
						COLORREF color;
						COLORREF transparent_color = (COLORREF)iconItem->udTransColor;
						bmp->CreateDiscardableBitmap(dc,width,height);
						oldbmp = selectedDC->SelectObject(bmp);
						for(int i = 0; i < height; i++)
						{
							for(int j = 0; j < width; j++)
							{
								color = iconItem->udDC->GetPixel(j,i);
								color = RGB(255-GetRValue(color),255-GetGValue(color),255-GetBValue(color));
								selectedDC->SetPixel(j,i,color);
							}
						}
						r = 255-GetRValue(transparent_color);
						g = 255-GetGValue(transparent_color);
						b = 255-GetBValue(transparent_color);
						TransparentBlt(dc->m_hDC, pixX - int(width * 0.5),pixY - int(height * 0.5), width, height, 
										selectedDC->m_hDC, 0, 0, width, height, RGB(r,g,b));
						selectedDC->SelectObject(oldbmp);
						bmp->DeleteObject();
						selectedDC->DeleteDC();
						delete selectedDC;
						delete bmp;
					}
				}
			}
		}
	}
	else if( si->pointType == ptFontChar )
	{
		if( si->FontCharListIndex == -1)
			dc->SetPixelV(pixX,pixY,si->pointClr);
		else
		{
			CFont * charFont = new CFont();
			CFont * oldFont = NULL;
			COLORREF oldColor;
			char cVal[2];
			LOGFONT lf;
			RECT myRect;
			CSize mySize;
			udPointListFontCharItem *fontCharItem = sli->PointFontCharList[si->FontCharListIndex];
			cVal[0] = (char)fontCharItem->udPointFontCharIdx;
			cVal[1] = '\0';
			sli->GetFont()->GetLogFont(&lf);
			lf.lfHeight = (LONG)(sli->fontSize * 10);
			charFont->CreatePointFontIndirect(&lf, dc);
			oldFont = dc->SelectObject(charFont);
			oldColor = dc->SetTextColor(fontCharItem->udPointFontCharColor);
			mySize = dc->GetTextExtent(cVal, 1);
			myRect.top = pixY - (mySize.cy / 2);
			myRect.bottom =myRect.top + mySize.cy;
			myRect.left = pixX - (mySize.cx / 2);
			myRect.right = myRect.left  + mySize.cx;
			dc->DrawTextEx(cVal, 1, &myRect, DT_CENTER | DT_VCENTER | DT_SINGLELINE , NULL);
			dc->SelectObject(oldFont);
			dc->SetTextColor(oldColor);
			charFont->DeleteObject();
		}
	}
}

// ************************************************************
//		DrawFontChar()
// ************************************************************
void CMapView::DrawFontChar(CDC * dc, ShapeLayerInfo * sli, ShapeInfo * si, int pixX, int pixY, double hsize)
{

	if( si->FontCharListIndex == -1)
		dc->SetPixelV(pixX,pixY,si->pointClr);
	else
	{
		CFont * charFont = new CFont();
		CFont * oldFont = NULL;
		COLORREF oldColor;
		char cVal[2];
		LOGFONT lf;
		RECT myRect;
		CSize mySize;
		udPointListFontCharItem *fontCharItem = sli->PointFontCharList[si->FontCharListIndex];
		cVal[0] = (char)fontCharItem->udPointFontCharIdx;
		cVal[1] = '\0';
		sli->GetFont()->GetLogFont(&lf);
		lf.lfHeight = (long)(sli->fontSize * 10.0);
		charFont->CreatePointFontIndirect(&lf, dc);
		oldFont = dc->SelectObject(charFont);
		oldColor = dc->SetTextColor(fontCharItem->udPointFontCharColor);
		mySize = dc->GetTextExtent(cVal, 1);
		myRect.top = pixY - (mySize.cy / 2);
		myRect.bottom =myRect.top + mySize.cy;
		myRect.left = pixX - (mySize.cx / 2);
		myRect.right = myRect.left  + mySize.cx;
		dc->DrawTextEx(cVal, 1, &myRect, DT_CENTER | DT_VCENTER | DT_SINGLELINE , NULL);
		dc->SelectObject(oldFont);
		dc->SetTextColor(oldColor);
		charFont->DeleteObject();
	}
}

/***********************************************************************/
/*							CreateCustomBrush()				           */
/***********************************************************************/

/*  Makes brush for drawing polygon fill, stipples and bit patterns
 */
void CMapView::CreateCustomBrush(CDC* dc, ShapeInfo* si, CBrush* newBrush)
{
	// creation of pattern brushes in separate function
	if (si->fillStipple == fsRaster || si->fillStipple == fsNone)
	{
		newBrush->CreateSolidBrush(si->fillClr);
	}
	else if( si->fillStipple != fsNone)
	{
		int hatchStyle;
		DECODE_FILL_STIPPLE(si->fillStipple, hatchStyle);
		
		/* lsu: we use fill color for stipple when there is no background		 */
		/* fill color supports color schemes and it's more interesting option   */
		if (si->transparentStipple)
			newBrush->CreateHatchBrush(hatchStyle, si->fillClr);
		else
			newBrush->CreateHatchBrush(hatchStyle, si->stippleLineClr);
	}
}

/***********************************************************************/
/*							CreateCustomPen()				           */
/***********************************************************************/

/*  Makes pen, defined by line drawing settings of the layer
 */
inline  void CMapView::CreateCustomPen(ShapeInfo* si,long udLineStipple, CPen* newPen)
{
	if (si->lineWidth == 0 || !(si->flags & slfDrawLine))
	{
		newPen->CreatePen(PS_NULL, (int)si->lineWidth,si->lineClr);
	}
	else
	{
		if( si->lineStipple == lsNone )
		{
			newPen->CreatePen(PS_SOLID, (int)si->lineWidth,si->lineClr);
		}
		else if (si->lineStipple == lsCustom)
		{
			int num;
			LOGBRUSH logBrush;
			logBrush.lbColor = si->lineClr;
			logBrush.lbStyle = BS_SOLID;
			DWORD * dashp = Utility::cvtUCharToDword(udLineStipple, num);
			newPen->CreatePen(PS_GEOMETRIC|PS_USERSTYLE, (int)si->lineWidth, &logBrush, num, dashp);
		}
		else
		{
			int penStyle;
			DECODE_LINE_STIPPLE(si->lineStipple, penStyle);
			if (si->lineWidth >1)
			{
				LOGBRUSH logBrush;
				logBrush.lbColor = si->lineClr;
				logBrush.lbStyle = BS_SOLID;
				newPen->CreatePen(PS_GEOMETRIC|penStyle, (int)si->lineWidth, &logBrush, 0, 0);
			}
			else
			{
				newPen->CreatePen(penStyle, (int)si->lineWidth,si->lineClr);
			}
		}
	}
}

/***********************************************************************/
/*						Image2PatternBrush()				           */
/***********************************************************************/
/*  Sets an pattern fill to a previously declared brush. Returns true on 
 *  success, and false otherwise.
 */
bool CMapView::Image2PatternBrush(CDC* dc, IImage* rasterFill, CBrush* brushRaster)
{
	long width, height;	
	rasterFill->get_Height(&height);
	rasterFill->get_Width(&width);
	if (width == 0 || height == 0) return false;
	CBitmap bmp;
	bmp.CreateCompatibleBitmap(dc, width, height);
	
	BITMAPINFO bif;
	BITMAPINFOHEADER bih;
	bih.biBitCount=24;
	bih.biWidth= width;
	bih.biHeight= height;
	bih.biPlanes=1;
	bih.biSize=sizeof(BITMAPINFOHEADER);
	bih.biCompression=0;
	bih.biXPelsPerMeter=0;
	bih.biYPelsPerMeter=0;
	bih.biClrUsed=0;
	bih.biClrImportant=0;
	bih.biSizeImage=width * height * 3;
	bif.bmiHeader = bih;

	unsigned char* bits = new unsigned char[width * height * 3];
	long value;
	for(int j = 0; j < height; j++ )
	{	
		for(int i = 0; i < width; i++ )
		{	
			rasterFill->get_Value(height - j - 1, i, &value); 
			bits[j*width*3+i*3] = GetBValue(value);
			bits[j*width*3+i*3+1] = GetGValue(value);
			bits[j*width*3+i*3+2] = GetRValue(value);
		}
	}
	int res;
	res = SetDIBits(dc->GetSafeHdc(), (HBITMAP)bmp.GetSafeHandle(), 0, height, bits, &bif, DIB_RGB_COLORS);
	
	brushRaster->CreatePatternBrush(&bmp);
	delete [] bits;
	//12-Oct-2009 Rob Cairns
	bmp.DeleteObject();

	return true;

	/* -----------------------------------------------------------------*/
	/* An effort to use Image.GetImageBitsDC method; it must work		*/
	/* faster for in memory bitmaps. But I was unable to make it work	*/
	/* -----------------------------------------------------------------*/
	//CDC dcTemp;
	//dcTemp.CreateCompatibleDC(dc);
	//CBitmap* bmp;
	//bmp->CreateCompatibleBitmap(&dcTemp, width, height);
	//dcTemp.SelectObject(bmp);
	//rasterFill->GetImageBitsDC((long)dcTemp.GetSafeHdc(), &vbretval);
	//bmp = dcTemp.GetCurrentBitmap();
	//bool b;
	//CBrush brush;
	//b = brush.CreatePatternBrush(bmp);
	//oldBrush = dc->SelectObject(&brush);
	/* -----------------------------------------------------------------*/
}

// **************************************************************
//		get_LineStipplePen()
// **************************************************************
void CMapView::get_LineStipplePen(ShapeLayerInfo* sli, ShapeInfo* si, CPen* newPen)
{
	if( si->lineStipple != lsNone )
	{	
		switch( si->lineStipple )
		{
			case( lsDotted ):
			{
				newPen->CreatePen(PS_DOT, (int)si->lineWidth,si->lineClr);
				break;
			}
			case( lsDashed ):
			{
				newPen->CreatePen(PS_DASH,1,si->lineClr);
				break;
			}
			case( lsDashDotDash ):
			{
				newPen->CreatePen(PS_DASHDOT,1,si->lineClr);
				break;
			}
			case ( lsCustom ):
			{
				/* Create a custom geometric pen.*/
				createCustomPen(sli, si, newPen);
				break;
			}
			case ( lsDoubleSolid ):
			{
				newPen->CreatePen(PS_SOLID, (int)si->lineWidth,si->fillClr);
				break;
			}
			case ( lsDashDotDot ):
			{
				newPen->CreatePen(PS_DASHDOTDOT, (int)si->lineWidth,si->fillClr);
				break;
			}
			default:
			{
				newPen->CreatePen(PS_SOLID, (int)si->lineWidth,si->fillClr);
				break;
			}
		}
	}
	else
	{ 
		newPen->CreatePen(PS_SOLID, (int)si->lineWidth,si->lineClr);
	}
}

// ************************************************************
//		DrawImage()
// ************************************************************
//void CMapView::DrawImage(const CRect & rcBounds, CDC * dc, Layer * layer)
//{
//	DEVMODE d;
//	EnumDisplaySettings(NULL,ENUM_CURRENT_SETTINGS,&d);
//// 	cout << "DrawImage: ";
////	cout.flush();
// 
//	// tws 04/07/2007: needs to use rcBounds to draw to a back buffer at other than screen res!
//	int screenPixelWidth =  (m_forceBounds) ? rcBounds.Width() : d.dmPelsWidth; //rcBounds.Width();
//	int screenPixelHeight = (m_forceBounds) ? rcBounds.Height() : d.dmPelsHeight; //rcBounds.Height();

//	if( layer->object == NULL )
//		return;
//	if( !(layer->flags & Visible ) )
//		return;

//	IImage * iimg = NULL;
//	layer->object->QueryInterface(IID_IImage,(void**)&iimg);
//	if( iimg == NULL )
//		return;

//	long imgWidth, imgHeight;
//	iimg->get_Width(&imgWidth);
//	iimg->get_Height(&imgHeight);

//	if( imgWidth <= 0 || imgHeight <= 0 )
//	{	iimg->Release();
//		iimg = NULL;
//		return;
//	}

//	double dx = 0, dy = 0;
//	iimg->get_dX( &dx );
//	iimg->get_dY( &dy );
//	if( dx <= 0.0 || dy <= 0.0 )
//	{	iimg->Release();
//		iimg = NULL;
//		return;
//	}

//	//Check if it is in the VIEW
//	double xllCorner, yllCorner, xtrCorner, ytrCorner;
//	iimg->get_XllCenter( &xllCorner );
//	iimg->get_YllCenter( &yllCorner );
//	xllCorner -= dx*.5;
//	yllCorner -= dy*.5;
//	xtrCorner = xllCorner+imgWidth*dx;
//	ytrCorner = yllCorner+imgHeight*dy;
//	if( OUT_OF_VIEW(xllCorner,yllCorner,xtrCorner,ytrCorner,extents) )
//	{	iimg->Release();
//		iimg = NULL;
//		return;
//	}

//	ImageLayerInfo * ili = (ImageLayerInfo*)layer->addInfo;

//	//Test for Memory and Bitmap Type
//	VARIANT_BOOL inram;
//	iimg->get_IsInRam(&inram);
//	ImageType type;
//	iimg->get_ImageType(&type);

//	bool create_low_res_file = false;
//	FILE * imgfile = NULL;
//	long fileHandle = -1;
//	iimg->get_FileHandle(&fileHandle);

//	//Multiple Res
//	//Flag indicating if the file should be
//		//the original or a low-resolution file
//	double imageSX, imageSY;
//	double imagelx,imagely,imagehx,imagehy;
//	PROJECTION_TO_PIXEL(xllCorner,yllCorner,imagelx,imagely);
//	PROJECTION_TO_PIXEL(xtrCorner,ytrCorner,imagehx,imagehy);
//	imageSX = imagehx - imagelx;
//	imageSY = imagehy - imagely;
//	bool use_low_resolution = false;

//	double onePixX, onePixY;
//	PixelToProjection(1,1,onePixX,onePixY);
//	double screenDX = onePixX - extents.left;   // Size (in map units) of one pixel in the x direction
//	double screenDY = onePixY - extents.bottom; // Size (in map units) of one pixel in the y direction
//	double sDX = (xtrCorner - xllCorner)/screenPixelWidth;   //used to determine if we want high/low res image
//	double sDY = (ytrCorner - yllCorner)/screenPixelHeight;

//	if( sDX < screenDX && sDY < screenDY )
//	{	
//		use_low_resolution = true;
//		//AfxMessageBox("Use Low Res Image");
//	}
//	else
//	{	//AfxMessageBox("Use Hi Res Image");
//	}
//	//Multiple Res

//	// tws 04/07/2007
//	if (m_forceBounds)
//		use_low_resolution = false;
//	// tws

//	if( fileHandle >= 0 )
//	{
//# ifdef _DEBUG
//	_CrtSetReportMode( _CRT_ASSERT, _CRTDBG_MODE_DEBUG );
//# endif
//		USES_CONVERSION;
//		if( imgfile == NULL )
//		{
//			//Multiple Res
//			//Switch to the other file if possible
//			if( use_low_resolution && ili->low_res_file != "" )
//			{	
//				imgfile = ::fopen(ili->low_res_file,"rb");
//				if( imgfile == NULL )
//				{
//					ili->low_res_file = "";
//					BSTR fname;
//					iimg->get_Filename(&fname);
//					imgfile = ::fopen(OLE2CA(fname),"rb");
//					::SysFreeString(fname);
//				}
//				else
//				{	// Set up screen widths as image to be drawn
//					imgWidth = screenPixelWidth;
//					imgHeight = screenPixelHeight;
//					dx = (xtrCorner - xllCorner)/screenPixelWidth;
//					dy = (ytrCorner - yllCorner)/screenPixelHeight;
//				}
//			}
//			else
//			{
//				BSTR fname;
//				iimg->get_Filename(&fname);
//				imgfile = ::fopen(OLE2CA(fname),"rb");
//				::SysFreeString(fname);
//			}
//			//Multiple Res
//		}
//# ifdef _DEBUG
//	_CrtSetReportMode( _CRT_ASSERT, _CRTDBG_MODE_WNDW );
//# endif
//	}

//	VARIANT_BOOL useTransparencyColor;
//	iimg->get_UseTransparencyColor(&useTransparencyColor);
//	OLE_COLOR imageTransparencyColor;
//	iimg->get_TransparencyColor( &imageTransparencyColor );
//	ili->use_transparency = useTransparencyColor;

//	long blockSize = 512;	//blockSize = 1024 if create_low_res_file == true
//							//	there is better performance the first time read @ 1024
//	CDC * subsetDC = new CDC();
//	CBitmap * bmp = new CBitmap();
//	CBitmap * oldBMP = NULL;

//// ----------------------------------------------------------------- //
////		Disk-based bitmap (BMP)										 //
//// ----------------------------------------------------------------- //
//	if( !inram && type == BITMAP_FILE && imgfile != NULL )
//	{
//		int	pad;
//		BITMAPFILEHEADER bmfh;
//		BITMAPINFOHEADER bmif;
//		RGBQUAD * bmiColors = NULL;

//		//read in BITMAPFILEHEADER
//		fread(&bmfh,sizeof(BITMAPFILEHEADER),1,imgfile);
//		//if not a valid bitmap file, fail to load
//		if (bmfh.bfType != 19778)
//		{	m_lastErrorCode = tkINVALID_FILE;
//			if( m_globalCallback != NULL )
//				m_globalCallback->Error(m_key.AllocSysString(),A2BSTR(ErrorMsg(m_lastErrorCode)));

//			iimg->Release();
//			iimg = NULL;
//			//subsetDC->SelectObject(oldBMP);
//			//bmp->DeleteObject();
//			//subsetDC->DeleteDC();
//			return;
//		}
//		//read in BITMAPINFOHEADER
//		fread(&bmif,sizeof(BITMAPINFOHEADER),1,imgfile);
//		bmif.biClrUsed = ( bmfh.bfOffBits - 54 )/4;
//		if( bmif.biClrUsed != 0 )
//		{	bmiColors = new RGBQUAD[bmif.biClrUsed];
//			fread( bmiColors, sizeof(RGBQUAD), bmif.biClrUsed, imgfile );
//		}
//		//Read to the beginning of the data
//		int sizeof_header = 54;
//		int n = bmfh.bfOffBits - (54 + bmif.biClrUsed*4);
//		if( n > 0 )
//			fseek(imgfile,n,SEEK_CUR);

//		//Compute the pad
//		pad = bmif.biWidth*bmif.biBitCount;
//		pad %= 32;
//		if(pad != 0)
//		{	pad = 32 - pad;
//			pad /= 8;
//		}

//		//Read the specific bitcount type
//		//	and Create a Palette
//		long rowLength = imgWidth;
//		if( bmif.biBitCount == 24 )
//			rowLength*=3;
//		else if( bmif.biBitCount == 8 )
//		{}
//		else if( bmif.biBitCount == 4 )
//		{	rowLength*=.5;
//			if( imgWidth % 2 )
//				rowLength++;
//		}
//		else if( bmif.biBitCount == 1 )
//			rowLength = ceil( ((double)imgWidth)/8 );

//		//Multiple Res
//		//Create a low resolution image if the screen size < .75*(image size)
//		CDC * low_res_dc = NULL;
//		CBitmap * low_res_bmp = NULL;
//		CBitmap * oldbmp = NULL;
//		
//		// works, but a little iffy
//		//if( ili->low_res_file == "" )
//		//{
//		//	long screenImageSize = screenPixelWidth*screenPixelHeight;
//		//	long imageSize = bmif.biWidth*bmif.biHeight;
//		//	if( imageSize > 3*screenImageSize )
//		//	{	create_low_res_file = true;
//		//		low_res_dc = new CDC();
//		//		low_res_bmp = new CBitmap();
//		//		low_res_dc->CreateCompatibleDC(dc);
//		//		low_res_bmp->CreateDiscardableBitmap(dc,screenPixelWidth,screenPixelHeight);
//		//		oldbmp = low_res_dc->SelectObject(low_res_bmp);
//		//		//AfxMessageBox("Create Low Res File");

//		//		//Change the blocksize ... there is better performance
//		//		blockSize = 1024;
//		//	}
//		//}

//		subsetDC->CreateCompatibleDC(dc);
//		bmp->CreateDiscardableBitmap(dc,imgWidth,blockSize);
//		oldBMP = subsetDC->SelectObject(bmp);

//		//bool read_data = false;
//		double imgRatio = screenPixelHeight/(double)imgHeight;
//		long stretchHeight, yPos;
//		//Multiple Res

//		unsigned char * data = new unsigned char[(rowLength+pad)*blockSize];

//		//Number of rows read
//		long numRead;

//		//World Coordinate of subset
//		double xlc, ylc, xtc, ytc;

//		//Projection Variables
//		double lPx, bPy;
//		double rPx, tPy;
//		PROJECTION_TO_PIXEL(xllCorner,yllCorner,lPx,bPy);

//		//SetDIBits variable
//		BITMAPINFO * bi = new BITMAPINFO;
//				//This adjusts the bi.bmiColors to the right size of array
//		bi = (BITMAPINFO*)realloc(bi,sizeof(BITMAPINFO)+sizeof(RGBQUAD)*bmif.biClrUsed);

//		//Blt adjustment variables
//		long nlPx, ntPy;
//		long nrPx, nbPy;
//		long sPx, sPy;
//		long sPw, sPh;
//		long offset;
//		double edge, hiddenRange;

//		int row;
//		for( row = 0; row < imgHeight; )
//		{
//			numRead = blockSize;
//			if( row + blockSize >= imgHeight )
//				numRead = imgHeight - row;

//			xlc=xllCorner;
//			ylc=yllCorner+row*dy;
//			xtc=xlc+imgWidth*dx;
//			ytc=ylc+numRead*dy;

//			PROJECTION_TO_PIXEL(xtc,ytc,rPx,tPy);

//			//Multiple Res
//			//read_data = false;
//			fread(data,sizeof(unsigned char),(rowLength+pad)*numRead, imgfile);
//			bi->bmiHeader = bmif;
//			bi->bmiHeader.biHeight = numRead;
//			bi->bmiHeader.biSizeImage = (rowLength+pad)*numRead;
//			memcpy(bi->bmiColors,bmiColors,sizeof(RGBQUAD)*bmif.biClrUsed);

//			if( create_low_res_file )
//			{          
//				stretchHeight = numRead*imgRatio + 1;
//				yPos = screenPixelHeight - stretchHeight - row*imgRatio-1;
//				low_res_dc->SetStretchBltMode(COLORONCOLOR);

//				// Chris M Aug 2006 -- Do not copy directly from screen;
//				// often causes problems. Copy right from the data source.
//				//::StretchBlt(low_res_dc->m_hDC,0,yPos,screenPixelWidth,stretchHeight,subsetDC->m_hDC,0,0,imgWidth,numRead,SRCCOPY);
//				StretchDIBits(low_res_dc->m_hDC, 0, yPos, screenPixelWidth, stretchHeight, 0, 0, imgWidth, numRead, data, bi,DIB_RGB_COLORS, SRCCOPY); 
//			}
//			//Multiple Res

//			if( !OUT_OF_VIEW(xlc,ylc,xtc,ytc,extents) )
//			{
//				SetDIBitsToDevice(subsetDC->m_hDC,0,0,imgWidth,numRead,0,0,0,numRead,data,bi,DIB_RGB_COLORS);

//				//Determine the needed area from subsetDC
//				nlPx = lPx; ntPy = tPy;
//				nrPx = rPx; nbPy = bPy;
//				sPx = 0; sPy = 0;
//				sPw = imgWidth; sPh = numRead;

//				double fraction = 0.0;
//				double imgCellsHidden;

//				if( nlPx < -1 )//adjust the left side
//				{
//					hiddenRange = extents.left - xllCorner; // in world coords
//					imgCellsHidden = hiddenRange/dx;//in img cells
//					fraction = imgCellsHidden - (int)imgCellsHidden; //fraction of cell off screen (in img cells)
//					offset = fraction * dx * m_pixelPerProjectionX; //screen pixels needed off screen
//					nlPx = rcBounds.left - offset;	//adjusted drawing area
//					sPx = (int)imgCellsHidden;//new img starting index
//					sPw -= (int)imgCellsHidden; //chop off hidden cells from width
//				}
//				if( ntPy < -1 )//adjust the top
//				{
//					hiddenRange = ytc - extents.top; // in world coords
//					imgCellsHidden = hiddenRange/dy;//in img cells
//					fraction = imgCellsHidden - (int)imgCellsHidden; //fraction of cell off screen (in img cells)
//					offset = fraction * dy * m_pixelPerProjectionY; //screen pixels needed off screen
//					ntPy = rcBounds.top - offset;	//adjusted drawing area
//					sPy = (int)imgCellsHidden;//new img starting index
//					sPh -= (int)imgCellsHidden; //chop off hidden cells from width

//				}
//				if( nrPx > m_viewWidth )//adjust the right side
//				{
//					hiddenRange = xtc - extents.right;//in world coords
//					imgCellsHidden = hiddenRange /dx;//in img cells
//					fraction = imgCellsHidden - (int)imgCellsHidden;//fraction of cell off screen
//					offset = fraction * dx * m_pixelPerProjectionX;//screen pixels needed off screen
//					nrPx = rcBounds.right + offset; //adjusted drawing area
//					sPw -=(int)imgCellsHidden;
//				}
//				if( nbPy > m_viewHeight )//adjust the bottom side
//				{
//					edge = extents.bottom;
//					hiddenRange = edge - ylc;//in world coords
//					imgCellsHidden = hiddenRange /dy;//in img cells
//					fraction = imgCellsHidden - (int)imgCellsHidden;//the fraction of a cell that is hidden
//					offset = fraction * dy * m_pixelPerProjectionY;//screen pixels out of view
//					nbPy = rcBounds.bottom + offset;//adjust the drawing area
//					sPh -= (int)imgCellsHidden;
//				}

//				//Blt the image
//				//Transparent Blt
//				if( useTransparencyColor != FALSE )
//					TransparentBlt(dc->m_hDC,nlPx,ntPy,nrPx-nlPx,nbPy-ntPy,subsetDC->m_hDC,sPx,sPy,sPw,sPh,imageTransparencyColor);
//				//Stretch Blt
//				else
//				{	dc->SetStretchBltMode(COLORONCOLOR);
//					StretchBlt(dc->m_hDC,nlPx,ntPy,nrPx-nlPx,nbPy-ntPy,subsetDC->m_hDC,sPx,sPy,sPw,sPh,SRCCOPY);
//				}
//			}
//			
//			//Reassign the bottom ... +1 to get rid of stretching seams
//			bPy = tPy+1;
//			row += numRead;
//		}

//		delete bi;

//		if( bmiColors )
//			delete [] bmiColors;
//		bmiColors=NULL;
//		if( data )
//			delete [] data;
//		data = NULL;

//		fclose(imgfile);
//		imgfile=NULL;

//		// Optimize
//		//Stop_Timer();
//		//Print_Timer(out,"fread : Disk");
//		//out.close();
//		//-Optimize

//		//Multiple Res
//		if( create_low_res_file )
//		{
//			// Optimize
//			//ofstream out("C:\\Documents and Settings\\BrianS\\Desktop\\Develop\\tkMap GDI\\Test Suite\\profile.txt",ios::app);
//			//Init_Timer();
//			//Start_Timer();
//			//-Optimize

//			//Get a filename
//			char * fname;
//			while( ili->low_res_file == "" )
//			{
//				fname = _tempnam("c:\\tmp","mgdi");
//				if( fname != NULL )
//				{	ili->low_res_file = (TCHAR*)fname + (CString)".bmp";
//					FILE * fileexists = fopen(ili->low_res_file,"r");
//					if( fileexists )
//					{	fclose(fileexists);
//						ili->low_res_file = "";
//					}
//				}
//			}
//			delete [] fname;
//			fname = NULL;

//			IImage * iimg = NULL;;
//			CoCreateInstance(CLSID_Image,NULL,CLSCTX_INPROC_SERVER,IID_IImage,(void**)&iimg);
//			VARIANT_BOOL retval;
//			//The image is created in SetImageBitsDC
//			//iimg->CreateNew(d.dmPelsWidth, d.dmPelsHeight, &retval);

//			//Debugging
//			//double dx = (xtrCorner - xllCorner)/d.dmPelsWidth;
//			//iimg->put_dX(dx);
//			//double dy = (ytrCorner - yllCorner)/d.dmPelsHeight;
//			//iimg->put_dY(dy);
//			//iimg->put_XllCenter(xllCorner);
//			//iimg->put_YllCenter(yllCorner);

//			//SetBitmapBits
//			iimg->SetImageBitsDC((long)low_res_dc->m_hDC,&retval);

//			iimg->Save(ili->low_res_file.AllocSysString(),FALSE,BITMAP_FILE,NULL,&retval);
//			iimg->Release();
//			iimg = NULL;

//			if( retval == FALSE )
//				ili->low_res_file = "";

//			// Optimize
//			//Stop_Timer();
//			//Print_Timer(out,"Create Low-Res File: Disk");
//			//out.close();
//			//-Optimize
//		}

//		if( low_res_dc )
//		{	low_res_dc->SelectObject(oldbmp);
//			low_res_bmp->DeleteObject();
//			delete low_res_bmp;
//			low_res_bmp = NULL;
//			low_res_dc->DeleteDC();
//			delete low_res_dc;
//			low_res_dc = NULL;
//		}
//		//Multiple Res

//		// Optimize
//		//Stop_Timer();
//		//Print_Timer(out,"Draw Image: Disk");
//		//out.close();
//		//-Optimize
//	}


//// ----------------------------------------------------------------- //
////		GDAL-based memory image										 //
//// ----------------------------------------------------------------- //
//	else
//	{
//		//Number of rows read
//		long numRead;

//		//World Coordinate of subset
//		double xlc, ylc, xtc, ytc;

//		//Projection Variables
//		double lPx, bPy;
//		double rPx, tPy;
//		PROJECTION_TO_PIXEL(xllCorner,yllCorner,lPx,bPy);

//		//Blt adjustment variables
//		long nlPx, ntPy;
//		long nrPx, nbPy;
//		long sPx, sPy;
//		long sPw, sPh;
//		long offset;
//		double edge, hiddenRange;

//		//Variables needed for Image
//		//Array of OLE_COLORs
//		//long * rowdata = new long[imgWidth+1];		// lsu: old version
//		//VARIANT_BOOL retval;
//		
//		// lsu: new version
//		unsigned char* pData;
//		CImageClass* img = (CImageClass*)iimg;
//		pData = img->get_ImageData();
//	
//		int pad = imgWidth*24;
//		pad %= 32;
//		if(pad != 0)
//		{	pad = 32 - pad;
//			pad /= 8;
//		}

//		subsetDC->CreateCompatibleDC(dc);
//		// tws 4/10/07: this *can* fail:
//		//bmp->CreateDiscardableBitmap(dc,imgWidth,blockSize);
//		if (!bmp->CreateDiscardableBitmap(dc,imgWidth,blockSize))
//		{
//			//AfxMessageBox("DrawImage() Failed to create bitmap; not enough memory?");
//			if( m_globalCallback != NULL )
//				m_globalCallback->Error(m_key.AllocSysString(),A2BSTR("DrawImage() Failed to create bitmap; not enough memory?"));
//			iimg->Release();
//			iimg = NULL;
//			return;
//		}

//		oldBMP = subsetDC->SelectObject(bmp);

//		register int row;
//		for( row = 0; row < imgHeight; )
//		{
//			numRead = blockSize;
//			if( row + blockSize >= imgHeight )
//				numRead = imgHeight - row;

//			xlc=xllCorner;
//			ylc=yllCorner+row*dy;
//			xtc=xlc+imgWidth*dx;
//			ytc=ylc+numRead*dy;

//			PROJECTION_TO_PIXEL(xtc,ytc,rPx,tPy);

//			if( !OUT_OF_VIEW(xlc,ylc,xtc,ytc,extents) )
//			{
//				// lsu: initial code for retrieving values
//				/*register int j, i;
//				for( j = numRead-1; j>=0; j-- )
//				{	iimg->GetRow( imgHeight - (row + j) - 1, rowdata, &retval );
//					for( i = 0; i < imgWidth; i++ )
//						subsetDC->SetPixelV(i,numRead - j - 1,rowdata[i]);
//				}*/
//			
//				// lsu - 4 oct 2009 - direct reading of data from CImageClass to bitmap
//				BITMAPINFO bif;
//				BITMAPINFOHEADER bih;
//				bih.biBitCount=24;
//				bih.biWidth= imgWidth;
//				bih.biHeight= imgHeight;
//				bih.biPlanes=1;
//				bih.biSize=sizeof(BITMAPINFOHEADER);
//				bih.biCompression=0;
//				bih.biXPelsPerMeter=0;
//				bih.biYPelsPerMeter=0;
//				bih.biClrUsed=0;
//				bih.biClrImportant=0;
//				bih.biSizeImage= (imgWidth * 3 + pad) * imgHeight;
//				bif.bmiHeader = bih;
//				
//				unsigned char* bits = (unsigned char*)(&pData[row * imgWidth * 3]);

//				if (pad == 0)
//				{
//					SetDIBitsToDevice(subsetDC->m_hDC,0,0,imgWidth,numRead,0,0,0,numRead,bits,&bif,DIB_RGB_COLORS);
//				}
//				else	/* would be great to include pad bytes in the ImageData itself
//						(data type should be changed to unsigned char then)
//						while reading it through GDAL; the we can avoid memcopy calls
//						then and decrease redraw time on a 30% more*/
//				{
//					unsigned char* bitsNew = new unsigned char[bih.biSizeImage - 1];
//					int nBytesInRow = imgWidth * 3 + pad;
//					for(int i = 0; i < numRead; i++)
//					{
//						memcpy(&bitsNew[i * nBytesInRow], &bits[i * imgWidth * 3], imgWidth * 3);
//					}
//					SetDIBitsToDevice(subsetDC->m_hDC,0,0,imgWidth,numRead,0,0,0,numRead,bitsNew,&bif,DIB_RGB_COLORS);
//					delete[] bitsNew;
//				}
//				// lsu - 4 oct 2009 - end of direct reading of data from CImageClass to bitmap

//				
//				//Determine the needed area from subsetDC
//				nlPx = lPx, ntPy = tPy;
//				nrPx = rPx, nbPy = bPy;
//				sPx = 0, sPy = 0;
//				sPw = imgWidth, sPh = numRead;

//				double fraction = 0.0;
//				double imgCellsHidden;

//				
//				if( nlPx < -1 )//adjust the left side
//				{
//					hiddenRange = extents.left - xllCorner; // in world coords
//					imgCellsHidden = hiddenRange/dx;//in img cells
//					fraction = imgCellsHidden - (int)imgCellsHidden; //fraction of cell off screen (in img cells)
//					offset = fraction * dx * m_pixelPerProjectionX; //screen pixels needed off screen
//					nlPx = rcBounds.left - offset;	//adjusted drawing area
//					sPx = (int)imgCellsHidden;//new img starting index
//					sPw -= (int)imgCellsHidden; //chop off hidden cells from width
//				}
//				if( ntPy < -1 )//adjust the top
//				{
//					hiddenRange = ytc - extents.top; // in world coords
//					imgCellsHidden = hiddenRange/dy;//in img cells
//					fraction = imgCellsHidden - (int)imgCellsHidden; //fraction of cell off screen (in img cells)
//					offset = fraction * dy * m_pixelPerProjectionY; //screen pixels needed off screen
//					ntPy = rcBounds.top - offset;	//adjusted drawing area
//					sPy = (int)imgCellsHidden;//new img starting index
//					sPh -= (int)imgCellsHidden; //chop off hidden cells from width

//				}
//				if( nrPx > m_viewWidth )//adjust the right side
//				{
//					hiddenRange = xtc - extents.right;//in world coords
//					imgCellsHidden = hiddenRange /dx;//in img cells
//					fraction = imgCellsHidden - (int)imgCellsHidden;//fraction of cell off screen
//					offset = fraction * dx * m_pixelPerProjectionX;//screen pixels needed off screen
//					nrPx = rcBounds.right + offset; //adjusted drawing area
//					sPw -=(int)imgCellsHidden;
//				}
//				if( nbPy > m_viewHeight )//adjust the bottom side
//				{
//					edge = extents.bottom;
//					hiddenRange = edge - ylc;//in world coords
//					imgCellsHidden = hiddenRange /dy;//in img cells
//					fraction = imgCellsHidden - (int)imgCellsHidden;//the fraction of a cell that is hidden
//					offset = fraction * dy * m_pixelPerProjectionY;//screen pixels out of view
//					nbPy = rcBounds.bottom + offset;//adjust the drawing area
//					sPh -= (int)imgCellsHidden;
//				}

//				//Blt the image
//				//Transparent Blt
//				if( useTransparencyColor != FALSE )
//					TransparentBlt(dc->m_hDC,nlPx,ntPy,nrPx-nlPx,nbPy-ntPy,subsetDC->m_hDC,sPx,sPy,sPw,sPh,imageTransparencyColor);
//				//Stretch Blt
//				else
//				{	dc->SetStretchBltMode(COLORONCOLOR);
//					StretchBlt(dc->m_hDC,nlPx,ntPy,nrPx-nlPx,nbPy-ntPy,subsetDC->m_hDC,sPx,sPy,sPw,sPh,SRCCOPY);
//				}
//			}

//			//Reassign the bottom ... +1 to get rid of stretching seams
//			bPy = tPy+1;
//			row += numRead;
//		}

//		//if( rowdata != NULL )		/* lsu: old version */
//		//	delete [] rowdata;
//	}

//	#ifdef _DEBUG
//		AfxMessageBox("mid CMapView::DrawImage");
//	#endif

//	if (fileHandle > 0) _close(fileHandle);
//	subsetDC->SelectObject(oldBMP);
//	bmp->DeleteObject();
//	delete bmp;

//	subsetDC->DeleteDC();
//	delete subsetDC;

//	//if (create_low_res_file)
//	//{
//	//	static bool firstPass = true;
//	//	if (firstPass)
//	//	{
//	//		// Reload the newly created file
//	//		firstPass = false;
//	//		DrawImage(rcBounds, dc, layer);
//	//	}
//	//}

//	//m_labelsToDraw.push(layer);
//	//DrawLabels(dc, layer);
//	#ifdef _DEBUG
//		AfxMessageBox("end CMapView::DrawImage");
//	#endif
//}