'********************************************************************************************************
'File Name: AssemblyInfo.vb
'Description: Contains general assembly information, e.g. versions.
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source. 
'
'The Initial Developer of this version of the Original Code is Daniel P. Ames using portions created by 
'Utah State University and the Idaho National Lab that were released as public domain in March 2004.  
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'Sept 01 2005: Chris Michaelis cmichaelis@happysquirrel.com - 
'              Replaced the Public Domain table editor with an enhanced version that was
'              contributed by Nathan Eaton at CALM Western Australia. This is released
'              as open source with his permission.

Imports System.Reflection
Imports System.Runtime.InteropServices

' General Information about an assembly is controlled through the following 
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

' Review the values of the assembly attributes

<Assembly: AssemblyTitle("Table Editor Plug-in")> 
<Assembly: AssemblyDescription("MapWindow plug-in for editing shapefile attribute tables.")> 
<Assembly: AssemblyCompany("MapWindow OSS Team - www.mapwindow.org")> 
<Assembly: AssemblyProduct("MapWindow GIS - Table Editor")> 
<Assembly: AssemblyCopyright("Copyright (C) 1998-2011 MapWindow OSS Team")> 
<Assembly: AssemblyTrademark("MapWindow GIS is a trademark of Daniel P. Ames, 2005-2011")> 
<Assembly: CLSCompliant(True)> 

'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: Guid("E79333C1-12EE-41D1-92C3-121900D70DF3")> 

' Version information for an assembly consists of the following four values:
'
'      Major Version
'      Minor Version 
'      Build Number
'      Revision
'
' You can specify all the values or you can default the Build and Revision Numbers 
' by using the '*' as shown below:

<Assembly: AssemblyVersion("4.8.6.*")>
<Assembly: AssemblyFileVersion("4.8.6.0")> 
<Assembly: ComVisibleAttribute(False)> 