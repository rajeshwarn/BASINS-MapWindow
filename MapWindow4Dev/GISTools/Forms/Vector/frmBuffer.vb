'********************************************************************************************************
'File Name: frmBuffer.vb
'Description: Tool for buffering shapefiles.
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
'TODO: fill this in
'08/24-29/06 - JLK      - Added use of Logger
'12/7/06 - JLK          - btnOK allowed after error, default to current layer
'********************************************************************************************************
Imports MapWinUtility

Public Class frmBuffer
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
    Friend WithEvents lblUnits As System.Windows.Forms.Label
    Friend WithEvents lblDistance As System.Windows.Forms.Label
    Friend WithEvents txtbxDistance As System.Windows.Forms.TextBox
    Friend WithEvents lblShapeTreatment As System.Windows.Forms.Label
    Friend WithEvents lblCapStyle As System.Windows.Forms.Label
    Friend WithEvents cmbxShapeTreatment As System.Windows.Forms.ComboBox
    Friend WithEvents cmbxCapStyle As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents stsBar As System.Windows.Forms.StatusBar
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents chkbxAddResults As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxUniteOverlaps As System.Windows.Forms.CheckBox
    Friend WithEvents btnBrowseToOut As System.Windows.Forms.Button
    Friend WithEvents txtbxOutFile As System.Windows.Forms.TextBox
    Friend WithEvents lblSaveAs As System.Windows.Forms.Label
    Friend WithEvents btnSelectShape As System.Windows.Forms.Button
    Friend WithEvents chkbxBufferAll As System.Windows.Forms.CheckBox
    Friend WithEvents btnBrowseToBuffer As System.Windows.Forms.Button
    Friend WithEvents cmbxToBuffer As System.Windows.Forms.ComboBox
    Friend WithEvents lblToBuffer As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBuffer))
        Me.lblUnits = New System.Windows.Forms.Label
        Me.lblDistance = New System.Windows.Forms.Label
        Me.txtbxDistance = New System.Windows.Forms.TextBox
        Me.lblShapeTreatment = New System.Windows.Forms.Label
        Me.lblCapStyle = New System.Windows.Forms.Label
        Me.cmbxShapeTreatment = New System.Windows.Forms.ComboBox
        Me.cmbxCapStyle = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.stsBar = New System.Windows.Forms.StatusBar
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOK = New System.Windows.Forms.Button
        Me.chkbxAddResults = New System.Windows.Forms.CheckBox
        Me.chkbxUniteOverlaps = New System.Windows.Forms.CheckBox
        Me.btnBrowseToOut = New System.Windows.Forms.Button
        Me.txtbxOutFile = New System.Windows.Forms.TextBox
        Me.lblSaveAs = New System.Windows.Forms.Label
        Me.btnSelectShape = New System.Windows.Forms.Button
        Me.chkbxBufferAll = New System.Windows.Forms.CheckBox
        Me.btnBrowseToBuffer = New System.Windows.Forms.Button
        Me.cmbxToBuffer = New System.Windows.Forms.ComboBox
        Me.lblToBuffer = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lblUnits
        '
        Me.lblUnits.AccessibleDescription = Nothing
        Me.lblUnits.AccessibleName = Nothing
        resources.ApplyResources(Me.lblUnits, "lblUnits")
        Me.lblUnits.Font = Nothing
        Me.lblUnits.Name = "lblUnits"
        '
        'lblDistance
        '
        Me.lblDistance.AccessibleDescription = Nothing
        Me.lblDistance.AccessibleName = Nothing
        resources.ApplyResources(Me.lblDistance, "lblDistance")
        Me.lblDistance.Font = Nothing
        Me.lblDistance.Name = "lblDistance"
        '
        'txtbxDistance
        '
        Me.txtbxDistance.AccessibleDescription = Nothing
        Me.txtbxDistance.AccessibleName = Nothing
        resources.ApplyResources(Me.txtbxDistance, "txtbxDistance")
        Me.txtbxDistance.BackgroundImage = Nothing
        Me.txtbxDistance.Font = Nothing
        Me.txtbxDistance.Name = "txtbxDistance"
        '
        'lblShapeTreatment
        '
        Me.lblShapeTreatment.AccessibleDescription = Nothing
        Me.lblShapeTreatment.AccessibleName = Nothing
        resources.ApplyResources(Me.lblShapeTreatment, "lblShapeTreatment")
        Me.lblShapeTreatment.Font = Nothing
        Me.lblShapeTreatment.Name = "lblShapeTreatment"
        '
        'lblCapStyle
        '
        Me.lblCapStyle.AccessibleDescription = Nothing
        Me.lblCapStyle.AccessibleName = Nothing
        resources.ApplyResources(Me.lblCapStyle, "lblCapStyle")
        Me.lblCapStyle.Font = Nothing
        Me.lblCapStyle.Name = "lblCapStyle"
        '
        'cmbxShapeTreatment
        '
        Me.cmbxShapeTreatment.AccessibleDescription = Nothing
        Me.cmbxShapeTreatment.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbxShapeTreatment, "cmbxShapeTreatment")
        Me.cmbxShapeTreatment.BackgroundImage = Nothing
        Me.cmbxShapeTreatment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxShapeTreatment.Font = Nothing
        Me.cmbxShapeTreatment.Name = "cmbxShapeTreatment"
        '
        'cmbxCapStyle
        '
        Me.cmbxCapStyle.AccessibleDescription = Nothing
        Me.cmbxCapStyle.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbxCapStyle, "cmbxCapStyle")
        Me.cmbxCapStyle.BackgroundImage = Nothing
        Me.cmbxCapStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxCapStyle.Font = Nothing
        Me.cmbxCapStyle.Name = "cmbxCapStyle"
        '
        'Label1
        '
        Me.Label1.AccessibleDescription = Nothing
        Me.Label1.AccessibleName = Nothing
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'stsBar
        '
        Me.stsBar.AccessibleDescription = Nothing
        Me.stsBar.AccessibleName = Nothing
        resources.ApplyResources(Me.stsBar, "stsBar")
        Me.stsBar.BackgroundImage = Nothing
        Me.stsBar.Font = Nothing
        Me.stsBar.Name = "stsBar"
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
        'btnOK
        '
        Me.btnOK.AccessibleDescription = Nothing
        Me.btnOK.AccessibleName = Nothing
        resources.ApplyResources(Me.btnOK, "btnOK")
        Me.btnOK.BackgroundImage = Nothing
        Me.btnOK.Font = Nothing
        Me.btnOK.Name = "btnOK"
        '
        'chkbxAddResults
        '
        Me.chkbxAddResults.AccessibleDescription = Nothing
        Me.chkbxAddResults.AccessibleName = Nothing
        resources.ApplyResources(Me.chkbxAddResults, "chkbxAddResults")
        Me.chkbxAddResults.BackgroundImage = Nothing
        Me.chkbxAddResults.Checked = True
        Me.chkbxAddResults.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbxAddResults.Font = Nothing
        Me.chkbxAddResults.Name = "chkbxAddResults"
        '
        'chkbxUniteOverlaps
        '
        Me.chkbxUniteOverlaps.AccessibleDescription = Nothing
        Me.chkbxUniteOverlaps.AccessibleName = Nothing
        resources.ApplyResources(Me.chkbxUniteOverlaps, "chkbxUniteOverlaps")
        Me.chkbxUniteOverlaps.BackgroundImage = Nothing
        Me.chkbxUniteOverlaps.Checked = True
        Me.chkbxUniteOverlaps.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbxUniteOverlaps.Font = Nothing
        Me.chkbxUniteOverlaps.Name = "chkbxUniteOverlaps"
        '
        'btnBrowseToOut
        '
        Me.btnBrowseToOut.AccessibleDescription = Nothing
        Me.btnBrowseToOut.AccessibleName = Nothing
        resources.ApplyResources(Me.btnBrowseToOut, "btnBrowseToOut")
        Me.btnBrowseToOut.BackgroundImage = Nothing
        Me.btnBrowseToOut.Font = Nothing
        Me.btnBrowseToOut.Name = "btnBrowseToOut"
        '
        'txtbxOutFile
        '
        Me.txtbxOutFile.AccessibleDescription = Nothing
        Me.txtbxOutFile.AccessibleName = Nothing
        resources.ApplyResources(Me.txtbxOutFile, "txtbxOutFile")
        Me.txtbxOutFile.BackgroundImage = Nothing
        Me.txtbxOutFile.Font = Nothing
        Me.txtbxOutFile.Name = "txtbxOutFile"
        '
        'lblSaveAs
        '
        Me.lblSaveAs.AccessibleDescription = Nothing
        Me.lblSaveAs.AccessibleName = Nothing
        resources.ApplyResources(Me.lblSaveAs, "lblSaveAs")
        Me.lblSaveAs.Font = Nothing
        Me.lblSaveAs.Name = "lblSaveAs"
        '
        'btnSelectShape
        '
        Me.btnSelectShape.AccessibleDescription = Nothing
        Me.btnSelectShape.AccessibleName = Nothing
        resources.ApplyResources(Me.btnSelectShape, "btnSelectShape")
        Me.btnSelectShape.BackgroundImage = Nothing
        Me.btnSelectShape.Font = Nothing
        Me.btnSelectShape.Name = "btnSelectShape"
        '
        'chkbxBufferAll
        '
        Me.chkbxBufferAll.AccessibleDescription = Nothing
        Me.chkbxBufferAll.AccessibleName = Nothing
        resources.ApplyResources(Me.chkbxBufferAll, "chkbxBufferAll")
        Me.chkbxBufferAll.BackgroundImage = Nothing
        Me.chkbxBufferAll.Font = Nothing
        Me.chkbxBufferAll.Name = "chkbxBufferAll"
        '
        'btnBrowseToBuffer
        '
        Me.btnBrowseToBuffer.AccessibleDescription = Nothing
        Me.btnBrowseToBuffer.AccessibleName = Nothing
        resources.ApplyResources(Me.btnBrowseToBuffer, "btnBrowseToBuffer")
        Me.btnBrowseToBuffer.BackgroundImage = Nothing
        Me.btnBrowseToBuffer.Font = Nothing
        Me.btnBrowseToBuffer.Name = "btnBrowseToBuffer"
        '
        'cmbxToBuffer
        '
        Me.cmbxToBuffer.AccessibleDescription = Nothing
        Me.cmbxToBuffer.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbxToBuffer, "cmbxToBuffer")
        Me.cmbxToBuffer.BackgroundImage = Nothing
        Me.cmbxToBuffer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxToBuffer.Font = Nothing
        Me.cmbxToBuffer.Name = "cmbxToBuffer"
        '
        'lblToBuffer
        '
        Me.lblToBuffer.AccessibleDescription = Nothing
        Me.lblToBuffer.AccessibleName = Nothing
        resources.ApplyResources(Me.lblToBuffer, "lblToBuffer")
        Me.lblToBuffer.Font = Nothing
        Me.lblToBuffer.Name = "lblToBuffer"
        '
        'frmBuffer
        '
        Me.AcceptButton = Me.btnOK
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.CancelButton = Me.btnCancel
        Me.Controls.Add(Me.lblUnits)
        Me.Controls.Add(Me.lblDistance)
        Me.Controls.Add(Me.txtbxDistance)
        Me.Controls.Add(Me.lblShapeTreatment)
        Me.Controls.Add(Me.lblCapStyle)
        Me.Controls.Add(Me.cmbxShapeTreatment)
        Me.Controls.Add(Me.cmbxCapStyle)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.stsBar)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.chkbxAddResults)
        Me.Controls.Add(Me.chkbxUniteOverlaps)
        Me.Controls.Add(Me.btnBrowseToOut)
        Me.Controls.Add(Me.txtbxOutFile)
        Me.Controls.Add(Me.lblSaveAs)
        Me.Controls.Add(Me.btnSelectShape)
        Me.Controls.Add(Me.chkbxBufferAll)
        Me.Controls.Add(Me.btnBrowseToBuffer)
        Me.Controls.Add(Me.cmbxToBuffer)
        Me.Controls.Add(Me.lblToBuffer)
        Me.Font = Nothing
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmBuffer"
        Me.ShowInTaskbar = False
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Real Code"
    Private g_bufferType As Integer
    Private g_shapeSel As Integer
    Private g_selectingShapes As Boolean
    Private g_lastToBuffer As String
    Private g_selShapeCount As Integer
    Private g_lineSide As MapWinGeoProc.Enumerations.Buffer_LineSide
    Private g_holeTreatment As MapWinGeoProc.Enumerations.Buffer_HoleTreatment
    Private g_capStyle As MapWinGeoProc.Enumerations.Buffer_CapStyle
    Private g_distance As Double
    Private g_sfType As MapWinGIS.ShpfileType

    Public Sub Initialize()
        g_selShapeCount = 0
        g_distance = 0
        PopulateToBuffer()
        cmbxCapStyle.Items.Add("Pointed")
        cmbxCapStyle.Items.Add("Rounded")
        cmbxCapStyle.SelectedItem = "Rounded"
        setDefaultColoring()
    End Sub

    Private Sub PopulateToBuffer()
        Dim newlayer As MapWindow.Interfaces.Layer
        Dim i As Integer
        cmbxToBuffer.Items.Clear()
        If g_MW.Layers.NumLayers > 0 Then
            Dim lCurrentLayer As Integer = g_MW.Layers.CurrentLayer
            For i = 0 To g_MW.Layers.NumLayers - 1
                'newlayer = g_MW.Layers.Item(i)
                'CDM 11/14/2006 - Aaugh! Item takes a handle. NOT a position. Get the handle first
                newlayer = g_MW.Layers.Item(g_MW.Layers.GetHandle(i))

                If newlayer.LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Or _
                  (newlayer.LayerType = MapWindow.Interfaces.eLayerType.LineShapefile) Or _
                  (newlayer.LayerType = MapWindow.Interfaces.eLayerType.PointShapefile) Then
                    cmbxToBuffer.Items.Add(newlayer.Name)
                End If
                If lCurrentLayer = i Then
                    cmbxToBuffer.SelectedIndex = cmbxToBuffer.Items.Count - 1
                End If
            Next
        End If

        If cmbxToBuffer.Items.Count > 0 Then
            If cmbxToBuffer.SelectedIndex < 0 Then cmbxToBuffer.SelectedIndex = 0
            btnSelectShape.Enabled = True
        End If
        If g_lastToBuffer <> "" Then
            cmbxToBuffer.SelectedIndex = cmbxToBuffer.Items.IndexOf(g_lastToBuffer)
            btnSelectShape.Enabled = True
        End If

    End Sub

    Private Sub setDefaultColoring()
        lblToBuffer.ForeColor = Drawing.Color.Black
        lblSaveAs.ForeColor = Drawing.Color.Black
        lblDistance.ForeColor = Drawing.Color.Black
        btnSelectShape.ForeColor = Drawing.Color.Black
    End Sub

    Private Function getPathByName(ByVal strName As String) As String
        Dim i As Integer
        For i = 0 To g_MW.Layers.NumLayers - 1
            If g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).Name = strName Then
                Return g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).FileName
            End If
        Next
        Return ""
    End Function

    Private Function getNameByPath(ByVal strPath As String) As String
        Dim i As Integer
        For i = 0 To g_MW.Layers.NumLayers - 1
            If g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).FileName = strPath Then
                Return g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).Name
            End If
        Next
        Return ""
    End Function

    Private Function getIndexByName(ByVal strName As String) As Integer
        Dim i As Integer
        For i = 0 To g_MW.Layers.NumLayers - 1
            If g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).Name = strName Then
                Return g_MW.Layers.GetHandle(i)
            End If
        Next
    End Function

    Private Function LayerExists(ByVal aLayerPath As String) As Boolean
        For i As Integer = 0 To g_MW.Layers.NumLayers - 1
            If g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).FileName = aLayerPath Then
                Return True
            End If
        Next
        Return False
    End Function

    'add a new layer to the map
    Private Sub AddMap(ByVal fname As String, ByVal addType As Integer)
        Dim FileDesc As String
        FileDesc = System.IO.Path.GetFileName(fname)
        If (FileDesc = "sta.adf") Then
            FileDesc = System.IO.Path.GetDirectoryName(fname)
            FileDesc = System.IO.Path.GetFileNameWithoutExtension(FileDesc)
        End If

        Dim NewLayer As MapWindow.Interfaces.Layer

        If FileExists(fname) Then
            NewLayer = g_MW.Layers.Add(fname, FileDesc, True, True)
            NewLayer.OutlineColor = System.Drawing.Color.Red
            NewLayer.LineOrPointSize = 3
            NewLayer.DrawFill = False 'so that extent of buffering will be visible
        End If
    End Sub

    Private Sub RemoveLayer(ByVal fname As String)
        Dim LyrHandle As Integer
        Dim i As Integer
        For i = 0 To g_MW.Layers.NumLayers - 1
            LyrHandle = g_MW.Layers.GetHandle(i)
            If LCase(g_MW.Layers(LyrHandle).FileName) = LCase(fname) Then
                g_MW.Layers.Remove(LyrHandle)
            End If
        Next
    End Sub

    Public Function AppendExt(ByVal fname As String, ByVal Ext As String) As String
        Dim i As Integer
        For i = Len(fname) To 1 Step -1
            If Mid(fname, i, 1) = "." Then Exit For
        Next i
        If (i = 0) Then
            AppendExt = fname & Ext
        Else
            AppendExt = Microsoft.VisualBasic.Strings.Left(fname, i - 1) & Ext & Mid(fname, i)
        End If
    End Function

    Private Sub cmbxToBuffer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxToBuffer.SelectedIndexChanged
        PopulateOutFile()
        setShapeTreatment()
        setDefaultColoring()
    End Sub

    Private Sub PopulateOutFile()
        If cmbxToBuffer.Items.Count > 0 Then 'default the output path
            Dim lOutFile As String = AppendExt(getPathByName(cmbxToBuffer.Items(cmbxToBuffer.SelectedIndex)), "_buffer")
            Dim i As Integer = 0
            Dim lBuffExt As String
            While FileExists(lOutFile)
                lBuffExt = "_buffer" & i
                lOutFile = AppendExt(getPathByName(cmbxToBuffer.Items(cmbxToBuffer.SelectedIndex)), lBuffExt)
                i += 1
            End While
            txtbxOutFile.Text = lOutFile
        Else
            txtbxOutFile.Text = ""
        End If
    End Sub

    Private Sub setShapeTreatment()
        'find what type of shapefile we are dealing with
        Dim shpfile As New MapWinGIS.Shapefile
        shpfile.Open(getPathByName(cmbxToBuffer.Items(cmbxToBuffer.SelectedIndex)))
        Debug.WriteLine(getPathByName(cmbxToBuffer.Items(cmbxToBuffer.SelectedIndex)))
        g_sfType = shpfile.ShapefileType
        Debug.WriteLine(g_sfType.ToString)
        shpfile.Close()
        'set shape treatment selection based on type
        'line file
        If ((g_sfType = MapWinGIS.ShpfileType.SHP_POLYLINE) Or _
                    (g_sfType = MapWinGIS.ShpfileType.SHP_POLYLINEM) Or _
                    (g_sfType = MapWinGIS.ShpfileType.SHP_POLYLINEZ)) Then
            lblShapeTreatment.Text = "Line Side"
            cmbxShapeTreatment.Items.Clear()
            cmbxShapeTreatment.Items.Add("Both")
            cmbxShapeTreatment.Items.Add("Left")
            cmbxShapeTreatment.Items.Add("Right")
            cmbxShapeTreatment.SelectedItem = "Both"
            g_lineSide = MapWinGeoProc.Enumerations.Buffer_LineSide.Both
            cmbxShapeTreatment.Enabled = True
            cmbxCapStyle.Enabled = True
            'polygon file
        ElseIf ((g_sfType = MapWinGIS.ShpfileType.SHP_POLYGON) Or _
                    (g_sfType = MapWinGIS.ShpfileType.SHP_POLYGONM) Or _
                    (g_sfType = MapWinGIS.ShpfileType.SHP_POLYGONZ)) Then
            lblShapeTreatment.Text = "Hole Treatment"
            cmbxShapeTreatment.Items.Clear()
            cmbxShapeTreatment.Items.Add("Ignore")
            cmbxShapeTreatment.Items.Add("Opposite")
            cmbxShapeTreatment.Items.Add("Same")
            cmbxShapeTreatment.Items.Add("Original")
            cmbxShapeTreatment.SelectedItem = "Opposite"
            g_holeTreatment = MapWinGeoProc.Enumerations.Buffer_HoleTreatment.Opposite
            cmbxShapeTreatment.Enabled = True
            cmbxCapStyle.Enabled = True
        Else 'point file
            cmbxShapeTreatment.Enabled = False
            cmbxCapStyle.Enabled = False
        End If
    End Sub

    Private Sub txtbxOutFile_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtbxOutFile.TextChanged
        checkOutFile(False)
    End Sub

    Private Sub checkOutFile(ByVal aQuietOverwrite As Boolean)
        btnOK.Enabled = True
        If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(txtbxOutFile.Text)) Then
            If System.IO.File.Exists(txtbxOutFile.Text) Then
                'make sure that the file is not already in use
                If LayerExists(txtbxOutFile.Text) Then
                    'Do not continue until user has changed the file name!
                    Logger.Msg("Layer <" & txtbxOutFile.Text & "> is a layer in MapWindow." & Environment.NewLine & "Please choose another path before continuing.", _
                           MsgBoxStyle.Exclamation, _
                           "GISTools:Buffer:FileInUse")
                    btnOK.Enabled = False
                ElseIf Not aQuietOverwrite Then
                    Logger.Msg("Layer <" & txtbxOutFile.Text & "> already exists." & Environment.NewLine & _
                                   "Please choose another path if you don't want this file to be overwritten.", _
                               MsgBoxStyle.Exclamation, _
                               "GISTools:Buffer:ExistingFileToBeOverwritten")
                End If
            End If
        End If
        setDefaultColoring()
    End Sub

    Private Sub btnBrowseToBuffer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseToBuffer.Click
        Dim strPath As String
        Dim sf As New MapWinGIS.Shapefile
        Dim fdiagOpen As System.Windows.Forms.OpenFileDialog = New System.Windows.Forms.OpenFileDialog
        fdiagOpen.Filter = sf.CdlgFilter
        fdiagOpen.FilterIndex = 1
        If fdiagOpen.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            strPath = fdiagOpen.FileName
            If Not LayerExists(strPath) Then
                AddMap(strPath, 0)
                PopulateToBuffer()
                g_lastToBuffer = getNameByPath(strPath)
                cmbxToBuffer.SelectedIndex = cmbxToBuffer.Items.IndexOf(g_lastToBuffer)
            Else
                Logger.Msg("Layer <" & strPath & "> already exists. Please select it from the drop down list.", _
                           MsgBoxStyle.Exclamation, _
                           "GISTools:Buffer:FileExists")
            End If
        End If
    End Sub

    Private Sub btnBrowseToOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseToOut.Click
        btnOK.Enabled = True
        Dim strPath As String
        Dim sf As New MapWinGIS.Shapefile
        Dim fdiagOpen As System.Windows.Forms.SaveFileDialog = New System.Windows.Forms.SaveFileDialog
        fdiagOpen.Filter = sf.CdlgFilter
        fdiagOpen.FilterIndex = 1
        If fdiagOpen.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            strPath = fdiagOpen.FileName
            If Not LayerExists(strPath) Then
                txtbxOutFile.Text = strPath
            Else
                Logger.Msg("Layer <" & strPath & "> already exists.  Please remove it or select a different file to save to.", _
                           MsgBoxStyle.Exclamation, _
                           "GISTools:Buffer:FileExists")
                btnOK.Enabled = False
            End If
        End If
        setDefaultColoring()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnSelectShape_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectShape.Click
        If g_selectingShapes Then
            g_selectingShapes = False
            btnSelectShape.Text = "Select Shapes"
            g_selShapeCount = g_MW.View.SelectedShapes.NumSelected
            If g_selShapeCount = 0 Then
                stsBar.Text = "0 shapes selected. Click the Select Shapes button to select."
                chkbxUniteOverlaps.Enabled = False
            ElseIf g_selShapeCount = 1 Then
                stsBar.Text = g_selShapeCount.ToString + " shape selected. Click OK to buffer."
                chkbxUniteOverlaps.Enabled = False
            Else
                stsBar.Text = g_selShapeCount.ToString + " shapes selected. Click OK to buffer."
                chkbxUniteOverlaps.Enabled = True
            End If
        Else
            g_selectingShapes = True
            stsBar.Text = "Click to Select. Hold CTRL to select multiple. Click Done once finished."
            btnSelectShape.Text = "Done"
            g_MW.Layers.CurrentLayer = getIndexByName(cmbxToBuffer.Items(cmbxToBuffer.SelectedIndex))
            g_MW.View.CursorMode = MapWinGIS.tkCursorMode.cmSelection
        End If
        setDefaultColoring()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim okToStart As Boolean
        okToStart = False
        setDefaultColoring()
        Try

            If (cmbxToBuffer.Items.Count > 0) Then
                okToStart = True
            Else
                okToStart = False
                stsBar.Text = "Please select the buffer shapefile."
                lblToBuffer.ForeColor = Drawing.Color.Red
                cmbxToBuffer.Focus()
            End If

            If (okToStart) Then
                If (chkbxBufferAll.Checked = True) Then
                    okToStart = True
                Else
                    If (g_selShapeCount > 0) Then
                        okToStart = True
                    Else
                        okToStart = False
                        stsBar.Text = "Please select the shapes to buffer."
                        btnSelectShape.ForeColor = Drawing.Color.Red
                        btnSelectShape.Focus()
                    End If
                    If (okToStart) Then
                        If (Not g_selectingShapes) Then
                            okToStart = True
                        Else
                            okToStart = False
                            stsBar.Text = "Finish selecting shapes."
                            btnSelectShape.ForeColor = Drawing.Color.Red
                            btnSelectShape.Focus()
                        End If
                    End If
                End If
            End If

            If (okToStart) Then
                If (txtbxDistance.Text <> "") Then
                    'verify that user has an appropriate distance
                    If (g_distance = 0) Then
                        Logger.Msg("Buffering requires a distance other than 0.", _
                                   MsgBoxStyle.Exclamation, _
                                   "GISTools:Buffer:IncorrectDistance")
                        okToStart = False
                    ElseIf (g_distance < 0) Then
                        If ((g_sfType = MapWinGIS.ShpfileType.SHP_POLYGON) Or _
                        (g_sfType = MapWinGIS.ShpfileType.SHP_POLYGONM) Or _
                        (g_sfType = MapWinGIS.ShpfileType.SHP_POLYGONZ)) Then
                            okToStart = True
                        Else
                            Logger.Msg("Negative distance only allowed when buffering polygons.", _
                                       MsgBoxStyle.Exclamation, _
                                       "GISTools:Buffer:NegativeDistance")
                            okToStart = False
                            txtbxDistance.Focus()
                        End If
                    End If
                Else
                    okToStart = False
                    stsBar.Text = "Please specify a distance."
                    lblDistance.ForeColor = Drawing.Color.Red
                    txtbxDistance.Focus()
                End If
            End If

            If (okToStart) Then
                If (txtbxOutFile.Text <> "") Then
                    okToStart = True
                Else
                    okToStart = False
                    stsBar.Text = "Please specify an output file."
                    lblSaveAs.ForeColor = Drawing.Color.Black
                    txtbxOutFile.Focus()
                End If
            End If

            If (okToStart) Then 'verify that the outFile is not already in use
                checkOutFile(True)
            End If
        Catch ex As Exception
            'Paul Meems 22 July 2009: Added
            Logger.Dbg(ex.ToString)
        End Try

        If (okToStart) Then
            Cursor = Windows.Forms.Cursors.WaitCursor
            If ((g_selShapeCount > 1) Or _
            (chkbxBufferAll.Checked = True)) Then
                stsBar.Text = "Buffering shapes"
            Else
                stsBar.Text = "Buffering shape"
            End If
            BufferShapes()
            Cursor = Windows.Forms.Cursors.Default
        End If

    End Sub

    Private Sub BufferShapes()
        Dim lSourcePath, lResultPath As String
        Dim success As Boolean = False
        Dim sf As New MapWinGIS.Shapefile
        Dim resultSF As New MapWinGIS.Shapefile

        Logger.Dbg("GISTools:BufferShapes:Begin " & g_distance)

        lSourcePath = getPathByName(cmbxToBuffer.Items(cmbxToBuffer.SelectedIndex))
        Logger.Dbg("GISTools:Buffer:BufferShapeFile " & lSourcePath)
        lResultPath = txtbxOutFile.Text
        If Not (lResultPath.ToLower.EndsWith(".shp")) Then
            lResultPath &= ".shp"
        End If
        Logger.Dbg("GISTools:Buffer:Output " & lResultPath)

        'delete result file if it already exists
        If FileExists(lResultPath) Then
            Dim lPathNoExt As String = FilenameNoExt(lResultPath)
            Logger.Dbg("GISTools:Buffer:DeleteExistingOutputFiles " & lPathNoExt)
            TryDelete(lPathNoExt & ".shp")
            TryDelete(lPathNoExt & ".shx")
            TryDelete(lPathNoExt & ".dbf")
            TryDelete(lPathNoExt & ".prj")
        End If

        Dim lUniteOverlaps As Boolean = chkbxUniteOverlaps.Checked

        'create the result file
        resultSF.CreateNew(lResultPath, MapWinGIS.ShpfileType.SHP_POLYGON)
        ' Paul Meems 22 July 2009:
        ' After a shapefile is created, the attribute table and shapefile are automatically in editing mode
        ' resultSF.StartEditingShapes(True)

        Dim numSel As Integer = g_MW.View.SelectedShapes.NumSelected
        If numSel = 1 Then
            Logger.Dbg("GISTools:Buffer:Buffer " & numSel & " Shape")
            Dim shape As New MapWinGIS.Shape
            Dim resultShp As New MapWinGIS.Shape
            sf.Open(lSourcePath)

            PreserveAttributeDefns(sf, resultSF)

            shape.Create(g_sfType)
            shape = sf.Shape(g_MW.View.SelectedShapes.Item(0).ShapeIndex)
            resultShp.Create(MapWinGIS.ShpfileType.SHP_POLYGON)

            If ((g_sfType = MapWinGIS.ShpfileType.SHP_POINT) Or _
                    (g_sfType = MapWinGIS.ShpfileType.SHP_POINTM) Or _
                    (g_sfType = MapWinGIS.ShpfileType.SHP_POINTZ)) Then
                Logger.Dbg("GISTools:Buffer:Calling BufferShape() on point object.")
                success = MapWinGeoProc.SpatialOperations.BufferShape(shape, g_distance, resultShp)
            ElseIf ((g_sfType = MapWinGIS.ShpfileType.SHP_POLYLINE) Or _
                        (g_sfType = MapWinGIS.ShpfileType.SHP_POLYLINEM) Or _
                        (g_sfType = MapWinGIS.ShpfileType.SHP_POLYLINEZ)) Then
                Logger.Dbg("GISTools:Buffer:Calling BufferShape() on line object.")
                success = MapWinGeoProc.SpatialOperations.BufferLine(shape, g_distance, g_lineSide, g_capStyle, resultShp)
            ElseIf ((g_sfType = MapWinGIS.ShpfileType.SHP_POLYGON) Or _
                        (g_sfType = MapWinGIS.ShpfileType.SHP_POLYGONM) Or _
                        (g_sfType = MapWinGIS.ShpfileType.SHP_POLYGONZ)) Then
                Logger.Dbg("GISTools:Buffer:Calling BufferShape() on polygon object.")
                success = MapWinGeoProc.SpatialOperations.BufferPolygon(shape, g_distance, g_holeTreatment, g_capStyle, resultShp)
            End If

            If success = True Then
                resultSF.EditInsertShape(resultShp, 0)
                PreserveAttributeVals(sf, resultSF, g_MW.View.SelectedShapes.Item(0).ShapeIndex, 0)
            End If
            resultSF.SaveAs(lResultPath)
            resultSF.StopEditingShapes(True)
            resultSF.Close()
            sf.Close()
            'end of if only 1 shape selected
        ElseIf (chkbxBufferAll.Checked = True) Then
            Logger.Dbg("GISTools:Buffer:Buffer All Shapes")
            If ((g_sfType = MapWinGIS.ShpfileType.SHP_POINT) Or _
                    (g_sfType = MapWinGIS.ShpfileType.SHP_POINTM) Or _
                    (g_sfType = MapWinGIS.ShpfileType.SHP_POINTZ)) Then
                Logger.Dbg("GISTools:Buffer:Calling BufferShape() on point object.")
                success = MapWinGeoProc.SpatialOperations.BufferSF(lSourcePath, lResultPath, g_distance, lUniteOverlaps)
            ElseIf ((g_sfType = MapWinGIS.ShpfileType.SHP_POLYLINE) Or _
                        (g_sfType = MapWinGIS.ShpfileType.SHP_POLYLINEM) Or _
                        (g_sfType = MapWinGIS.ShpfileType.SHP_POLYLINEZ)) Then
                Logger.Dbg("GISTools:Buffer:Calling BufferShape() on line object.")
                success = MapWinGeoProc.SpatialOperations.BufferLineSF(lSourcePath, lResultPath, g_distance, lUniteOverlaps, g_lineSide, g_capStyle)
            ElseIf ((g_sfType = MapWinGIS.ShpfileType.SHP_POLYGON) Or _
                        (g_sfType = MapWinGIS.ShpfileType.SHP_POLYGONM) Or _
                        (g_sfType = MapWinGIS.ShpfileType.SHP_POLYGONZ)) Then
                Logger.Dbg("GISTools:Buffer:Calling BufferShape() on polygon object.")
                success = MapWinGeoProc.SpatialOperations.BufferPolygonSF(lSourcePath, lResultPath, g_distance, lUniteOverlaps, g_holeTreatment, g_capStyle)
            End If
            'end of if all were selected
        Else
            Logger.Dbg("GISTools:Buffer:Buffer " & numSel & " Shapes")
            Logger.Dbg("GISTools:Buffer:Combine all selected shapes into a temporary shapefile.")
            Dim i As Integer
            Dim tempSF As New MapWinGIS.Shapefile
            Dim tempSFPath As String
            tempSFPath = System.IO.Path.GetTempPath & "tempSF.shp"
            If FileExists(tempSFPath) Then
                Dim lPathNoExt As String = FilenameNoExt(tempSFPath)
                TryDelete(lPathNoExt & ".shp")
                TryDelete(lPathNoExt & ".shx")
                TryDelete(lPathNoExt & ".dbf")
            End If
            'create the temporary result file
            tempSF.CreateNew(tempSFPath, g_sfType)
            tempSF.StartEditingShapes(True)

            sf.Open(lSourcePath)

            PreserveAttributeDefns(sf, resultSF)

            For i = 0 To numSel - 1
                Dim currShp As New MapWinGIS.Shape
                currShp.Create(g_sfType)
                currShp = sf.Shape(g_MW.View.SelectedShapes.Item(i).ShapeIndex)
                tempSF.EditInsertShape(currShp, i)
                PreserveAttributeVals(sf, resultSF, g_MW.View.SelectedShapes.Item(i).ShapeIndex, i)
            Next
            sf.Close()
            tempSF.SaveAs(tempSFPath)
            tempSF.StopEditingShapes(True)
            tempSF.Close()

            Logger.Dbg("GISTools:Buffer:Buffered all seletected shapes in the temporary shapefile <" & tempSFPath & ">.")
            If ((g_sfType = MapWinGIS.ShpfileType.SHP_POINT) Or _
                    (g_sfType = MapWinGIS.ShpfileType.SHP_POINTM) Or _
                    (g_sfType = MapWinGIS.ShpfileType.SHP_POINTZ)) Then
                Logger.Dbg("GISTools:Buffer:Calling BufferShape() on point object.")
                success = MapWinGeoProc.SpatialOperations.BufferSF(tempSFPath, lResultPath, g_distance, lUniteOverlaps)
            ElseIf ((g_sfType = MapWinGIS.ShpfileType.SHP_POLYLINE) Or _
                        (g_sfType = MapWinGIS.ShpfileType.SHP_POLYLINEM) Or _
                        (g_sfType = MapWinGIS.ShpfileType.SHP_POLYLINEZ)) Then
                Logger.Dbg("GISTools:Buffer:Calling BufferShape() on line object.")
                success = MapWinGeoProc.SpatialOperations.BufferLineSF(tempSFPath, lResultPath, g_distance, lUniteOverlaps, g_lineSide, g_capStyle)
            ElseIf ((g_sfType = MapWinGIS.ShpfileType.SHP_POLYGON) Or _
                        (g_sfType = MapWinGIS.ShpfileType.SHP_POLYGONM) Or _
                        (g_sfType = MapWinGIS.ShpfileType.SHP_POLYGONZ)) Then
                Logger.Dbg("GISTools:Buffer:Calling BufferShape() on polygon object.")
                success = MapWinGeoProc.SpatialOperations.BufferPolygonSF(tempSFPath, lResultPath, g_distance, lUniteOverlaps, g_holeTreatment, g_capStyle)
            End If

            If FileExists(tempSFPath) Then
                Dim lPathNoExt As String = FilenameNoExt(tempSFPath)
                TryDelete(tempSFPath & ".shp")
                TryDelete(tempSFPath & ".shx")
                TryDelete(tempSFPath & ".dbf")
            End If
            'end if some were selected
        End If

        If success Then
            Logger.Dbg("GISTools:Buffer:Success")
            Dim lSourcePathNoExt As String = FilenameNoExt(lSourcePath)
            If FileExists(lSourcePathNoExt & ".prj") Then
                FileCopy(lSourcePathNoExt & ".prj", FilenameNoExt(lResultPath) & ".prj")
            End If
            If chkbxAddResults.Checked Then
                AddMap(lResultPath, 1)
            End If
        Else
            Dim errorMsg As String = MapWinGeoProc.Error.GetLastErrorMsg()
            Logger.Msg(errorMsg, _
                       MsgBoxStyle.Critical, _
                       "GISTools:Buffer:Error")
        End If
        Me.Close()
    End Sub

    Private Sub PreserveAttributeDefns(ByRef sf As MapWinGIS.Shapefile, ByRef resultSF As MapWinGIS.Shapefile)
        For i As Integer = 0 To sf.NumFields - 1
            Dim fld As New MapWinGIS.Field
            fld.Key = sf.Field(i).Key
            fld.Name = sf.Field(i).Name
            fld.Precision = sf.Field(i).Precision
            fld.Type = sf.Field(i).Type
            fld.Width = sf.Field(i).Width
            resultSF.EditInsertField(fld, i)
        Next
    End Sub

    Private Sub PreserveAttributeVals(ByRef sf As MapWinGIS.Shapefile, ByRef resultSF As MapWinGIS.Shapefile, ByVal origIdx As Long, ByVal destIdx As Long)
        For i As Integer = 0 To sf.NumFields - 1
            resultSF.EditCellValue(i, destIdx, sf.CellValue(i, origidx))
        Next
    End Sub

    Private Sub txtbxDistance_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtbxDistance.TextChanged
        Dim distance As Double
        distance = 0
        Try
            distance = System.Convert.ToDouble(txtbxDistance.Text.ToString())
        Catch exception As System.OverflowException
            MapWinGeoProc.Error.SetErrorMsg( _
                "Overflow when converting Distance to a double value.")
        Catch exception As System.FormatException
            MapWinGeoProc.Error.SetErrorMsg( _
                "Distance is not formatted as a Double.")
        Catch exception As System.ArgumentException
            MapWinGeoProc.Error.SetErrorMsg("Distance is null.")
        End Try
        'if the user is typeing in '-' then we want to wait unit
        'there is a number assoicated with the sign before checking the value
        If txtbxDistance.Text <> "-" Then
            g_distance = distance
        End If
        setDefaultColoring()
    End Sub

    Private Sub cmbxCapStyle_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxCapStyle.SelectedIndexChanged
        If cmbxCapStyle.SelectedItem = "Pointed" Then
            g_capStyle = MapWinGeoProc.Enumerations.Buffer_CapStyle.Pointed
        Else
            g_capStyle = MapWinGeoProc.Enumerations.Buffer_CapStyle.Rounded
        End If
        setDefaultColoring()
    End Sub

    Private Sub cmbxShapeTreatment_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxShapeTreatment.SelectedIndexChanged
        If ((g_sfType = MapWinGIS.ShpfileType.SHP_POLYLINE) Or _
                (g_sfType = MapWinGIS.ShpfileType.SHP_POLYLINEM) Or _
                (g_sfType = MapWinGIS.ShpfileType.SHP_POLYLINEZ)) Then
            If (cmbxShapeTreatment.SelectedItem = "Both") Then
                g_lineSide = MapWinGeoProc.Enumerations.Buffer_LineSide.Both
            ElseIf (cmbxShapeTreatment.SelectedItem = "Left") Then
                g_lineSide = MapWinGeoProc.Enumerations.Buffer_LineSide.Left
            Else
                g_lineSide = MapWinGeoProc.Enumerations.Buffer_LineSide.Right
            End If

        ElseIf ((g_sfType = MapWinGIS.ShpfileType.SHP_POLYGON) Or _
                    (g_sfType = MapWinGIS.ShpfileType.SHP_POLYGONM) Or _
                    (g_sfType = MapWinGIS.ShpfileType.SHP_POLYGONZ)) Then
            If (cmbxShapeTreatment.SelectedItem = "Ignore") Then
                g_holeTreatment = MapWinGeoProc.Enumerations.Buffer_HoleTreatment.Ignore
            ElseIf (cmbxShapeTreatment.SelectedItem = "Opposite") Then
                g_holeTreatment = MapWinGeoProc.Enumerations.Buffer_HoleTreatment.Opposite
            ElseIf (cmbxShapeTreatment.SelectedItem = "Same") Then
                g_holeTreatment = MapWinGeoProc.Enumerations.Buffer_HoleTreatment.Same
            Else
                g_holeTreatment = MapWinGeoProc.Enumerations.Buffer_HoleTreatment.Original
            End If
        End If
        setDefaultColoring()
    End Sub

    Private Sub chkbxBufferAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkbxBufferAll.CheckedChanged
        If (chkbxBufferAll.Checked = True) Then
            btnSelectShape.Enabled = False
            chkbxUniteOverlaps.Enabled = True
        Else
            btnSelectShape.Enabled = True
            chkbxUniteOverlaps.Enabled = False
            If (g_selShapeCount > 1) Then
                chkbxUniteOverlaps.Enabled = True
            End If
        End If
        setDefaultColoring()
    End Sub

#End Region
End Class
