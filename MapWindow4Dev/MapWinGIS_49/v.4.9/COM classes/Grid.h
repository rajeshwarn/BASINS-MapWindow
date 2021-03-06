/**************************************************************************************
 * File name: Grid.h
 *
 * Project: MapWindow Open Source (MapWinGis ActiveX control) 
 * Description: Declaration of the CGrid 
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

#pragma once
#include "MapWinGis.h"
#include <comsvcs.h>

#include "varH.h"
#include "dgrid.h"
#include "fgrid.h"
#include "lgrid.h"
#include "sgrid.h"
#include "GridManager.h"
#include "tkGridRaster.h"


// CGrid
class ATL_NO_VTABLE CGrid : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CGrid, &CLSID_Grid>,
	public IDispatchImpl<IGrid, &IID_IGrid, &LIBID_MapWinGIS, /*wMajor =*/ VERSION_MAJOR, /*wMinor =*/ VERSION_MINOR>
{
public:
	CGrid();
	~CGrid();
	void CallBack(long percent,BSTR message);

	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}
	
	void FinalRelease() 
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_GRID)

DECLARE_NOT_AGGREGATABLE(CGrid)

BEGIN_COM_MAP(CGrid)
	COM_INTERFACE_ENTRY(IGrid)
	COM_INTERFACE_ENTRY(IDispatch)
END_COM_MAP()


// IGrid
public:
	STDMETHOD(get_CdlgFilter)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(CellToProj)(/*[in]*/ long Column, /*[in]*/ long Row, /*[out]*/ double * x, /*[out]*/ double * y);
	STDMETHOD(ProjToCell)(/*[in]*/ double x, /*[in]*/ double y, /*[out]*/ long * Column, /*[out]*/ long * Row);
	STDMETHOD(Clear)(/*[in]*/ VARIANT ClearValue, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(Save)(/*[in, optional]*/ BSTR Filename, /*[in, optional, defaultvalue(tkUseExtension)]*/ GridFileType FileType, /*[in]*/ ICallback * cBack, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(Close)(/*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(CreateNew)(/*[in]*/ BSTR Filename, /*[in]*/ IGridHeader * Header, /*[in]*/ GridDataType DataType, /*[in]*/ VARIANT InitialValue, /*[in, optional, defaultvalue(TRUE)]*/ VARIANT_BOOL InRam, /*[in, optional, defaultvalue(tkUseExtension)]*/ GridFileType FileType, /*[in, optional]*/ ICallback * cBack, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(Open)(/*[in]*/ BSTR Filename, /*[in, optional, defaultvalue(tkUnknown)]*/ GridDataType DataType, /*[in, optional, defaultvalue(TRUE)]*/ VARIANT_BOOL InRam, /*[in, optional, defaultvalue(tkUseExtension)]*/ GridFileType FileType, /*[in, optional]*/ ICallback * cBack, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(get_Key)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_Key)(/*[in]*/ BSTR newVal);
	STDMETHOD(get_GlobalCallback)(/*[out, retval]*/ ICallback * *pVal);
	STDMETHOD(put_GlobalCallback)(/*[in]*/ ICallback * newVal);
	STDMETHOD(get_ErrorMsg)(/*[in]*/ long ErrorCode, /*[out, retval]*/ BSTR *pVal);
	STDMETHOD(get_LastErrorCode)(/*[out, retval]*/ long *pVal);
	STDMETHOD(get_Filename)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(get_DataType)(/*[out, retval]*/ GridDataType *pVal);
	STDMETHOD(get_Minimum)(/*[out, retval]*/ VARIANT *pVal);
	STDMETHOD(get_Maximum)(/*[out, retval]*/ VARIANT *pVal);
	STDMETHOD(get_InRam)(/*[out, retval]*/ VARIANT_BOOL *pVal);
	STDMETHOD(get_Value)(/*[in]*/ long Column, /*[in]*/ long Row, /*[out, retval]*/ VARIANT *pVal);
	STDMETHOD(put_Value)(/*[in]*/ long Column, /*[in]*/ long Row, /*[in]*/ VARIANT newVal);
	STDMETHOD(get_Header)(/*[out, retval]*/ IGridHeader * *pVal);
	STDMETHOD(AssignNewProjection)(/*[in]*/ BSTR projection, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(get_RasterColorTableColoringScheme)(/*[out, retval]*/ IGridColorScheme * *pVal);
	STDMETHOD(GetRow)(/*[in]*/ long Row, /*[in, out]*/ float *Vals, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(PutRow)(/*[in]*/ long Row, /*[in]*/ float *Vals, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(GetFloatWindow)(/*[in]*/ long StartRow, /*[in]*/ long EndRow, /*[in]*/ long StartCol, /*[in]*/ long EndCol, /*[in, out]*/ float * Vals, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(PutFloatWindow)(/*[in]*/ long StartRow, /*[in]*/ long EndRow, /*[in]*/ long StartCol, /*[in]*/ long EndCol, /*[in, out]*/ float * Vals, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(SetInvalidValuesToNodata)(/*[in*/ double MinThresholdValue, /*[in*/ double MaxThresholdValue, /*out, retval*/ VARIANT_BOOL * retval);
	STDMETHOD(Resource)(/*[in]*/ BSTR newSrcPath, /*[out, retval]*/ VARIANT_BOOL * retval);

private:
	dGrid * dgrid;
	fGrid * fgrid;
	lGrid * lgrid;
	sGrid * sgrid;
	tkGridRaster * trgrid;

	bool MemoryAvailable(double bytes);

	long lastErrorCode;
	BSTR filename;
	BSTR key;
	ICallback * globalCallback;

	void set_ProjectionIntoHeader(char * projection);
	void ResolveFileType(GridFileType &newFileType, CString extension);

	inline bool FloatsEqual(const float &a, const float &b)
	{
		return (fabs(a - b) <= 1.0e-20f);
	}

};

OBJECT_ENTRY_AUTO(__uuidof(Grid), CGrid)
