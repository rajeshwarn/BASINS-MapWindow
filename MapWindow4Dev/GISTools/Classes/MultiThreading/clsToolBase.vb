'********************************************************************************************************
'File Name: clsToolBase.vb
'Description: a class which acts as parent for all tools using multithreading
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source GIS Tools Plug-in. 
'
'Contributor(s): (Open source contributors should list themselves and their modifications here).
'30-mar-11 - Sergei Leschinski

'********************************************************************************************************
Imports System.Threading
Imports MapWindow.Interfaces

Public MustInherit Class clsToolBase
    Inherits System.Windows.Forms.Form

    ' Reference to progress form
    Protected _progress As frmProgressNew = Nothing

    ' Back thread to carry calculation in
    Protected _thread As Thread = Nothing

    ''' <summary>
    '''  Creates new instance of the clsToolBase class
    ''' </summary>
    Public Sub New()
        _progress = New frmProgressNew()
        AddHandler _progress.FormClosed, AddressOf DoClose
    End Sub

    
    ' Methods to be implemented in derived classes
    ' Starts the execution of task
    Protected MustOverride Sub Execute()

    ' Handles the finishing of the task
    Protected MustOverride Sub TaskFinished(ByVal success As Boolean, ByVal errorMessage As String)

    ' Handles the closing of the progress form
    Protected MustOverride Sub DoClose(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs)
End Class
