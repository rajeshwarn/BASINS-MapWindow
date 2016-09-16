/**************************************************************************************
 * File name: Tiles.h
 *
 * Project: MapWindow Open Source (MapWinGis ActiveX control) 
 * Description: implementation of CTiles
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
 // lsu 21 aug 2011 - created the file

#include "stdafx.h"
#include "Tiles.h"
#include "Templates.h"

#pragma region Properties
// *********************************************************
//	     get_Count
// *********************************************************
STDMETHODIMP CTiles::get_Count(LONG* pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*pVal = m_tiles.size();
	return S_OK;
}

// *********************************************************
//	     Clear()
// *********************************************************
STDMETHODIMP CTiles::Clear()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	for (unsigned int i = 0; i < m_tiles.size(); i++)
	{
		if(!m_tiles[i]->m_preserved)
		{
			m_tiles[i]->Release();
		}
		else
		{
			m_tiles[i]->m_drawn = false;
		}
	}
	m_tiles.clear();
	
	m_tilesLoaded = false;
	return S_OK;
}

// *********************************************************
//	     Visible
// *********************************************************
STDMETHODIMP CTiles::get_Visible(VARIANT_BOOL* pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*pVal = m_visible;
	return S_OK;
}

STDMETHODIMP CTiles::put_Visible(VARIANT_BOOL newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	m_visible = newVal;
	return S_OK;
}
#pragma endregion

#pragma region Adding tiles
// *********************************************************
//	     Add()
// *********************************************************
STDMETHODIMP CTiles::Add(VARIANT bytesArray, double xLon, double yLat, double widthLon, double heightLat, ITile** retVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*retVal = NULL;

	if (bytesArray.vt != (VT_ARRAY|VT_UI1))
		return S_FALSE;
	
	unsigned char* p = NULL;
	SafeArrayAccessData(bytesArray.parray,(void HUGEP* FAR*)(&p));
	char* data = reinterpret_cast<char*>(p);
	
	CMemoryBitmap* bmp = new CMemoryBitmap();
	int size = (int)bytesArray.parray->rgsabound[0].cElements;
	bmp->LoadFromRawData(data, size);

	//TileData* tile = new TileData(bmp, xLon, yLat, widthLon, heightLat);
	
	ITile* tile = NULL;
	CoCreateInstance(CLSID_Tile,NULL,CLSCTX_INPROC_SERVER,IID_ITile,(void**)&tile);
	CTile* tileNew = (CTile*)tile;
	tileNew->Create(bmp, xLon, yLat, widthLon, heightLat);
	tileNew->m_parent = this;
	m_tiles.push_back(tileNew);
	
	*retVal = tile;
	tile->AddRef();
	
	SafeArrayUnaccessData(bytesArray.parray);

	m_tilesLoaded = true;

	return S_OK;
}
#pragma endregion

#pragma region Caching - extracting a tile

// ********************************************************
//		get_TileCore()
// ********************************************************
// Extracts a single tile from the cache
CTile* CTiles::get_TileCore(CString provider, LONG scale, LONG tileX, LONG tileY)
{
	if (m_tilesMap.find(provider) != m_tilesMap.end())
	{
		std::vector<CTiles::TilePoints*>* scales = m_tilesMap[provider];
		if (scale >= 0 && scale < scales->size())
		{
			TilePoints* points = (*scales)[scale];
			if (points != NULL)
			{
				if (points->find(tileX) != points->end())
				{
					std::map<int, CTile*>* map = (*points)[tileX];
					if (map->find(tileY) != map->end())
					{
						CTile* tile = ((*map)[tileY]);
						return tile;
					}
				}
			}
		}
	}
	return NULL;
}

// *********************************************************
//	     get_Exists()
// *********************************************************
//typedef std::map<int, std::map<int, ITile*>> TilePoints;
//std::map<CString, TilePoints> m_dict;
STDMETHODIMP CTiles::get_Exists(BSTR Provider, LONG Scale, LONG TileX, LONG TileY, VARIANT_BOOL* retVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*retVal = VARIANT_FALSE;
	
	USES_CONVERSION;
	CString provider = OLE2A(Provider);
	
	CTile* tile = this->get_TileCore(provider, Scale, TileX, TileY);
	*retVal = tile != NULL ? VARIANT_TRUE : VARIANT_FALSE;
	return S_OK;
}

// *********************************************************
//	     Prepare()
// *********************************************************
STDMETHODIMP CTiles::PrepareFromCache(BSTR Provider, LONG Scale, LONG TileX, LONG TileY, VARIANT_BOOL* retVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*retVal = VARIANT_FALSE;
	USES_CONVERSION;
	CString provider = OLE2A(Provider);
	CTile* tile = this->get_TileCore(provider, Scale, TileX, TileY);
	*retVal = tile != NULL? VARIANT_TRUE : VARIANT_FALSE;
	if (tile != NULL)
	{
		m_tiles.push_back(tile);
	}
	return S_OK;
}

// *********************************************************
//	     get_Tile()
// *********************************************************
STDMETHODIMP CTiles::get_Tile(BSTR Provider, LONG Scale, LONG TileX, LONG TileY, ITile** retVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*retVal = NULL;
	
	USES_CONVERSION;
	CString provider = OLE2A(Provider);
	CTile* tile = this->get_TileCore(provider, Scale, TileX, TileY);
	if (tile != NULL)
	{
		ITile* item = (ITile*)(tile);
		item->AddRef();
		*retVal = item;
	}
	return S_OK;
}
#pragma endregion

#pragma region Caching Add/Clear
// *********************************************************
//	     AddToCache()
// *********************************************************
void CTiles::AddToCache(ITile* newTile)
{
	CTile* tile = (CTile*)newTile;
	BSTR provider;
	tile->get_Provider(&provider);
	
	if (tile->m_scale < 0 || tile->m_scale >= 25)
	{
		// report error: unexpected scale
		return;
	}

	VARIANT_BOOL vbretval;
	this->get_Exists(provider, tile->m_scale, tile->m_tileX, tile->m_tileY, &vbretval);
	if (!vbretval)
	{
		// seeking provider
		std::vector<CTiles::TilePoints*>* list = NULL;
		if (m_tilesMap.find(tile->m_provider) != m_tilesMap.end())
		{
			list = m_tilesMap[tile->m_provider];
		}
		else
		{
			list = new std::vector<CTiles::TilePoints*>();
			list->resize(25, NULL);
			m_tilesMap[tile->m_provider] = list;
		}

		// seeking scale
		TilePoints* points = (*list)[tile->m_scale];
		if (!points)
		{
			points = new TilePoints();
			(*list)[tile->m_scale] = points;
		}
		
		// seeking X
		std::map<int, CTile*>* map = NULL;
		if (points->find(tile->m_tileX) != points->end())
		{
			map = (*points)[tile->m_tileX];
		}
		else
		{
			map = new std::map<int, CTile*>();
			(*points)[tile->m_tileX] = map;
		}

		// seeking Y
		ASSERT (map->find(tile->m_tileY) != map->end());
		(*map)[tile->m_tileY] = (CTile*)newTile;
	}
}

// *********************************************************
//	     ClearCache()
// *********************************************************
STDMETHODIMP CTiles::ClearCache()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	
	// let's protect tiles that was drawn or about to be drawn to be released
	for (int i = 0; i < m_tiles.size(); i++)
	{
		if (m_tiles[i]->m_preserved)
			m_tiles[i]->AddRef();
	}

	for( TilesCache::iterator it = m_tilesMap.begin(); it != m_tilesMap.end(); ++it ) 
	{
		vector<TilePoints*>* list = it->second;
		if (list)
		{
			for (int i = 0; i< list->size(); i++)
			{
				TilePoints* points = (*list)[i];
				if (points)
				{
					for (TilePoints::iterator it2 = points->begin(); it2 != points->end(); ++it2)
					{
						TilePositions* tiles = it2->second;
						for (TilePositions::iterator it3 = tiles->begin(); it3 != tiles->end(); ++it3)
						{
							CTile* tile = it3->second;
							tile->m_preserved = false;
							tile->Release();
						}
						delete tiles;
					}
					delete points;
				}
			}
			delete list;
		}
    }
	m_tilesMap.clear();
	return S_OK;
}
#pragma endregion

#pragma region Caching - browse

// *********************************************************
//	     GetTilesCore()
// *********************************************************
// Returns list of tiles for specified provider/scale
std::vector<ITile*>* CTiles::GetTilesCore(TilePoints* points)
{
	std::vector<ITile*>* result = new std::vector<ITile*>();
	if (points)
	{
		for (TilePoints::iterator it2 = points->begin(); it2 != points->end(); ++it2)
		{
			TilePositions* tiles = it2->second;
			for (TilePositions::iterator it3 = tiles->begin(); it3 != tiles->end(); ++it3)
			{
				CTile* tile = it3->second;
				result->push_back(tile);
			}
		}
	}
	return result;
}

// *********************************************************
//	     GetCache()
// *********************************************************
STDMETHODIMP CTiles::BrowseCache(BSTR Provider, LONG Scale, SAFEARRAY** result, VARIANT_BOOL* retVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	USES_CONVERSION;
	CString provider = OLE2A(Provider);
	std::vector<ITile*>* tiles = NULL;

	if (m_tilesMap.find(provider) != m_tilesMap.end())
	{
		std::vector<TilePoints*>* scales = m_tilesMap[provider];
		if (Scale >= 0 && Scale < scales->size())
		{
			TilePoints* points = (*scales)[Scale];
			if (points)
			{
				tiles = this->GetTilesCore(points);
				// adding refrences as values will be passed to the caller
				for (int i = 0; i < tiles->size(); i++)
				{
					(*tiles)[i]->AddRef();
				}
			}
		}
	}
	
	VARIANT arr;
	Templates::Vector2SafeArray(tiles, &arr);
	ASSERT(arr.vt == VT_ARRAY|VT_DISPATCH);
	(*result) = arr.parray;

	*retVal = VARIANT_TRUE;
	return S_OK;
}

// ***********************************************************
//		get_Size()
// ***********************************************************
STDMETHODIMP CTiles::get_RamSize(int* pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	int sum = 0;
	for( TilesCache::iterator it = m_tilesMap.begin(); it != m_tilesMap.end(); ++it ) 
	{
		std::vector<TilePoints*>* scales = it->second;
		for (int i = 0; i < scales->size(); i++)
		{
			TilePoints* points = (*scales)[i];
			if (points)
			{
				std::vector<ITile*>* tiles = this->GetTilesCore(points);
				for (int j = 0; j < tiles->size(); j++)
				{
					int size;
					(*tiles)[j]->get_RamSize(&size);
					sum += size;
				}
			}
		}
	}
	return S_OK;
}

#pragma endregion

