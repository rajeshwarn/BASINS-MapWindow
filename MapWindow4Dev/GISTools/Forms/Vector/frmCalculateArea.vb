'********************************************************************************************************
'File Name: frmCalculateArea.vb
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
' Change Log: 
' Date          Changed By      Notes
' 08/22/2006    JLK             Added use of Logger
' 08/29/2006    JLK             Added Logger.Progress call
' 08/29/2008    Jiri Kadlec     Use the unit conversion and area calculation functions from MapWinGeoProc
'                               Corrected area calculation for Lat/Long shapefiles with large shapes
'********************************************************************************************************

Imports MapWindow
Imports MapWinUtility

Public Class frmCalculateArea
    Inherits System.Windows.Forms.Form


#Region " Windows Form Designer generated code "

    Private m_MapWin As MapWindow.Interfaces.IMapWin
    Private ProcessSF As MapWinGIS.Shapefile

    <CLSCompliant(False)> _
    Public Sub New(ByVal IMapWin As MapWindow.Interfaces.IMapWin)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        m_MapWin = IMapWin

        cmbPolyLayers.Items.Clear()
        For i As Integer = 0 To m_MapWin.Layers.NumLayers - 1
            If m_MapWin.Layers(i).LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                cmbPolyLayers.Items.Add(m_MapWin.Layers(i).Name)
                If (m_MapWin.Layers.CurrentLayer = m_MapWin.Layers(i).Handle) Then
                    cmbPolyLayers.SelectedIndex = cmbPolyLayers.Items.Count - 1
                End If
            End If
        Next

        If Not cmbPolyLayers.Items.Count = 0 And _
               cmbPolyLayers.SelectedIndex = -1 Then
            cmbPolyLayers.SelectedIndex = 0
        End If
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
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbPolyLayers As System.Windows.Forms.ComboBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rdUseLoaded As System.Windows.Forms.RadioButton
    Friend WithEvents rdUseExternal As System.Windows.Forms.RadioButton
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rdUseExistingField As System.Windows.Forms.RadioButton
    Friend WithEvents txtNewFieldName As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents txtExistingField As System.Windows.Forms.ComboBox
    Friend WithEvents rdNewField As System.Windows.Forms.RadioButton
    Friend WithEvents cmbMapUnit As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbAreaUnit As System.Windows.Forms.ComboBox
    Friend WithEvents lblFilen As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCalculateArea))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbPolyLayers = New System.Windows.Forms.ComboBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Label3 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lblFilen = New System.Windows.Forms.Label
        Me.rdUseExternal = New System.Windows.Forms.RadioButton
        Me.rdUseLoaded = New System.Windows.Forms.RadioButton
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.txtNewFieldName = New System.Windows.Forms.TextBox
        Me.rdNewField = New System.Windows.Forms.RadioButton
        Me.rdUseExistingField = New System.Windows.Forms.RadioButton
        Me.txtExistingField = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.cmbAreaUnit = New System.Windows.Forms.ComboBox
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.cmbMapUnit = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
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
        'Label2
        '
        Me.Label2.AccessibleDescription = Nothing
        Me.Label2.AccessibleName = Nothing
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Font = Nothing
        Me.Label2.Name = "Label2"
        Me.ToolTip1.SetToolTip(Me.Label2, resources.GetString("Label2.ToolTip"))
        '
        'cmbPolyLayers
        '
        Me.cmbPolyLayers.AccessibleDescription = Nothing
        Me.cmbPolyLayers.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbPolyLayers, "cmbPolyLayers")
        Me.cmbPolyLayers.BackgroundImage = Nothing
        Me.cmbPolyLayers.Font = Nothing
        Me.cmbPolyLayers.Name = "cmbPolyLayers"
        Me.ToolTip1.SetToolTip(Me.cmbPolyLayers, resources.GetString("cmbPolyLayers.ToolTip"))
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
        'Panel1
        '
        Me.Panel1.AccessibleDescription = Nothing
        Me.Panel1.AccessibleName = Nothing
        resources.ApplyResources(Me.Panel1, "Panel1")
        Me.Panel1.BackgroundImage = Nothing
        Me.Panel1.Controls.Add(Me.lblFilen)
        Me.Panel1.Controls.Add(Me.rdUseExternal)
        Me.Panel1.Controls.Add(Me.rdUseLoaded)
        Me.Panel1.Controls.Add(Me.cmbPolyLayers)
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
        'Panel2
        '
        Me.Panel2.AccessibleDescription = Nothing
        Me.Panel2.AccessibleName = Nothing
        resources.ApplyResources(Me.Panel2, "Panel2")
        Me.Panel2.BackgroundImage = Nothing
        Me.Panel2.Controls.Add(Me.txtNewFieldName)
        Me.Panel2.Controls.Add(Me.rdNewField)
        Me.Panel2.Controls.Add(Me.rdUseExistingField)
        Me.Panel2.Controls.Add(Me.txtExistingField)
        Me.Panel2.Font = Nothing
        Me.Panel2.Name = "Panel2"
        Me.ToolTip1.SetToolTip(Me.Panel2, resources.GetString("Panel2.ToolTip"))
        '
        'txtNewFieldName
        '
        Me.txtNewFieldName.AccessibleDescription = Nothing
        Me.txtNewFieldName.AccessibleName = Nothing
        resources.ApplyResources(Me.txtNewFieldName, "txtNewFieldName")
        Me.txtNewFieldName.BackgroundImage = Nothing
        Me.txtNewFieldName.Font = Nothing
        Me.txtNewFieldName.Name = "txtNewFieldName"
        Me.ToolTip1.SetToolTip(Me.txtNewFieldName, resources.GetString("txtNewFieldName.ToolTip"))
        '
        'rdNewField
        '
        Me.rdNewField.AccessibleDescription = Nothing
        Me.rdNewField.AccessibleName = Nothing
        resources.ApplyResources(Me.rdNewField, "rdNewField")
        Me.rdNewField.BackgroundImage = Nothing
        Me.rdNewField.Font = Nothing
        Me.rdNewField.Name = "rdNewField"
        Me.ToolTip1.SetToolTip(Me.rdNewField, resources.GetString("rdNewField.ToolTip"))
        '
        'rdUseExistingField
        '
        Me.rdUseExistingField.AccessibleDescription = Nothing
        Me.rdUseExistingField.AccessibleName = Nothing
        resources.ApplyResources(Me.rdUseExistingField, "rdUseExistingField")
        Me.rdUseExistingField.BackgroundImage = Nothing
        Me.rdUseExistingField.Font = Nothing
        Me.rdUseExistingField.Name = "rdUseExistingField"
        Me.ToolTip1.SetToolTip(Me.rdUseExistingField, resources.GetString("rdUseExistingField.ToolTip"))
        '
        'txtExistingField
        '
        Me.txtExistingField.AccessibleDescription = Nothing
        Me.txtExistingField.AccessibleName = Nothing
        resources.ApplyResources(Me.txtExistingField, "txtExistingField")
        Me.txtExistingField.BackgroundImage = Nothing
        Me.txtExistingField.Font = Nothing
        Me.txtExistingField.Name = "txtExistingField"
        Me.ToolTip1.SetToolTip(Me.txtExistingField, resources.GetString("txtExistingField.ToolTip"))
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
        'cmbAreaUnit
        '
        Me.cmbAreaUnit.AccessibleDescription = Nothing
        Me.cmbAreaUnit.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbAreaUnit, "cmbAreaUnit")
        Me.cmbAreaUnit.BackgroundImage = Nothing
        Me.cmbAreaUnit.Font = Nothing
        Me.cmbAreaUnit.Items.AddRange(New Object() {resources.GetString("cmbAreaUnit.Items"), resources.GetString("cmbAreaUnit.Items1"), resources.GetString("cmbAreaUnit.Items2"), resources.GetString("cmbAreaUnit.Items3"), resources.GetString("cmbAreaUnit.Items4"), resources.GetString("cmbAreaUnit.Items5"), resources.GetString("cmbAreaUnit.Items6"), resources.GetString("cmbAreaUnit.Items7"), resources.GetString("cmbAreaUnit.Items8"), resources.GetString("cmbAreaUnit.Items9")})
        Me.cmbAreaUnit.Name = "cmbAreaUnit"
        Me.ToolTip1.SetToolTip(Me.cmbAreaUnit, resources.GetString("cmbAreaUnit.ToolTip"))
        '
        'Button2
        '
        Me.Button2.AccessibleDescription = Nothing
        Me.Button2.AccessibleName = Nothing
        resources.ApplyResources(Me.Button2, "Button2")
        Me.Button2.BackgroundImage = Nothing
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
        'cmbMapUnit
        '
        Me.cmbMapUnit.AccessibleDescription = Nothing
        Me.cmbMapUnit.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbMapUnit, "cmbMapUnit")
        Me.cmbMapUnit.BackgroundImage = Nothing
        Me.cmbMapUnit.Font = Nothing
        Me.cmbMapUnit.Items.AddRange(New Object() {resources.GetString("cmbMapUnit.Items"), resources.GetString("cmbMapUnit.Items1"), resources.GetString("cmbMapUnit.Items2"), resources.GetString("cmbMapUnit.Items3"), resources.GetString("cmbMapUnit.Items4"), resources.GetString("cmbMapUnit.Items5"), resources.GetString("cmbMapUnit.Items6"), resources.GetString("cmbMapUnit.Items7"), resources.GetString("cmbMapUnit.Items8")})
        Me.cmbMapUnit.Name = "cmbMapUnit"
        Me.ToolTip1.SetToolTip(Me.cmbMapUnit, resources.GetString("cmbMapUnit.ToolTip"))
        '
        'Label5
        '
        Me.Label5.AccessibleDescription = Nothing
        Me.Label5.AccessibleName = Nothing
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Font = Nothing
        Me.Label5.Name = "Label5"
        Me.ToolTip1.SetToolTip(Me.Label5, resources.GetString("Label5.ToolTip"))
        '
        'frmCalculateArea
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.CancelButton = Me.Button3
        Me.Controls.Add(Me.cmbMapUnit)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.cmbAreaUnit)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Font = Nothing
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCalculateArea"
        Me.ToolTip1.SetToolTip(Me, resources.GetString("$this.ToolTip"))
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub frmCalculateArea_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        rdUseLoaded.Checked = True

        'Chris Michaelis April 17 2006 - Changed this to the safer default
        'for bugzilla 125.
        rdNewField.Checked = True
        txtNewFieldName.Text = "Area_NEW"
    End Sub

    Private Sub rdUseExternal_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdUseExternal.CheckedChanged
        Button1.Enabled = rdUseExternal.Checked
        If ProcessSF Is Nothing Then Button1_Click(Nothing, Nothing)
    End Sub

    Private Sub rdUseLoaded_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdUseLoaded.CheckedChanged
        cmbPolyLayers.Enabled = rdUseLoaded.Checked
        lblFilen.Text = ""
    End Sub

    Private Sub rdUseExistingField_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdUseExistingField.CheckedChanged
        txtExistingField.Enabled = rdUseExistingField.Checked
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdNewField.CheckedChanged
        txtNewFieldName.Enabled = rdNewField.Checked
    End Sub

    Private Sub cmbPolyLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPolyLayers.SelectedIndexChanged
        ProcessSF = Nothing

        For i As Integer = 0 To m_MapWin.Layers.NumLayers - 1
            If m_MapWin.Layers(i).Name = cmbPolyLayers.Text Then
                ProcessSF = m_MapWin.Layers(i).GetObject()
                Exit For
            End If
        Next

        Dim SelectField As Integer = 0
        Dim MatchField As String = GetSetting("MapWindow", "Area", "LastField", "Area")

        txtExistingField.Items.Clear()
        If Not ProcessSF Is Nothing Then
            For j As Integer = 0 To ProcessSF.NumFields - 1
                If MatchField.Trim().ToLower() = ProcessSF.Field(j).Name.ToLower().Trim() Then
                    SelectField = txtExistingField.Items.Add(ProcessSF.Field(j).Name)
                Else
                    txtExistingField.Items.Add(ProcessSF.Field(j).Name)
                End If
            Next
        End If

        If Not txtExistingField.Items.Count = 0 Then txtExistingField.SelectedIndex = SelectField

        ' Paul Meems - 26 Sept 2011
        If ProcessSF.Projection.Trim() = String.Empty Then
            MsgBox("The projection couldn't be recognized. Try to fix that first.")
            Me.Close()
            Return
        Else
            'Jiri Kadlec 8/29/2008
            cmbMapUnit.Text = MapWinGeoProc.UnitConverter.GetShapefileUnits(ProcessSF.Projection).ToString()
            'cmbMapUnit.Text = GetSFUnits(ProcessSF.Projection).ToString()
        End If

        lblFilen.Text = ""
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim ofd As New Windows.Forms.OpenFileDialog
        ProcessSF = New MapWinGIS.Shapefile
        ofd.Filter = ProcessSF.CdlgFilter
        ofd.Title = "GISTools:CalculateArea:Specify Shapefile to Calculate Areas For"
        ofd.ShowDialog(g_MapWindowForm)

        If ofd.FileName = "" Then Exit Sub

        If Not ProcessSF.Open(ofd.FileName) Then Exit Sub

        If Not ProcessSF.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGON And Not _
               ProcessSF.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONM And Not _
               ProcessSF.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONZ Then
            Logger.Msg("Please select a polygon shapefile only.", _
                       MsgBoxStyle.Exclamation, _
                       "GISTools:CalculateArea:Polygon Shapefiles Only")
            ProcessSF.Close()
            Exit Sub
        End If

        txtExistingField.Items.Clear()
        For j As Integer = 0 To ProcessSF.NumFields - 1
            txtExistingField.Items.Add(ProcessSF.Field(j).Name)
        Next
        If Not txtExistingField.Items.Count = 0 Then
            txtExistingField.SelectedIndex = 0
        End If

        'Jiri Kadlec 8/29/2008
        cmbMapUnit.Text = MapWinGeoProc.UnitConverter.GetShapefileUnits(ProcessSF.Projection).ToString()
        'cmbMapUnit.Text = GetSFUnits(ProcessSF).ToString()

        lblFilen.Text = Microsoft.VisualBasic.Left(ofd.FileName, 12) & ".."
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If Not Me.rdUseExternal.Checked And Not Me.rdUseLoaded.Checked Then
            Logger.Msg("Please select a loaded polygon layer or click the folder icon to browse to a file before clicking Process.", _
                       MsgBoxStyle.Exclamation, _
                       "GISTools:CalculateArea:Choose a Shape file before processing")
            Exit Sub
        End If

        If ProcessSF Is Nothing Then
            Logger.Msg("Please select a loaded polygon layer or click the folder icon to browse to a file before clicking Process.", _
                       MsgBoxStyle.Exclamation, _
                       "GISTools:CalculateArea:Choose a Shape file before processing")
            Exit Sub
        End If

        If Not Me.rdUseExistingField.Checked And Not Me.rdNewField.Checked Then
            Logger.Msg("Please select whether to overwrite an existing field or create a new field before clicking Process.", _
                       MsgBoxStyle.Exclamation, _
                       "GISTools:CalculateArea:Choose a Shape file before processing")
            Exit Sub
        End If

        If rdUseExistingField.Checked Then
            SaveSetting("MapWindow", "Area", "LastField", txtExistingField.Items(txtExistingField.SelectedIndex).Trim())
        End If

        Dim FieldToEdit As Integer = -1

        Me.Cursor = Windows.Forms.Cursors.WaitCursor

        If (rdNewField.Checked) Then
            For i As Integer = 0 To ProcessSF.NumFields - 1
                If ProcessSF.Field(i).Name = txtNewFieldName.Text Then
                    Logger.Msg("The new field you've proposed already exists! Please choose it from the existing field list or choose a different name.", _
                      MsgBoxStyle.Exclamation, _
                      "GISTools:CalculateArea:Field Already Exists")
                    Me.Cursor = Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            Next

            'Create the field
            ProcessSF.StartEditingTable()
            Dim newFld As New MapWinGIS.Field
            newFld.Name = txtNewFieldName.Text
            newFld.Precision = 12
            newFld.Type = MapWinGIS.FieldType.DOUBLE_FIELD
            newFld.Width = 10
            FieldToEdit = ProcessSF.NumFields
            ProcessSF.EditInsertField(newFld, ProcessSF.NumFields)
            ProcessSF.StopEditingTable(True)
        Else
            For i As Integer = 0 To ProcessSF.NumFields - 1
                If ProcessSF.Field(i).Name = txtExistingField.Text Then
                    FieldToEdit = i
                    Exit For
                End If
            Next
        End If

        If FieldToEdit = -1 Then
            Logger.Msg("An error occurred trying to find the field to edit!", _
                       MsgBoxStyle.Exclamation, _
                       "GISTools:CalculateArea:Error Finding Field")
            Me.Cursor = Windows.Forms.Cursors.Default
            Exit Sub
        End If

        'OK.. do it.
        Logger.Dbg("GISTools:CalculateArea:StartProcessing File <" & ProcessSF.Filename & _
                    "> Field <" & ProcessSF.Field(FieldToEdit).Name & ">")
        ProcessSF.StartEditingTable()
        Dim iLast As Integer = ProcessSF.NumShapes - 1
        For i As Integer = 0 To ProcessSF.NumShapes - 1
            Logger.Progress("CalculateArea", i, iLast)
            ProcessSF.EditCellValue(FieldToEdit, i, CalculateArea(i))
        Next
        Logger.Progress("CalculateArea", iLast, iLast)
        ProcessSF.StopEditingTable()

        'Don't close if MW is still using it.
        'Should we close it?
        If Not rdUseLoaded.Checked Then ProcessSF.Close()
        ProcessSF = Nothing

        Logger.Msg("Done! The specified field has been filled with area values.", _
                   MsgBoxStyle.Information, "GISTools:CalculateArea:Done")
        Logger.Progress("", 0, 0)
        Me.Cursor = Windows.Forms.Cursors.Default
        Me.Close()
    End Sub

    Private Function CalculateArea(ByRef idx As Integer) As Double
        '8/29/2008 modified by Jiri Kadlec - use UnitConverters methods from MapWinGeoProc
        Dim CalculatedUnit As MapWindow.Interfaces.UnitOfMeasure = MapWinGeoProc.UnitConverter.StringToUOM(cmbMapUnit.Text)
        Dim DestUnit As MapWindow.Interfaces.UnitOfMeasure = MapWinGeoProc.UnitConverter.StringToUOM(cmbAreaUnit.Text)
        Dim calcArea As Double
        If CalculatedUnit = Interfaces.UnitOfMeasure.DecimalDegrees Then
            'Special calculation for lat/long
            calcArea = MapWinGeoProc.Utils.Area(ProcessSF.Shape(idx), Interfaces.UnitOfMeasure.DecimalDegrees)
            CalculatedUnit = Interfaces.UnitOfMeasure.Kilometers
        Else
            'Calculate normally.
            calcArea = MapWinGeoProc.Utils.Area(ProcessSF.Shape(idx))
        End If

        'Convert the units
        Return MapWinGeoProc.UnitConverter.ConvertArea(CalculatedUnit, DestUnit, calcArea)
    End Function

    
    '08/29/2008 Jiri Kadlec - use acres as default area unit when map units are feet
    Private Sub cmbMapUnit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMapUnit.SelectedIndexChanged
        If cmbMapUnit.Text.ToLower().IndexOf("feet") >= 0 Then
            cmbAreaUnit.Text = "Acres"
        End If
    End Sub
End Class

