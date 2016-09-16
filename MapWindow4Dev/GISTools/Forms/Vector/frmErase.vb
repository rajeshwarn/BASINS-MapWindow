'********************************************************************************************************
'File Name: frmErase.vb
'Description: Tool for erasing portions of shapefiles with polygons.
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specific language governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source GIS Tools Plug-in. 
'
'Contributor(s): (Open source contributors should list themselves and their modifications here).
'05/19/06 - Angela Hillier - created erase tool (based off of buffer and clip tools)
'5/22/06 - Angela Hillier - enabled 'ok' button so that it validates each input
'********************************************************************************************************
Public Class frmErase
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
    Friend WithEvents btnBrowseToOut As System.Windows.Forms.Button
    Friend WithEvents txtbxOutFile As System.Windows.Forms.TextBox
    Friend WithEvents chkbxAddResult As System.Windows.Forms.CheckBox
    Friend WithEvents stsBar As System.Windows.Forms.StatusBar
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents lblSaveAs As System.Windows.Forms.Label
    Friend WithEvents btnSelectShape As System.Windows.Forms.Button
    Friend WithEvents cmbxEraseWith As System.Windows.Forms.ComboBox
    Friend WithEvents btnBrowseEraseWith As System.Windows.Forms.Button
    Friend WithEvents lblEraseWith As System.Windows.Forms.Label
    Friend WithEvents cmbxToErase As System.Windows.Forms.ComboBox
    Friend WithEvents btnBrowseToErase As System.Windows.Forms.Button
    Friend WithEvents lblToErase As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmErase))
        Me.btnBrowseToOut = New System.Windows.Forms.Button
        Me.txtbxOutFile = New System.Windows.Forms.TextBox
        Me.chkbxAddResult = New System.Windows.Forms.CheckBox
        Me.stsBar = New System.Windows.Forms.StatusBar
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOK = New System.Windows.Forms.Button
        Me.lblSaveAs = New System.Windows.Forms.Label
        Me.btnSelectShape = New System.Windows.Forms.Button
        Me.cmbxEraseWith = New System.Windows.Forms.ComboBox
        Me.btnBrowseEraseWith = New System.Windows.Forms.Button
        Me.lblEraseWith = New System.Windows.Forms.Label
        Me.cmbxToErase = New System.Windows.Forms.ComboBox
        Me.btnBrowseToErase = New System.Windows.Forms.Button
        Me.lblToErase = New System.Windows.Forms.Label
        Me.SuspendLayout()
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
        'chkbxAddResult
        '
        Me.chkbxAddResult.AccessibleDescription = Nothing
        Me.chkbxAddResult.AccessibleName = Nothing
        resources.ApplyResources(Me.chkbxAddResult, "chkbxAddResult")
        Me.chkbxAddResult.BackgroundImage = Nothing
        Me.chkbxAddResult.Checked = True
        Me.chkbxAddResult.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbxAddResult.Font = Nothing
        Me.chkbxAddResult.Name = "chkbxAddResult"
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
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Font = Nothing
        Me.btnOK.Name = "btnOK"
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
        'cmbxEraseWith
        '
        Me.cmbxEraseWith.AccessibleDescription = Nothing
        Me.cmbxEraseWith.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbxEraseWith, "cmbxEraseWith")
        Me.cmbxEraseWith.BackgroundImage = Nothing
        Me.cmbxEraseWith.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxEraseWith.Font = Nothing
        Me.cmbxEraseWith.Name = "cmbxEraseWith"
        '
        'btnBrowseEraseWith
        '
        Me.btnBrowseEraseWith.AccessibleDescription = Nothing
        Me.btnBrowseEraseWith.AccessibleName = Nothing
        resources.ApplyResources(Me.btnBrowseEraseWith, "btnBrowseEraseWith")
        Me.btnBrowseEraseWith.BackgroundImage = Nothing
        Me.btnBrowseEraseWith.Font = Nothing
        Me.btnBrowseEraseWith.Name = "btnBrowseEraseWith"
        '
        'lblEraseWith
        '
        Me.lblEraseWith.AccessibleDescription = Nothing
        Me.lblEraseWith.AccessibleName = Nothing
        resources.ApplyResources(Me.lblEraseWith, "lblEraseWith")
        Me.lblEraseWith.Font = Nothing
        Me.lblEraseWith.Name = "lblEraseWith"
        '
        'cmbxToErase
        '
        Me.cmbxToErase.AccessibleDescription = Nothing
        Me.cmbxToErase.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbxToErase, "cmbxToErase")
        Me.cmbxToErase.BackgroundImage = Nothing
        Me.cmbxToErase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxToErase.Font = Nothing
        Me.cmbxToErase.Name = "cmbxToErase"
        '
        'btnBrowseToErase
        '
        Me.btnBrowseToErase.AccessibleDescription = Nothing
        Me.btnBrowseToErase.AccessibleName = Nothing
        resources.ApplyResources(Me.btnBrowseToErase, "btnBrowseToErase")
        Me.btnBrowseToErase.BackgroundImage = Nothing
        Me.btnBrowseToErase.Font = Nothing
        Me.btnBrowseToErase.Name = "btnBrowseToErase"
        '
        'lblToErase
        '
        Me.lblToErase.AccessibleDescription = Nothing
        Me.lblToErase.AccessibleName = Nothing
        resources.ApplyResources(Me.lblToErase, "lblToErase")
        Me.lblToErase.Font = Nothing
        Me.lblToErase.Name = "lblToErase"
        '
        'frmErase
        '
        Me.AcceptButton = Me.btnOK
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.CancelButton = Me.btnCancel
        Me.Controls.Add(Me.btnBrowseToOut)
        Me.Controls.Add(Me.txtbxOutFile)
        Me.Controls.Add(Me.chkbxAddResult)
        Me.Controls.Add(Me.stsBar)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lblSaveAs)
        Me.Controls.Add(Me.btnSelectShape)
        Me.Controls.Add(Me.cmbxEraseWith)
        Me.Controls.Add(Me.btnBrowseEraseWith)
        Me.Controls.Add(Me.lblEraseWith)
        Me.Controls.Add(Me.cmbxToErase)
        Me.Controls.Add(Me.btnBrowseToErase)
        Me.Controls.Add(Me.lblToErase)
        Me.Font = Nothing
        Me.Icon = Nothing
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmErase"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Real Code"
    Private g_shapeSel As Integer
    Private g_selectingShapes As Boolean
    Private g_lastToErase As String
    Private g_lastEraseWith As String
    Private g_selShapeCount As Integer

    Public Sub Initialize()
        g_selShapeCount = 0
        stsBar.Text = "Select the shapefile to erase and polygon to erase with."
        PopulateToErase()
        setDefaultColoring()
    End Sub

    Private Sub PopulateToErase()
        Dim newlayer As MapWindow.Interfaces.Layer
        Dim i As Integer
        cmbxToErase.Items.Clear()
        cmbxEraseWith.Items.Clear()
        If g_MW.Layers.NumLayers > 0 Then
            For i = 0 To g_MW.Layers.NumLayers - 1
                newlayer = g_MW.Layers.Item(g_MW.Layers.GetHandle(i))
                If newlayer.LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                    cmbxEraseWith.Items.Add(newlayer.Name)
                    cmbxToErase.Items.Add(newlayer.Name)
                ElseIf ((newlayer.LayerType = MapWindow.Interfaces.eLayerType.LineShapefile) Or _
                        (newlayer.LayerType = MapWindow.Interfaces.eLayerType.PointShapefile)) Then
                    cmbxToErase.Items.Add(newlayer.Name)
                End If
            Next
        End If

        If (cmbxToErase.Items.Count > 0) Then
            If g_lastToErase <> "" Then
                cmbxToErase.SelectedIndex = cmbxToErase.Items.IndexOf(g_lastToErase)
            Else
                cmbxToErase.SelectedIndex = 0
            End If
        End If

        If (cmbxEraseWith.Items.Count > 0) Then
            If g_lastEraseWith <> "" Then
                cmbxEraseWith.SelectedIndex = cmbxEraseWith.Items.IndexOf(g_lastEraseWith)
            Else
                cmbxEraseWith.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Function setDefaultColoring() As Boolean
        'just set text to original color
        'validation occurs when user hits the OK button
        lblToErase.ForeColor = Drawing.Color.Black
        lblEraseWith.ForeColor = Drawing.Color.Black
        btnSelectShape.ForeColor = Drawing.Color.Black
        lblSaveAs.ForeColor = Drawing.Color.Black
    End Function

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

    Private Function layerExists(ByVal strPath As String) As Boolean
        Dim i As Integer
        For i = 0 To g_MW.Layers.NumLayers - 1
            If g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).FileName = strPath Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub AddMap(ByVal fname As String, ByVal isResult As Boolean)
        Dim FileDesc As String
        FileDesc = System.IO.Path.GetFileName(fname)
        If (FileDesc = "sta.adf") Then
            FileDesc = System.IO.Path.GetDirectoryName(fname)
            FileDesc = System.IO.Path.GetFileNameWithoutExtension(FileDesc)
        End If

        Dim NewLayer, inputLayer As MapWindow.Interfaces.Layer

        If System.IO.File.Exists(fname) Then
            NewLayer = g_MW.Layers.Add(fname, FileDesc)
            Dim shapeCS As New MapWinGIS.ShapefileColorScheme
            If isResult Then
                inputLayer = g_MW.Layers(getIndexByName(cmbxToErase.Text))
                shapeCS = inputLayer.ColoringScheme
                NewLayer.ColoringScheme = shapeCS
                NewLayer.Color = inputLayer.Color
                NewLayer.OutlineColor = inputLayer.OutlineColor
                NewLayer.LineOrPointSize = inputLayer.LineOrPointSize
                NewLayer.DrawFill = inputLayer.DrawFill
            Else
                NewLayer.OutlineColor = System.Drawing.Color.Red
                NewLayer.Color = Drawing.Color.Pink
                NewLayer.LineOrPointSize = 2
                NewLayer.DrawFill = True
            End If
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
            AppendExt = Strings.Left(fname, i - 1) & Ext & Mid(fname, i)
        End If
    End Function

    Private Sub cmbxToErase_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxToErase.SelectedIndexChanged
        g_lastToErase = cmbxToErase.SelectedItem().ToString()
        If (g_lastEraseWith <> "") Then
            cmbxEraseWith.SelectedIndex = cmbxEraseWith.Items.IndexOf(g_lastEraseWith)
        End If
        PopulateOutFile()
        setDefaultColoring()
    End Sub

    Private Sub PopulateOutFile()
        Dim outFile As String
        outFile = ""
        If cmbxToErase.Items.Count > 0 Then
            'set the output path
            outFile = AppendExt(getPathByName(cmbxToErase.Items(cmbxToErase.SelectedIndex)), "_erase")
            Dim i As Integer
            i = 0
            Dim eraseExt As String
            While System.IO.File.Exists(outFile)
                eraseExt = "_erase" & i
                outFile = AppendExt(getPathByName(cmbxToErase.Items(cmbxToErase.SelectedIndex)), eraseExt)
                i = i + 1
            End While
            txtbxOutFile.Text = outFile
        Else
            txtbxOutFile.Text = ""
        End If
    End Sub

    Private Sub cmbxEraseWith_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxEraseWith.SelectedIndexChanged
        If cmbxEraseWith.Items.Count > 0 Then
            g_selShapeCount = 0
            stsBar.Text = "0 shapes selected. Click 'Select Shapes' to select."
            btnSelectShape.Enabled = True
            g_lastToErase = cmbxToErase.SelectedItem().ToString()
        Else
            btnSelectShape.Enabled = False
        End If

        If (g_lastEraseWith <> "") Then
            cmbxEraseWith.SelectedIndex = cmbxEraseWith.Items.IndexOf(g_lastEraseWith)
        End If

        setDefaultColoring()
    End Sub

    Private Sub txtbxOutFile_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtbxOutFile.TextChanged
        Dim enableOK As Boolean
        enableOK = False
        If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(txtbxOutFile.Text)) Then
            enableOK = True
            If System.IO.File.Exists(txtbxOutFile.Text) Then
                'make sure that the file is not already in use
                Dim inUse As Boolean
                inUse = False
                Dim i As Integer
                For i = 0 To g_MW.Layers.NumLayers - 1
                    If g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).FileName = txtbxOutFile.Text Then
                        inUse = True
                        i = g_MW.Layers.NumLayers 'break from loop
                    End If
                Next
                If inUse = True Then
                    'Do not continue until user has changed the file name
                    System.Windows.Forms.MessageBox.Show(txtbxOutFile.Text & " is a layer in MapWindow." & Environment.NewLine & "Please choose another path before continuing.", "File In Use", System.Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Warning)
                    btnOK.Enabled = False
                    enableOK = False
                Else
                    'Warn user that this file already exists and will be overwritten
                    System.Windows.Forms.MessageBox.Show(txtbxOutFile.Text & " already exists." & Environment.NewLine & "Please choose another path if you don't want this file to be overwritten.", "File Already Exists", System.Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Warning)
                    enableOK = True
                End If
            End If
        End If

        If (enableOK = True) Then
            setDefaultColoring()
        End If
    End Sub

    Private Sub btnBrowseToErase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseToErase.Click
        Dim strPath As String
        Dim sf As New MapWinGIS.Shapefile
        Dim fdiagOpen As System.Windows.Forms.OpenFileDialog = New System.Windows.Forms.OpenFileDialog
        fdiagOpen.Filter = sf.CdlgFilter
        fdiagOpen.FilterIndex = 1
        If fdiagOpen.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            strPath = fdiagOpen.FileName
            If Not layerExists(strPath) Then
                AddMap(strPath, 0)
                PopulateToErase()
                g_lastToErase = getNameByPath(strPath)
                cmbxToErase.SelectedIndex = cmbxToErase.Items.IndexOf(g_lastToErase)
            Else
                mapwinutility.logger.msg("This layer already exists. Please select it from the drop down list.")
            End If
        End If
    End Sub

    Private Sub btnBrowseEraseWith_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseEraseWith.Click
        Dim strPath As String
        Dim sf As New MapWinGIS.Shapefile

        Dim fdiagOpen As System.Windows.Forms.OpenFileDialog = New System.Windows.Forms.OpenFileDialog
        fdiagOpen.Filter = sf.CdlgFilter
        fdiagOpen.FilterIndex = 1

        If fdiagOpen.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            strPath = fdiagOpen.FileName
            If Not layerExists(strPath) Then
                sf.Open(strPath)
                If sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGON Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONM Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONZ _
                Then
                    AddMap(strPath, False)
                    PopulateToErase()
                    g_lastEraseWith = getNameByPath(strPath)
                    cmbxEraseWith.SelectedIndex = cmbxEraseWith.Items.IndexOf(g_lastEraseWith)

                    If (g_lastToErase <> "") Then
                        cmbxToErase.SelectedIndex = cmbxToErase.Items.IndexOf(g_lastToErase)
                    End If
                Else
                    mapwinutility.logger.msg("Please select a POLYGON shapefile.")
                End If
                sf.Close()
            Else
                MapWinUtility.Logger.Msg("That layer already exists. Please select it from the drop down list.")
            End If
        End If
    End Sub

    Private Sub btnBrowseToOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseToOut.Click
        Dim enableOK As Boolean
        enableOK = True
        Dim strPath As String
        Dim sf As New MapWinGIS.Shapefile
        Dim fdiagSave As System.Windows.Forms.SaveFileDialog = New System.Windows.Forms.SaveFileDialog
        fdiagSave.Filter = sf.CdlgFilter
        fdiagSave.FilterIndex = 1
        If fdiagSave.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            strPath = fdiagSave.FileName
            If Not layerExists(strPath) Then
                txtbxOutFile.Text = strPath
            Else
                MapWinUtility.Logger.Msg("This layer already exists. Please remove it or select a different file to save to.")
                enableOK = False
                btnOK.Enabled = False
            End If
        End If

        If (enableOK = True) Then
            setDefaultColoring()
        End If
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
                stsBar.Text = "0 shapes selected. Click the 'Select Shapes' button to select."
            ElseIf g_selShapeCount = 1 Then
                stsBar.Text = g_selShapeCount.ToString + " shape selected. Click OK to erase."
            Else
                stsBar.Text = g_selShapeCount.ToString + " shapes selected. Click OK to erase."
            End If
        Else
            g_selectingShapes = True
            stsBar.Text = "Click to Select. Hold CTRL to select multiple. Click Done once finished."
            btnSelectShape.Text = "Done"
            g_MW.Layers.CurrentLayer = getIndexByName(cmbxEraseWith.Items(cmbxEraseWith.SelectedIndex))
            g_MW.View.CursorMode = MapWinGIS.tkCursorMode.cmSelection
        End If
        setDefaultColoring()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim okToStart As Boolean
        okToStart = True
        setDefaultColoring()

        Try
            If (cmbxToErase.Items.Count > 0) Then
                okToStart = True
            Else
                okToStart = False
                stsBar.Text = "Please select a shapefile to erase."
                lblToErase.ForeColor = Drawing.Color.Red
                cmbxToErase.Focus()
            End If

            If (okToStart) Then
                If (cmbxEraseWith.Items.Count > 0) Then
                    okToStart = True
                Else
                    okToStart = False
                    stsBar.Text = "Please select a polygon shapefile to erase with."
                    lblEraseWith.ForeColor = Drawing.Color.Red
                    cmbxEraseWith.Focus()
                End If
            End If

            If (okToStart) Then
                If (g_selShapeCount > 0) Then
                    okToStart = True
                Else
                    okToStart = False
                    stsBar.Text = "Please select shapes to erase with."
                    btnSelectShape.ForeColor = Drawing.Color.Red
                    btnSelectShape.Focus()
                End If
            End If

            If (okToStart) Then
                If (Not g_selectingShapes) Then
                    okToStart = True
                Else
                    okToStart = False
                    stsBar.Text = "Please finish selecting shapes."
                    btnSelectShape.ForeColor = Drawing.Color.Red
                    btnSelectShape.Focus()
                End If
            End If

            If (okToStart) Then
                If (txtbxOutFile.Text <> "") Then
                    okToStart = True
                Else
                    okToStart = False
                    stsBar.Text = "Please specify an output file."
                    lblSaveAs.ForeColor = Drawing.Color.Red
                    txtbxOutFile.Focus()
                End If
            End If

            If (okToStart) Then
                If System.IO.File.Exists(txtbxOutFile.Text) Then
                    'make sure that the file is not already in use
                    Dim inUse As Boolean
                    inUse = False
                    Dim i As Integer
                    For i = 0 To g_MW.Layers.NumLayers - 1
                        If g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).FileName = txtbxOutFile.Text Then
                            inUse = True
                            i = g_MW.Layers.NumLayers 'break from loop
                        End If
                    Next
                    If inUse = True Then
                        'Do not continue until user has changed the file name
                        System.Windows.Forms.MessageBox.Show(txtbxOutFile.Text & " is a layer in MapWindow." & Environment.NewLine & "Please choose another path before continuing.", "File In Use", System.Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Warning)
                        okToStart = False
                        lblSaveAs.ForeColor = Drawing.Color.Red
                        txtbxOutFile.Focus()
                    End If
                End If 'end of checking if outFile exists and is in use

            End If
        Catch ex As Exception
        End Try

        If (okToStart) Then
            Cursor = Windows.Forms.Cursors.WaitCursor
            If (g_selShapeCount > 1) Then
                stsBar.Text = "Erasing with shapes"
            Else
                stsBar.Text = "Erasing with shape"
            End If
            EraseShapes()
            Cursor = Windows.Forms.Cursors.Default
        End If

    End Sub

    Private Sub EraseShapes()
        Dim polyfile, sfPath, resultfile As String
        Dim polygon As New MapWinGIS.Shape
        Dim success As Boolean = True
        Dim i As Integer
        Dim sf As New MapWinGIS.Shapefile
        Dim numSel As Integer

        sfPath = getPathByName(cmbxToErase.Items(cmbxToErase.SelectedIndex))
        polyfile = getPathByName(cmbxEraseWith.Items(cmbxEraseWith.SelectedIndex))
        resultfile = txtbxOutFile.Text

        numSel = g_MW.View.SelectedShapes.NumSelected

        sf.Open(polyfile)
        If numSel = 1 Then
            polygon = sf.Shape(g_MW.View.SelectedShapes.Item(0).ShapeIndex)

            If MapWinGeoProc.SpatialOperations.Erase(sfPath, polygon, resultfile) Then
                If chkbxAddResult.Checked Then
                    RemoveLayer(resultfile)
                    AddMap(resultfile, True)
                End If
            Else
                success = False
            End If

        Else
            stsBar.Text = "Erasing shapefile with multiple polygons."
            Dim eraseFile As New MapWinGIS.Shapefile
            Dim erasePath As String
            erasePath = System.IO.Path.GetTempPath & "tempErasePath.shp"
            MapWinGeoProc.DataManagement.DeleteShapefile(erasePath)
            eraseFile.CreateNew(erasePath, sf.ShapefileType)
            eraseFile.StartEditingShapes()
            Dim shpIndex As Integer
            shpIndex = 0
            For i = 0 To numSel - 1
                Dim polyShp As New MapWinGIS.Shape
                polyShp.Create(MapWinGIS.ShpfileType.SHP_POLYGON)
                polyShp = sf.Shape(g_MW.View.SelectedShapes.Item(i).ShapeIndex)
                eraseFile.EditInsertShape(polyShp, shpIndex)
                shpIndex += 1
            Next
            eraseFile.StopEditingShapes()
            eraseFile.SaveAs(erasePath)
            eraseFile.Close()

            If MapWinGeoProc.SpatialOperations.Erase(sfPath, erasePath, resultfile) Then
                If chkbxAddResult.Checked Then
                    RemoveLayer(resultfile)
                    AddMap(resultfile, True)
                End If
            Else
                success = False
            End If

            MapWinGeoProc.DataManagement.DeleteShapefile(erasePath)
        End If

        sf.Close()
        If success Then
            Me.Close()
        Else
            Dim errorMsg As String = MapWinGeoProc.Error.GetLastErrorMsg()
            System.Windows.Forms.MessageBox.Show(errorMsg, "Erase Error")
            Debug.WriteLine("ErrorMsg: " + errorMsg)
            Me.Close()
        End If
    End Sub
#End Region

End Class
