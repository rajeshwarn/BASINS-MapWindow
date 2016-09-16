/**************************************************************************************
 * File name: Tile.h
 *
 * Project: MapWindow Open Source (MapWinGis ActiveX control) 
 * Description: implementation of CTile
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
 // lsu 18 oct 2011 - created the file

#pragma once
#include "MapWinGIS.h"
#include "InMemoryBitmap.h"
#include "ogr_spatialref.h"

#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

// CTile
class ATL_NO_VTABLE CTile :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CTile, &CLSID_Tile>,
	public IDispatchImpl<ITile, &IID_ITile, &LIBID_MapWinGIS, /*wMajor =*/ VERSION_MAJOR, /*wMinor =*/ VERSION_MINOR>
{
public:
	#pragma region Contructor/destructor
	CTile()
	{
		m_scale = 0;
		m_tileX = 0;
		m_tileY = 0;
		m_provider = "";
		m_preserved = false;
		
		Bitmap = NULL;
		m_xLon = 0.0;
		m_yLat = 0.0;
		m_widthLon = 0.0;
		m_heightLat = 0.0;
		m_drawn = false;

		m_parent = NULL;
		m_projectionOk = false;
	}

	~CTile()
	{
		if (this->Bitmap)
			delete this->Bitmap;
	}

	DECLARE_REGISTRY_RESOURCEID(IDR_TILE)

	BEGIN_COM_MAP(CTile)
		COM_INTERFACE_ENTRY(ITile)
		COM_INTERFACE_ENTRY(IDispatch)
	END_COM_MAP()

	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}

	void FinalRelease()
	{
	}
	#pragma endregion

	#pragma region "COM interface"
	STDMETHOD(get_TileScale)(LONG* pVal);
	STDMETHOD(put_TileScale)(LONG newVal);
	STDMETHOD(get_TileX)(LONG* pVal);
	STDMETHOD(put_TileX)(LONG newVal);
	STDMETHOD(get_TileY)(LONG* pVal);
	STDMETHOD(put_TileY)(LONG newVal);
	STDMETHOD(get_Provider)(BSTR* pVal);
	STDMETHOD(put_Provider)(BSTR newVal);
	STDMETHOD(get_Cached)(VARIANT_BOOL* pVal);
	STDMETHOD(put_Cached)(VARIANT_BOOL newVal);

	STDMETHOD(get_xLongitude)(DOUBLE* pVal);
	STDMETHOD(get_yLatitude)(DOUBLE* pVal);
	STDMETHOD(get_WidthLongitude)(DOUBLE* pVal);
	STDMETHOD(get_HeightLatitude)(DOUBLE* pVal);

	STDMETHOD(get_RamSize)(int* pVal);
	
	//STDMETHOD(Clear)(VARIANT_BOOL clearPreserved);
	#pragma endregion

	void Create(CMemoryBitmap* bmp, double x, double y, double width, double height)
	{
		Bitmap = bmp;
		m_xLon = x;
		m_yLat= y;
		m_widthLon = width;
		m_heightLat = height;
	}
	
	VARIANT_BOOL m_preserved;

public:
	long m_scale;
	long m_tileX;
	long m_tileY;
	CString m_provider;
	
	bool m_drawn;
	CMemoryBitmap* Bitmap;

	double m_xLon;			// geographical coordinates
	double m_yLat;
	double m_widthLon;
	double m_heightLat;

	double m_xProj;			// projected coordinates
	double m_yProj;
	double m_widthProj;
	double m_heightProj;

	ITiles* m_parent;

	bool m_projectionOk;	// position of tile was recalculated using current 
	
	// Recalculates projected coordinates; it is assumed that map projection is different from WGS84
	bool UpdateProjection(OGRCoordinateTransformation* transformation)
	{
		if (transformation)
		{
			//double x = m_xLon;
			//double y = m_yLat;
			//if (transformation->Transform(1, &x, &y))
			//{
			//	m_xProj = x;
			//	m_yProj = y;
			//	
			//	x = m_xLon + m_widthLon;
			//	y = m_yLat - m_heightLat;
			//	if (transformation->Transform(1, &x, &y))
			//	{
			//		m_widthProj = x - m_xProj;
			//		m_heightProj = m_yProj - y;
			//		
			//		//m_heightProj = m_widthProj;
			//		
			//		m_projectionOk = true;
			//		return true;
			//	}
			//}

			double xMin = m_xLon;
			double yMax = m_yLat;
			double xMax = m_xLon + m_widthLon;
			double yMin = m_yLat - m_heightLat;
			
			double xTL, xTR, xBL, xBR;
			double yTL, yTR, yBL, yBR;

			xTL = xBL = xMin;
			xTR = xBR = xMax;
			yTL = yTR = yMax;
			yBL = yBR = yMin;

			if (!transformation->Transform(1, &xTL, &yTL))
				return false;

			if (!transformation->Transform(1, &xTR, &yTR))
				return false;

			if (!transformation->Transform(1, &xBL, &yBL))
				return false;

			if (!transformation->Transform(1, &xBR, &yBR))
				return false;
			
			m_xProj = (xBL + xTL)/2.0;
			m_yProj = (yTL + yTR)/2.0;

			m_widthProj = (xBR + xTR)/2.0 - m_xProj;
			m_heightProj = m_yProj - (yBL + yBR)/2.0;

			m_projectionOk = true;
			return true;

			//if (transformation->Transform(1, &x, &y))
			//{
			//	m_xProj = x;
			//	m_yProj = y;
			//	
			//	x = m_xLon + m_widthLon;
			//	y = m_yLat - m_heightLat;
			//	if (transformation->Transform(1, &x, &y))
			//	{
			//		m_widthProj = x - m_xProj;
			//		m_heightProj = m_yProj - y;
			//		
			//		//m_heightProj = m_widthProj;
			//		
			//		m_projectionOk = true;
			//		return true;
			//	}
			//}
		}
		return false;
	}
};

OBJECT_ENTRY_AUTO(__uuidof(Tile), CTile)
