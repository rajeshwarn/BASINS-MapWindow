#include "stdafx.h"
#include "TilesDrawer.h"
#include "Tiles.h"
#include <iostream>
#include <fstream>
#include "ImageAttributes.h"

using namespace Gdiplus;

// *********************************************************
//		DrawTiles()
// *********************************************************
void CTilesDrawer::DrawTiles(ITiles* tiles, double pixelsPerMapUnit)
{
	this->DrawTiles(((CTiles*)tiles)->m_tiles, pixelsPerMapUnit);
}

// ***************************************************************
//		DrawTiles()
// ***************************************************************
void CTilesDrawer::DrawTiles(vector<CTile*>& tiles, double pixelsPerMapUnit)
{
	Graphics* g = NULL;
	if (m_graphics)
	{
		g = m_graphics;
	}
	else
	{
		g = Graphics::FromHDC(_dc->m_hDC);
	}

	#ifdef _DEBUG
	//ofstream out("c:\\tiles.txt");
	#endif

	CImageAttributesEx attr(1.0, false, false, 0, 0);
	attr.SetWrapMode(Gdiplus::WrapModeTileFlipXY);

	for (unsigned int i = 0; i < tiles.size();i++)
	{
		CTile* tile = (CTile*)tiles[i];
		if (!tile->m_drawn)
		{
			Gdiplus::Image* bmp = tile->Bitmap->m_bitmap;
			if (bmp)
			{
				// doing reprojection on the first drawing of tile
				if (!tile->m_projectionOk)
				{
					if (m_transfomation)
					{
						if (!tile->UpdateProjection(m_transfomation))
							continue;
					}
					else
					{
						tile->m_xProj = tile->m_xLon;
						tile->m_yProj = tile->m_yLat;
						tile->m_widthProj = tile->m_widthLon;
						tile->m_heightProj = tile->m_heightLat;
					}
				}
				
				double x, y;
				this->ProjectionToPixel(tile->m_xProj, tile->m_yProj, x, y);

				double width = tile->m_widthProj * pixelsPerMapUnit;
				double height = tile->m_heightProj * pixelsPerMapUnit;
				
				Gdiplus::RectF rect((Gdiplus::REAL)x, (Gdiplus::REAL)y, (Gdiplus::REAL)width, (Gdiplus::REAL)height);
				
				g->DrawImage(bmp, rect, (Gdiplus::REAL)(-0.5), (Gdiplus::REAL)(-0.5), 
							(Gdiplus::REAL)bmp->GetWidth(), (Gdiplus::REAL)bmp->GetHeight(), Gdiplus::UnitPixel, &attr);

				bool drawGrid = true;
				if (drawGrid)
				{
					Gdiplus::Pen pen(Gdiplus::Color::Red, 1.0f);
					pen.SetDashStyle(Gdiplus::DashStyleDash);
					g->DrawRectangle(&pen, rect);
				}

				bool drawText = true;
				if (drawText)
				{
					CString str;
					str.Format("X = %d; Y = %d", tile->m_tileX, tile->m_tileY);
					//str.Format("Lat = %f; Lng = %f; H = %f; W = %f", tile->m_yLat, tile->m_xLon, tile->m_heightLat, tile->m_widthLon);
					
					WCHAR* wStr = Utility::StringToWideChar(str);
					Gdiplus::Font* font = Utility::GetGdiPlusFont("Arial", 14);
					
					Gdiplus::SolidBrush brush(Gdiplus::Color::Red);
					Gdiplus::RectF r((Gdiplus::REAL)rect.X, (Gdiplus::REAL)rect.Y, (Gdiplus::REAL)rect.Width, (Gdiplus::REAL)rect.Height);
					
					Gdiplus::StringFormat format;
					format.SetAlignment(StringAlignmentCenter);
					format.SetLineAlignment(StringAlignmentCenter);

					g->DrawString(wStr, wcslen(wStr), font, r, &format,  &brush);
					
					delete font;
					delete wStr;
				}
				
				#ifdef _DEBUG
				//out << "Tile: lat = " << tile->yLat << " ; Y = " << y << " ; height = " << height << endl;
				#endif
			}
			tile->m_drawn = true;
		}
	}
	if (!m_graphics)
	{
		g->ReleaseHDC(_dc->m_hDC);
		delete g;
	}
}


