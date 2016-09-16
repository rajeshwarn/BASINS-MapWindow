'********************************************************************************************************
'File Name: clsCallback.vb
'Description: callback class to report progress and cancel operation. Works in the back thread.
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
'31-mar-11 - Sergei Leschinski
'********************************************************************************************************

Imports System.Threading
Imports System.ComponentModel

''' <summary>
''' Callback class. Shows progress information from back thread to the interface thread. Works in the back thread.
''' </summary>
Public Class Callback
    Implements MapWinGIS.ICallback, MapWinGIS.IStopExecution

    ' Reference to the form with progress bar, Form class supports ISynchronizeInvoke interface
    Dim _syncOject As ISynchronizeInvoke = Nothing

    ' Refrence to the instance the progress window
    Dim _progress As frmProgressNew = Nothing

    ' The operation was aborted by user
    Dim _canceled As Boolean = False

    ' The prototype of function for showing progress
    Public Delegate Sub ProgressChangesDelegate(ByVal Percent As Integer)

    ''' <summary>
    ''' Returns true in case you has pressed the Cancel button, to enable appropriate message in the tool form
    ''' </summary>
    Friend ReadOnly Property Canceled() As Boolean
        Get
            Return _canceled
        End Get
    End Property

    ''' <summary>
    ''' Creates a new instance of the Callback class. Reference to the progress form is passed (it's executed in the interface thread)
    ''' </summary>
    Public Sub New(ByRef progress As frmProgressNew)
        _progress = progress
        AddHandler _progress.ExecutionAborted, AddressOf DoCancel
        _syncOject = CType(progress, ISynchronizeInvoke)
    End Sub

    ''' <summary>
    ''' Passing progress to the interface thread
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Progress(ByVal KeyOfSender As String, ByVal Percent As Integer, ByVal Message As String) Implements MapWinGIS.ICallback.Progress
        Dim args(0) As Object
        args(0) = Percent
        ' we are in the back thread, so can't update progress bar directly
        _syncOject.BeginInvoke(New ProgressChangesDelegate(AddressOf _progress.Progress), args)
    End Sub

    ''' <summary>
    ''' Implementing ICallback interface
    ''' </summary>
    Public Sub ErrorOccured(ByVal KeyOfSender As String, ByVal ErrorMsg As String) Implements MapWinGIS.ICallback.Error
        ' do nothing
    End Sub

    ''' <summary>
    ''' Cancel the operation
    ''' </summary>
    Private Sub DoCancel()
        _canceled = True
    End Sub

    ''' <summary>
    ''' Implement IStopExecution interface
    ''' </summary>
    Public Function StopFunction() As Boolean Implements MapWinGIS.IStopExecution.StopFunction
        Return _canceled
    End Function
End Class
