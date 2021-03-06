/**************************************************************************************
 * File name: GridColorScheme.h
 *
 * Project: MapWindow Open Source (MapWinGis ActiveX control) 
 * Description: Declaration of the CGridColorScheme 
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
#include <deque>
#include "vector.h"

// CGridColorScheme

class ATL_NO_VTABLE CGridColorScheme : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CGridColorScheme, &CLSID_GridColorScheme>,
	public IDispatchImpl<IGridColorScheme, &IID_IGridColorScheme, &LIBID_MapWinGIS, /*wMajor =*/ VERSION_MAJOR, /*wMinor =*/ VERSION_MINOR>
{
public:
	CGridColorScheme()
	{
		globalCallback = NULL;
		
		AmbientIntensity = 0.7;
		LightSourceIntensity = 0.7;		
		LightSourceIntensity = 0.7;

		LightSourceAzimuth = 90;
		LightSourceElevation = 45;
		
		NoDataColor = 0;		
		LightSource = cppVector(0.0,-0.707,1.0);

		lastErrorCode = tkNO_ERROR;
		
		USES_CONVERSION;
		key = A2BSTR("");
	}

	~CGridColorScheme()
	{
		::SysFreeString(key);
		Clear();
	}

	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}
	
	void FinalRelease() 
	{
	}

	DECLARE_REGISTRY_RESOURCEID(IDR_GRIDCOLORSCHEME)

	DECLARE_NOT_AGGREGATABLE(CGridColorScheme)

	BEGIN_COM_MAP(CGridColorScheme)
		COM_INTERFACE_ENTRY(IGridColorScheme)
		COM_INTERFACE_ENTRY(IDispatch)
	END_COM_MAP()


// IGridColorScheme
public:
	STDMETHOD(get_Key)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_Key)(/*[in]*/ BSTR newVal);
	STDMETHOD(get_GlobalCallback)(/*[out, retval]*/ ICallback * *pVal);
	STDMETHOD(put_GlobalCallback)(/*[in]*/ ICallback * newVal);
	STDMETHOD(get_ErrorMsg)(/*[in]*/ long ErrorCode, /*[out, retval]*/ BSTR *pVal);
	STDMETHOD(get_LastErrorCode)(/*[out, retval]*/ long *pVal);
	STDMETHOD(GetLightSource)(/*[out,retval]*/ IVector ** result);
	STDMETHOD(UsePredefined)(/*[in]*/ double LowValue, /*[in]*/ double HighValue, /*[in]*/ PredefinedColorScheme Preset);
	STDMETHOD(get_NoDataColor)(/*[out, retval]*/ OLE_COLOR *pVal);
	STDMETHOD(put_NoDataColor)(/*[in]*/ OLE_COLOR newVal);
	STDMETHOD(Clear)();
	STDMETHOD(DeleteBreak)(/*[in]*/ long Index);
	STDMETHOD(get_Break)(/*[in]*/ long Index, /*[out, retval]*/ IGridColorBreak ** pVal);
	STDMETHOD(InsertBreak)(/*[in]*/ IGridColorBreak * ColorBreak);
	STDMETHOD(SetLightSource)(/*[in]*/ double Azimuth, /*[in]*/ double Elevation);
	STDMETHOD(get_LightSourceElevation)(/*[out, retval]*/ double *pVal);
	STDMETHOD(get_LightSourceAzimuth)(/*[out, retval]*/ double *pVal);
	STDMETHOD(get_LightSourceIntensity)(/*[out, retval]*/ double *pVal);
	STDMETHOD(put_LightSourceIntensity)(/*[in]*/ double newVal);
	STDMETHOD(get_AmbientIntensity)(/*[out, retval]*/ double *pVal);
	STDMETHOD(put_AmbientIntensity)(/*[in]*/ double newVal);
	STDMETHOD(get_NumBreaks)(/*[out, retval]*/ long *pVal);
	STDMETHOD(InsertAt)(/*[in]*/ int Position, /*[in]*/ IGridColorBreak *Break);
	STDMETHOD(Serialize)(BSTR* retVal);
	STDMETHOD(Deserialize)(BSTR newVal);
	
	CPLXMLNode* CGridColorScheme::SerializeCore(CString ElementName);
	bool CGridColorScheme::DeserializeCore(CPLXMLNode* node);
private:
	std::deque<IGridColorBreak *> Breaks;	//stack of breaks to be used
	OLE_COLOR NoDataColor;				//color to be used when there is no data in the grid
	cppVector LightSource;	 				//where is the light coming from?
	double LightSourceIntensity;		//How bright is the light from the lightsource?
										//valid values between 0 and 1, inclusive.
	double AmbientIntensity;			//How bright is the overall lighting in the area?
										//valid values between 0 and 1, inclusive.
	double LightSourceAzimuth;
	double LightSourceElevation;

private:
	long lastErrorCode;
	ICallback * globalCallback;
	BSTR key;
};

OBJECT_ENTRY_AUTO(__uuidof(GridColorScheme), CGridColorScheme)
