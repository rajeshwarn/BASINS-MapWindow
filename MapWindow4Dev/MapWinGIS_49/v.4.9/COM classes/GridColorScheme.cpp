//********************************************************************************************************
//File name: GridColorScheme.cpp
//Description: Implementation of CGridColorScheme
//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//http://www.mozilla.org/MPL/ 
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
//limitations under the License. 
//
//The Original Code is MapWindow Open Source. 
//
//The Initial Developer of this version of the Original Code is Daniel P. Ames using portions created by 
//Utah State University and the Idaho National Engineering and Environmental Lab that were released as 
//public domain in March 2004.  
//
//Contributor(s): (Open source contributors should list themselves and their modifications here). 
//3-28-2005 dpa - Identical to public domain version.
//********************************************************************************************************

#include "stdafx.h"
#include "GridColorScheme.h"
//#include "UtilityFunctions.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

// CGridColorScheme
STDMETHODIMP CGridColorScheme::get_NumBreaks(long *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	// TODO: Add your implementation code here
	*pVal = Breaks.size();

	return S_OK;
}

STDMETHODIMP CGridColorScheme::get_AmbientIntensity(double *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	// TODO: Add your implementation code here
	*pVal = AmbientIntensity;

	return S_OK;
}

STDMETHODIMP CGridColorScheme::put_AmbientIntensity(double newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;

	//Intensity must be between 0 and 1 	
	if ( newVal >=0 && newVal <= 1)
	{
		AmbientIntensity = newVal;
	}
	else
	{	lastErrorCode = tkOUT_OF_RANGE_0_TO_1;
		if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));		
	}

	return S_OK;
}

STDMETHODIMP CGridColorScheme::get_LightSourceIntensity(double *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	// TODO: Add your implementation code here
	*pVal = LightSourceIntensity;

	return S_OK;
}

STDMETHODIMP CGridColorScheme::put_LightSourceIntensity(double newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	//Intensity must be between 0 and 1 
	if ( newVal >=0 && newVal <= 1)
	{
		LightSourceIntensity = newVal;
	}
	else
	{	lastErrorCode = tkOUT_OF_RANGE_0_TO_1;
		if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));		
	}

	return S_OK;
}

STDMETHODIMP CGridColorScheme::get_LightSourceAzimuth(double *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	// TODO: Add your implementation code here
	*pVal = LightSourceAzimuth;

	return S_OK;
}

STDMETHODIMP CGridColorScheme::get_LightSourceElevation(double *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	// TODO: Add your implementation code here
	*pVal = LightSourceElevation;

	return S_OK;
}

STDMETHODIMP CGridColorScheme::SetLightSource(double Azimuth, double Elevation)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	// elevation is between 0-180
	//azimuth is between 0-360 (mod if necessary)
	if (Elevation > 180 || Elevation < 0)
	{	lastErrorCode = tkOUT_OF_RANGE_0_TO_180;
		if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));		
		return S_OK;
	}

	if (Azimuth > 360 || Azimuth < -360)
	{	lastErrorCode = tkOUT_OF_RANGE_M360_TO_360;
		if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));		
		return S_OK;
	}

	LightSourceAzimuth = Azimuth;
	LightSourceElevation = Elevation;

	Matrix ry;
	ry.rotateMY((int)Azimuth);
	
	Matrix rx;
	rx.rotateX((int)Elevation);

	Matrix comp = rx*ry;

	LightSource.seti(0);
	LightSource.setj(0);
	LightSource.setk(1);

	LightSource = LightSource * comp;
	LightSource.Normalize();

	return S_OK;
}

STDMETHODIMP CGridColorScheme::InsertBreak(IGridColorBreak *BrkInfo)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	if( BrkInfo == NULL )
	{	lastErrorCode = tkUNEXPECTED_NULL_PARAMETER;
		if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));		
	}

	BrkInfo->AddRef();
	Breaks.push_back( BrkInfo );

	return S_OK;
}

STDMETHODIMP CGridColorScheme::InsertAt(int Position, IGridColorBreak *Break)
{
	if( Break == NULL )
		return S_OK;

	Break->AddRef();
	Breaks.insert(Breaks.begin() + Position, Break);
	return S_OK;
}

STDMETHODIMP CGridColorScheme::get_Break(long Index, IGridColorBreak **pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	if( Index >= 0 && Index < (long)Breaks.size() )
	{	Breaks[Index]->AddRef();
		*pVal = Breaks[Index];
	}
	else
	{	*pVal = NULL;
		lastErrorCode = tkINDEX_OUT_OF_BOUNDS;
		if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));		
	}

	return S_OK;
}

STDMETHODIMP CGridColorScheme::DeleteBreak(long Index)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	
	if( Index >= 0 && Index < (long)Breaks.size() )
	{
		Breaks[Index]->Release();
		Breaks.erase( Breaks.begin() + Index );
	}
	else
	{	lastErrorCode = tkINDEX_OUT_OF_BOUNDS;
		if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));		
	}

	return S_OK;
}

STDMETHODIMP CGridColorScheme::Clear()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	while( Breaks.size() > 0 )
	{	Breaks[0]->Release();
		Breaks.erase( Breaks.begin() );
	}

	return S_OK;
}

STDMETHODIMP CGridColorScheme::get_NoDataColor(OLE_COLOR *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	// TODO: Add your implementation code here
	*pVal = NoDataColor;

	return S_OK;
}

STDMETHODIMP CGridColorScheme::put_NoDataColor(OLE_COLOR newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	// TODO: Add your implementation code here
	NoDataColor = newVal;

	return S_OK;
}

STDMETHODIMP CGridColorScheme::UsePredefined(double LowValue, double HighValue, PredefinedColorScheme Preset)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	AmbientIntensity = 0.7;
	LightSourceIntensity = 0.7;
	//Modified by Ted Dunsford 6/16/06 to normalize & match colorscheme application mechanism
	//LightSource = vector(0,-0.707,1);
	LightSource = cppVector(-.707, -.707, 0);
	LightSourceIntensity = 0.7;

	NoDataColor = 0;
	Clear();

	if( LowValue > HighValue )
	{	double temp = LowValue;
		LowValue = HighValue;
		HighValue = temp;
	}
	
	if( Preset == SummerMountains )
	{	
		IGridColorBreak* lowbreak, * highbreak;
		CoCreateInstance(CLSID_GridColorBreak,NULL,CLSCTX_INPROC_SERVER,IID_IGridColorBreak,(void**)&lowbreak);
		CoCreateInstance(CLSID_GridColorBreak,NULL,CLSCTX_INPROC_SERVER,IID_IGridColorBreak,(void**)&highbreak);

		lowbreak->put_LowValue( LowValue );
		lowbreak->put_HighValue( (HighValue + LowValue) / 2 );
		lowbreak->put_LowColor( RGB( 10, 100, 10 ) );
		lowbreak->put_HighColor( RGB( 153, 125, 25 ) );

		highbreak->put_LowValue( (HighValue + LowValue) / 2 );
		highbreak->put_HighValue( HighValue );
		highbreak->put_LowColor( RGB( 153, 125, 25 ) );
		highbreak->put_HighColor( RGB( 255, 255, 255 ) );

		InsertBreak(lowbreak);
		InsertBreak(highbreak);

		lowbreak->Release();
		highbreak->Release();
	}
	else if( Preset == FallLeaves )
	{	
		IGridColorBreak * lowbreak, * highbreak;
		CoCreateInstance(CLSID_GridColorBreak,NULL,CLSCTX_INPROC_SERVER,IID_IGridColorBreak,(void**)&lowbreak);
		CoCreateInstance(CLSID_GridColorBreak,NULL,CLSCTX_INPROC_SERVER,IID_IGridColorBreak,(void**)&highbreak);

		lowbreak->put_LowValue( LowValue );
		lowbreak->put_HighValue( (HighValue + LowValue) / 2 );
		lowbreak->put_LowColor( RGB( 10, 100, 10 ) );
		lowbreak->put_HighColor( RGB( 199, 130, 61 ) );

		highbreak->put_LowValue( (HighValue + LowValue) / 2 );
		highbreak->put_HighValue( HighValue );
		highbreak->put_LowColor( RGB( 199, 130, 61 ) );
		highbreak->put_HighColor( RGB( 241, 220, 133 ) );

		InsertBreak(lowbreak);
		InsertBreak(highbreak);

		lowbreak->Release();
		highbreak->Release();
	}
	else if( Preset == Desert )
	{
		IGridColorBreak * lowbreak, * highbreak;
		CoCreateInstance(CLSID_GridColorBreak,NULL,CLSCTX_INPROC_SERVER,IID_IGridColorBreak,(void**)&lowbreak);
		CoCreateInstance(CLSID_GridColorBreak,NULL,CLSCTX_INPROC_SERVER,IID_IGridColorBreak,(void**)&highbreak);

		lowbreak->put_LowValue(LowValue);
		lowbreak->put_HighValue( (HighValue + LowValue) / 2 );
		lowbreak->put_LowColor( RGB( 211, 206, 97 ) );
		lowbreak->put_HighColor( RGB( 139, 120, 112 ) );

		highbreak->put_LowValue( (HighValue + LowValue) / 2 );
		highbreak->put_HighValue(HighValue);
		highbreak->put_LowColor( RGB( 139, 120, 112 ) );
		highbreak->put_HighColor( RGB( 255, 255, 255 ) );

		InsertBreak(lowbreak);
		InsertBreak(highbreak);

		lowbreak->Release();
		highbreak->Release();
	}
	else if( Preset == Glaciers )
	{
		IGridColorBreak * lowbreak, * highbreak;
		CoCreateInstance(CLSID_GridColorBreak,NULL,CLSCTX_INPROC_SERVER,IID_IGridColorBreak,(void**)&lowbreak);
		CoCreateInstance(CLSID_GridColorBreak,NULL,CLSCTX_INPROC_SERVER,IID_IGridColorBreak,(void**)&highbreak);

		lowbreak->put_LowValue(LowValue);
		lowbreak->put_HighValue( (HighValue + LowValue) / 2 );
		lowbreak->put_LowColor( RGB( 105, 171, 224 ) );
		lowbreak->put_HighColor( RGB( 162, 234, 240 ) );

		highbreak->put_LowValue( (HighValue + LowValue) / 2 );
		highbreak->put_HighValue(HighValue);
		highbreak->put_LowColor( RGB( 162, 234, 240 ) );
		highbreak->put_HighColor( RGB( 255, 255, 255 ) );

		InsertBreak(lowbreak);
		InsertBreak(highbreak);

		lowbreak->Release();
		highbreak->Release();
	}
	else if( Preset == Meadow )
	{	
		IGridColorBreak * lowbreak, * highbreak;
		CoCreateInstance(CLSID_GridColorBreak,NULL,CLSCTX_INPROC_SERVER,IID_IGridColorBreak,(void**)&lowbreak);
		CoCreateInstance(CLSID_GridColorBreak,NULL,CLSCTX_INPROC_SERVER,IID_IGridColorBreak,(void**)&highbreak);

		lowbreak->put_LowValue(LowValue);
		lowbreak->put_HighValue( (HighValue + LowValue) / 2 );
		lowbreak->put_LowColor( RGB( 68, 128, 71 ) );
		lowbreak->put_HighColor( RGB( 43, 91, 30 ) );

		highbreak->put_LowValue( (HighValue + LowValue) / 2 );
		highbreak->put_HighValue(HighValue);
		highbreak->put_LowColor( RGB( 43, 91, 30 ) );
		highbreak->put_HighColor( RGB( 167, 220, 168 ) );

		InsertBreak(lowbreak);
		InsertBreak(highbreak);

		lowbreak->Release();
		highbreak->Release();
	}
	else if( Preset == ValleyFires )
	{	
		IGridColorBreak * lowbreak, * highbreak;
		CoCreateInstance(CLSID_GridColorBreak,NULL,CLSCTX_INPROC_SERVER,IID_IGridColorBreak,(void**)&lowbreak);
		CoCreateInstance(CLSID_GridColorBreak,NULL,CLSCTX_INPROC_SERVER,IID_IGridColorBreak,(void**)&highbreak);

		lowbreak->put_LowValue(LowValue);
		lowbreak->put_HighValue( (HighValue + LowValue) / 2 );
		lowbreak->put_LowColor( RGB( 164, 0, 0 ) );
		lowbreak->put_HighColor( RGB( 255, 128, 64 ) );

		highbreak->put_LowValue( (HighValue + LowValue) / 2 );
		highbreak->put_HighValue(HighValue);
		highbreak->put_LowColor( RGB( 255, 128, 64 ) );
		highbreak->put_HighColor( RGB( 255, 255, 191 ) );

		InsertBreak(lowbreak);
		InsertBreak(highbreak);

		lowbreak->Release();
		highbreak->Release();
	}
	else if( Preset == DeadSea )
	{	
		IGridColorBreak * lowbreak, * highbreak;
		CoCreateInstance(CLSID_GridColorBreak,NULL,CLSCTX_INPROC_SERVER,IID_IGridColorBreak,(void**)&lowbreak);
		CoCreateInstance(CLSID_GridColorBreak,NULL,CLSCTX_INPROC_SERVER,IID_IGridColorBreak,(void**)&highbreak);

		lowbreak->put_LowValue(LowValue);
		lowbreak->put_HighValue( (HighValue + LowValue) / 2 );
		lowbreak->put_LowColor( RGB( 51, 137, 208 ) );
		lowbreak->put_HighColor( RGB( 226, 227, 166 ) );

		highbreak->put_LowValue( (HighValue + LowValue) / 2 );
		highbreak->put_HighValue(HighValue);
		highbreak->put_LowColor( RGB( 226, 227, 166 ) );
		highbreak->put_HighColor( RGB( 151, 146, 117 ) );

		InsertBreak(lowbreak);
		InsertBreak(highbreak);

		lowbreak->Release();
		highbreak->Release();
	}
	else if( Preset == Highway1 )
	{	
		IGridColorBreak * lowbreak, * highbreak;
		CoCreateInstance(CLSID_GridColorBreak,NULL,CLSCTX_INPROC_SERVER,IID_IGridColorBreak,(void**)&lowbreak);
		CoCreateInstance(CLSID_GridColorBreak,NULL,CLSCTX_INPROC_SERVER,IID_IGridColorBreak,(void**)&highbreak);

		lowbreak->put_LowValue(LowValue);
		lowbreak->put_HighValue( (HighValue + LowValue) / 2 );
		lowbreak->put_LowColor( RGB( 51, 137, 208 ) );
		lowbreak->put_HighColor( RGB( 214, 207, 124 ) );

		highbreak->put_LowValue( (HighValue + LowValue) / 2 );
		highbreak->put_HighValue(HighValue);
		highbreak->put_LowColor( RGB( 214, 207, 124 ) );
		highbreak->put_HighColor( RGB( 54, 152, 69 ) );

		InsertBreak(lowbreak);
		InsertBreak(highbreak);

		lowbreak->Release();
		highbreak->Release();
	}

	return S_OK;
}

STDMETHODIMP CGridColorScheme::GetLightSource(IVector **result)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	CoCreateInstance(CLSID_Vector,NULL,CLSCTX_INPROC_SERVER,IID_IVector,(void**)result);
	(*result)->put_i(LightSource.geti());
	(*result)->put_j(LightSource.getj());
	(*result)->put_k(LightSource.getk());
	return S_OK;
}


STDMETHODIMP CGridColorScheme::get_LastErrorCode(long *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	*pVal = lastErrorCode;
	lastErrorCode = tkNO_ERROR;

	return S_OK;
}

STDMETHODIMP CGridColorScheme::get_ErrorMsg(long ErrorCode, BSTR *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	*pVal = A2BSTR(ErrorMsg(ErrorCode));

	return S_OK;
}

STDMETHODIMP CGridColorScheme::get_GlobalCallback(ICallback **pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	*pVal = globalCallback;
	if( globalCallback != NULL )
	{	
		globalCallback->AddRef();
	}
	return S_OK;
}

STDMETHODIMP CGridColorScheme::put_GlobalCallback(ICallback *newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	put_ComReference(newVal, (IDispatch**)&globalCallback);
	return S_OK;
}

STDMETHODIMP CGridColorScheme::get_Key(BSTR *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	USES_CONVERSION;

	*pVal = OLE2BSTR(key);

	return S_OK;
}

STDMETHODIMP CGridColorScheme::put_Key(BSTR newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	USES_CONVERSION;

	::SysFreeString(key);
	key = OLE2BSTR(newVal);

	return S_OK;
}

#pragma region "Serialization"

// ********************************************************
//     Serialize()
// ********************************************************
STDMETHODIMP CGridColorScheme::Serialize(BSTR* retVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;

	CPLXMLNode* node = SerializeCore("GridColorSchemeClass");
	if (node)
	{
		CString str = CPLSerializeXMLTree(node);	
		*retVal = A2BSTR(str);
	}
	else
	{
		*retVal = A2BSTR("");
	}
	return S_OK;
}

// ********************************************************
//     SerializeCore()
// ********************************************************
CPLXMLNode* CGridColorScheme::SerializeCore(CString ElementName)
{
	USES_CONVERSION;
	
	CPLXMLNode* psTree = CPLCreateXMLNode( NULL, CXT_Element, "GridColorSchemeClass");
	 
	CString str = OLE2CA(key);
	CPLCreateXMLAttributeAndValue(psTree, "Key", str);
	CPLCreateXMLAttributeAndValue(psTree, "NoDataColor", CPLString().Printf("%d", NoDataColor));
	CPLCreateXMLAttributeAndValue(psTree, "LightSourceIntensity", CPLString().Printf("%f", LightSourceIntensity));
	CPLCreateXMLAttributeAndValue(psTree, "AmbientIntensity", CPLString().Printf("%f", AmbientIntensity));
	CPLCreateXMLAttributeAndValue(psTree, "LightSourceElevation", CPLString().Printf("%f", LightSourceElevation));
	CPLCreateXMLAttributeAndValue(psTree, "LightSourceAzimuth", CPLString().Printf("%f", LightSourceAzimuth));
	CPLCreateXMLAttributeAndValue(psTree, "LightSourceI", CPLString().Printf("%f", LightSource.geti()));
	CPLCreateXMLAttributeAndValue(psTree, "LightSourceJ", CPLString().Printf("%f", LightSource.getj()));
	CPLCreateXMLAttributeAndValue(psTree, "LightSourceK", CPLString().Printf("%f", LightSource.getk()));
	
	// color breaks
	if (Breaks.size() > 0)
	{
		CPLXMLNode* psBreaks = CPLCreateXMLNode(psTree, CXT_Element, "GridColorBreaks");
		if (psBreaks)
		{
			for (unsigned int i = 0; i < Breaks.size(); i++)
			{
				CPLXMLNode* psNode = CPLCreateXMLNode(psBreaks, CXT_Element, "GridColorBreakClass");

				OLE_COLOR color;
				Breaks[i]->get_HighColor(&color);
				CPLCreateXMLAttributeAndValue(psNode, "HighColor", CPLString().Printf("%d", color));

				Breaks[i]->get_LowColor(&color);
				CPLCreateXMLAttributeAndValue(psNode, "LowColor", CPLString().Printf("%d", color));

				double val;
				Breaks[i]->get_LowValue(&val);
				CPLCreateXMLAttributeAndValue(psNode, "LowValue", CPLString().Printf("%f", val));

				Breaks[i]->get_HighValue(&val);
				CPLCreateXMLAttributeAndValue(psNode, "HighValue", CPLString().Printf("%f", val));

				BSTR caption;
				Breaks[i]->get_Caption(&caption);
				CPLCreateXMLAttributeAndValue(psNode, "Caption", OLE2CA(caption));
				SysFreeString(caption);
				
				ColoringType colorType;
				Breaks[i]->get_ColoringType(&colorType);
				CPLCreateXMLAttributeAndValue(psNode, "ColoringType", CPLString().Printf("%d", (int)colorType));

				GradientModel gradient;
				Breaks[i]->get_GradientModel(&gradient);
				CPLCreateXMLAttributeAndValue(psNode, "GradientModel", CPLString().Printf("%d", (int)gradient));
				
				BSTR key;
				Breaks[i]->get_Key(&key);
				CPLCreateXMLAttributeAndValue(psNode, "Key", OLE2CA(key));
				SysFreeString(key);
			}
		}
	}
	return psTree;
}

// ********************************************************
//     DeserializeCore()
// ********************************************************
bool CGridColorScheme::DeserializeCore(CPLXMLNode* node)
{
	if (!node)
		return false;
	
	CString s;
	s = CPLGetXMLValue( node, "Key", NULL );
	if (s != "") this->put_Key(A2BSTR(s));

	s = CPLGetXMLValue( node, "NoDataColor", NULL );
	if (s != "") NoDataColor = (OLE_COLOR)atoi(s);

	s = CPLGetXMLValue( node, "LightSourceIntensity", NULL );
	if (s != "") LightSourceIntensity = Utility::atof_custom(s);
	
	s = CPLGetXMLValue( node, "AmbientIntensity", NULL );
	if (s != "") AmbientIntensity = Utility::atof_custom(s);

	s = CPLGetXMLValue( node, "LightSourceElevation", NULL );
	if (s != "") LightSourceElevation = Utility::atof_custom(s);

	s = CPLGetXMLValue( node, "LightSourceAzimuth", NULL );
	if (s != "") LightSourceAzimuth = Utility::atof_custom(s);

	s = CPLGetXMLValue( node, "LightSourceI", NULL );
	if (s != "") LightSource.seti(Utility::atof_custom(s));

	s = CPLGetXMLValue( node, "LightSourceJ", NULL );
	if (s != "") LightSource.setj(Utility::atof_custom(s));

	s = CPLGetXMLValue( node, "LightSourceK", NULL );
	if (s != "") LightSource.setk(Utility::atof_custom(s));
	
	// restoring breaks
	this->Clear();

	node = CPLGetXMLNode(node, "GridColorBreaks");
	if (node)
	{
		node = node->psChild;
		while (node)
		{
			if (strcmp(node->pszValue, "GridColorBreakClass") == 0)
			{
				IGridColorBreak* br = NULL;
				CoCreateInstance(CLSID_GridColorBreak,NULL,CLSCTX_INPROC_SERVER,IID_IGridColorBreak,(void**)&br);
				
				if (br)
				{
					// high color
					OLE_COLOR color = RGB(0,0,0);
					s = CPLGetXMLValue( node, "HighColor", NULL );
					if (s != "") color = (OLE_COLOR)atoi( s );
					br->put_HighColor(color);

					// low color
					color = RGB(0,0,0);
					s = CPLGetXMLValue( node, "LowColor", NULL );
					if (s != "") color = (OLE_COLOR)atoi( s );
					br->put_LowColor(color);

					// low value
					double val = 0.0;
					s = CPLGetXMLValue( node, "LowValue", NULL );
					if (s != "") val = Utility::atof_custom( s );
					br->put_LowValue(val);

					// high value
					val = 0.0;
					s = CPLGetXMLValue( node, "HighValue", NULL );
					if (s != "") val = Utility::atof_custom( s );
					br->put_HighValue(val);

					// caption
					s = CPLGetXMLValue( node, "Caption", NULL );
					br->put_Caption(A2BSTR(s));
					
					// key
					s = CPLGetXMLValue( node, "Key", NULL );
					br->put_Key(A2BSTR(s));
					
					// coloring type
					ColoringType type = Hillshade;
					s = CPLGetXMLValue( node, "ColoringType", NULL );
					if (s != "") type = (ColoringType)atoi( s );
					br->put_ColoringType(type);

					// gradient model
					GradientModel model = Linear;
					s = CPLGetXMLValue( node, "GradientModel", NULL );
					if (s != "") model = (GradientModel)atoi( s );
					br->put_GradientModel(model);

					this->InsertBreak(br);
					br->Release();
				}
			}
			
			node = node->psNext;
		}
	}
	return true;
}

// ********************************************************
//     Deserialize()
// ********************************************************
STDMETHODIMP CGridColorScheme::Deserialize(BSTR newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;

	CString s = OLE2CA(newVal);
	CPLXMLNode* node = CPLParseXMLString(s.GetString());
	if (node)
	{
		node = CPLGetXMLNode(node, "=GridColorSchemeClass");
		if (node)
		{
			this->DeserializeCore(node);
		}
	}
	return S_OK;
}
#pragma endregion

				