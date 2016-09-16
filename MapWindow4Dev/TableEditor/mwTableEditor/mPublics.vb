'********************************************************************************************************
'File Name: mPublics.vb
'Description: Contains a few public data items, most notably the reference to MapWindow
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
'Utah State University and the Idaho National Engineering and Environmental Lab that were released as 
'public domain in March 2004.  
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'Sept 01 2005: Chris Michaelis cmichaelis@happysquirrel.com - 
'              Replaced the Public Domain table editor with an enhanced version that was
'              contributed by Nathan Eaton at CALM Western Australia. This is released
'              as open source with his permission.

Module mPublics
    Public g_MW As MapWindow.Interfaces.IMapWin
    Public g_IgnoreEvents As Boolean = False

    Friend WithEvents TableEditor As frmTableEditor

    Public Sub TableEditor_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TableEditor.Closing
        TableEditor.Dispose()
        TableEditor = Nothing
        Debug.WriteLine("TableEditor_Closing")
    End Sub
End Module
