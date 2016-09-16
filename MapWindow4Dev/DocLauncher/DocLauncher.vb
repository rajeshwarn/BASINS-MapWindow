Public Class DocLauncher
    Implements MapWindow.Interfaces.IPlugin

    'Internationalization added 10/26/2005 Lailin Chen
    Private resMan As New Resources.ResourceManager("DocLaunCher.Resource", System.Reflection.Assembly.GetExecutingAssembly())

    'Toolbar Menu added 04/01/2008 Earljon Hidalgo
    Private Const _toolbarName As String = "DocLauncher"
    Private Const _btnName As String = "LaunchDocument"
    Private Const _toolTip As String = "Enable/Disable Open external document"
    Private _bOpenDoc As Boolean

    Public ReadOnly Property Name() As String Implements MapWindow.Interfaces.IPlugin.Name
        'This is one of the more important plug-in properties because if it is not set to something then
        'your plug-in will not load at all. This is the name that appears in the Plug-ins menu to identify
        'this plug-in.
        Get
            Return resMan.GetString("Docluncher.Name") 'Internationalization added 10/26/2005 Lailin Chen
        End Get
    End Property

    Public ReadOnly Property Author() As String Implements MapWindow.Interfaces.IPlugin.Author
        'This is the author of the plug-in.  It can be a company name, individual, or organization name.
        'The name you place here will appear in the plug-in information box. 
        Get
            Return "Daniel P. Ames"
        End Get
    End Property

    Public ReadOnly Property SerialNumber() As String Implements MapWindow.Interfaces.IPlugin.SerialNumber
        'This is a deprecated property that is not used anymore. 
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements MapWindow.Interfaces.IPlugin.Description
        'This is a description of the plug-in.  It appears in the plug-ins dialog box when a user selects
        'your plug-in.  
        Get
            Return resMan.GetString("Description.Text") 'Internationalization added 10/26/2005 Lailin Chen
        End Get
    End Property

    Public ReadOnly Property BuildDate() As String Implements MapWindow.Interfaces.IPlugin.BuildDate
        'This is the Build Date for the plug-in.  You can either return a string of a hard-coded date
        'such as "January 1, 2003" or you can use the .NET function below to dynamically obtain the build
        'date of the assembly.
        Get
            Return System.IO.File.GetLastWriteTime(Me.GetType().Assembly.Location)
        End Get
    End Property

    Public ReadOnly Property Version() As String Implements MapWindow.Interfaces.IPlugin.Version
        'This is the version number of the plug-in.  You can either return a hard-coded string
        'such as "1.0.0.1" or you can use the .NET function shown below to dynamically return 
        'the version number from the assembly itself.
        Get
            Return System.Diagnostics.FileVersionInfo.GetVersionInfo(Me.GetType().Assembly.Location).FileVersion
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer) Implements MapWindow.Interfaces.IPlugin.Initialize
        'This event is fired when the user loads your plug-in either through the plug-in dialog 
        'box, or by checkmarking it in the plug-ins menu.  This is where you would add buttons to the
        'tool bar or menu items to the menu.  
        '
        'It is also standard to set a global reference to the IMapWin that is passed through here so that
        'you can access it elsewhere in your project to act on MapWindow.
        g_MapWin = MapWin

        ' Create a new toolbar and a new button with an icon attached on it
        ' Added 04/01/2008 by Earljon Hidalgo
        Try
            Dim tbr As MapWindow.Interfaces.Toolbar = MapWin.Toolbar
            tbr.AddToolbar(_toolbarName)

            Dim btn As MapWindow.Interfaces.ToolbarButton = tbr.AddButton(_btnName, _toolbarName, "", "")

            btn.BeginsGroup = True
            ' if we like icon + text to appear, uncomment this
            'btn.Text = "OpenDoc"
            btn.Tooltip = _toolTip
            btn.Category = _toolbarName
            btn.Picture = New System.Drawing.Icon(My.Resources.Resource.AppIcon, New System.Drawing.Size(16, 16))

            ' initialized to not selected
            _bOpenDoc = False
            tbr = Nothing
            btn = Nothing
        Catch ex As Exception
            g_MapWin.ShowErrorDialog(ex)
        End Try
    End Sub

    Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
        'This event is fired when the user unloads your plug-in either through the plug-in dialog 
        'box, or by un-checkmarking it in the plug-ins menu.  This is where you would remove any
        'buttons from the tool bar tool bar or menu items from the menu that you may have added.
        'If you don't do this, then you will leave dangling menus and buttons that don't do anything.

        ' Remove button and the toolbar if plugin is unloaded
        ' Added 04/01/2008 by Earljon Hidalgo
        g_MapWin.Toolbar.RemoveButton(_btnName)
        g_MapWin.Toolbar.RemoveToolbar(_toolbarName)
    End Sub

    Public Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.ItemClicked
        'This event fires when a menu item or toolbar button is clicked.  So if you added a button or menu
        'on the Initialize event, then this is where you would handle it.

        ' Check and save the state of the button
        ' Added 04/01/2008 by Earljon Hidalgo
        Dim bToggle As Boolean = IIf(Not _bOpenDoc, True, False)
        Try
            If ItemName = _btnName Then
                g_MapWin.Toolbar.ButtonItem(_btnName).Pressed = bToggle
                _bOpenDoc = bToggle
                Handled = True
            End If
        Catch ex As System.Exception
            g_MapWin.ShowErrorDialog(ex)
        End Try
    End Sub

    Public Sub LayerRemoved(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerRemoved
        'This event fires when the user removes a layer from MapWindow.  This is useful to know if your
        'plug-in depends on a particular layer being present. 
    End Sub

    <CLSCompliant(False)> _
    Public Sub LayersAdded(ByVal Layers() As MapWindow.Interfaces.Layer) Implements MapWindow.Interfaces.IPlugin.LayersAdded
        'This event fires when the user adds a layer to MapWindow.  This is useful to know if your
        'plug-in depends on a particular layer being present. Also, if you keep an internal list of 
        'available layers, for example you may be keeping a list of all "point" shapefiles, then you
        'would use this event to know when layers have been added or removed.
    End Sub

    Public Sub LayersCleared() Implements MapWindow.Interfaces.IPlugin.LayersCleared
        'This event fires when the user clears all of the layers from MapWindow.  As with LayersAdded 
        'and LayersRemoved, this is useful to know if your plug-in depends on a particular layer being 
        'present or if you are maintaining your own list of layers.
    End Sub

    Public Sub LayerSelected(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerSelected
        'This event fires when a user selects a layer in the legend. 
    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendDoubleClick(ByVal Handle As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendDoubleClick
        'This event fires when a user double-clicks a layer in the legend.
    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendMouseDown(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseDown
        'This event fires when a user holds a mouse button down in the legend.
    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendMouseUp(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseUp
        'This event fires when a user releases a mouse button in the legend.
    End Sub

    Public Sub MapDragFinished(ByVal Bounds As System.Drawing.Rectangle, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapDragFinished
        'If a user drags (ie draws a box) with the mouse on the map, this event fires at completion of the drag
        'and returns a system.drawing.rectangle that has the bounds of the box that was "drawn"
    End Sub

    Public Sub MapExtentsChanged() Implements MapWindow.Interfaces.IPlugin.MapExtentsChanged
        'This event fires any time there is a zoom or pan that changes the extents of the map.
    End Sub

    Public Sub MapMouseDown(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseDown
        'This event fires when the user holds a mouse button down on the map. Note that x and y are returned
        'as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates
        'then you need to use g_MapWin.View.PixelToProj()
    End Sub

    Public Sub MapMouseMove(ByVal ScreenX As Integer, ByVal ScreenY As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseMove
        'This event fires when the user moves the mouse over the map. Note that x and y are returned
        'as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates
        'then you need to use g_MapWin.View.PixelToProj()
    End Sub

    Public Sub MapMouseUp(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseUp
        'This event fires when the user releases a mouse button down on the map. Note that x and y are returned
        'as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates
        'then you need to use g_MapWin.View.PixelToProj()
    End Sub

    Public Sub Message(ByVal msg As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.Message
        'Plug-ins can communicate with eachother using Messages.  If a message is sent then this event fires.
        'If you know the message is "for you" then you can set Handled=True and then it will not be sent to any
        'other plug-ins.
    End Sub

    Public Sub ProjectLoading(ByVal ProjectFile As String, ByVal SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectLoading
        'When the user opens a project in MapWindow, this event fires.  The ProjectFile is the file name of the
        'project that the user opened (including its path in case that is important for this this plug-in to know).
        'The SettingsString variable contains any string of data that is connected to this plug-in but is stored 
        'on a project level. For example, a plug-in that shows streamflow data might allow the user to set a 
        'separate database for each project (i.e. one database for the upper Missouri River Basin, a different 
        'one for the Lower Colorado Basin.) In this case, the plug-in would store the database name in the 
        'SettingsString of the project. 
    End Sub

    Public Sub ProjectSaving(ByVal ProjectFile As String, ByRef SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectSaving
        'When the user saves a project in MapWindow, this event fires.  The ProjectFile is the file name of the
        'project that the user is saving (including its path in case that is important for this this plug-in to know).
        'The SettingsString variable contains any string of data that is connected to this plug-in but is stored 
        'on a project level. For example, a plug-in that shows streamflow data might allow the user to set a 
        'separate database for each project (i.e. one database for the upper Missouri River Basin, a different 
        'one for the Lower Colorado Basin.) In this case, the plug-in would store the database name in the 
        'SettingsString of the project. 
    End Sub

    <CLSCompliant(False)> _
    Public Sub ShapesSelected(ByVal Handle As Integer, ByVal SelectInfo As MapWindow.Interfaces.SelectInfo) Implements MapWindow.Interfaces.IPlugin.ShapesSelected
        'This event fires when the user selects one or more shapes using the select tool in MapWindow. Handle is the 
        'Layer handle for the shapefile on which shapes were selected. SelectInfo holds information abou the 
        'shapes that were selected. 
        Dim i As Integer
        Dim sf As MapWinGIS.Shapefile
        Dim FileOrURL As String = ""
        Dim newProcess As New Process
        Try
            'make sure something was returned in SelectInfo
            If SelectInfo Is Nothing Then Exit Sub

            ' Check the state of the button if we need to open external document
            ' or ignore it
            ' Added 04/01/2008 by Earljon Hidalgo
            If Not _bOpenDoc Then Exit Sub

            'get the shapefile object on which a shape was selected
            sf = g_MapWin.Layers(Handle).GetObject
            'Check to see if this shapefile has an attribute field "FileOrURL"
            For i = 0 To sf.NumFields - 1
                If LCase(sf.Field(i).Name) = "fileorurl" Then
                    'This shapefile has the needed attribute field.
                    'We will only be launching one document, so if they selected more than one shape
                    'then we will just launch the document associated with the first selected shape.
                    'Also, i is the index of the field that has the FileOrURL that we are going to 
                    'launch.
                    FileOrURL = sf.CellValue(i, SelectInfo(0).ShapeIndex)
                    Exit For
                End If
            Next
            If FileOrURL <> "" Then
                'Use a .NET process() to launch the file or URL
                MapWinUtility.Logger.Dbg("DocLauncher: Launched File or URL: " + FileOrURL)
                newProcess.StartInfo.FileName = FileOrURL
                newProcess.Start()
            End If
        Catch ex As System.Exception
            MapWinUtility.Logger.Msg(ex.Message)
        End Try
    End Sub

End Class
