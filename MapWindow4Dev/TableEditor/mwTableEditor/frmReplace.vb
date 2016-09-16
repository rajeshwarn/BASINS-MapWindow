'********************************************************************************************************
'File Name: frmReplace.vb
'Description: This is a find-and-replace style screen.
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

Public Class frmReplace
    Inherits System.Windows.Forms.Form
    Public Tableform As frmTableEditor
#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Find_tb As System.Windows.Forms.TextBox
    Friend WithEvents Replace_tb As System.Windows.Forms.TextBox
    Friend WithEvents Replace_bn As System.Windows.Forms.Button
    Friend WithEvents cancel_bn As System.Windows.Forms.Button
    Friend WithEvents label34 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReplace))
        Me.Find_tb = New System.Windows.Forms.TextBox
        Me.Replace_tb = New System.Windows.Forms.TextBox
        Me.Replace_bn = New System.Windows.Forms.Button
        Me.cancel_bn = New System.Windows.Forms.Button
        Me.label34 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Find_tb
        '
        Me.Find_tb.AccessibleDescription = Nothing
        Me.Find_tb.AccessibleName = Nothing
        resources.ApplyResources(Me.Find_tb, "Find_tb")
        Me.Find_tb.BackgroundImage = Nothing
        Me.Find_tb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Find_tb.Font = Nothing
        Me.Find_tb.Name = "Find_tb"
        '
        'Replace_tb
        '
        Me.Replace_tb.AccessibleDescription = Nothing
        Me.Replace_tb.AccessibleName = Nothing
        resources.ApplyResources(Me.Replace_tb, "Replace_tb")
        Me.Replace_tb.BackgroundImage = Nothing
        Me.Replace_tb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Replace_tb.Font = Nothing
        Me.Replace_tb.Name = "Replace_tb"
        '
        'Replace_bn
        '
        Me.Replace_bn.AccessibleDescription = Nothing
        Me.Replace_bn.AccessibleName = Nothing
        resources.ApplyResources(Me.Replace_bn, "Replace_bn")
        Me.Replace_bn.BackgroundImage = Nothing
        Me.Replace_bn.Font = Nothing
        Me.Replace_bn.Name = "Replace_bn"
        '
        'cancel_bn
        '
        Me.cancel_bn.AccessibleDescription = Nothing
        Me.cancel_bn.AccessibleName = Nothing
        resources.ApplyResources(Me.cancel_bn, "cancel_bn")
        Me.cancel_bn.BackgroundImage = Nothing
        Me.cancel_bn.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cancel_bn.Font = Nothing
        Me.cancel_bn.Name = "cancel_bn"
        '
        'label34
        '
        Me.label34.AccessibleDescription = Nothing
        Me.label34.AccessibleName = Nothing
        resources.ApplyResources(Me.label34, "label34")
        Me.label34.Font = Nothing
        Me.label34.Name = "label34"
        '
        'Label1
        '
        Me.Label1.AccessibleDescription = Nothing
        Me.Label1.AccessibleName = Nothing
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Font = Nothing
        Me.Label1.Name = "Label1"
        '
        'frmReplace
        '
        Me.AcceptButton = Me.Replace_bn
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.CancelButton = Me.cancel_bn
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.label34)
        Me.Controls.Add(Me.cancel_bn)
        Me.Controls.Add(Me.Replace_bn)
        Me.Controls.Add(Me.Replace_tb)
        Me.Controls.Add(Me.Find_tb)
        Me.Font = Nothing
        Me.Name = "frmReplace"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    '***Added by Nathan Eaton, CALM 02/05***'

    Private Sub Replace_bn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Replace_bn.Click
        Dim find_string, replace_string As String

        find_string = Me.Find_tb.Text
        replace_string = Me.Replace_tb.Text

        'send teh find string and the replace string to the routine that will perform the operation
        Tableform.Replace_sub(find_string, replace_string)

        Me.Close()
    End Sub

    Private Sub cancel_bn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancel_bn.Click
        Me.Close()
    End Sub
End Class
