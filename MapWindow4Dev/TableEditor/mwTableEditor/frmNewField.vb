'********************************************************************************************************
'File Name: frmNewField.vb
'Description: This screen is used to specify parameters for adding a new field.
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

Public Class frmNewField
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents txtFieldName As System.Windows.Forms.TextBox
    Friend WithEvents cmbFieldType As System.Windows.Forms.ComboBox
    Friend WithEvents fldWidth As System.Windows.Forms.NumericUpDown
    Friend WithEvents fldPrecision As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents lblType As System.Windows.Forms.Label
    Friend WithEvents lblPrecision As System.Windows.Forms.Label
    Friend WithEvents lblWidth As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNewField))
        Me.txtFieldName = New System.Windows.Forms.TextBox
        Me.cmbFieldType = New System.Windows.Forms.ComboBox
        Me.lblName = New System.Windows.Forms.Label
        Me.lblType = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.fldWidth = New System.Windows.Forms.NumericUpDown
        Me.fldPrecision = New System.Windows.Forms.NumericUpDown
        Me.lblPrecision = New System.Windows.Forms.Label
        Me.lblWidth = New System.Windows.Forms.Label
        CType(Me.fldWidth, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.fldPrecision, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtFieldName
        '
        Me.txtFieldName.AccessibleDescription = Nothing
        Me.txtFieldName.AccessibleName = Nothing
        resources.ApplyResources(Me.txtFieldName, "txtFieldName")
        Me.txtFieldName.BackgroundImage = Nothing
        Me.txtFieldName.Font = Nothing
        Me.txtFieldName.Name = "txtFieldName"
        '
        'cmbFieldType
        '
        Me.cmbFieldType.AccessibleDescription = Nothing
        Me.cmbFieldType.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbFieldType, "cmbFieldType")
        Me.cmbFieldType.BackgroundImage = Nothing
        Me.cmbFieldType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFieldType.Font = Nothing
        Me.cmbFieldType.Items.AddRange(New Object() {resources.GetString("cmbFieldType.Items"), resources.GetString("cmbFieldType.Items1"), resources.GetString("cmbFieldType.Items2")})
        Me.cmbFieldType.Name = "cmbFieldType"
        '
        'lblName
        '
        Me.lblName.AccessibleDescription = Nothing
        Me.lblName.AccessibleName = Nothing
        resources.ApplyResources(Me.lblName, "lblName")
        Me.lblName.Font = Nothing
        Me.lblName.Name = "lblName"
        '
        'lblType
        '
        Me.lblType.AccessibleDescription = Nothing
        Me.lblType.AccessibleName = Nothing
        resources.ApplyResources(Me.lblType, "lblType")
        Me.lblType.Font = Nothing
        Me.lblType.Name = "lblType"
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
        'fldWidth
        '
        Me.fldWidth.AccessibleDescription = Nothing
        Me.fldWidth.AccessibleName = Nothing
        resources.ApplyResources(Me.fldWidth, "fldWidth")
        Me.fldWidth.Font = Nothing
        Me.fldWidth.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.fldWidth.Name = "fldWidth"
        Me.fldWidth.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'fldPrecision
        '
        Me.fldPrecision.AccessibleDescription = Nothing
        Me.fldPrecision.AccessibleName = Nothing
        resources.ApplyResources(Me.fldPrecision, "fldPrecision")
        Me.fldPrecision.Font = Nothing
        Me.fldPrecision.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.fldPrecision.Name = "fldPrecision"
        Me.fldPrecision.Value = New Decimal(New Integer() {3, 0, 0, 0})
        '
        'lblPrecision
        '
        Me.lblPrecision.AccessibleDescription = Nothing
        Me.lblPrecision.AccessibleName = Nothing
        resources.ApplyResources(Me.lblPrecision, "lblPrecision")
        Me.lblPrecision.Font = Nothing
        Me.lblPrecision.Name = "lblPrecision"
        '
        'lblWidth
        '
        Me.lblWidth.AccessibleDescription = Nothing
        Me.lblWidth.AccessibleName = Nothing
        resources.ApplyResources(Me.lblWidth, "lblWidth")
        Me.lblWidth.Font = Nothing
        Me.lblWidth.Name = "lblWidth"
        '
        'frmNewField
        '
        Me.AcceptButton = Me.btnOK
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.CancelButton = Me.btnCancel
        Me.Controls.Add(Me.lblWidth)
        Me.Controls.Add(Me.lblPrecision)
        Me.Controls.Add(Me.fldPrecision)
        Me.Controls.Add(Me.fldWidth)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lblType)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.cmbFieldType)
        Me.Controls.Add(Me.txtFieldName)
        Me.Font = Nothing
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmNewField"
        CType(Me.fldWidth, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.fldPrecision, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If ValidateForm() Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Hide()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Hide()
    End Sub

    Private Sub cmbFieldType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFieldType.SelectedIndexChanged
        fldPrecision.Enabled = (cmbFieldType.Text = "Double")
        lblPrecision.Enabled = (cmbFieldType.Text = "Double")
    End Sub

    Private Function ValidateForm() As Boolean
        Dim retval As Boolean = True
        Dim err_msg As String = "Errors:" & vbCrLf & "-------"

        If txtFieldName.Text = String.Empty OrElse txtFieldName.Text.Length < 1 Then
            err_msg &= vbCrLf & "Invalid field name"
            retval = False
        End If
        If cmbFieldType.Text = "[Select one]" Then
            err_msg &= vbCrLf & "Please select a type"
            retval = False
        End If

        If retval = False Then mapwinutility.logger.msg(err_msg)
        Return retval
    End Function

End Class
