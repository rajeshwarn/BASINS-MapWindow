'********************************************************************************************************
'File Name: frmGenerateContour.vb
'Description: Form to allow calculation of the area of the polygons in a shapefile. Area is then placed in a shapefile field.
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
'The Initial Developer of this version of the Original Code is Christopher Michaelis.
'The area computation for latitute and longitude was taken from code contributed by
'neztypezero of the mapwindow forums.
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'
'********************************************************************************************************

Imports MapWindow

Public Class frmGenerateContour
    Inherits System.Windows.Forms.Form


#Region " Windows Form Designer generated code "

    Private m_MapWin As MapWindow.Interfaces.IMapWin
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents GroupContourOptions As System.Windows.Forms.GroupBox
    Friend WithEvents chkAddToMap As System.Windows.Forms.CheckBox
    Friend WithEvents chb3D As System.Windows.Forms.CheckBox
    Friend WithEvents txtInterval As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtNoData As System.Windows.Forms.TextBox
    Friend WithEvents lblWarning As System.Windows.Forms.Label
    Private inputGridFile As String = ""

    <CLSCompliant(False)> _
    Public Sub New(ByVal IMapWin As MapWindow.Interfaces.IMapWin)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        m_MapWin = IMapWin

        cmbGrids.Items.Clear()
        For i As Integer = 0 To m_MapWin.Layers.NumLayers - 1
            If m_MapWin.Layers(m_MapWin.Layers.GetHandle(i)).LayerType = MapWindow.Interfaces.eLayerType.Grid Then
                cmbGrids.Items.Add(m_MapWin.Layers(m_MapWin.Layers.GetHandle(i)).Name)
                If (m_MapWin.Layers.CurrentLayer = m_MapWin.Layers(m_MapWin.Layers.GetHandle(i)).Handle) Then
                    cmbGrids.SelectedIndex = cmbGrids.Items.Count - 1
                End If
            End If
        Next

        If Not cmbGrids.Items.Count = 0 And cmbGrids.SelectedIndex = -1 Then cmbGrids.SelectedIndex = 0
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupInputRaster As System.Windows.Forms.GroupBox
    Friend WithEvents cmbGrids As System.Windows.Forms.ComboBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rdUseLoaded As System.Windows.Forms.RadioButton
    Friend WithEvents rdUseExternal As System.Windows.Forms.RadioButton
    Friend WithEvents txtOutputSF As System.Windows.Forms.TextBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents lblFilen As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rdCI As System.Windows.Forms.RadioButton
    Friend WithEvents rdFl As System.Windows.Forms.RadioButton
    Friend WithEvents txtFLInterval As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGenerateContour))
        Me.Label1 = New System.Windows.Forms.Label
        Me.GroupInputRaster = New System.Windows.Forms.GroupBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lblFilen = New System.Windows.Forms.Label
        Me.rdUseExternal = New System.Windows.Forms.RadioButton
        Me.rdUseLoaded = New System.Windows.Forms.RadioButton
        Me.cmbGrids = New System.Windows.Forms.ComboBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtOutputSF = New System.Windows.Forms.TextBox
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.GroupContourOptions = New System.Windows.Forms.GroupBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.rdCI = New System.Windows.Forms.RadioButton
        Me.txtFLInterval = New System.Windows.Forms.TextBox
        Me.rdFl = New System.Windows.Forms.RadioButton
        Me.txtInterval = New System.Windows.Forms.TextBox
        Me.lblWarning = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtNoData = New System.Windows.Forms.TextBox
        Me.chkAddToMap = New System.Windows.Forms.CheckBox
        Me.chb3D = New System.Windows.Forms.CheckBox
        Me.GroupInputRaster.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupContourOptions.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AccessibleDescription = Nothing
        Me.Label1.AccessibleName = Nothing
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Font = Nothing
        Me.Label1.Name = "Label1"
        Me.ToolTip1.SetToolTip(Me.Label1, resources.GetString("Label1.ToolTip"))
        '
        'GroupInputRaster
        '
        Me.GroupInputRaster.AccessibleDescription = Nothing
        Me.GroupInputRaster.AccessibleName = Nothing
        resources.ApplyResources(Me.GroupInputRaster, "GroupInputRaster")
        Me.GroupInputRaster.BackgroundImage = Nothing
        Me.GroupInputRaster.Controls.Add(Me.Panel1)
        Me.GroupInputRaster.Font = Nothing
        Me.GroupInputRaster.Name = "GroupInputRaster"
        Me.GroupInputRaster.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupInputRaster, resources.GetString("GroupInputRaster.ToolTip"))
        '
        'Panel1
        '
        Me.Panel1.AccessibleDescription = Nothing
        Me.Panel1.AccessibleName = Nothing
        resources.ApplyResources(Me.Panel1, "Panel1")
        Me.Panel1.BackgroundImage = Nothing
        Me.Panel1.Controls.Add(Me.lblFilen)
        Me.Panel1.Controls.Add(Me.rdUseExternal)
        Me.Panel1.Controls.Add(Me.rdUseLoaded)
        Me.Panel1.Controls.Add(Me.cmbGrids)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Font = Nothing
        Me.Panel1.Name = "Panel1"
        Me.ToolTip1.SetToolTip(Me.Panel1, resources.GetString("Panel1.ToolTip"))
        '
        'lblFilen
        '
        Me.lblFilen.AccessibleDescription = Nothing
        Me.lblFilen.AccessibleName = Nothing
        resources.ApplyResources(Me.lblFilen, "lblFilen")
        Me.lblFilen.Font = Nothing
        Me.lblFilen.Name = "lblFilen"
        Me.ToolTip1.SetToolTip(Me.lblFilen, resources.GetString("lblFilen.ToolTip"))
        '
        'rdUseExternal
        '
        Me.rdUseExternal.AccessibleDescription = Nothing
        Me.rdUseExternal.AccessibleName = Nothing
        resources.ApplyResources(Me.rdUseExternal, "rdUseExternal")
        Me.rdUseExternal.BackgroundImage = Nothing
        Me.rdUseExternal.Font = Nothing
        Me.rdUseExternal.Name = "rdUseExternal"
        Me.ToolTip1.SetToolTip(Me.rdUseExternal, resources.GetString("rdUseExternal.ToolTip"))
        '
        'rdUseLoaded
        '
        Me.rdUseLoaded.AccessibleDescription = Nothing
        Me.rdUseLoaded.AccessibleName = Nothing
        resources.ApplyResources(Me.rdUseLoaded, "rdUseLoaded")
        Me.rdUseLoaded.BackgroundImage = Nothing
        Me.rdUseLoaded.Font = Nothing
        Me.rdUseLoaded.Name = "rdUseLoaded"
        Me.ToolTip1.SetToolTip(Me.rdUseLoaded, resources.GetString("rdUseLoaded.ToolTip"))
        '
        'cmbGrids
        '
        Me.cmbGrids.AccessibleDescription = Nothing
        Me.cmbGrids.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbGrids, "cmbGrids")
        Me.cmbGrids.BackgroundImage = Nothing
        Me.cmbGrids.Font = Nothing
        Me.cmbGrids.Name = "cmbGrids"
        Me.ToolTip1.SetToolTip(Me.cmbGrids, resources.GetString("cmbGrids.ToolTip"))
        '
        'Button1
        '
        Me.Button1.AccessibleDescription = Nothing
        Me.Button1.AccessibleName = Nothing
        resources.ApplyResources(Me.Button1, "Button1")
        Me.Button1.BackgroundImage = Nothing
        Me.Button1.Font = Nothing
        Me.Button1.Name = "Button1"
        Me.ToolTip1.SetToolTip(Me.Button1, resources.GetString("Button1.ToolTip"))
        '
        'Label3
        '
        Me.Label3.AccessibleDescription = Nothing
        Me.Label3.AccessibleName = Nothing
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Font = Nothing
        Me.Label3.Name = "Label3"
        Me.ToolTip1.SetToolTip(Me.Label3, resources.GetString("Label3.ToolTip"))
        '
        'txtOutputSF
        '
        Me.txtOutputSF.AccessibleDescription = Nothing
        Me.txtOutputSF.AccessibleName = Nothing
        resources.ApplyResources(Me.txtOutputSF, "txtOutputSF")
        Me.txtOutputSF.BackgroundImage = Nothing
        Me.txtOutputSF.Font = Nothing
        Me.txtOutputSF.Name = "txtOutputSF"
        Me.txtOutputSF.ReadOnly = True
        Me.ToolTip1.SetToolTip(Me.txtOutputSF, resources.GetString("txtOutputSF.ToolTip"))
        '
        'Button2
        '
        Me.Button2.AccessibleDescription = Nothing
        Me.Button2.AccessibleName = Nothing
        resources.ApplyResources(Me.Button2, "Button2")
        Me.Button2.BackgroundImage = Nothing
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button2.Font = Nothing
        Me.Button2.Name = "Button2"
        Me.ToolTip1.SetToolTip(Me.Button2, resources.GetString("Button2.ToolTip"))
        '
        'Button3
        '
        Me.Button3.AccessibleDescription = Nothing
        Me.Button3.AccessibleName = Nothing
        resources.ApplyResources(Me.Button3, "Button3")
        Me.Button3.BackgroundImage = Nothing
        Me.Button3.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button3.Font = Nothing
        Me.Button3.Name = "Button3"
        Me.ToolTip1.SetToolTip(Me.Button3, resources.GetString("Button3.ToolTip"))
        '
        'Button4
        '
        Me.Button4.AccessibleDescription = Nothing
        Me.Button4.AccessibleName = Nothing
        resources.ApplyResources(Me.Button4, "Button4")
        Me.Button4.BackgroundImage = Nothing
        Me.Button4.Font = Nothing
        Me.Button4.Name = "Button4"
        Me.ToolTip1.SetToolTip(Me.Button4, resources.GetString("Button4.ToolTip"))
        '
        'GroupContourOptions
        '
        Me.GroupContourOptions.AccessibleDescription = Nothing
        Me.GroupContourOptions.AccessibleName = Nothing
        resources.ApplyResources(Me.GroupContourOptions, "GroupContourOptions")
        Me.GroupContourOptions.BackgroundImage = Nothing
        Me.GroupContourOptions.Controls.Add(Me.Panel2)
        Me.GroupContourOptions.Controls.Add(Me.lblWarning)
        Me.GroupContourOptions.Controls.Add(Me.Label4)
        Me.GroupContourOptions.Controls.Add(Me.txtNoData)
        Me.GroupContourOptions.Controls.Add(Me.chkAddToMap)
        Me.GroupContourOptions.Controls.Add(Me.chb3D)
        Me.GroupContourOptions.Font = Nothing
        Me.GroupContourOptions.Name = "GroupContourOptions"
        Me.GroupContourOptions.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupContourOptions, resources.GetString("GroupContourOptions.ToolTip"))
        '
        'Panel2
        '
        Me.Panel2.AccessibleDescription = Nothing
        Me.Panel2.AccessibleName = Nothing
        resources.ApplyResources(Me.Panel2, "Panel2")
        Me.Panel2.BackgroundImage = Nothing
        Me.Panel2.Controls.Add(Me.rdCI)
        Me.Panel2.Controls.Add(Me.txtFLInterval)
        Me.Panel2.Controls.Add(Me.rdFl)
        Me.Panel2.Controls.Add(Me.txtInterval)
        Me.Panel2.Font = Nothing
        Me.Panel2.Name = "Panel2"
        Me.ToolTip1.SetToolTip(Me.Panel2, resources.GetString("Panel2.ToolTip"))
        '
        'rdCI
        '
        Me.rdCI.AccessibleDescription = Nothing
        Me.rdCI.AccessibleName = Nothing
        resources.ApplyResources(Me.rdCI, "rdCI")
        Me.rdCI.BackgroundImage = Nothing
        Me.rdCI.Font = Nothing
        Me.rdCI.Name = "rdCI"
        Me.ToolTip1.SetToolTip(Me.rdCI, resources.GetString("rdCI.ToolTip"))
        '
        'txtFLInterval
        '
        Me.txtFLInterval.AccessibleDescription = Nothing
        Me.txtFLInterval.AccessibleName = Nothing
        resources.ApplyResources(Me.txtFLInterval, "txtFLInterval")
        Me.txtFLInterval.BackgroundImage = Nothing
        Me.txtFLInterval.Font = Nothing
        Me.txtFLInterval.Name = "txtFLInterval"
        Me.ToolTip1.SetToolTip(Me.txtFLInterval, resources.GetString("txtFLInterval.ToolTip"))
        '
        'rdFl
        '
        Me.rdFl.AccessibleDescription = Nothing
        Me.rdFl.AccessibleName = Nothing
        resources.ApplyResources(Me.rdFl, "rdFl")
        Me.rdFl.BackgroundImage = Nothing
        Me.rdFl.Font = Nothing
        Me.rdFl.Name = "rdFl"
        Me.ToolTip1.SetToolTip(Me.rdFl, resources.GetString("rdFl.ToolTip"))
        '
        'txtInterval
        '
        Me.txtInterval.AccessibleDescription = Nothing
        Me.txtInterval.AccessibleName = Nothing
        resources.ApplyResources(Me.txtInterval, "txtInterval")
        Me.txtInterval.BackgroundImage = Nothing
        Me.txtInterval.Font = Nothing
        Me.txtInterval.Name = "txtInterval"
        Me.ToolTip1.SetToolTip(Me.txtInterval, resources.GetString("txtInterval.ToolTip"))
        '
        'lblWarning
        '
        Me.lblWarning.AccessibleDescription = Nothing
        Me.lblWarning.AccessibleName = Nothing
        resources.ApplyResources(Me.lblWarning, "lblWarning")
        Me.lblWarning.Name = "lblWarning"
        Me.ToolTip1.SetToolTip(Me.lblWarning, resources.GetString("lblWarning.ToolTip"))
        '
        'Label4
        '
        Me.Label4.AccessibleDescription = Nothing
        Me.Label4.AccessibleName = Nothing
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Font = Nothing
        Me.Label4.Name = "Label4"
        Me.ToolTip1.SetToolTip(Me.Label4, resources.GetString("Label4.ToolTip"))
        '
        'txtNoData
        '
        Me.txtNoData.AccessibleDescription = Nothing
        Me.txtNoData.AccessibleName = Nothing
        resources.ApplyResources(Me.txtNoData, "txtNoData")
        Me.txtNoData.BackgroundImage = Nothing
        Me.txtNoData.Font = Nothing
        Me.txtNoData.Name = "txtNoData"
        Me.ToolTip1.SetToolTip(Me.txtNoData, resources.GetString("txtNoData.ToolTip"))
        '
        'chkAddToMap
        '
        Me.chkAddToMap.AccessibleDescription = Nothing
        Me.chkAddToMap.AccessibleName = Nothing
        resources.ApplyResources(Me.chkAddToMap, "chkAddToMap")
        Me.chkAddToMap.BackgroundImage = Nothing
        Me.chkAddToMap.Checked = True
        Me.chkAddToMap.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAddToMap.Font = Nothing
        Me.chkAddToMap.Name = "chkAddToMap"
        Me.ToolTip1.SetToolTip(Me.chkAddToMap, resources.GetString("chkAddToMap.ToolTip"))
        '
        'chb3D
        '
        Me.chb3D.AccessibleDescription = Nothing
        Me.chb3D.AccessibleName = Nothing
        resources.ApplyResources(Me.chb3D, "chb3D")
        Me.chb3D.BackgroundImage = Nothing
        Me.chb3D.Font = Nothing
        Me.chb3D.Name = "chb3D"
        Me.ToolTip1.SetToolTip(Me.chb3D, resources.GetString("chb3D.ToolTip"))
        '
        'frmGenerateContour
        '
        Me.AcceptButton = Me.Button2
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.CancelButton = Me.Button3
        Me.Controls.Add(Me.GroupInputRaster)
        Me.Controls.Add(Me.GroupContourOptions)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.txtOutputSF)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Font = Nothing
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmGenerateContour"
        Me.ToolTip1.SetToolTip(Me, resources.GetString("$this.ToolTip"))
        Me.GroupInputRaster.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.GroupContourOptions.ResumeLayout(False)
        Me.GroupContourOptions.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Function GetArr(ByRef dblArr() As Double) As Boolean
        Dim strArr() As String
        Dim i As Integer
        strArr = txtFLInterval.Text.Split(" ")
        ReDim dblArr(strArr.GetUpperBound(0))
        For i = 0 To strArr.GetUpperBound(0)
            If Not IsNumeric(strArr(i)) Then
                mapwinutility.logger.msg("Please specify only numbers for the fixed level contour values.", MsgBoxStyle.Exclamation, "Only Numbers in Fixed Level Intervals")
                GetArr = False
                Exit Function
            End If
            dblArr(i) = Convert.ToDouble(strArr(i))
        Next
        GetArr = True
    End Function
    Private Sub frmGenerateContour_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        rdUseLoaded.Checked = True
        rdCI.Checked = True
        txtFLInterval.Enabled = False
    End Sub

    Private Sub rdUseExternal_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdUseExternal.CheckedChanged
        Button1.Enabled = rdUseExternal.Checked
        If inputGridFile = "" Then Button1_Click(Nothing, Nothing)
    End Sub

    Private Sub rdUseLoaded_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdUseLoaded.CheckedChanged
        cmbGrids.Enabled = rdUseLoaded.Checked
        lblFilen.Text = ""
    End Sub

    Private Sub cmbGrids_IndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbGrids.SelectedIndexChanged
        inputGridFile = ""

        For i As Integer = 0 To m_MapWin.Layers.NumLayers - 1
            If m_MapWin.Layers(m_MapWin.Layers.GetHandle(i)).LayerType = Interfaces.eLayerType.Grid And m_MapWin.Layers(m_MapWin.Layers.GetHandle(i)).Name = cmbGrids.Text Then
                Dim g As MapWinGIS.Grid = m_MapWin.Layers(m_MapWin.Layers.GetHandle(i)).GetGridObject()
                inputGridFile = g.Filename
                txtNoData.Text = g.Header.NodataValue
                g = Nothing
                Exit For
            End If
        Next

        lblFilen.Text = inputGridFile

        If txtOutputSF.Text = "" Then
            txtOutputSF.Text = System.IO.Path.GetDirectoryName(inputGridFile) + "\" + System.IO.Path.GetFileNameWithoutExtension(inputGridFile) + "_Contour.shp"
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim ofd As New Windows.Forms.OpenFileDialog
        Dim newgrd As New MapWinGIS.Grid
        ofd.Filter = newgrd.CdlgFilter
        ofd.ShowDialog(g_MapWindowForm)

        If ofd.FileName = "" Then Exit Sub

        inputGridFile = ofd.FileName

        lblFilen.Text = Microsoft.VisualBasic.Left(ofd.FileName, 12) & "..."

        Dim g As New MapWinGIS.Grid
        g.Open(ofd.FileName, MapWinGIS.GridDataType.UnknownDataType, False, MapWinGIS.GridFileType.UseExtension)
        txtNoData.Text = g.Header.NodataValue
        g.Close()

        If txtOutputSF.Text = "" Then
            txtOutputSF.Text = System.IO.Path.GetDirectoryName(inputGridFile) + "\" + System.IO.Path.GetFileNameWithoutExtension(inputGridFile) + "_Contour.shp"
        End If
        Me.Activate()

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim dblArray() As Double = Nothing
        Dim boolRes As Boolean
        Try
            If Not Me.rdUseExternal.Checked And Not Me.rdUseLoaded.Checked Then
                mapwinutility.logger.msg("Please select a loaded grid layer or click the folder icon to browse to a file before clicking Process.", MsgBoxStyle.Exclamation, "Choose a File First")
                Exit Sub
            End If

            If inputGridFile = "" Then
                mapwinutility.logger.msg("Please select a loaded grid layer or click the folder icon to browse to a file before clicking Process.", MsgBoxStyle.Exclamation, "Choose a File First")
                Exit Sub
            End If

            If Me.txtOutputSF.Text = "" Then
                mapwinutility.logger.msg("Please select an output shapefile by clicking the folder icon.", MsgBoxStyle.Exclamation, "Choose an Output File")
                Exit Sub
            End If

            If rdCI.Checked Then
                If Not IsNumeric(txtInterval.Text) Then
                    mapwinutility.logger.msg("Please specify only numbers for the interval.", MsgBoxStyle.Exclamation, "Only Numbers in Interval")
                    Exit Sub
                End If
            Else
                If Not GetArr(dblArray) Then
                    Exit Sub
                End If
            End If

            If Not IsNumeric(txtNoData.Text) Then
                mapwinutility.logger.msg("Please specify only numbers for the nodata value. Zero indicates automatic.", MsgBoxStyle.Exclamation, "Only Numbers in NoData")
                Exit Sub
            End If

            Try
                If System.IO.File.Exists(txtOutputSF.Text) Then
                    If (mapwinutility.logger.msg(txtOutputSF.Text.ToString + " already exists. Do you want to replace it?", MsgBoxStyle.OkCancel, "Save As") = Microsoft.VisualBasic.MsgBoxResult.Ok) Then
                        Kill(txtOutputSF.Text)
                        Kill(Mid(txtOutputSF.Text, 1, Len(txtOutputSF.Text) - 4) & ".shx")
                        Kill(Mid(txtOutputSF.Text, 1, Len(txtOutputSF.Text) - 4) & ".dbf")
                    Else
                        Me.Cursor = Windows.Forms.Cursors.Default
                        Me.Close()
                        Exit Sub
                    End If
                End If
            Catch ex As Exception
                mapwinutility.logger.msg(ex.Message.ToString)
                Me.Cursor = Windows.Forms.Cursors.Default
                Me.Close()
                Exit Sub
            End Try

            Me.Cursor = Windows.Forms.Cursors.WaitCursor

            Dim actualInput As String = inputGridFile

            'Rob Cairns 4-Mar-06: There is a problem with this function somewhere
            'It shifts the contour half a pixel up and to the right
            'The latest gdal build recognises .asc format so I have made a temporary
            'fix by circumventing this conversion for AAIGrid formats
            If inputGridFile.ToLower().EndsWith("arc") Or inputGridFile.ToLower().EndsWith("bgd") Then ' inputGridFile.ToLower().EndsWith("asc") Or 
                'If it's not a GDAL-compatible format, must interchange the filename
                actualInput = System.IO.Path.GetDirectoryName(inputGridFile) + "\" + System.IO.Path.GetFileNameWithoutExtension(inputGridFile) + ".tif"

                Try
                    Dim formatChange As New MapWinGIS.Grid
                    formatChange.Open(inputGridFile, MapWinGIS.GridDataType.UnknownDataType, True, MapWinGIS.GridFileType.UseExtension)
                    formatChange.Save(actualInput, MapWinGIS.GridFileType.GeoTiff)
                    formatChange.Close()
                Catch ex As Exception
                    If Not System.IO.File.Exists(actualInput) Then
                        mapwinutility.logger.msg("Aborting: An error occurred while translating the grid. The error follows." + vbCrLf + vbCrLf + ex.ToString)
                        Exit Sub
                    End If
                End Try
            End If

            'Do it
            Dim u As New MapWinGIS.Utils

            If rdCI.Checked Then
                boolRes = u.GenerateContour(actualInput, txtOutputSF.Text, Double.Parse(txtInterval.Text), Double.Parse(txtNoData.Text), chb3D.Checked, 0.0)
            Else
                boolRes = u.GenerateContour(actualInput, txtOutputSF.Text, 0.0, Double.Parse(txtNoData.Text), chb3D.Checked, dblArray)
            End If

            If boolRes Then
                MapWinUtility.Logger.Msg("Done! The contour shapefile has been generated.", MsgBoxStyle.Information, "Done")
                If chkAddToMap.Checked And System.IO.File.Exists(txtOutputSF.Text) Then m_MapWin.Layers.Add(txtOutputSF.Text)
            Else
                MapWinUtility.Logger.Msg("An error occurred generating the contour.", MsgBoxStyle.Information, "Failure")
            End If

            'Clean up
            If Not actualInput = inputGridFile Then
                Try
                    Kill(actualInput)
                Catch
                End Try
            End If

            Me.Cursor = Windows.Forms.Cursors.Default
            Me.Close()
        Catch ex As Exception
        End Try

        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim ofd As New Windows.Forms.SaveFileDialog
        Dim newsf As New MapWinGIS.Shapefile
        ofd.Filter = newsf.CdlgFilter
        ofd.ShowDialog(g_MapWindowForm)

        If ofd.FileName = "" Then Exit Sub
        txtOutputSF.Text = ofd.FileName

        Try
            If System.IO.File.Exists(txtOutputSF.Text) Then
                Kill(txtOutputSF.Text)
                Kill(Mid(txtOutputSF.Text, 1, Len(txtOutputSF.Text) - 4) & ".shx")
                Kill(Mid(txtOutputSF.Text, 1, Len(txtOutputSF.Text) - 4) & ".dbf")
            End If
        Catch ex As Exception
        End Try
        Me.Activate()

    End Sub

    Private Sub chb3D_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chb3D.CheckedChanged
        lblWarning.Visible = chb3D.Checked
    End Sub

    Private Sub rdCI_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdCI.CheckedChanged
        txtInterval.Enabled = rdCI.Checked
    End Sub

    Private Sub rdFl_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdFl.CheckedChanged
        txtFLInterval.Enabled = rdFl.Checked
    End Sub
End Class
