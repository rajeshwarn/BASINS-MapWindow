// Tile.cpp : Implementation of CTile

#include "stdafx.h"
#include "Tile.h"
#include "Tiles.h"

#pragma region Information
// ***********************************************************
//		Scale
// ***********************************************************
STDMETHODIMP CTile::get_TileScale(LONG* pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*pVal = m_scale;
	return S_OK;
}
STDMETHODIMP CTile::put_TileScale(LONG newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	m_scale = newVal;
	return S_OK;
}

// ***********************************************************
//		TileX
// ***********************************************************
STDMETHODIMP CTile::get_TileX(LONG* pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*pVal = m_tileX;
	return S_OK;
}
STDMETHODIMP CTile::put_TileX(LONG newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	m_tileX = newVal;
	return S_OK;
}

// ***********************************************************
//		TileY
// ***********************************************************
STDMETHODIMP CTile::get_TileY(LONG* pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*pVal = m_tileY;
	return S_OK;
}
STDMETHODIMP CTile::put_TileY(LONG newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	m_tileY = newVal;
	return S_OK;
}

// ***********************************************************
//		Provider
// ***********************************************************
STDMETHODIMP CTile::get_Provider(BSTR* pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	USES_CONVERSION;
	*pVal = A2BSTR(m_provider);
	return S_OK;
}
STDMETHODIMP CTile::put_Provider(BSTR newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	USES_CONVERSION;
	m_provider = OLE2A(newVal);
	return S_OK;
}
#pragma endregion

#pragma region Position
// ***********************************************************
//		get_xLon
// ***********************************************************
STDMETHODIMP CTile::get_xLongitude(DOUBLE* pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*pVal = m_xLon;
	return S_OK;
}

// ***********************************************************
//		get_yLat
// ***********************************************************
STDMETHODIMP CTile::get_yLatitude(DOUBLE* pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*pVal = m_yLat;
	return S_OK;
}

// ***********************************************************
//		get_WidthLon
// ***********************************************************
STDMETHODIMP CTile::get_WidthLongitude(DOUBLE* pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*pVal = m_widthLon;
	return S_OK;
}

// ***********************************************************
//		get_HeightLat
// ***********************************************************
STDMETHODIMP CTile::get_HeightLatitude(DOUBLE* pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*pVal = m_heightLat;
	return S_OK;
}
#pragma endregion

#pragma region Preserved
// ***********************************************************
//		Preserved
// ***********************************************************
STDMETHODIMP CTile::get_Cached(VARIANT_BOOL* pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*pVal = m_preserved;
	return S_OK;
}
STDMETHODIMP CTile::put_Cached(VARIANT_BOOL newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	if (newVal && !m_preserved && m_parent)
	{
		((CTiles*)m_parent)->AddToCache(this);
	}
	m_preserved = newVal;
	return S_OK;
}

// ***********************************************************
//		get_Size()
// ***********************************************************
STDMETHODIMP CTile::get_RamSize(int* pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*pVal = Bitmap ? Bitmap->get_Size() : 0;
	return S_OK;
}
#pragma endregion