Imports MapWinUtility

Public Class frmOutput
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
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents lblOutputPath As System.Windows.Forms.Label
    Friend WithEvents txtOutputPath As System.Windows.Forms.TextBox
    Friend WithEvents btnOutputPath As System.Windows.Forms.Button
    Friend WithEvents lblFormat As System.Windows.Forms.Label
    Friend WithEvents cmbFormat As System.Windows.Forms.ComboBox
    Friend WithEvents cmbDataType As System.Windows.Forms.ComboBox
    Friend WithEvents lblDataType As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents lblMultiplier As System.Windows.Forms.Label
    Friend WithEvents txtMultiplier As System.Windows.Forms.TextBox
    Friend WithEvents chkAdd As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOutput))
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnNext = New System.Windows.Forms.Button
        Me.lblOutputPath = New System.Windows.Forms.Label
        Me.txtOutputPath = New System.Windows.Forms.TextBox
        Me.btnOutputPath = New System.Windows.Forms.Button
        Me.lblFormat = New System.Windows.Forms.Label
        Me.cmbFormat = New System.Windows.Forms.ComboBox
        Me.cmbDataType = New System.Windows.Forms.ComboBox
        Me.lblDataType = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.lblName = New System.Windows.Forms.Label
        Me.chkAdd = New System.Windows.Forms.CheckBox
        Me.lblMultiplier = New System.Windows.Forms.Label
        Me.txtMultiplier = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
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
        'btnNext
        '
        Me.btnNext.AccessibleDescription = Nothing
        Me.btnNext.AccessibleName = Nothing
        resources.ApplyResources(Me.btnNext, "btnNext")
        Me.btnNext.BackgroundImage = Nothing
        Me.btnNext.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnNext.Font = Nothing
        Me.btnNext.Name = "btnNext"
        '
        'lblOutputPath
        '
        Me.lblOutputPath.AccessibleDescription = Nothing
        Me.lblOutputPath.AccessibleName = Nothing
        resources.ApplyResources(Me.lblOutputPath, "lblOutputPath")
        Me.lblOutputPath.Font = Nothing
        Me.lblOutputPath.Name = "lblOutputPath"
        '
        'txtOutputPath
        '
        Me.txtOutputPath.AccessibleDescription = Nothing
        Me.txtOutputPath.AccessibleName = Nothing
        resources.ApplyResources(Me.txtOutputPath, "txtOutputPath")
        Me.txtOutputPath.BackgroundImage = Nothing
        Me.txtOutputPath.Font = Nothing
        Me.txtOutputPath.Name = "txtOutputPath"
        '
        'btnOutputPath
        '
        Me.btnOutputPath.AccessibleDescription = Nothing
        Me.btnOutputPath.AccessibleName = Nothing
        resources.ApplyResources(Me.btnOutputPath, "btnOutputPath")
        Me.btnOutputPath.BackgroundImage = Nothing
        Me.btnOutputPath.Font = Nothing
        Me.btnOutputPath.Name = "btnOutputPath"
        Me.btnOutputPath.TabStop = False
        '
        'lblFormat
        '
        Me.lblFormat.AccessibleDescription = Nothing
        Me.lblFormat.AccessibleName = Nothing
        resources.ApplyResources(Me.lblFormat, "lblFormat")
        Me.lblFormat.Font = Nothing
        Me.lblFormat.Name = "lblFormat"
        '
        'cmbFormat
        '
        Me.cmbFormat.AccessibleDescription = Nothing
        Me.cmbFormat.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbFormat, "cmbFormat")
        Me.cmbFormat.BackgroundImage = Nothing
        Me.cmbFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFormat.Font = Nothing
        Me.cmbFormat.Items.AddRange(New Object() {resources.GetString("cmbFormat.Items"), resources.GetString("cmbFormat.Items1"), resources.GetString("cmbFormat.Items2")})
        Me.cmbFormat.Name = "cmbFormat"
        '
        'cmbDataType
        '
        Me.cmbDataType.AccessibleDescription = Nothing
        Me.cmbDataType.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbDataType, "cmbDataType")
        Me.cmbDataType.BackgroundImage = Nothing
        Me.cmbDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDataType.Font = Nothing
        Me.cmbDataType.Items.AddRange(New Object() {resources.GetString("cmbDataType.Items"), resources.GetString("cmbDataType.Items1"), resources.GetString("cmbDataType.Items2"), resources.GetString("cmbDataType.Items3")})
        Me.cmbDataType.Name = "cmbDataType"
        '
        'lblDataType
        '
        Me.lblDataType.AccessibleDescription = Nothing
        Me.lblDataType.AccessibleName = Nothing
        resources.ApplyResources(Me.lblDataType, "lblDataType")
        Me.lblDataType.Font = Nothing
        Me.lblDataType.Name = "lblDataType"
        '
        'txtName
        '
        Me.txtName.AccessibleDescription = Nothing
        Me.txtName.AccessibleName = Nothing
        resources.ApplyResources(Me.txtName, "txtName")
        Me.txtName.BackgroundImage = Nothing
        Me.txtName.Font = Nothing
        Me.txtName.Name = "txtName"
        '
        'lblName
        '
        Me.lblName.AccessibleDescription = Nothing
        Me.lblName.AccessibleName = Nothing
        resources.ApplyResources(Me.lblName, "lblName")
        Me.lblName.Font = Nothing
        Me.lblName.Name = "lblName"
        '
        'chkAdd
        '
        Me.chkAdd.AccessibleDescription = Nothing
        Me.chkAdd.AccessibleName = Nothing
        resources.ApplyResources(Me.chkAdd, "chkAdd")
        Me.chkAdd.BackgroundImage = Nothing
        Me.chkAdd.Checked = True
        Me.chkAdd.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAdd.Font = Nothing
        Me.chkAdd.Name = "chkAdd"
        '
        'lblMultiplier
        '
        Me.lblMultiplier.AccessibleDescription = Nothing
        Me.lblMultiplier.AccessibleName = Nothing
        resources.ApplyResources(Me.lblMultiplier, "lblMultiplier")
        Me.lblMultiplier.Font = Nothing
        Me.lblMultiplier.Name = "lblMultiplier"
        '
        'txtMultiplier
        '
        Me.txtMultiplier.AccessibleDescription = Nothing
        Me.txtMultiplier.AccessibleName = Nothing
        resources.ApplyResources(Me.txtMultiplier, "txtMultiplier")
        Me.txtMultiplier.BackgroundImage = Nothing
        Me.txtMultiplier.Font = Nothing
        Me.txtMultiplier.Name = "txtMultiplier"
        '
        'frmOutput
        '
        Me.AcceptButton = Me.btnNext
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.CancelButton = Me.btnCancel
        Me.Controls.Add(Me.txtMultiplier)
        Me.Controls.Add(Me.lblMultiplier)
        Me.Controls.Add(Me.chkAdd)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.cmbDataType)
        Me.Controls.Add(Me.lblDataType)
        Me.Controls.Add(Me.cmbFormat)
        Me.Controls.Add(Me.lblFormat)
        Me.Controls.Add(Me.btnOutputPath)
        Me.Controls.Add(Me.txtOutputPath)
        Me.Controls.Add(Me.lblOutputPath)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnNext)
        Me.Font = Nothing
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOutput"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Cleanup()
        Logger.Dbg("User Cancelled")

        'Must set dialog result or GISTools routines will hang.
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        Logger.Dbg("Checking Details")

        Select Case cmbDataType.Text
            Case "Short Integer (2 bytes)"
                g_newDataType = MapWinGIS.GridDataType.ShortDataType
            Case "Long Integer (4 bytes)"
                g_newDataType = MapWinGIS.GridDataType.LongDataType
            Case "Single Precision Float (4 bytes)"
                g_newDataType = MapWinGIS.GridDataType.FloatDataType
            Case "Double Precision Float (8 bytes)"
                g_newDataType = MapWinGIS.GridDataType.DoubleDataType
            Case Else
                g_newDataType = MapWinGIS.GridDataType.DoubleDataType
        End Select
        Logger.Dbg("NewDataType " & g_newDataType)

        Select Case cmbFormat.Text
            Case "USU Binary (*.bgd)"
                g_newFormat = MapWinGIS.GridFileType.Binary
                g_newExt = ".bgd"
            Case "ASCII (*.asc)"
                g_newFormat = MapWinGIS.GridFileType.Ascii
                g_newExt = ".asc"
            Case "ESRI Binary"
                g_newFormat = MapWinGIS.GridFileType.Esri
                g_newExt = ""
            Case "GeoTIFF (*.tif)"
                g_newFormat = MapWinGIS.GridFileType.GeoTiff
                g_newExt = ".tif"
            Case Else
                g_newFormat = MapWinGIS.GridFileType.Ascii
                g_newExt = ".asc"
        End Select
        Logger.Dbg("NewFormat " & g_newFormat & " NewExt '" & g_newExt & "'")

        If txtOutputPath.Text.Substring(txtOutputPath.Text.Length - 1) = "\" Then
            txtOutputPath.Text = txtOutputPath.Text.Substring(0, txtOutputPath.Text.Length - 1)
        End If

        If (txtOutputPath.Enabled) Then
            g_OutputPath = txtOutputPath.Text
        End If
        Logger.Dbg("OutputPath '" & g_OutputPath & "'")

        If (txtName.Enabled) Then
            g_OutputName = txtName.Text
        End If
        Logger.Dbg("OutputName '" & g_OutputName & "'")

        'Must set dialog result or GISTools routines will hang.
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnOutputPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOutputPath.Click
        Dim fldr As New System.Windows.Forms.FolderBrowserDialog
        fldr.SelectedPath = txtOutputPath.Text
        fldr.Description = "Choose output path"
        If fldr.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtOutputPath.Text = fldr.SelectedPath
        End If
    End Sub

    Private Function ValidateInputs() As Boolean
        btnNext.Enabled = False
        If cmbFormat.Enabled AndAlso cmbFormat.Text = "[Choose File Format]" Then Return False
        If cmbDataType.Enabled AndAlso cmbDataType.Text = "[Choose Data Type]" Then Return False
        If txtOutputPath.Text = "" Then Return False
        If Dir(txtOutputPath.Text, FileAttribute.Directory) = "" Then Return False
        If txtName.Enabled AndAlso txtName.Text = "" Then Return False
        btnNext.Enabled = True
        Return True
    End Function

    Private Sub cmbFormat_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFormat.SelectedIndexChanged
        ValidateInputs()
    End Sub

    Private Sub cmbDataType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDataType.SelectedIndexChanged
        ValidateInputs()
    End Sub

    Private Sub txtOutputPath_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOutputPath.TextChanged
        ValidateInputs()
    End Sub

    Private Sub txtName_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName.Leave
        Try
            If txtName.Text.Contains("\") Then
                mapwinutility.logger.msg("Please choose the path in the 'Output Path' box, not in the 'Output Name' box.", MsgBoxStyle.Exclamation, "Output Path")
                Try
                    txtName.Text = System.IO.Path.GetFileNameWithoutExtension(txtName.Text)
                Catch
                End Try
            End If
            If txtName.Text.Contains(".") Then txtName.Text = System.IO.Path.GetFileNameWithoutExtension(txtName.Text)
        Catch
        End Try
        ValidateInputs()
    End Sub

    Private Sub txtName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtName.TextChanged
        ValidateInputs()
    End Sub

    Public Sub SetOptionsEnabled(ByVal DataTypeEnable As Boolean, ByVal FormatEnable As Boolean, ByVal PathEnable As Boolean, ByVal NameEnable As Boolean, Optional ByVal AddToMapWinEnable As Boolean = True, Optional ByVal ShowMultiplier As Boolean = False)
        cmbDataType.Enabled = DataTypeEnable
        lblDataType.Enabled = DataTypeEnable
        cmbFormat.Enabled = FormatEnable
        lblFormat.Enabled = FormatEnable
        lblOutputPath.Enabled = PathEnable
        txtOutputPath.Enabled = PathEnable
        btnOutputPath.Enabled = PathEnable
        txtName.Enabled = NameEnable
        lblName.Enabled = NameEnable
        chkAdd.Enabled = AddToMapWinEnable

        lblMultiplier.Visible = ShowMultiplier
        txtMultiplier.Visible = ShowMultiplier
    End Sub

    <CLSCompliant(False)> _
    Public Sub SetDefaultDataType(ByVal DataType As MapWinGIS.GridDataType)
        Select Case DataType
            Case MapWinGIS.GridDataType.DoubleDataType
                cmbDataType.SelectedItem = "Double Precision Float (8 bytes)"
            Case MapWinGIS.GridDataType.FloatDataType
                cmbDataType.SelectedItem = "Single Precision Float (4 bytes)"
            Case MapWinGIS.GridDataType.LongDataType
                cmbDataType.SelectedItem = "Long Integer (4 bytes)"
            Case MapWinGIS.GridDataType.ShortDataType
                cmbDataType.SelectedItem = "Short Integer (2 bytes)"
            Case MapWinGIS.GridDataType.UnknownDataType
            Case MapWinGIS.GridDataType.InvalidDataType
        End Select
    End Sub

    <CLSCompliant(False)> _
    Public Sub SetDefaultOutputFormat(ByVal Extension As String)
        If (Extension.ToLower().EndsWith(".asc")) Then
            cmbFormat.SelectedItem = "ASCII (*.asc)"
        ElseIf (Extension.ToLower().EndsWith(".bgd")) Then
            cmbFormat.SelectedItem = "USU Binary (*.bgd)"
        Else
            cmbFormat.SelectedItem = "GeoTIFF (*.tif)"
        End If
    End Sub

    Private Sub OutputForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If cmbDataType.SelectedIndex = -1 Then cmbDataType.SelectedIndex = 0
        If cmbFormat.SelectedIndex = -1 Then cmbFormat.SelectedIndex = 0
        txtOutputPath.Text = CurDir()
        txtName.Focus()
    End Sub

    Private Sub chkAdd_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAdd.CheckedChanged
        g_AddOutputToMW = chkAdd.Checked
    End Sub

    Private Sub txtMultiplier_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMultiplier.TextChanged
        If Not txtMultiplier.Text.Trim() = "" Then
            Dim x As Double = 0
            If Not Double.TryParse(txtMultiplier.Text, x) Then
                mapwinutility.logger.msg("Please enter only numbers in the multiplier field. Leave it at 1 (or 1.0) if no multiplier is needed.", MsgBoxStyle.Information, "Multiplier Value")
            End If
        End If
    End Sub
End Class
