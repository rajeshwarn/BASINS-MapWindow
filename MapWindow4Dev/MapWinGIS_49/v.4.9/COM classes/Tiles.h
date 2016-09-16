/**************************************************************************************
 * File name: Tiles.h
 *
 * Project: MapWindow Open Source (MapWinGis ActiveX control) 
 * Description: declaration of CTiles
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

#pragma once
#include "MapWinGIS.h"
#include "Tile.h"
#include "InMemoryBitmap.h"
#include <vector>
#include <map>
#include <set>

using namespace std;

#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

class ATL_NO_VTABLE CTiles :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CTiles, &CLSID_Tiles>,
	public IDispatchImpl<ITiles, &IID_ITiles, &LIBID_MapWinGIS, /*wMajor =*/ VERSION_MAJOR, /*wMinor =*/ VERSION_MINOR>
{
public:
	#pragma region Constructor/destructor
	CTiles()
	{
		m_visible = false;
		m_tilesLoaded = false;
	}

	DECLARE_REGISTRY_RESOURCEID(IDR_TILES)

	BEGIN_COM_MAP(CTiles)
		COM_INTERFACE_ENTRY(ITiles)
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

public:
	#pragma region "COM interface"
	STDMETHOD(Clear)(void);
	STDMETHOD(ClearCache)(void);
	STDMETHOD(get_Count)(LONG* pVal);
	STDMETHOD(get_Visible)(VARIANT_BOOL* pVal);
	STDMETHOD(put_Visible)(VARIANT_BOOL newVal);
	STDMETHOD(Add)(VARIANT bytesArray, double xLon, double yLat, double widthLon, double heightLat, ITile** retVal);
	STDMETHOD(get_Exists)(BSTR Provider, LONG Scale, LONG TileX, LONG TileY, VARIANT_BOOL* retVal);
	STDMETHOD(get_Tile)(BSTR Provider, LONG Scale, LONG TileX, LONG TileY, ITile** retVal);
	STDMETHOD(PrepareFromCache)(BSTR Provider, LONG Scale, LONG TileX, LONG TileY, VARIANT_BOOL* retVal);
	STDMETHOD(BrowseCache)(BSTR Provider, LONG Scale, SAFEARRAY** result, VARIANT_BOOL* retVal);
	STDMETHOD(get_RamSize)(int* pVal);
	#pragma endregion

	std::vector<CTile*> m_tiles;
	bool m_tilesLoaded;
	
	// local tile cache (provider, scale, tileX, TileY)
	typedef std::map<int, CTile*>							TilePositions;
	typedef std::map<int, TilePositions*>					TilePoints;
	typedef std::map<CString, std::vector<TilePoints*>*>	TilesCache;
	
	TilesCache m_tilesMap;

	#pragma region Public methods
	
	void AddToCache(ITile* newTile);
	CTile* get_TileCore(CString provider, LONG scale, LONG tileX, LONG tileY);
	std::vector<ITile*>* GetTilesCore(TilePoints* points);

	// Returns true if at least one undrawn tile exists
	bool UndrawnTilesExist()
	{
		for (unsigned int i = 0; i < m_tiles.size(); i++)
		{
			if (!((CTile*)m_tiles[i])->m_drawn)
				return true;
		}
		return false;
	}
	
	// Returns true if at least one drawn tile exists
	bool DrawnTilesExist()
	{
		for (unsigned int i = 0; i < m_tiles.size(); i++)
		{
			if (((CTile*)m_tiles[i])->m_drawn)
				return true;
		}
		return false;
	}
	#pragma endregion
private:	
	VARIANT_BOOL m_visible;
};

OBJECT_ENTRY_AUTO(__uuidof(Tiles), CTiles)
