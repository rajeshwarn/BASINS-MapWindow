'********************************************************************************************************
'File Name: frmClip.vb
'Description: Tool for clipping grids and shapefiles with polygons.
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
'11/16/05 - Allen Anselmo - created original tool
'11/17/05 - Angela Hillier - wrote clipShapes() to work with multiple polygon selections
'03/30/06 - Angela Hillier - added checks to see if output file already existed and warn user accordingly
'04/06/06 - Angela Hillier - fixed the browse buttons, eliminated duplicate user instructions
'05/19/06 - Allen Anselmo  - Changed addmap to allow result files to be added with input grid or shape's colorscheme
'05/23/06 - Angela Hillier - Changed 'ok' button behavior so that it validates all inputs when clicked
'08/23/06 - JLK            - Added use of Logger
'06/20/2007 - Jack MacDonald - Create foreign key fields in the resultant attribute table
'********************************************************************************************************
Imports System.Runtime.InteropServices
Imports MapWinUtility

Public Class frmClip
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
    Friend WithEvents lblToClip As System.Windows.Forms.Label
    Friend WithEvents btnBrowseToClip As System.Windows.Forms.Button
    Friend WithEvents cmbxToClip As System.Windows.Forms.ComboBox
    Friend WithEvents cmbxClipWith As System.Windows.Forms.ComboBox
    Friend WithEvents btnBrowseClipWith As System.Windows.Forms.Button
    Friend WithEvents lblClipWith As System.Windows.Forms.Label
    Friend WithEvents btnSelectShape As System.Windows.Forms.Button
    Friend WithEvents lblSaveAs As System.Windows.Forms.Label
    Friend WithEvents chkbxAddClip As System.Windows.Forms.CheckBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents chkbxFastClip As System.Windows.Forms.CheckBox
    Friend WithEvents txtbxOutFile As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowseToOut As System.Windows.Forms.Button
    Friend WithEvents stsBar As System.Windows.Forms.StatusBar
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmClip))
        Me.lblToClip = New System.Windows.Forms.Label
        Me.btnBrowseToClip = New System.Windows.Forms.Button
        Me.cmbxToClip = New System.Windows.Forms.ComboBox
        Me.cmbxClipWith = New System.Windows.Forms.ComboBox
        Me.btnBrowseClipWith = New System.Windows.Forms.Button
        Me.lblClipWith = New System.Windows.Forms.Label
        Me.btnSelectShape = New System.Windows.Forms.Button
        Me.lblSaveAs = New System.Windows.Forms.Label
        Me.txtbxOutFile = New System.Windows.Forms.TextBox
        Me.chkbxAddClip = New System.Windows.Forms.CheckBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.chkbxFastClip = New System.Windows.Forms.CheckBox
        Me.stsBar = New System.Windows.Forms.StatusBar
        Me.btnBrowseToOut = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lblToClip
        '
        Me.lblToClip.AccessibleDescription = Nothing
        Me.lblToClip.AccessibleName = Nothing
        resources.ApplyResources(Me.lblToClip, "lblToClip")
        Me.lblToClip.Font = Nothing
        Me.lblToClip.Name = "lblToClip"
        '
        'btnBrowseToClip
        '
        Me.btnBrowseToClip.AccessibleDescription = Nothing
        Me.btnBrowseToClip.AccessibleName = Nothing
        resources.ApplyResources(Me.btnBrowseToClip, "btnBrowseToClip")
        Me.btnBrowseToClip.BackgroundImage = Nothing
        Me.btnBrowseToClip.Font = Nothing
        Me.btnBrowseToClip.Name = "btnBrowseToClip"
        '
        'cmbxToClip
        '
        Me.cmbxToClip.AccessibleDescription = Nothing
        Me.cmbxToClip.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbxToClip, "cmbxToClip")
        Me.cmbxToClip.BackgroundImage = Nothing
        Me.cmbxToClip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxToClip.Font = Nothing
        Me.cmbxToClip.Name = "cmbxToClip"
        '
        'cmbxClipWith
        '
        Me.cmbxClipWith.AccessibleDescription = Nothing
        Me.cmbxClipWith.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbxClipWith, "cmbxClipWith")
        Me.cmbxClipWith.BackgroundImage = Nothing
        Me.cmbxClipWith.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxClipWith.Font = Nothing
        Me.cmbxClipWith.Name = "cmbxClipWith"
        '
        'btnBrowseClipWith
        '
        Me.btnBrowseClipWith.AccessibleDescription = Nothing
        Me.btnBrowseClipWith.AccessibleName = Nothing
        resources.ApplyResources(Me.btnBrowseClipWith, "btnBrowseClipWith")
        Me.btnBrowseClipWith.BackgroundImage = Nothing
        Me.btnBrowseClipWith.Font = Nothing
        Me.btnBrowseClipWith.Name = "btnBrowseClipWith"
        '
        'lblClipWith
        '
        Me.lblClipWith.AccessibleDescription = Nothing
        Me.lblClipWith.AccessibleName = Nothing
        resources.ApplyResources(Me.lblClipWith, "lblClipWith")
        Me.lblClipWith.Font = Nothing
        Me.lblClipWith.Name = "lblClipWith"
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
        'lblSaveAs
        '
        Me.lblSaveAs.AccessibleDescription = Nothing
        Me.lblSaveAs.AccessibleName = Nothing
        resources.ApplyResources(Me.lblSaveAs, "lblSaveAs")
        Me.lblSaveAs.Font = Nothing
        Me.lblSaveAs.Name = "lblSaveAs"
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
        'chkbxAddClip
        '
        Me.chkbxAddClip.AccessibleDescription = Nothing
        Me.chkbxAddClip.AccessibleName = Nothing
        resources.ApplyResources(Me.chkbxAddClip, "chkbxAddClip")
        Me.chkbxAddClip.BackgroundImage = Nothing
        Me.chkbxAddClip.Checked = True
        Me.chkbxAddClip.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbxAddClip.Font = Nothing
        Me.chkbxAddClip.Name = "chkbxAddClip"
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
        'chkbxFastClip
        '
        Me.chkbxFastClip.AccessibleDescription = Nothing
        Me.chkbxFastClip.AccessibleName = Nothing
        resources.ApplyResources(Me.chkbxFastClip, "chkbxFastClip")
        Me.chkbxFastClip.BackgroundImage = Nothing
        Me.chkbxFastClip.Font = Nothing
        Me.chkbxFastClip.Name = "chkbxFastClip"
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
        'btnBrowseToOut
        '
        Me.btnBrowseToOut.AccessibleDescription = Nothing
        Me.btnBrowseToOut.AccessibleName = Nothing
        resources.ApplyResources(Me.btnBrowseToOut, "btnBrowseToOut")
        Me.btnBrowseToOut.BackgroundImage = Nothing
        Me.btnBrowseToOut.Font = Nothing
        Me.btnBrowseToOut.Name = "btnBrowseToOut"
        '
        'frmClip
        '
        Me.AcceptButton = Me.btnOK
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.CancelButton = Me.btnCancel
        Me.Controls.Add(Me.btnBrowseToOut)
        Me.Controls.Add(Me.txtbxOutFile)
        Me.Controls.Add(Me.chkbxAddClip)
        Me.Controls.Add(Me.stsBar)
        Me.Controls.Add(Me.chkbxFastClip)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lblSaveAs)
        Me.Controls.Add(Me.btnSelectShape)
        Me.Controls.Add(Me.cmbxClipWith)
        Me.Controls.Add(Me.btnBrowseClipWith)
        Me.Controls.Add(Me.lblClipWith)
        Me.Controls.Add(Me.cmbxToClip)
        Me.Controls.Add(Me.btnBrowseToClip)
        Me.Controls.Add(Me.lblToClip)
        Me.Font = Nothing
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmClip"
        Me.ShowInTaskbar = False
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Real Code"
    Private g_clipType As Integer
    Private g_ShapeSel As Integer
    Private g_SelectingShapes As Boolean
    Private lastToClip As String
    Private lastClipWith As String
    Private SelShapeCount As Integer

    Public Sub Initialize(ByVal clipType As Integer)
        g_clipType = clipType
        SelShapeCount = 0
        chkbxFastClip.Enabled = False
        Select Case g_clipType
            Case 0
                Me.Text = "Clip Grid"
                stsBar.Text = "Select the grid to clip and polygon to clip with."
                lblToClip.Text = "Select a Grid to Clip:"
                lblClipWith.Text = "Select a Polygon Shapefile to Clip With:"
                PopulateGridClip()
            Case 1
                Me.Text = "Clip Shapefile"
                stsBar.Text = "Select the shapefile to clip and polygon to clip with."
                lblToClip.Text = "Select a Shapefile to Clip:"
                lblClipWith.Text = "Select a Polygon Shapefile to Clip With:"
                PopulateShapeClip()
        End Select
        setDefaultColoring()
        setFastEnable()
    End Sub

    Private Sub PopulateLists()
        Select Case g_clipType
            Case 0
                PopulateGridClip()
            Case 1
                PopulateShapeClip()
        End Select
    End Sub

    Private Sub PopulateGridClip()
        Dim newlayer As MapWindow.Interfaces.Layer
        Dim i As Integer
        cmbxToClip.Items.Clear()
        cmbxClipWith.Items.Clear()
        If g_MW.Layers.NumLayers > 0 Then
            For i = 0 To g_MW.Layers.NumLayers - 1
                newlayer = g_MW.Layers.Item(g_MW.Layers.GetHandle(i))
                If newlayer.LayerType = MapWindow.Interfaces.eLayerType.Grid Then
                    cmbxToClip.Items.Add(newlayer.Name)
                ElseIf newlayer.LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                    cmbxClipWith.Items.Add(newlayer.Name)
                End If
            Next
        End If

        If (cmbxToClip.Items.Count > 0) Then
            If lastToClip <> "" Then
                cmbxToClip.SelectedIndex = cmbxToClip.Items.IndexOf(lastToClip)
            Else
                cmbxToClip.SelectedIndex = 0
            End If
        End If

        If (cmbxClipWith.Items.Count > 0) Then
            If lastClipWith <> "" Then
                cmbxClipWith.SelectedIndex = cmbxClipWith.Items.IndexOf(lastClipWith)
            Else
                cmbxClipWith.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub PopulateShapeClip()
        Dim newlayer As MapWindow.Interfaces.Layer
        Dim i As Integer
        cmbxToClip.Items.Clear()
        cmbxClipWith.Items.Clear()
        If g_MW.Layers.NumLayers > 0 Then
            For i = 0 To g_MW.Layers.NumLayers - 1
                newlayer = g_MW.Layers.Item(g_MW.Layers.GetHandle(i))
                If newlayer.LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                    cmbxClipWith.Items.Add(newlayer.Name)
                    cmbxToClip.Items.Add(newlayer.Name)
                ElseIf ((newlayer.LayerType = MapWindow.Interfaces.eLayerType.LineShapefile) Or _
                        (newlayer.LayerType = MapWindow.Interfaces.eLayerType.PointShapefile)) Then
                    cmbxToClip.Items.Add(newlayer.Name)
                End If
            Next
        End If

        If (cmbxToClip.Items.Count > 0) Then
            If lastToClip <> "" Then
                cmbxToClip.SelectedIndex = cmbxToClip.Items.IndexOf(lastToClip)
            Else
                cmbxToClip.SelectedIndex = 0 ' Select first layer
            End If
        End If

        If (cmbxClipWith.Items.Count > 0) Then
            If lastClipWith <> "" Then
                cmbxClipWith.SelectedIndex = cmbxClipWith.Items.IndexOf(lastClipWith)
            Else
                cmbxClipWith.SelectedIndex = cmbxClipWith.Items.Count - 1 ' Select last layer
            End If
        End If

    End Sub

    Private Sub setDefaultColoring()
        lblToClip.ForeColor = Drawing.Color.Black
        lblClipWith.ForeColor = Drawing.Color.Black
        lblSaveAs.ForeColor = Drawing.Color.Black
        btnSelectShape.ForeColor = Drawing.Color.Black
    End Sub

    Private Sub setFastEnable()
        Dim indx As Integer
        chkbxFastClip.Enabled = False
        If g_clipType = 1 Then
            If cmbxClipWith.Items.Count > 0 Then
                indx = getIndexByName(cmbxToClip.Items(cmbxToClip.SelectedIndex))
                If g_MW.Layers.Item(indx).LayerType = MapWindow.Interfaces.eLayerType.LineShapefile Then
                    chkbxFastClip.Enabled = True
                End If
            End If
        ElseIf g_clipType = 0 Then
            chkbxFastClip.Enabled = True
            chkbxFastClip.Text = "Clip to Extents (Fast)"
        End If
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

    Private Function layerExists(ByVal strPath As String) As Boolean
        Dim i As Integer
        For i = 0 To g_MW.Layers.NumLayers - 1
            If g_MW.Layers.Item(g_MW.Layers.GetHandle(i)).FileName = strPath Then
                Return True
            End If
        Next
        Return False
    End Function
    'removed addType/legend name descriptions because it got very confusing when
    'multiple clippings were being performed in the same project -- Angela Hillier 4/6/06
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

            If NewLayer.LayerType = MapWindow.Interfaces.eLayerType.Grid Then
                Dim g As MapWinGIS.Grid = NewLayer.GetGridObject
                Dim CS As New MapWinGIS.GridColorScheme
                If isResult Then
                    inputLayer = g_MW.Layers(getIndexByName(cmbxToClip.Text))
                    CS = inputLayer.ColoringScheme
                    NewLayer.ColoringScheme = CS
                    g_MW.Layers.RebuildGridLayer(NewLayer.Handle, NewLayer.GetGridObject, CS)
                Else
                    NewLayer.ColoringScheme = CS
                    CS.UsePredefined(g.Minimum, g.Maximum, MapWinGIS.PredefinedColorScheme.SummerMountains)
                    g_MW.Layers.RebuildGridLayer(NewLayer.Handle, NewLayer.GetGridObject, CS)
                End If
                '9-Oct-2009 Rob Cairns. If project settings are set to load grid as image 
                'an Else statement will cause the following crash here 
                'System.InvalidCastException: Unable to cast COM object of type 'System.__ComObject' to interface type 'MapWinGIS.ShapefileColorScheme'. 
            ElseIf NewLayer.LayerType <> MapWindow.Interfaces.eLayerType.Image Then
                Dim shapeCS As New MapWinGIS.ShapefileColorScheme
                If isResult Then
                    inputLayer = g_MW.Layers(getIndexByName(cmbxToClip.Text))
                    shapeCS = inputLayer.ColoringScheme
                    NewLayer.ColoringScheme = shapeCS
                    NewLayer.Color = inputLayer.Color
                    NewLayer.OutlineColor = inputLayer.OutlineColor
                    NewLayer.LineOrPointSize = inputLayer.LineOrPointSize
                    NewLayer.DrawFill = inputLayer.DrawFill
                Else
                    NewLayer.OutlineColor = System.Drawing.Color.Red
                    NewLayer.Color = Drawing.Color.Red
                    NewLayer.LineOrPointSize = 2
                    NewLayer.DrawFill = True
                End If
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
            AppendExt = Microsoft.VisualBasic.Strings.Left(fname, i - 1) & Ext & Mid(fname, i)
        End If
    End Function

    Private Sub cmbxToClip_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxToClip.SelectedIndexChanged
        lastToClip = cmbxToClip.SelectedItem().ToString()
        If (lastClipWith <> "") Then
            cmbxClipWith.SelectedIndex = cmbxClipWith.Items.IndexOf(lastClipWith)
        End If
        PopulateOutFile()
        setDefaultColoring()
        setFastEnable()
    End Sub

    Private Sub PopulateOutFile()
        Dim outFile, inPath As String
        outFile = ""
        If cmbxToClip.Items.Count > 0 Then
            'set the output path
            inPath = getPathByName(cmbxToClip.Items(cmbxToClip.SelectedIndex))
            If IO.Path.GetFileName(inPath) = "sta.adf" Then
                inPath = IO.Path.GetDirectoryName(inPath) + ".bgd"
            End If
            outFile = AppendExt(inPath, "_clip")
            Dim i As Integer
            i = 0
            Dim clipExt As String
            While System.IO.File.Exists(outFile)
                clipExt = "_clip" & i
                outFile = AppendExt(getPathByName(cmbxToClip.Items(cmbxToClip.SelectedIndex)), clipExt)
                i = i + 1
            End While
            txtbxOutFile.Text = outFile
        Else
            txtbxOutFile.Text = ""
        End If
    End Sub

    Private Sub cmbxClipWith_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxClipWith.SelectedIndexChanged
        If cmbxClipWith.Items.Count > 0 And Not cmbxToClip.SelectedIndex = -1 Then
            SelShapeCount = 0
            stsBar.Text = "0 shapes selected. Click Select Shapes to select."
            btnSelectShape.Enabled = True
            lastToClip = cmbxToClip.SelectedItem().ToString()
        Else
            btnSelectShape.Enabled = False
        End If

        If (lastClipWith <> "" And cmbxClipWith.Items.Count > 0) Then
            cmbxClipWith.SelectedIndex = cmbxClipWith.Items.IndexOf(lastClipWith)
        End If

        setDefaultColoring()
        setFastEnable()
    End Sub

    Private Sub txtbxOutFile_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtbxOutFile.Leave
        If Not txtbxOutFile.Text.Contains(".") And Not txtbxOutFile.Text = "" Then
            txtbxOutFile.Text += ".bgd"
        End If
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
                    Logger.Msg(txtbxOutFile.Text & " is a layer in MapWindow." & Environment.NewLine & "Please choose another path before continuing.", _
                               MsgBoxStyle.Exclamation, "GISTools:Clip:File In Use")
                    btnOK.Enabled = False
                    enableOK = False
                Else
                    'Warn user that this file already exists and will be overwritten
                    Logger.Msg(txtbxOutFile.Text & " already exists." & Environment.NewLine & "Please choose another path if you don't want this file to be overwritten.", _
                               MsgBoxStyle.Exclamation, "GISTools:Clip:File Already Exists")
                    enableOK = True
                End If
            End If
        End If

        If (enableOK = True) Then
            setDefaultColoring()
        End If
    End Sub

    Private Sub BrowseToClipGrid()
        Dim g As New MapWinGIS.Grid
        Dim strPath As String
        Dim fdiagOpen As System.Windows.Forms.OpenFileDialog = New System.Windows.Forms.OpenFileDialog

        fdiagOpen.Filter = g.CdlgFilter
        fdiagOpen.FilterIndex = 1

        If fdiagOpen.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            strPath = fdiagOpen.FileName
            If Not layerExists(strPath) Then
                AddMap(strPath, False)
                PopulateLists()
                lastToClip = getNameByPath(strPath)
                If (cmbxToClip.Items.IndexOf(lastToClip) = -1) Then
                    cmbxToClip.Items.Add(lastToClip)
                End If
                cmbxToClip.SelectedIndex = cmbxToClip.Items.IndexOf(lastToClip)
            Else
                Logger.Msg("Grid layer < " & strPath & "> already exists. Please select it from the drop down list.", _
                           MsgBoxStyle.Information, "GISTools:ClipGrid")
            End If
        End If

        If (lastClipWith <> "") Then
            cmbxClipWith.SelectedIndex = cmbxClipWith.Items.IndexOf(lastClipWith)
        End If

    End Sub

    Private Sub BrowseToClipShape()
        Dim strPath As String
        Dim sf As New MapWinGIS.Shapefile
        Dim fdiagOpen As System.Windows.Forms.OpenFileDialog = New System.Windows.Forms.OpenFileDialog
        fdiagOpen.Filter = sf.CdlgFilter
        fdiagOpen.FilterIndex = 1

        If fdiagOpen.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            strPath = fdiagOpen.FileName
            If Not layerExists(strPath) Then
                AddMap(strPath, False)
                PopulateLists()
                lastToClip = getNameByPath(strPath)
                cmbxToClip.SelectedIndex = cmbxToClip.Items.IndexOf(lastToClip)
            Else
                Logger.Msg("Shapefile layer <" & strPath & "> already exists. Please select it from the drop down list.", _
                           MsgBoxStyle.Information, "GISTools:ClipShape")
            End If
        End If

        If (lastClipWith <> "") Then
            cmbxClipWith.SelectedIndex = cmbxClipWith.Items.IndexOf(lastClipWith)
        End If
    End Sub

    Private Sub BrowsePoly(ByVal addType As Integer)
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
                    PopulateLists()
                    If addType = 0 Then
                        lastToClip = getNameByPath(strPath)
                        cmbxToClip.SelectedIndex = cmbxToClip.Items.IndexOf(lastToClip)

                        If (lastClipWith <> "") Then
                            cmbxClipWith.SelectedIndex = cmbxClipWith.Items.IndexOf(lastClipWith)
                        End If
                    Else
                        lastClipWith = getNameByPath(strPath)
                        cmbxClipWith.SelectedIndex = cmbxClipWith.Items.IndexOf(lastClipWith)

                        If (lastToClip <> "") Then
                            cmbxToClip.SelectedIndex = cmbxToClip.Items.IndexOf(lastToClip)
                        End If
                    End If
                Else
                    Logger.Msg("Please select a POLYGON shapefile.", _
                               MsgBoxStyle.Information, "GISTools:Clip")
                End If
                sf.Close()
            Else
                Logger.Msg("Layer <" & strPath & "> already exists. Please select it from the drop down list.", _
                           MsgBoxStyle.Information, "GISTools:Clip")
            End If
        End If
    End Sub

    Private Sub btnBrowseToClip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseToClip.Click
        Select Case g_clipType
            Case 0
                BrowseToClipGrid()
            Case 1
                BrowseToClipShape()
        End Select
    End Sub

    Private Sub btnBrowseClipWith_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseClipWith.Click
        BrowsePoly(1)
    End Sub

    Private Sub btnBrowseToOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseToOut.Click
        Dim enableOK As Boolean
        enableOK = True
        Dim strPath As String
        If (g_clipType = 1) Then 'saving as a shapefile
            Dim sf As New MapWinGIS.Shapefile
            Dim fdiagSave As System.Windows.Forms.SaveFileDialog = New System.Windows.Forms.SaveFileDialog
            fdiagSave.Filter = sf.CdlgFilter
            fdiagSave.FilterIndex = 1
            If fdiagSave.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                strPath = fdiagSave.FileName
                If Not layerExists(strPath) Then
                    txtbxOutFile.Text = strPath
                Else
                    Logger.Msg("Layer <" & strPath & "> already exists. Please remove it or select a different file to save to.", _
                               MsgBoxStyle.Information, "GISTools:Clip")
                    enableOK = False
                    btnOK.Enabled = False
                End If
            End If
        Else 'saving as a grid
            Dim grid As New MapWinGIS.Grid
            Dim fdiagSave As System.Windows.Forms.SaveFileDialog = New System.Windows.Forms.SaveFileDialog
            fdiagSave.Filter = grid.CdlgFilter
            fdiagSave.DefaultExt = ".tif"
            If fdiagSave.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                If System.IO.Path.GetExtension(fdiagSave.FileName).ToLower().EndsWith("adf") Then fdiagSave.FileName = System.IO.Path.ChangeExtension(fdiagSave.FileName, ".tif")
                strPath = fdiagSave.FileName
                If Not layerExists(strPath) Then
                    txtbxOutFile.Text = strPath
                Else
                    Logger.Msg("Layer <" & strPath & "> already exists. Please remove it or select a different file to save to.", _
                               MsgBoxStyle.Information, "GISTools:Clip")
                    enableOK = False
                    btnOK.Enabled = False
                End If
            End If
        End If
        If (enableOK = True) Then
            setDefaultColoring()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Logger.Dbg("GISTools:Clip:UserCanceled")
        Me.Close()
    End Sub

    Private Sub btnSelectShape_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectShape.Click
        If g_SelectingShapes Then
            g_SelectingShapes = False
            btnSelectShape.Text = "Select Shapes"
            SelShapeCount = g_MW.View.SelectedShapes.NumSelected
            If SelShapeCount = 0 Then
                stsBar.Text = "0 shapes selected. Click the Select Shapes button to select."
            ElseIf SelShapeCount = 1 Then
                stsBar.Text = SelShapeCount.ToString + " shape selected. Click OK to clip."
            Else
                stsBar.Text = SelShapeCount.ToString + " shapes selected. Click OK to clip."
            End If
        Else
            g_SelectingShapes = True
            stsBar.Text = "Click to Select. Hold CTRL to select multiple. Click Done once finished."
            btnSelectShape.Text = "Done"
            Dim clippingLayername As String = cmbxClipWith.Items(cmbxClipWith.SelectedIndex)
            Logger.Dbg("Set active layer to: " & clippingLayername)
            'Todo This isn't always working, sometimes shapes on the wrong layer get selected.
            g_MW.Layers.CurrentLayer = getIndexByName(clippingLayername)
            g_MW.View.CursorMode = MapWinGIS.tkCursorMode.cmSelection
        End If
        setDefaultColoring()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Logger.Dbg("GISTools:Clip:UserSelectedOK")
        setDefaultColoring()
        Dim okToStart As Boolean
        okToStart = False
        Try
            If (cmbxToClip.Items.Count > 0) Then
                okToStart = True
            Else
                okToStart = False
                stsBar.Text = "Please select a file to clip."
                lblToClip.ForeColor = Drawing.Color.Red
                cmbxToClip.Focus()
            End If

            If (okToStart) Then
                If (cmbxClipWith.Items.Count > 0) Then
                    okToStart = True
                Else
                    okToStart = False
                    stsBar.Text = "Please select a polygon shapefile to clip with."
                    lblClipWith.ForeColor = Drawing.Color.Red
                    cmbxClipWith.Focus()
                End If
            End If

            If (okToStart) Then
                If (SelShapeCount > 0) Then
                    okToStart = True
                Else
                    okToStart = False
                    stsBar.Text = "Please select a polygon to clip with."
                    btnSelectShape.ForeColor = Drawing.Color.Red
                    btnSelectShape.Focus()
                End If
            End If

            If (okToStart) Then
                If (Not g_SelectingShapes) Then
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
                        Logger.Msg(txtbxOutFile.Text & " is a layer in MapWindow." & Environment.NewLine & "Please choose another path before continuing.", _
                                   MsgBoxStyle.Exclamation, "GISTools:Clip:File In Use")
                        btnOK.Enabled = False
                        txtbxOutFile.Focus()
                    End If
                End If 'end of checking if outFile exists and is in use
            End If

        Catch ex As Exception
            Logger.Dbg("GISTools:Clip:UserSelectedOK:Exception:" & ex.ToString)
        End Try

        If (okToStart) Then
            Logger.Dbg("GISTools:Clip:Start")
            Cursor = Windows.Forms.Cursors.WaitCursor
            Select Case g_clipType
                Case 0
                    stsBar.Text = "Clipping Grid"
                    clipGrid()
                Case 1
                    stsBar.Text = "Clipping Shapefile"
                    clipShapes()
            End Select
            Cursor = Windows.Forms.Cursors.Default
            Logger.Dbg("GISTools:Clip:Done")
        Else
            Logger.Dbg("GISTools:Clip:UserNeedsTo:" & stsBar.Text)
        End If
    End Sub

    Private Sub clipGrid()
        Dim polyfile, gridfile, baseresultfile, resultfile As String
        Dim polygon As New MapWinGIS.Shape
        Dim success As Boolean = True
        Dim clipToExtents As Boolean = False
        Dim i As Integer
        Dim sf As New MapWinGIS.Shapefile
        Dim u As New MapWinGIS.Utils
        Dim grids(), mergegrid As MapWinGIS.Grid
        Dim numSel As Integer

        If chkbxFastClip.Checked Then
            clipToExtents = True
        Else
            clipToExtents = False
        End If

        gridfile = getPathByName(cmbxToClip.Items(cmbxToClip.SelectedIndex))
        polyfile = getPathByName(cmbxClipWith.Items(cmbxClipWith.SelectedIndex))
        baseresultfile = txtbxOutFile.Text
        resultfile = txtbxOutFile.Text

        numSel = g_MW.View.SelectedShapes.NumSelected

        Logger.Dbg("GISTools:ClipGrid:GridFile:" & gridfile)
        Logger.Dbg("GISTools:ClipGrid:PolyFile:" & polyfile)
        Logger.Dbg("GISTools:ClipGrid:SelecetedCount:" & numSel)
        Logger.Dbg("GISTools:ClipGrid:ResultFile:" & resultfile)

        ' Paul Meems, 11 Oct 2011, Add check for projection:
        If chkbxAddClip.Checked AndAlso g_MW.Project.ProjectProjection <> String.Empty Then
            Dim grd As New MapWinGIS.Grid
            If grd.Open(gridfile, MapWinGIS.GridDataType.UnknownDataType, False) Then
                If grd.Header.Projection = String.Empty Then
                    ' Assign project projection:
                    If Not g_MW.Project.ProjectProjection = String.Empty Then
                        grd.AssignNewProjection(g_MW.Project.ProjectProjection)
                    End If
                End If
                grd.Close()
            End If
            ' Done using COM objects, release them
            While Marshal.ReleaseComObject(grd) <> 0
            End While
        End If

        sf.Open(polyfile)
        If numSel = 1 Then
            polygon = sf.Shape(g_MW.View.SelectedShapes.Item(0).ShapeIndex)

            If MapWinGeoProc.SpatialOperations.ClipGridWithPolygon(gridfile, polygon, baseresultfile, clipToExtents) Then
                If chkbxAddClip.Checked Then
                    RemoveLayer(resultfile)
                    AddMap(resultfile, True)
                End If
            Else
                success = False
            End If
        Else
            stsBar.Text = "Clipping multiple polygons from Grid."
            ReDim grids(numSel - 1)
            For i = 0 To numSel - 1
                resultfile = AppendExt(baseresultfile, (i + 1).ToString)
                polygon = sf.Shape(g_MW.View.SelectedShapes.Item(i).ShapeIndex)

                If MapWinGeoProc.SpatialOperations.ClipGridWithPolygon(gridfile, polygon, resultfile, clipToExtents) Then
                    Dim tmpgrid As New MapWinGIS.Grid
                    tmpgrid.Open(resultfile)
                    grids(i) = tmpgrid
                Else
                    success = False
                    Exit For
                End If
            Next
            If success Then
                stsBar.Text = "Merging clipped Grids."
                resultfile = AppendExt(baseresultfile, "Merged")
                mergegrid = u.GridMerge(grids, resultfile)
                mergegrid.Save(resultfile)
                mergegrid.Close()
                For i = 0 To numSel - 1
                    grids(i).Close()
                Next
                If chkbxAddClip.Checked Then
                    RemoveLayer(resultfile)
                    AddMap(resultfile, True)
                End If
            End If
        End If

        sf.Close()
        If success Then
            Me.Close()
        Else
            Dim errorMsg As String = MapWinGeoProc.Error.GetLastErrorMsg()
            If Not errorMsg.Contains("No Error") Then Logger.Msg(errorMsg, MsgBoxStyle.Exclamation, "GISTools:Clip:ClipGridError")
            Me.Close()
        End If
    End Sub
    ' Paul Meems 22 July 2009: Adding some logging and a try-catch, all for bug #1068
    Private Sub clipShapes()
        Dim polyfile, sfPath, resultfile As String
        Dim polygon As New MapWinGIS.Shape
        Dim success As Boolean = True
        Dim i, j As Integer
        Dim sf As New MapWinGIS.Shapefile
        Dim numSel As Integer
        Dim fastClip As Boolean

        sfPath = getPathByName(cmbxToClip.Items(cmbxToClip.SelectedIndex))
        polyfile = getPathByName(cmbxClipWith.Items(cmbxClipWith.SelectedIndex))
        resultfile = txtbxOutFile.Text
        fastClip = chkbxFastClip.Checked

        numSel = g_MW.View.SelectedShapes.NumSelected

        Logger.Dbg("GISTools:ClipShapes:GridFile:" & sfPath)
        Logger.Dbg("GISTools:ClipShapes:PolyFile:" & polyfile)
        Logger.Dbg("GISTools:ClipShapes:SelecetedCount:" & numSel)
        Logger.Dbg("GISTools:ClipShapes:ResultFile:" & resultfile)

        Try

            sf.Open(polyfile)
            If numSel = 1 Then
                polygon = sf.Shape(g_MW.View.SelectedShapes.Item(0).ShapeIndex)
                If MapWinGeoProc.SpatialOperations.ClipShapesWithPolygon(sfPath, polygon, resultfile, fastClip, True) Then
                    'Paul Meems 22 July 2009: Add projection of clipped shapefile:
                    copyProjectionFile(sfPath, resultfile)
                    If chkbxAddClip.Checked Then
                        RemoveLayer(resultfile)
                        AddMap(resultfile, True)
                    End If
                Else
                    success = False
                End If

            Else 'numSel > 1
                Dim tempSFPath As String
                Dim sourceSF As New MapWinGIS.Shapefile
                Dim shpType As New MapWinGIS.ShpfileType
                Dim resultSF As New MapWinGIS.Shapefile

                stsBar.Text = "Clipping shapefile with multiple polygons."
                tempSFPath = System.IO.Path.GetTempPath()
                'find what type of shapefile should be created
                sourceSF.Open(sfPath)
                shpType = sourceSF.ShapefileType
                'delete file if it already exists
                If System.IO.File.Exists(resultfile) Then
                    Dim length As Integer
                    Dim path As String
                    length = resultfile.IndexOf(".", 0)
                    path = resultfile.Substring(0, length)
                    System.IO.File.Delete(path & ".shp")
                    System.IO.File.Delete(path & ".shx")
                    System.IO.File.Delete(path & ".dbf")
                End If
                'create the result file
                resultSF.CreateNew(resultfile, shpType)
                'insert 3 fields into the result .dbf
                ' MWShapeID is the sequential ID number of the record in the results shapefile
                ' ClipPolyID is the MWShapeID number of the polygon that was used for clipping
                ' BackPolyID is the MWShapeID number of the item on the background that was clipped
                Dim ID As New MapWinGIS.Field
                Dim ClipPolyID As New MapWinGIS.Field
                Dim BackPolyID As New MapWinGIS.Field
                Dim ClipPolyMWShapeIndex As Integer
                Dim ClipPolyMWShapeID As Integer
                Dim BackPolyMWShapeIndex As Integer
                ID.Type = MapWinGIS.FieldType.INTEGER_FIELD
                ID.Name = "MWShapeID"
                resultSF.EditInsertField(ID, 0)
                ClipPolyID.Type = MapWinGIS.FieldType.INTEGER_FIELD
                ClipPolyID.Name = "ClipPolyID"
                resultSF.EditInsertField(ClipPolyID, 1)
                BackPolyID.Type = MapWinGIS.FieldType.INTEGER_FIELD
                BackPolyID.Name = "BackPolyID"
                resultSF.EditInsertField(BackPolyID, 2)

                ' determine where the MWShapeID is stored in the clipping shapefile
                ClipPolyMWShapeIndex = FieldIndexNumber("MWShapeID", sf, _
                                    "The clipping shapefile does not have a field named MWShapeID. " & _
                                        "The results shapefile will have zeros in the ClipPolyID field until you add the MWShapeID field; " & _
                                        "use the Tools menu in the Attribute Table Editor to create and populate the MWShapeID field.")

                Dim numIDs As Integer
                Dim numShapes As Integer
                Dim shpIndex As Integer
                numIDs = 0

                For i = 0 To numSel - 1
                    polygon = sf.Shape(g_MW.View.SelectedShapes.Item(i).ShapeIndex)
                    'find and store the value of MWShapeID of the current polygon that is used for clipping
                    ClipPolyMWShapeID = sf.CellValue(ClipPolyMWShapeIndex, g_MW.View.SelectedShapes.Item(i).ShapeIndex)

                    'delete temporary shapefile if it exists
                    If System.IO.File.Exists(tempSFPath & ".shp") Then
                        System.IO.File.Delete(tempSFPath & ".shx")
                        System.IO.File.Delete(tempSFPath & ".dbf")
                    End If
                    'create the temporary shapefile
                    Dim tempSF As New MapWinGIS.Shapefile
                    tempSF.CreateNew(tempSFPath, shpType)

                    If MapWinGeoProc.SpatialOperations.ClipShapesWithPolygon(sourceSF, polygon, tempSF, fastClip, True) Then

                        numShapes = tempSF.NumShapes
                        If numShapes > 0 Then
                            ' for the first polygon clipped, determine the location of the MWShapeID in the background shapefile
                            If i = 0 Then
                                BackPolyMWShapeIndex = FieldIndexNumber("MWShapeID", sourceSF, _
                                    "The background shapefile does not have a field named MWShapeID. " & _
                                        "The results shapefile will have zeros in the BackPolyID field until you add the MWShapeID field; " & _
                                        "use the Tools menu in the Attribute Table Editor to create and populate the MWShapeID field.")

                            End If

                            shpIndex = resultSF.NumShapes
                            For j = 0 To numShapes - 1
                                resultSF.EditInsertShape(tempSF.Shape(j), shpIndex)
                                shpIndex += 1
                                resultSF.EditCellValue(0, numIDs, numIDs)
                                If ClipPolyMWShapeIndex >= 0 Then
                                    resultSF.EditCellValue(1, numIDs, ClipPolyMWShapeID)
                                End If
                                If BackPolyMWShapeIndex >= 0 Then
                                    resultSF.EditCellValue(2, numIDs, tempSF.CellValue(BackPolyMWShapeIndex, j))
                                End If
                                stsBar.Text = "Creating clipped shape: " + numIDs.ToString + _
                                    ". Using shape " + (i + 1).ToString + " of " + numSel.ToString + " for clipping."

                                numIDs += 1
                            Next
                        End If
                    Else
                        success = False
                        Exit For
                    End If
                Next
                If success Then
                    'save result file and add to map
                    resultSF.StopEditingShapes(True, True)
                    resultSF.Close()
                    If chkbxAddClip.Checked Then
                        RemoveLayer(resultfile)
                        AddMap(resultfile, True)
                    End If
                Else
                    resultSF.StopEditingShapes(False, True)
                    resultSF.Close()
                End If
                sourceSF.Close()
            End If 'numSel
        Catch ex As Exception
            Logger.Msg("GISTools:Clip:clipShapes:Exception:" & ex.ToString)
        Finally
            sf.Close()
            If Not success Then

                Dim errorMsg As String = MapWinGeoProc.Error.GetLastErrorMsg()
                Logger.Msg(errorMsg, MsgBoxStyle.Exclamation, "GISTools:Clip:ClipShapeError")

            End If
            Me.Close()
        End Try
    End Sub
    ' Paul Meems 22 July 2009
    ' Copy the projection file from orgFile to newFile
    Private Sub copyProjectionFile(ByVal orgFile As String, ByVal newFile As String)
        Dim orgProjectionFile As String = System.IO.Path.ChangeExtension(orgFile, "prj")
        Dim newProjectionFile As String = System.IO.Path.ChangeExtension(newFile, "prj")
        ' If orgFile has a projection file
        If System.IO.File.Exists(orgProjectionFile) Then
            ' Copy it if it does not exists
            If Not System.IO.File.Exists(newProjectionFile) Then
                System.IO.File.Copy(orgProjectionFile, newProjectionFile, False)
            End If
        End If
    End Sub


    ' find and return the index number of a named field in a specified shapefile
    ' display an optional message if the fieldname is not found
    '    function returns -1 if the fieldname is not found
    <CLSCompliant(False)> _
    Function FieldIndexNumber(ByVal psFieldName As String, ByRef pShapeFile As MapWinGIS.Shapefile, _
            Optional ByVal psMSGIfNotFound As String = "") As Integer
        Dim j As Integer
        FieldIndexNumber = -1
        For j = 0 To pShapeFile.NumFields - 1
            If pShapeFile.Field(j).Name = psFieldName Then
                FieldIndexNumber = j
                Exit For
            End If
        Next
        If psMSGIfNotFound <> "" And FieldIndexNumber = -1 Then
            Logger.Msg(psMSGIfNotFound)
        End If
    End Function


#End Region

End Class

