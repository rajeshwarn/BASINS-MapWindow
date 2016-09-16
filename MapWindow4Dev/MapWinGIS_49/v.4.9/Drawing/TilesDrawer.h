#pragma once
#include "basedrawer.h"
#include <vector>
#include "Tiles.h"
#include "ogr_spatialref.h"

class CTilesDrawer : public CBaseDrawer
{
public:
	CTilesDrawer(CDC* dc, Extent* extents, double pixelPerProjectionX, double pixelPerProjectionY)
	{
		_dc = dc;
		_extents = extents;
		_pixelPerProjectionX = pixelPerProjectionX;
		_pixelPerProjectionY = pixelPerProjectionY;
		m_transfomation = NULL;
	};
	CTilesDrawer(Gdiplus::Graphics* g, Extent* extents, double pixelPerProjectionX, double pixelPerProjectionY)
	{
		_dc = NULL;
		m_graphics = g;
		_extents = extents;
		_pixelPerProjectionX = pixelPerProjectionX;
		_pixelPerProjectionY = pixelPerProjectionY;
		m_transfomation = NULL;
	};
	~CTilesDrawer(void){};

	void DrawTiles(vector<CTile*>& tiles, double pixelsPerDegree);
	void DrawTiles(ITiles* tiles, double pixelsPerDegree);

	OGRCoordinateTransformation* m_transfomation;
	Gdiplus::Graphics* m_graphics;
};
