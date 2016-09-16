'********************************************************************************************************
'File Name: frmMerge.vb
'Description: Merges shapes of the same type together into one shape.
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
'5/23/06 - Angela Hillier - Created original tool (based on frmClipPolyWLine)
'********************************************************************************************************
Public Class frmMerge
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
    Friend WithEvents stsBar As System.Windows.Forms.StatusBar
    Friend WithEvents btnSelectShape2 As System.Windows.Forms.Button
    Friend WithEvents btnBrowseSrc2 As System.Windows.Forms.Button
    Friend WithEvents cmbxSrc2 As System.Windows.Forms.ComboBox
    Friend WithEvents btnSelectShape1 As System.Windows.Forms.Button
    Friend WithEvents lblSrc2 As System.Windows.Forms.Label
    Friend WithEvents btnBrowseSrc1 As System.Windows.Forms.Button
    Friend WithEvents cmbxSrc1 As System.Windows.Forms.ComboBox
    Friend WithEvents lblSrc1 As System.Windows.Forms.Label
    Friend WithEvents btnBrowseToOut As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents txtbxOutFile As System.Windows.Forms.TextBox
    Friend WithEvents lblSaveAs As System.Windows.Forms.Label
    Friend WithEvents chkbxAddResults As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMerge))
        Me.stsBar = New System.Windows.Forms.StatusBar
        Me.btnSelectShape2 = New System.Windows.Forms.Button
        Me.btnBrowseSrc2 = New System.Windows.Forms.Button
        Me.cmbxSrc2 = New System.Windows.Forms.ComboBox
        Me.btnSelectShape1 = New System.Windows.Forms.Button
        Me.lblSrc2 = New System.Windows.Forms.Label
        Me.btnBrowseSrc1 = New System.Windows.Forms.Button
        Me.cmbxSrc1 = New System.Windows.Forms.ComboBox
        Me.lblSrc1 = New System.Windows.Forms.Label
        Me.btnBrowseToOut = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.chkbxAddResults = New System.Windows.Forms.CheckBox
        Me.txtbxOutFile = New System.Windows.Forms.TextBox
        Me.lblSaveAs = New System.Windows.Forms.Label
        Me.SuspendLayout()
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
        'btnSelectShape2
        '
        Me.btnSelectShape2.AccessibleDescription = Nothing
        Me.btnSelectShape2.AccessibleName = Nothing
        resources.ApplyResources(Me.btnSelectShape2, "btnSelectShape2")
        Me.btnSelectShape2.BackgroundImage = Nothing
        Me.btnSelectShape2.Font = Nothing
        Me.btnSelectShape2.Name = "btnSelectShape2"
        '
        'btnBrowseSrc2
        '
        Me.btnBrowseSrc2.AccessibleDescription = Nothing
        Me.btnBrowseSrc2.AccessibleName = Nothing
        resources.ApplyResources(Me.btnBrowseSrc2, "btnBrowseSrc2")
        Me.btnBrowseSrc2.BackgroundImage = Nothing
        Me.btnBrowseSrc2.Font = Nothing
        Me.btnBrowseSrc2.Name = "btnBrowseSrc2"
        '
        'cmbxSrc2
        '
        Me.cmbxSrc2.AccessibleDescription = Nothing
        Me.cmbxSrc2.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbxSrc2, "cmbxSrc2")
        Me.cmbxSrc2.BackgroundImage = Nothing
        Me.cmbxSrc2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxSrc2.Font = Nothing
        Me.cmbxSrc2.Name = "cmbxSrc2"
        '
        'btnSelectShape1
        '
        Me.btnSelectShape1.AccessibleDescription = Nothing
        Me.btnSelectShape1.AccessibleName = Nothing
        resources.ApplyResources(Me.btnSelectShape1, "btnSelectShape1")
        Me.btnSelectShape1.BackgroundImage = Nothing
        Me.btnSelectShape1.Font = Nothing
        Me.btnSelectShape1.Name = "btnSelectShape1"
        '
        'lblSrc2
        '
        Me.lblSrc2.AccessibleDescription = Nothing
        Me.lblSrc2.AccessibleName = Nothing
        resources.ApplyResources(Me.lblSrc2, "lblSrc2")
        Me.lblSrc2.Font = Nothing
        Me.lblSrc2.Name = "lblSrc2"
        '
        'btnBrowseSrc1
        '
        Me.btnBrowseSrc1.AccessibleDescription = Nothing
        Me.btnBrowseSrc1.AccessibleName = Nothing
        resources.ApplyResources(Me.btnBrowseSrc1, "btnBrowseSrc1")
        Me.btnBrowseSrc1.BackgroundImage = Nothing
        Me.btnBrowseSrc1.Font = Nothing
        Me.btnBrowseSrc1.Name = "btnBrowseSrc1"
        '
        'cmbxSrc1
        '
        Me.cmbxSrc1.AccessibleDescription = Nothing
        Me.cmbxSrc1.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbxSrc1, "cmbxSrc1")
        Me.cmbxSrc1.BackgroundImage = Nothing
        Me.cmbxSrc1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxSrc1.Font = Nothing
        Me.cmbxSrc1.Name = "cmbxSrc1"
        '
        'lblSrc1
        '
        Me.lblSrc1.AccessibleDescription = Nothing
        Me.lblSrc1.AccessibleName = Nothing
        resources.ApplyResources(Me.lblSrc1, "lblSrc1")
        Me.lblSrc1.Font = Nothing
        Me.lblSrc1.Name = "lblSrc1"
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
        'btnOk
        '
        Me.btnOk.AccessibleDescription = Nothing
        Me.btnOk.AccessibleName = Nothing
        resources.ApplyResources(Me.btnOk, "btnOk")
        Me.btnOk.BackgroundImage = Nothing
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.Font = Nothing
        Me.btnOk.Name = "btnOk"
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
        'frmMerge
        '
        Me.AcceptButton = Me.btnOk
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.CancelButton = Me.btnCancel
        Me.Controls.Add(Me.stsBar)
        Me.Controls.Add(Me.btnSelectShape2)
        Me.Controls.Add(Me.btnBrowseSrc2)
        Me.Controls.Add(Me.cmbxSrc2)
        Me.Controls.Add(Me.btnSelectShape1)
        Me.Controls.Add(Me.lblSrc2)
        Me.Controls.Add(Me.btnBrowseSrc1)
        Me.Controls.Add(Me.cmbxSrc1)
        Me.Controls.Add(Me.lblSrc1)
        Me.Controls.Add(Me.btnBrowseToOut)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.chkbxAddResults)
        Me.Controls.Add(Me.txtbxOutFile)
        Me.Controls.Add(Me.lblSaveAs)
        Me.Font = Nothing
        Me.Icon = Nothing
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMerge"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Real Code"
    Private g_SelectingShapes As Boolean
    Private g_LastSrc1 As String
    Private g_LastSrc2 As String
    Private g_SelSrc1Count As Integer
    Private g_SelSrc2Count As Integer
    Private g_Src1Array As ArrayList
    Private g_Src2Array As ArrayList

    Public Sub Initialize()
        g_SelSrc1Count = 0
        g_SelSrc2Count = 0
        g_Src1Array = New ArrayList(0)
        g_Src2Array = New ArrayList(0)
        stsBar.Text = "Select the shapes to merge."
        PopulateSrc1List()
        PopulateSrc2List()
        'PopulateLists()
        setDefaultColoring()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim okToStart As Boolean
        okToStart = False
        setDefaultColoring()

        Try
            If (cmbxSrc1.Items.Count > 0) Then
                okToStart = True
            Else
                stsBar.Text = "Please select a source file."
                lblSrc1.ForeColor = Drawing.Color.Red
                cmbxSrc1.Focus()
                okToStart = False
            End If

            If (okToStart) Then
                If (g_Src1Array.Count > 0) Then
                    okToStart = True
                Else
                    stsBar.Text = "Please select a shape from source 1."
                    btnSelectShape1.ForeColor = Drawing.Color.Red
                    btnSelectShape1.Focus()
                    okToStart = False
                End If
            End If

            'It's ok to not have a second source file as long as two shapes are selected from the first source file
            If (okToStart) Then
                If (cmbxSrc2.Items.Count > 0) Or (g_Src1Array.Count > 1) Then
                    okToStart = True
                Else
                    stsBar.Text = "Please select a source file."
                    lblSrc2.ForeColor = Drawing.Color.Red
                    cmbxSrc2.Focus()
                    okToStart = False
                End If
            End If

            If (okToStart) Then
                If (g_Src2Array.Count > 0) Or (g_Src1Array.Count > 1) Then
                    okToStart = True
                Else
                    stsBar.Text = "Please select a shape from source 2."
                    btnSelectShape2.ForeColor = Drawing.Color.Red
                    btnSelectShape2.Focus()
                    okToStart = False
                End If
            End If

            If (okToStart) Then
                If (txtbxOutFile.Text <> "") Then
                    okToStart = True
                Else
                    stsBar.Text = "Please specify an output file."
                    lblSaveAs.ForeColor = Drawing.Color.Red
                    txtbxOutFile.Focus()
                    okToStart = False
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
                        txtbxOutFile.Focus()
                    End If
                End If 'end of checking if outFile exists and is in use
            End If 'end of checking if all variables are valid
        Catch ex As Exception
        End Try

        If (okToStart) Then
            Cursor = Windows.Forms.Cursors.WaitCursor
            stsBar.Text = "Merging Shapes"
            MergeShapes()
            Cursor = Windows.Forms.Cursors.Default
        End If

    End Sub

    Private Sub PopulateSrc1List()
        Dim newlayer As MapWindow.Interfaces.Layer
        Dim i As Integer
        cmbxSrc1.Items.Clear()

        If g_MW.Layers.NumLayers > 0 Then
            For i = 0 To g_MW.Layers.NumLayers - 1
                newlayer = g_MW.Layers.Item(g_MW.Layers.GetHandle(i))
                If (newlayer.LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile) Or _
                    (newlayer.LayerType = MapWindow.Interfaces.eLayerType.LineShapefile) Or _
                    (newlayer.LayerType = MapWindow.Interfaces.eLayerType.PointShapefile) Then
                    cmbxSrc1.Items.Add(newlayer.Name)
                End If
            Next
        End If

        If (cmbxSrc1.Items.Count > 0) Then
            btnSelectShape1.Enabled = True
            If (g_LastSrc1 <> "") Then
                cmbxSrc1.SelectedIndex = cmbxSrc1.Items.IndexOf(g_LastSrc1)
            Else
                cmbxSrc1.SelectedIndex = 0
                g_LastSrc1 = cmbxSrc1.SelectedItem().ToString()
            End If
        End If

    End Sub

    Private Sub PopulateSrc2List()
        If (cmbxSrc1.Items.Count > 0) Then
            Dim newlayer As MapWindow.Interfaces.Layer
            Dim sfType As New MapWinGIS.ShpfileType
            Dim currSel As New MapWinGIS.Shapefile
            currSel.Open(getPathByName(cmbxSrc1.Items(cmbxSrc1.SelectedIndex)))
            sfType = currSel.ShapefileType
            currSel.Close()
            Dim i As Integer
            cmbxSrc2.Items.Clear()

            If g_MW.Layers.NumLayers > 0 Then
                For i = 0 To g_MW.Layers.NumLayers - 1
                    newlayer = g_MW.Layers.Item(g_MW.Layers.GetHandle(i))
                    If (newlayer.LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile) And _
                    (sfType = MapWinGIS.ShpfileType.SHP_POLYGON Or sfType = MapWinGIS.ShpfileType.SHP_POLYGONM Or sfType = MapWinGIS.ShpfileType.SHP_POLYGONZ) Then
                        cmbxSrc2.Items.Add(newlayer.Name)
                    ElseIf (newlayer.LayerType = MapWindow.Interfaces.eLayerType.PointShapefile) And _
                    (sfType = MapWinGIS.ShpfileType.SHP_POINT Or sfType = MapWinGIS.ShpfileType.SHP_POINTM Or sfType = MapWinGIS.ShpfileType.SHP_POINTZ) Then
                        cmbxSrc2.Items.Add(newlayer.Name)
                    ElseIf (newlayer.LayerType = MapWindow.Interfaces.eLayerType.LineShapefile) And _
                    (sfType = MapWinGIS.ShpfileType.SHP_POLYLINE Or sfType = MapWinGIS.ShpfileType.SHP_POLYLINEM Or sfType = MapWinGIS.ShpfileType.SHP_POLYLINEZ) Then
                        cmbxSrc2.Items.Add(newlayer.Name)
                    End If
                Next
            End If

            If (cmbxSrc2.Items.Count > 0) Then
                btnSelectShape2.Enabled = True
                If (g_LastSrc2 <> "") Then
                    If (cmbxSrc2.Items.IndexOf(g_LastSrc2) >= 0) Then
                        cmbxSrc2.SelectedIndex = cmbxSrc2.Items.IndexOf(g_LastSrc2)
                    End If
                Else
                    cmbxSrc2.SelectedIndex = 0
                    g_LastSrc2 = cmbxSrc2.SelectedItem().ToString()
                End If
            End If
        End If

    End Sub

    Private Sub setDefaultColoring()
        lblSrc1.ForeColor = Drawing.Color.Black
        lblSrc2.ForeColor = Drawing.Color.Black
        txtbxOutFile.ForeColor = Drawing.Color.Black
        btnSelectShape1.ForeColor = Drawing.Color.Black
        btnSelectShape2.ForeColor = Drawing.Color.Black
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

    Private Sub AddMap(ByVal fname As String)
        Dim FileDesc As String
        FileDesc = System.IO.Path.GetFileName(fname)
        If (FileDesc = "sta.adf") Then
            FileDesc = System.IO.Path.GetDirectoryName(fname)
            FileDesc = System.IO.Path.GetFileNameWithoutExtension(FileDesc)
        End If

        Dim NewLayer As MapWindow.Interfaces.Layer
        Dim CS As New MapWinGIS.GridColorScheme

        If System.IO.File.Exists(fname) Then
            NewLayer = g_MW.Layers.Add(fname, FileDesc)
            If NewLayer.LayerType = MapWindow.Interfaces.eLayerType.Grid Then
                Dim g As MapWinGIS.Grid = NewLayer.GetGridObject
                NewLayer.ColoringScheme = CS
                CS.UsePredefined(g.Minimum, g.Maximum, MapWinGIS.PredefinedColorScheme.SummerMountains)
                g_MW.Layers.RebuildGridLayer(NewLayer.Handle, NewLayer.GetGridObject, CS)
            Else
                If g_MW.Layers.NumLayers >= 2 Then
                    NewLayer.OutlineColor = System.Drawing.Color.Blue
                Else
                    NewLayer.OutlineColor = System.Drawing.Color.Red
                End If
                NewLayer.LineOrPointSize = 2
                NewLayer.DrawFill = True
            End If
        End If
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

    Private Sub cmbxSrc1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxSrc1.SelectedIndexChanged
        If cmbxSrc1.Items.Count > 0 Then
            PopulateOutFile()
            If (g_LastSrc1 <> "") Then
                If (g_LastSrc1 = cmbxSrc1.SelectedItem().ToString()) Then
                    'no need to change anything
                Else
                    g_SelSrc1Count = 0
                    g_Src1Array.Clear()
                    stsBar.Text = "0 shapes selected. Click 'Select Shape 1' to select."
                    btnSelectShape1.Enabled = True
                    g_LastSrc1 = cmbxSrc1.SelectedItem().ToString()
                End If
            End If
        Else
            btnSelectShape1.Enabled = False
        End If

        If (cmbxSrc2.Items.Count > 0) Then
            g_LastSrc2 = cmbxSrc2.SelectedItem().ToString()
        End If
        PopulateSrc2List()

        setDefaultColoring()
    End Sub

    Private Sub PopulateOutFile()
        Dim outFile As String
        outFile = ""
        If cmbxSrc1.Items.Count > 0 Then
            'set the output path
            outFile = AppendExt(getPathByName(cmbxSrc1.Items(cmbxSrc1.SelectedIndex)), "_merge")
            Dim i As Integer
            i = 0
            Dim mergeExt As String
            While System.IO.File.Exists(outFile)
                mergeExt = "_merge" & i
                outFile = AppendExt(getPathByName(cmbxSrc1.Items(cmbxSrc1.SelectedIndex)), mergeExt)
                i = i + 1
            End While
            txtbxOutFile.Text = outFile
        Else
            txtbxOutFile.Text = ""
        End If
        setDefaultColoring()
    End Sub

    Private Sub cmbxSrc2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxSrc2.SelectedIndexChanged
        If cmbxSrc2.Items.Count > 0 Then
            If (g_LastSrc2 <> "") Then
                If (g_LastSrc2 = cmbxSrc2.SelectedItem().ToString()) Then
                    'no need to change selection
                Else
                    g_SelSrc2Count = 0
                    g_Src2Array.Clear()
                    stsBar.Text = "0 shapes selected. Click 'Select Shape 2' to select."
                    btnSelectShape2.Enabled = True
                    g_LastSrc2 = cmbxSrc2.SelectedItem().ToString()
                End If
            End If
        Else
            btnSelectShape2.Enabled = False
        End If
        setDefaultColoring()
    End Sub
    '4/6/05 - Angela Hillier - changed to check if file is valid before allowing user to continue
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
                    btnOk.Enabled = False
                    enableOK = False
                Else
                    'Warn user that this file already exists and will be overwritten
                    System.Windows.Forms.MessageBox.Show(txtbxOutFile.Text & " already exists." & Environment.NewLine & "Please choose another path if you don't want this file to be overwritten.", "File Already Exists", System.Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Warning)
                    enableOK = True
                End If
            End If
        End If
        setDefaultColoring()
    End Sub
    'have the user select a source shapefile
    Private Sub btnBrowseSrc1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseSrc1.Click
        Dim strPath As String
        Dim sf As New MapWinGIS.Shapefile

        Dim fdiagOpen As System.Windows.Forms.OpenFileDialog = New System.Windows.Forms.OpenFileDialog
        fdiagOpen.Filter = sf.CdlgFilter
        fdiagOpen.FilterIndex = 1

        If fdiagOpen.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            strPath = fdiagOpen.FileName
            If Not layerExists(strPath) Then
                AddMap(strPath)
                PopulateSrc1List()
                If (cmbxSrc2.Items.Count > 0) Then
                    g_LastSrc2 = cmbxSrc2.SelectedItem.ToString()
                End If
                PopulateSrc2List()
                g_LastSrc1 = getNameByPath(strPath)
                cmbxSrc1.SelectedIndex = cmbxSrc1.Items.IndexOf(g_LastSrc1)
                g_SelSrc1Count = 0
                g_Src1Array.Clear()
            Else
                mapwinutility.logger.msg("That layer already exists. Please select it from the drop down list.")
            End If
        End If
        setDefaultColoring()
    End Sub
    'have the user select a second shapefile
    Private Sub btnBrowseSrc2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseSrc2.Click
        Dim strPath As String
        Dim sf As New MapWinGIS.Shapefile
        Dim sfType As New MapWinGIS.ShpfileType
        Dim currSel As New MapWinGIS.Shapefile
        currSel.Open(getPathByName(cmbxSrc1.Items(cmbxSrc1.SelectedIndex)))
        sfType = currSel.ShapefileType
        currSel.Close()

        Dim fdiagOpen As System.Windows.Forms.OpenFileDialog = New System.Windows.Forms.OpenFileDialog
        fdiagOpen.Filter = sf.CdlgFilter
        fdiagOpen.FilterIndex = 1

        If fdiagOpen.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            strPath = fdiagOpen.FileName
            If Not layerExists(strPath) Then
                sf.Open(strPath)
                If sf.ShapefileType = sfType Then
                    AddMap(strPath)
                    If (cmbxSrc1.Items.Count > 0) Then
                        g_LastSrc1 = cmbxSrc1.SelectedItem.ToString()
                    End If
                    PopulateSrc1List()
                    PopulateSrc2List()
                    g_LastSrc2 = getNameByPath(strPath)
                    cmbxSrc2.SelectedIndex = cmbxSrc2.Items.IndexOf(g_LastSrc2)
                    g_SelSrc2Count = 0
                    g_Src2Array.Clear()
                Else
                    Dim type As String
                    If (sfType = MapWinGIS.ShpfileType.SHP_POINT Or sfType = MapWinGIS.ShpfileType.SHP_POINTM Or sfType = MapWinGIS.ShpfileType.SHP_POINTZ) Then
                        type = "POINT"
                    ElseIf (sfType = MapWinGIS.ShpfileType.SHP_POLYGON Or sfType = MapWinGIS.ShpfileType.SHP_POLYGONM Or sfType = MapWinGIS.ShpfileType.SHP_POLYGONZ) Then
                        type = "POLYGON"
                    ElseIf (sfType = MapWinGIS.ShpfileType.SHP_POLYLINE Or sfType = MapWinGIS.ShpfileType.SHP_POLYLINEM Or sfType = MapWinGIS.ShpfileType.SHP_POLYLINEZ) Then
                        type = "LINE"
                    Else
                        type = "UNKNOWN"
                    End If
                    mapwinutility.logger.msg("Please select a " & type & " shapefile. Or change source 1.")
                End If
                sf.Close()
            Else
                MapWinUtility.Logger.Msg("That layer already exists. Please select it from the drop down list.")
            End If
        End If

        setDefaultColoring()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    'get the shapes selected in source file 1
    Private Sub btnSelectShape1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectShape1.Click
        Dim i As Integer

        If g_SelectingShapes Then
            g_SelectingShapes = False
            btnSelectShape1.Text = "Select Shape 1"
            g_SelSrc1Count = g_MW.View.SelectedShapes.NumSelected
            If g_SelSrc1Count = 0 Then
                stsBar.Text = "0 shapes selected from source 1. Click the 'Select Shape 1' button to select."
            ElseIf g_SelSrc1Count = 1 Then
                stsBar.Text = g_SelSrc1Count.ToString + " shape selected from source 1."
                'fill array of polygon indices
                g_Src1Array = New ArrayList(1)
                g_Src1Array.Add(g_MW.View.SelectedShapes.Item(0).ShapeIndex)
            Else
                g_Src1Array = New ArrayList(g_SelSrc1Count)
                For i = 0 To g_SelSrc1Count - 1
                    g_Src1Array.Add(g_MW.View.SelectedShapes.Item(i).ShapeIndex)
                Next
                stsBar.Text = g_SelSrc1Count.ToString + " shapes selected from source 1."
            End If
        Else
            g_SelectingShapes = True
            stsBar.Text = "Click to Select. Hold CTRL to select multiple. Click Done once finished."
            btnSelectShape1.Text = "Done"
            g_MW.Layers.CurrentLayer = getIndexByName(cmbxSrc1.Items(cmbxSrc1.SelectedIndex))
            g_MW.View.CursorMode = MapWinGIS.tkCursorMode.cmSelection
        End If
        setDefaultColoring()
    End Sub
    'get the shapes selected in source file 2
    Private Sub btnSelectShape2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectShape2.Click
        Dim i As Integer
        If g_SelectingShapes Then
            g_SelectingShapes = False
            btnSelectShape2.Text = "Select Shape 2"
            g_SelSrc2Count = g_MW.View.SelectedShapes.NumSelected
            If g_SelSrc2Count = 0 Then
                stsBar.Text = "0 shapes selected from source 2. Click the 'Select Shape 2' button to select."
            ElseIf g_SelSrc2Count = 1 Then
                g_Src2Array = New ArrayList(1)
                g_Src2Array.Add(g_MW.View.SelectedShapes.Item(0).ShapeIndex)
                stsBar.Text = g_SelSrc2Count.ToString + " shape selected from source 2."
            Else
                g_Src2Array = New ArrayList(g_SelSrc2Count)
                For i = 0 To g_SelSrc2Count - 1
                    g_Src2Array.Add(g_MW.View.SelectedShapes.Item(i).ShapeIndex)
                Next
                stsBar.Text = g_SelSrc2Count.ToString + " shapes selected from source 2."
            End If
        Else
            g_SelectingShapes = True
            stsBar.Text = "Click to Select. Hold CTRL to select multiple. Click Done once finished."
            btnSelectShape2.Text = "Done"
            g_MW.Layers.CurrentLayer = getIndexByName(cmbxSrc2.Items(cmbxSrc2.SelectedIndex))
            g_MW.View.CursorMode = MapWinGIS.tkCursorMode.cmSelection
        End If
        setDefaultColoring()
    End Sub

    '4/6/06 - Angela Hillier - added for browsing to output file, will check if file is already in use before allowing user to continue
    Private Sub btnBrowseToOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseToOut.Click
        Dim enableOK As Boolean
        enableOK = True
        Dim strPath As String
        Dim sf As New MapWinGIS.Shapefile
        Dim fdiagSave As New System.Windows.Forms.SaveFileDialog
        fdiagSave.Filter = sf.CdlgFilter
        fdiagSave.FilterIndex = 1
        If fdiagSave.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            strPath = fdiagSave.FileName
            If Not layerExists(strPath) Then
                txtbxOutFile.Text = strPath
            Else
                MapWinUtility.Logger.Msg("This layer already exists. Please remove it or select a different file to save to.")
                enableOK = False
                btnOk.Enabled = False
            End If
        End If
        setDefaultColoring()
    End Sub

    Private Sub MergeShapes()
        '3 cases: multiple selections from source 1 only, 1 selection from source 1 and 1 from source 2, multiple form 1 and multiple from 2
        'idea: merge all shapes within the same source first, then combine with result from other source
        Dim source1, source2, resultPath As String
        Dim numSrc1, numSrc2, shpIndex, i As Integer
        Dim sf1 As New MapWinGIS.Shapefile
        Dim resultSF As New MapWinGIS.Shapefile
        Dim sfType As New MapWinGIS.ShpfileType
        Dim success As Boolean
        success = True
        source1 = getPathByName(cmbxSrc1.Items(cmbxSrc1.SelectedIndex))
        source2 = getPathByName(cmbxSrc2.Items(cmbxSrc2.SelectedIndex))
        resultPath = txtbxOutFile.Text
        MapWinGeoProc.DataManagement.DeleteShapefile(resultPath)
        numSrc1 = g_Src1Array.Count()
        numSrc2 = g_Src2Array.Count()
        shpIndex = 0
        i = 0
        sf1.Open(source1)
        sfType = sf1.ShapefileType
        resultSF.CreateNew(resultPath, sfType)
        resultSF.StartEditingShapes()

        If numSrc1 = 1 And numSrc2 = 1 Then
            'merge the two shapes together into a result shape
            Dim shp1 As New MapWinGIS.Shape
            Dim shp2 As New MapWinGIS.Shape
            Dim result As New MapWinGIS.Shape
            Dim sf2 As New MapWinGIS.Shapefile
            sf1.Open(source1)
            sf2.Open(source2)
            shp1.Create(sfType)
            shp2.Create(sfType)
            shp1 = sf1.Shape(g_Src1Array(0))
            shp2 = sf2.Shape(g_Src2Array(0))
            result.Create(sfType)

            success = MapWinGeoProc.SpatialOperations.MergeShapes(shp1, shp2, result)
            If (success = True) And (result.numPoints > 0) Then
                resultSF.EditInsertShape(result, shpIndex)
            Else
                MapWinGeoProc.Error.SetErrorMsg("No result found. Possible error: " & MapWinGeoProc.Error.GetLastErrorMsg)
            End If

            sf2.Close()

        ElseIf numSrc1 > 0 Then
            If (sfType = MapWinGIS.ShpfileType.SHP_POLYGON Or sfType = MapWinGIS.ShpfileType.SHP_POLYGONM Or sfType = MapWinGIS.ShpfileType.SHP_POLYGONZ) Then
                'merge all shapes from source 1 together
                Dim resultSrc1 As New MapWinGIS.Shape
                resultSrc1.Create(sfType)
                If (numSrc1 = 1) Then
                    resultSrc1 = sf1.Shape(g_Src1Array.Item(i))
                Else
                    Dim prevShp As New MapWinGIS.Shape
                    prevShp.Create(sfType)
                    For i = 0 To numSrc1 - 1
                        Dim currShp As New MapWinGIS.Shape
                        currShp.Create(sfType)
                        currShp = sf1.Shape(g_Src1Array.Item(i))
                        If i = 0 Then
                            prevShp = currShp
                        ElseIf i = numSrc1 - 1 Then
                            success = MapWinGeoProc.SpatialOperations.MergeShapes(prevShp, currShp, resultSrc1)
                        Else
                            Dim tempShp As New MapWinGIS.Shape
                            tempShp.Create(sfType)
                            success = MapWinGeoProc.SpatialOperations.MergeShapes(prevShp, currShp, tempShp)
                            prevShp = tempShp
                        End If

                        If (success = False) Then
                            i = numSrc1
                        End If
                    Next
                End If 'numSrc = 1

                If (numSrc2 > 0) Then
                    'merge all shapes from source 2 together
                    Dim sf2 As New MapWinGIS.Shapefile
                    sf2.Open(source2)
                    Dim prevResult As New MapWinGIS.Shape
                    prevResult.Create(sfType)
                    Dim resultSrc2 As New MapWinGIS.Shape
                    resultSrc2.Create(sfType)
                    If numSrc2 = 1 Then
                        resultSrc2 = sf2.Shape(g_Src2Array.Item(0))
                    Else
                        For i = 0 To numSrc2 - 1
                            Dim currShape As New MapWinGIS.Shape
                            currShape.Create(sfType)
                            currShape = sf2.Shape(g_Src2Array.Item(i))
                            If i = 0 Then
                                prevResult = currShape
                            ElseIf i = numSrc2 - 1 Then
                                success = MapWinGeoProc.SpatialOperations.MergeShapes(prevResult, currShape, resultSrc2)
                            Else
                                Dim tempResult As New MapWinGIS.Shape
                                tempResult.Create(sfType)
                                success = MapWinGeoProc.SpatialOperations.MergeShapes(prevResult, currShape, tempResult)
                                prevResult = tempResult
                            End If

                            If success = False Then
                                i = numSrc2
                            End If
                        Next
                        sf2.Close()
                    End If 'numSrc2 > 0
                    'merge the two result shapes
                    If (resultSrc1.numPoints > 0) Then
                        If (resultSrc2.numPoints > 0) Then
                            Dim resultShp As New MapWinGIS.Shape
                            resultShp.Create(sfType)
                            success = MapWinGeoProc.SpatialOperations.MergeShapes(resultSrc1, resultSrc2, resultShp)
                            If success And (resultShp.numPoints > 0) Then
                                resultSF.EditInsertShape(resultShp, shpIndex)
                            End If
                        Else
                            resultSF.EditInsertShape(resultSrc1, shpIndex)
                        End If
                    End If 'valid result
                Else
                    'just insert resultSrc1 into resultSF
                    If (resultSrc1.numPoints > 0) Then
                        resultSF.EditInsertShape(resultSrc1, shpIndex)
                    End If
                End If 'numSrc2 > 0

            ElseIf (sfType = MapWinGIS.ShpfileType.SHP_POLYLINE Or sfType = MapWinGIS.ShpfileType.SHP_POLYLINEM Or sfType = MapWinGIS.ShpfileType.SHP_POLYLINEZ Or _
                sfType = MapWinGIS.ShpfileType.SHP_POINT Or sfType = MapWinGIS.ShpfileType.SHP_POINTM Or sfType = MapWinGIS.ShpfileType.SHP_POINTZ) Then
                'try merging shapes together. Because lines are more tricky (must line up at end points), we must loop through
                'all possible combinations before giving up on finding a successful merge.
                Dim tempPath As String = System.IO.Path.GetTempPath & "tempMergeSF.shp"
                Dim tempSF As New MapWinGIS.Shapefile
                MapWinGeoProc.DataManagement.DeleteShapefile(tempPath)
                tempSF.CreateNew(tempPath, sfType)
                tempSF.StartEditingShapes()
                For i = 0 To numSrc1 - 1
                    Dim shp As New MapWinGIS.Shape
                    shp.Create(sfType)
                    shp = sf1.Shape(g_Src1Array.Item(i))
                    tempSF.EditInsertShape(shp, shpIndex)
                    shpIndex += 1
                Next
                If (numSrc2 > 0) Then
                    Dim sf2 As New MapWinGIS.Shapefile
                    sf2.Open(source2)
                    For i = 0 To numSrc2 - 1
                        Dim shp As New MapWinGIS.Shape
                        shp.Create(sfType)
                        shp = sf2.Shape(g_Src2Array.Item(i))
                        tempSF.EditInsertShape(shp, shpIndex)
                        shpIndex += 1
                    Next
                    sf2.Close()
                End If

                Dim shp1 As New MapWinGIS.Shape
                shp1.Create(sfType)
                shp1 = tempSF.Shape(0)
                resultSF.EditInsertShape(shp1, 0)
                tempSF.EditDeleteShape(0)

                For i = 1 To tempSF.NumShapes - 1
                    Dim mergeShp As New MapWinGIS.Shape
                    mergeShp.Create(sfType)
                    mergeShp = resultSF.Shape(0)
                    Dim tempShp As New MapWinGIS.Shape
                    tempShp.Create(sfType)
                    tempShp = tempSF.Shape(i)
                    Dim resultShp As New MapWinGIS.Shape
                    resultShp.Create(sfType)
                    success = MapWinGeoProc.SpatialOperations.MergeShapes(mergeShp, tempShp, resultShp)
                    If (resultShp.numPoints > 0) Then
                        success = True
                        resultSF.EditClear()
                        resultSF.EditInsertShape(resultShp, 0)
                        tempSF.EditDeleteShape(i)
                        If (tempSF.NumShapes = 0) Then
                            ' i = 2 'exit the loop
                            Exit For
                        Else
                            i = -1 'restart the loop
                        End If
                    End If
                Next

            Else
                Dim errorMsg As String = "Error: Unsupported shapefile type. Merging can only occur on lines, points, or polygons."
                System.Windows.Forms.MessageBox.Show(errorMsg, "Merging Error")
                Debug.WriteLine("ErrorMsg: " + errorMsg)
                sf1.Close()
                Me.Close()
            End If
        End If

        If (success = True) Then
            If (resultSF.NumShapes > 0) Then
                Dim ID As New MapWinGIS.Field
                ID.Name = "ID"
                ID.Type = MapWinGIS.FieldType.INTEGER_FIELD
                Dim fieldIndex As Integer
                fieldIndex = 0
                resultSF.EditInsertField(ID, fieldIndex)
                'add id values to .dbf table
                resultSF.EditCellValue(0, 0, 0)

                resultSF.StopEditingShapes()
                resultSF.SaveAs(resultPath)
                resultSF.Close()

                'add result to map
                If (chkbxAddResults.Checked) Then
                    RemoveLayer(resultPath)
                    AddMap(resultPath)
                End If
                sf1.Close()
                Me.Close()
            Else
                System.Windows.Forms.MessageBox.Show("No valid shapes created.")
                sf1.Close()
                Me.Close()
            End If
        Else
            Dim errorMsg As String = MapWinGeoProc.Error.GetLastErrorMsg()
            System.Windows.Forms.MessageBox.Show(errorMsg, "Merging Error")
            Debug.WriteLine("ErrorMsg: " + errorMsg)
            sf1.Close()
            Me.Close()
        End If

    End Sub
#End Region
End Class
