'********************************************************************************************************
'File Name: frmProgressNew.vb
'Description: shows the operation progress, allows abortion of operation
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
Imports System.ComponentModel

''' <summary>
''' Shows progress information from the back thread provided by Callback class. Works in the interface thread.
''' Sends abortion and finishing events to the forms
''' </summary>
Public Class frmProgressNew

    ' Function prototype for the ExecutionAborted event
    Public Delegate Sub AbortedDelegate()

    ' Fires when user aborts the execution by pressing cancel button, or close button
    Public Event ExecutionAborted As AbortedDelegate

    ' the starting time
    Protected _startTime As Date

    ' a timer to show elapsed time (we shall just measure the time form is shown)
    Protected _timer As Windows.Forms.Timer

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _startTime = DateTime.Now()
        _timer = New Windows.Forms.Timer()
        _timer.Interval = 1000
        
        AddHandler _timer.Tick, AddressOf UpdateTime
    End Sub

    ''' <summary>
    ''' Updating the progress bar
    ''' </summary>
    Public Sub Progress(ByVal Percent As Integer)
        Me.ProgressBar1.Value = Percent
        Me.ProgressBar1.Refresh()
    End Sub

    ''' <summary>
    ''' Cancel the operation (by terminating the thread)
    ''' </summary>
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        btnCancel.Enabled = False
        lblSubject.Text = "Aborting the operation"
        _timer.Stop()
        RaiseEvent ExecutionAborted()
    End Sub

    ''' <summary>
    ''' Updates the elapsed time
    ''' </summary>
    Friend Sub UpdateTime()
        Dim span As TimeSpan = DateTime.Now().Subtract(_startTime)
        If span.Hours > 0 Then
            lblElapsed.Text = "Elapsed: " + String.Format("{0:00}:{1:00}:{2:00}", span.Hours, span.Minutes, span.Seconds)
        Else
            lblElapsed.Text = "Elapsed: " + String.Format("{0:00}:{1:00}", span.Minutes, span.Seconds)
        End If
    End Sub

    Private Sub frmProgressNew_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        _startTime = DateTime.Now()
        _timer.Start()
    End Sub
End Class


