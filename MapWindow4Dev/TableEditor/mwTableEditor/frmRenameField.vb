'********************************************************************************************************
'File Name: frmRenameField.vb
'Description: This screen retrieves the parameters for renaming a field.
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

Public Class frmRenameField
    Inherits System.Windows.Forms.Form

    Friend retval As Integer

    <CLSCompliant(False)> _
    Public Sub New(ByVal ShapefileObject As MapWinGIS.Shapefile, ByVal CurrentIndex As Integer)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Dim i As Integer
        btnOK.Enabled = False
        For i = 0 To ShapefileObject.NumFields - 1
            cmbField.Items.Insert(i, ShapefileObject.Field(i).Name)
            If i = CurrentIndex Then
                cmbField.Text = ShapefileObject.Field(i).Name
                btnOK.Enabled = True
            End If
        Next
    End Sub

#Region " Windows Form Designer generated code "

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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbField As System.Windows.Forms.ComboBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtNewName As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRenameField))
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbField = New System.Windows.Forms.ComboBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.txtNewName = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AccessibleDescription = Nothing
        Me.Label1.AccessibleName = Nothing
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Font = Nothing
        Me.Label1.Name = "Label1"
        '
        'cmbField
        '
        Me.cmbField.AccessibleDescription = Nothing
        Me.cmbField.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbField, "cmbField")
        Me.cmbField.BackgroundImage = Nothing
        Me.cmbField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbField.Font = Nothing
        Me.cmbField.Name = "cmbField"
        '
        'btnOK
        '
        Me.btnOK.AccessibleDescription = Nothing
        Me.btnOK.AccessibleName = Nothing
        resources.ApplyResources(Me.btnOK, "btnOK")
        Me.btnOK.BackgroundImage = Nothing
        Me.btnOK.Font = Nothing
        Me.btnOK.Name = "btnOK"
        '
        'btnCancel
        '
        Me.btnCancel.AccessibleDescription = Nothing
        Me.btnCancel.AccessibleName = Nothing
        resources.ApplyResources(Me.btnCancel, "btnCancel")
        Me.btnCancel.BackgroundImage = Nothing
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = Nothing
        Me.btnCancel.Name = "btnCancel"
        '
        'txtNewName
        '
        Me.txtNewName.AccessibleDescription = Nothing
        Me.txtNewName.AccessibleName = Nothing
        resources.ApplyResources(Me.txtNewName, "txtNewName")
        Me.txtNewName.BackgroundImage = Nothing
        Me.txtNewName.Font = Nothing
        Me.txtNewName.Name = "txtNewName"
        '
        'Label2
        '
        Me.Label2.AccessibleDescription = Nothing
        Me.Label2.AccessibleName = Nothing
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Font = Nothing
        Me.Label2.Name = "Label2"
        '
        'frmRenameField
        '
        Me.AcceptButton = Me.btnOK
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.CancelButton = Me.btnCancel
        Me.ControlBox = False
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtNewName)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.cmbField)
        Me.Controls.Add(Me.Label1)
        Me.Font = Nothing
        Me.Icon = Nothing
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRenameField"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub cmbField_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbField.SelectedIndexChanged
        btnOK.Enabled = True
        retval = cmbField.SelectedIndex
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If ValidateNewName() Then
            'If mapwinutility.logger.msg("Warning! This will permanently rename this field. Do you wish to proceed?", MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo, "Confirm rename") = MsgBoxResult.Yes Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            'End If
        Else
            MapWinUtility.Logger.Msg("Please enter only letters, numbers or _ (underscore) characters for the new name, and ensure that the name you have entered has no more than 11 characters", "Attribute Table Editor")
            'txtNewName.Focus()
        End If
    End Sub

    Private Function ValidateNewName() As Boolean
        Dim c As Char

        For Each c In txtNewName.Text
            If (Not Char.IsLetterOrDigit(c) And c <> "_") OrElse txtNewName.Text.Length > 11 Then Return False
        Next
        Return True
    End Function

End Class
