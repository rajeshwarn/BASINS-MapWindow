'********************************************************************************************************
'File Name: mwTableEditorClass.vb
'Description:  This class implements the MapWindow interface to make this a plugin
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
'Oct 03, 2008: Earljon Hidalgo earljon@gmail.com -
'               - Updated the plugin icon

Imports System.IO
Public Class mwTableEditorClass
    Implements MapWindow.Interfaces.IPlugin


    Private Const c_ButtonName As String = "TableEditorButton"
    Private Const c_BarName As String = "TableEditorBar"

    Private m_Button As MapWindow.Interfaces.ToolbarButton
    Private m_MapWindowForm As Windows.Forms.Form


    Public ReadOnly Property Author() As String Implements MapWindow.Interfaces.IPlugin.Author
        Get
            Return "EMRC - Utah Water Research Lab"
        End Get
    End Property

    Public ReadOnly Property BuildDate() As String Implements MapWindow.Interfaces.IPlugin.BuildDate
        Get
            Dim myDate As String
            Try
                myDate = FileDateTime(Reflection.Assembly.GetExecutingAssembly().Location)
            Catch
                myDate = "4/21/2003"
            End Try
            Return myDate
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements MapWindow.Interfaces.IPlugin.Description
        Get
            Return "The MapWindow Attribute Table Editor is a quick and easy way to view and modify shapefile table data."
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer) Implements MapWindow.Interfaces.IPlugin.Initialize
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager("mwTableEditor.Resource", System.Reflection.Assembly.GetExecutingAssembly())

        mPublics.g_MW = MapWin
        m_MapWindowForm = System.Windows.Forms.Control.FromHandle(New IntPtr(ParentHandle))

        'Dim t As MapWindow.Interfaces.Toolbar = MapWin.Toolbar
        't.AddToolbar("tlbMain")

        'Dim ico As New System.Drawing.Icon(Me.GetType, "ToolbarIconNew.ico")
        'Dim png As New System.Drawing.Bitmap(Me.GetType, "tableNew.png")
        'm_Button = MapWin.Toolbar.AddButton(c_ButtonName, ico)
        m_Button = MapWin.Toolbar.AddButton(c_ButtonName, "tlbLayers", String.Empty, String.Empty)
        m_Button.BeginsGroup = True
        m_Button.Text = "Table"
        'm_Button.Picture = New System.Drawing.Bitmap(Me.GetType, "tableNew.png")
        m_Button.Picture = New System.Drawing.Bitmap(My.Resources.table_editor, New System.Drawing.Size(24, 24))

        m_Button.Tooltip = resources.GetString("TableEditorToolbarBtn.Tooltip") '"Attribute Table Editor"
        If m_Button Is Nothing Then
            MapWinUtility.Logger.Msg("Error initializing the Attribute Table Editor Plug-in!", MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, "Error")
        End If

        If Not g_MW Is Nothing Then
            If g_MW.Layers.NumLayers > 0 AndAlso LayerIsShapefile(g_MW.Layers.CurrentLayer) Then
                m_Button.Enabled = True
            Else
                m_Button.Enabled = False
            End If
        End If

        AddHandler MapWin.LayerSelectionChanged, AddressOf LayerSelectionChanged
    End Sub

    ''' <summary>
    ''' Updates selection in the table
    ''' </summary>
    Public Sub LayerSelectionChanged(ByVal layerHandle As Integer, ByRef handled As Boolean)

        If g_IgnoreEvents Then Exit Sub ' the call was initiated from table editor

        If Not mPublics.TableEditor Is Nothing Then
            If mPublics.TableEditor.LayerHandle = layerHandle Then
                g_MW.View.MapCursor = MapWinGIS.tkCursor.crsrWait
                mPublics.TableEditor.SynchSelected()
                g_MW.View.MapCursor = MapWinGIS.tkCursor.crsrMapDefault
            End If
        End If
    End Sub

    Public Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.ItemClicked
        If ItemName = c_ButtonName AndAlso LayerIsShapefile(g_MW.Layers.CurrentLayer) Then
            Handled = True

            'mPublics.TableEditor = New frmTableEditor(CType(g_MW.Layers(g_MW.Layers.CurrentLayer).GetObject, MapWinGIS.Shapefile), m_MapWindowForm)
            mPublics.TableEditor = New frmTableEditor(g_MW.Layers.CurrentLayer, m_MapWindowForm)
            With mPublics.TableEditor
                .Show()
            End With
        End If
    End Sub

    Public Sub LayerRemoved(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerRemoved
        If g_MW.Layers.NumLayers > 0 AndAlso LayerIsShapefile(g_MW.Layers.CurrentLayer) Then
            m_Button.Enabled = True
        Else
            m_Button.Enabled = False
        End If
    End Sub

    <CLSCompliant(False)> _
    Public Sub LayersAdded(ByVal Layers() As MapWindow.Interfaces.Layer) Implements MapWindow.Interfaces.IPlugin.LayersAdded
        If Layers.Length > 0 Then
            LoadTableData(Layers(0).Handle)
        End If
    End Sub

    Public Sub LayersCleared() Implements MapWindow.Interfaces.IPlugin.LayersCleared
        m_Button.Enabled = False
    End Sub

    Public Sub LayerSelected(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerSelected
        If mPublics.TableEditor IsNot Nothing AndAlso Not mPublics.TableEditor.IsDisposed AndAlso mPublics.TableEditor.btnApply.Enabled Then
            'Check for changes to save -- bugzilla 775

            'Since the sender (the first param) is not the apply button itself,
            'the apply button will ask if changes should be saved.
            mPublics.TableEditor.btnApply_Click(Nothing, Nothing)
        End If
        LoadTableData(Handle)
    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendDoubleClick(ByVal Handle As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendDoubleClick

    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendMouseUp(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseUp

    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendMouseDown(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseDown

    End Sub

    Public Sub MapDragFinished(ByVal Bounds As System.Drawing.Rectangle, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapDragFinished

    End Sub

    Public Sub Message(ByVal msg As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.Message

        Select Case msg.ToUpper
            Case "TABLEEDITORSTART"
                Handled = True
                If LayerIsShapefile(g_MW.Layers.CurrentLayer) Then
                    'mPublics.TableEditor = New frmTableEditor(CType(g_MW.Layers(g_MW.Layers.CurrentLayer).GetObject, MapWinGIS.Shapefile), m_MapWindowForm)
                    mPublics.TableEditor = New frmTableEditor(g_MW.Layers.CurrentLayer, m_MapWindowForm)
                    With mPublics.TableEditor
                        .Show()
                    End With
                Else
                    MapWinUtility.Logger.Msg("This layer is not a shapefile, so it has no attribute table.", MsgBoxStyle.Information, "No Attribute Table")
                End If
            Case "TABLESTRUCTURECHANGED"
                ' Paul Meems - 26 Oct 2011, Implmented the handling of this message:
                If Not mPublics.TableEditor Is Nothing Then
                    mPublics.TableEditor.RefreshWithSavePrompt()
                End If
        End Select

    End Sub

    Public Sub MapExtentsChanged() Implements MapWindow.Interfaces.IPlugin.MapExtentsChanged

    End Sub

    Public Sub MapMouseDown(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseDown

    End Sub

    Public Sub MapMouseMove(ByVal ScreenX As Integer, ByVal ScreenY As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseMove

    End Sub

    Public Sub MapMouseUp(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseUp

    End Sub

    Public ReadOnly Property Name() As String Implements MapWindow.Interfaces.IPlugin.Name
        Get
            Return "Attribute Table Editor"
        End Get
    End Property

    Public Sub ProjectLoading(ByVal ProjectFile As String, ByVal SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectLoading

    End Sub

    Public Sub ProjectSaving(ByVal ProjectFile As String, ByRef SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectSaving

    End Sub

    Public ReadOnly Property SerialNumber() As String Implements MapWindow.Interfaces.IPlugin.SerialNumber
        Get
            Return "OCGHOFNASNKGD1C"
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Sub ShapesSelected(ByVal Handle As Integer, ByVal SelectInfo As MapWindow.Interfaces.SelectInfo) Implements MapWindow.Interfaces.IPlugin.ShapesSelected
        If SelectInfo Is Nothing Then Exit Sub
        If mPublics.TableEditor Is Nothing Then Exit Sub

        g_MW.View.MapCursor = MapWinGIS.tkCursor.crsrWait
        mPublics.TableEditor.SynchSelected()
        g_MW.View.MapCursor = MapWinGIS.tkCursor.crsrMapDefault
    End Sub

    Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
        g_MW.Toolbar.RemoveButton(c_ButtonName)
    End Sub

    Public ReadOnly Property Version() As String Implements MapWindow.Interfaces.IPlugin.Version
        Get
            With System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location)
                Return .FileMajorPart & "." & .FileMinorPart & "." & .FileBuildPart
            End With
        End Get
    End Property

    Private Function LayerIsShapefile(ByVal Handle As Integer) As Boolean
        If g_MW Is Nothing Then Return False
        If Handle < 0 Then Return False
        If Not g_MW.Layers.IsValidHandle(Handle) Then Return False
        Select Case g_MW.Layers(Handle).LayerType
            Case MapWindow.Interfaces.eLayerType.LineShapefile, MapWindow.Interfaces.eLayerType.PointShapefile, MapWindow.Interfaces.eLayerType.PolygonShapefile
                Return True
            Case Else
                Return False
        End Select
    End Function

    Private Sub LoadTableData(ByVal Handle As Integer)
        If g_MW Is Nothing Then
            MsgBox("The LoadTableData function can only be used from MapWindow or if g_MW has been set (directly or with Initialize()).", MsgBoxStyle.Exclamation, "Unavailable Functionality")
            Return
        End If

        If LayerIsShapefile(Handle) Then
            m_Button.Enabled = True
            If Not mPublics.TableEditor Is Nothing Then
                ' the table editor is already open, reload it
                mPublics.TableEditor.Initialize(g_MW.Layers.CurrentLayer, m_MapWindowForm)
            End If
        Else
            If Not mPublics.TableEditor Is Nothing Then
                mPublics.TableEditor.Hide()
                mPublics.TableEditor.Close()
            End If
            m_Button.Enabled = False
        End If
    End Sub
End Class
