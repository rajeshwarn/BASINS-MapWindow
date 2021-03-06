Module modGlobal

    Public Const g_AppName As String = "VataviaMap"

    'Assuming earth is a sphere
    Public Const g_RadiusOfEarth As Double = 6378137 'meters
    Public Const g_CircumferenceOfEarth As Double = 2.0 * Math.PI * g_RadiusOfEarth
    Public Const g_HalfCircumferenceOfEarth As Double = g_CircumferenceOfEarth / 2

    Public Const g_MetersPerMile As Double = 1609.344
    Public Const g_FeetPerMeter As Double = 3.2808399
    Public Const g_MilesPerKnot As Double = 1.15077945

    Public g_TileExtension As String = ".png"
    Public g_PathChar As String = IO.Path.DirectorySeparatorChar
    Public g_MarkedPrefix As String = "Marked" & g_PathChar

    Public g_IconMaxSize As Integer = 150

    Private g_LimitY As Double = ProjectF(85.0511)
    Private g_RangeY As Double = 2 * g_LimitY

    Public g_LastTileServerName As String = ""
    Public g_TileServer As New clsServer("OpenStreetMap", _
                                         "http://openstreetmap.org/", _
                                         "http://{abc}.tile.openstreetmap.org/{Zoom}/{X}/{Y}.png", _
                                         "http://www.openstreetmap.org/?lat={Lat}&lon={Lon}&zoom={Zoom}", _
                                         "� OpenStreetMap")
    Public g_TileServerExampleLabel As String = "URL of server"
    Public g_TileServerExampleFile As String = "http://wiki.openstreetmap.org/wiki/Slippy_map_tilenames#Tiles"

    Public g_TileServerTransparentName As String = "Maplint"
    Public g_TileServerTransparentURL As String = "http://tah.openstreetmap.org/Tiles/maplint/"

    'Top-level folder containing cached tiles, must end with trailing IO.Path.DirectorySeparatorChar
    Public g_TileCacheFolder As String = ""

    ' URL of server to upload points to
    Public g_UploadPointURL As String = "http://vatavia.net/cgi-bin/gps?u=unknown&y=#Lat#&x=#Lon#&e=#Alt#&s=#Speed#&h=#Heading#&t=#Time#&l=#Label#&c=#CellID#"
    ' URL of server to upload points to
    Public g_UploadTrackURL As String = "http://vatavia.net/cgi-bin/trk?u=unknown"

    Public g_DegreeFormat As EnumDegreeFormat = EnumDegreeFormat.DegreesDecimalMinutes

    Public g_WaypointIcons As New Generic.Dictionary(Of String, Drawing.Bitmap)

    Public g_Random As New Random

    Public g_TileProjection As String = "PROJCS[@Popular Visualisation CRS / Mercator@,GEOGCS[@Popular Visualisation CRS@,DATUM[@D_Popular_Visualisation_Datum@,SPHEROID[@Popular_Visualisation_Sphere@,6378137,0]],PRIMEM[@Greenwich@,0],UNIT[@Degree@,0.017453292519943295]],PROJECTION[@Mercator@],PARAMETER[@central_meridian@,0],PARAMETER[@scale_factor@,1],PARAMETER[@false_easting@,0],PARAMETER[@false_northing@,0],UNIT[@Meter@,1]]".Replace("@", """")

    Public g_Debug As Boolean = True
    Public g_DebugFilename As String = Nothing
    Public g_DebugWriter As System.IO.TextWriter = Nothing
    Private pDebugMutex As New Threading.Mutex()

    Public g_ShowStatusProgress As Boolean = False

    Public Enum EnumDegreeFormat
        DecimalDegrees = 0
        DegreesDecimalMinutes = 1
        DegreesMinutesSeconds = 2
    End Enum

    Friend Function FormattedDegrees(ByVal aDegrees As Double, ByVal aFormat As EnumDegreeFormat) As String
        Dim lStr As String = Nothing, lStrMinutes As String = Nothing, lStrSeconds As String = Nothing
        Select Case aFormat
            Case EnumDegreeFormat.DegreesDecimalMinutes
                DegreesMinutes(aDegrees, lStr, lStrMinutes)
                lStr &= " " & lStrMinutes

            Case EnumDegreeFormat.DegreesMinutesSeconds
                DegreesMinutesSeconds(aDegrees, lStr, lStrMinutes, lStrSeconds)
                lStr &= " " & lStrMinutes & "' " & lStrSeconds & """"

            Case Else
                lStr = Format(aDegrees, "0.######")
        End Select
        Return lStr
    End Function

    Public Sub DegreesMinutes(ByVal aDegrees As Double, ByRef aDegreesString As String, ByRef aMinutesString As String)
        Dim lDegreesInt As Integer
        Dim lDoubleMinutes As Double
        If aDegrees < 0 Then
            lDegreesInt = Math.Ceiling(aDegrees)
            lDoubleMinutes = (lDegreesInt - aDegrees) * 60.0
        Else
            lDegreesInt = Math.Floor(aDegrees)
            lDoubleMinutes = (aDegrees - lDegreesInt) * 60.0
        End If
        aDegreesString = CStr(lDegreesInt)
        aMinutesString = Format(lDoubleMinutes, "0.000")
    End Sub

    ''' <summary>
    ''' From given decimal degrees, populate strings with integer degrees and minutes and seconds rounded to one decimal place
    ''' </summary>
    ''' <param name="aDegrees">Decimal degrees</param>
    ''' <param name="aDegreesString">Integer degrees as string</param>
    ''' <param name="aMinutesString">Integer minutes as string</param>
    ''' <param name="aSecondsString">Seconds as string, rounded to one decimal place</param>
    Public Sub DegreesMinutesSeconds(ByVal aDegrees As Double, ByRef aDegreesString As String, ByRef aMinutesString As String, ByRef aSecondsString As String)
        Dim lDegreesInt As Integer
        Dim lDoubleMinutes As Double
        If aDegrees < 0 Then
            lDegreesInt = Math.Ceiling(aDegrees)
            lDoubleMinutes = (lDegreesInt - aDegrees) * 60.0
        Else
            lDegreesInt = Math.Floor(aDegrees)
            lDoubleMinutes = (aDegrees - lDegreesInt) * 60.0
        End If
        Dim lMinutesInt As Integer = Math.Floor(lDoubleMinutes)
        Dim lDoubleSeconds As Double = (lDoubleMinutes - lMinutesInt) * 60.0

        aDegreesString = CStr(lDegreesInt)
        aMinutesString = CStr(lMinutesInt)
        aSecondsString = Format(lDoubleSeconds, "0.#")
    End Sub

    Public Function DegToRad(ByVal aDegrees As Double) As Double
        Return aDegrees * Math.PI / 180.0
    End Function

    Private Function ProjectF(ByVal aLatitude As Double) As Double
        aLatitude = DegToRad(aLatitude)
        Return Math.Log(Math.Tan(aLatitude) + (1 / Math.Cos(aLatitude)))
    End Function

    Private Function ProjectMercatorToLatitude(ByVal aMercator As Double) As Double
        Return 180 / Math.PI * Math.Atan((Math.Exp(aMercator) - Math.Exp(-aMercator)) / 2)
    End Function

    ''' <summary>
    ''' Compute approximate distance in meters between two points on the earth's surface given in decimal degrees
    ''' </summary>
    Public Function MetersBetweenLatLon(ByVal aLatitude1 As Double, ByVal aLongitude1 As Double, _
                                        ByVal aLatitude2 As Double, ByVal aLongitude2 As Double) As Double
        Dim Lat1rad As Double = DegToRad(aLatitude1)
        Dim dy As Double = g_RadiusOfEarth * (Lat1rad - DegToRad(aLatitude2))
        Dim dx As Double = g_RadiusOfEarth * Math.Cos(Lat1rad) * (DegToRad(aLongitude1) - DegToRad(aLongitude2))
        Return Math.Sqrt(dy * dy + dx * dx)
    End Function

    Public Function MetersPerPixel(ByVal aZoom As Integer) As Double
        Return g_CircumferenceOfEarth / ((1 << aZoom) * g_TileServer.TileSize)
    End Function

    Public Function LatitudeToMeters(ByVal aLatitude As Double) As Double
        Dim sinLat As Double = Math.Sin(DegToRad(aLatitude))
        Return g_RadiusOfEarth / 2 * Math.Log((1 + sinLat) / (1 - sinLat))
    End Function

    Public Function LongitudeToMeters(ByVal aLongitude As Double) As Double
        Return g_RadiusOfEarth * DegToRad(aLongitude)
    End Function

    'helper function - converts a latitude at a certain zoom into a y pixel
    Private Function LatitudeToYAtZoom(ByVal aLatitude As Double, ByVal aZoom As Integer) As Integer
        Dim arc As Double = MetersPerPixel(aZoom)
        Return CInt(Math.Round((g_HalfCircumferenceOfEarth - LatitudeToMeters(aLatitude)) / arc))
    End Function

    'helper function - converts a longitude at a certain zoom into a x pixel
    Private Function LongitudeToXAtZoom(ByVal aLongitude As Double, ByVal aZoom As Integer) As Integer
        Dim arc As Double = MetersPerPixel(aZoom)
        Return CInt(Math.Round((g_HalfCircumferenceOfEarth + LongitudeToMeters(aLongitude)) / arc))
    End Function

    ''' <summary>
    ''' Given an OSM tile specified by aTilePoint(X, Y) and aZoom, 
    ''' compute Lat/Long of NorthWest and SouthEast corners of tile
    ''' </summary>
    ''' <param name="aTilePoint">X and Y of OSM tile</param>
    ''' <param name="aZoom">Zoom of OSM tile</param>
    ''' <param name="aNorth">computed Latitude of northwest corner of tile</param>
    ''' <param name="aWest">computed Longitude of northwest corner of tile</param>
    ''' <param name="aSouth">computed Latitude of southeast corner of tile</param>
    ''' <param name="aEast">computed Longitude of southeast corner of tile</param>
    Public Sub CalcLatLonFromTileXY(ByVal aTilePoint As Point, ByVal aZoom As Long, _
                                    ByRef aNorth As Double, ByRef aWest As Double, _
                                    ByRef aSouth As Double, ByRef aEast As Double)
        Dim lNumTiles As Integer = 1 << aZoom

        'Tile X to longitude
        Dim lTileXtoLongitude As Double = 360.0 / lNumTiles
        aWest = -180 + aTilePoint.X * lTileXtoLongitude
        aEast = aWest + lTileXtoLongitude

        'Tile Y to latitude
        Dim lEarthFractionPerTile As Double = 1.0 / lNumTiles
        Dim lNorthFraction As Double = aTilePoint.Y * lEarthFractionPerTile
        Dim lSouthFraction As Double = lNorthFraction + lEarthFractionPerTile

        aNorth = ProjectMercatorToLatitude(g_LimitY - g_RangeY * lNorthFraction)
        aSouth = ProjectMercatorToLatitude(g_LimitY - g_RangeY * lSouthFraction)
    End Sub

    ''' <summary>
    ''' Given an OSM tile specified by aTilePoint(X, Y) and aZoom, 
    ''' compute projected meters of NorthWest and SouthEast corners of tile
    ''' </summary>
    ''' <param name="aTilePoint">X and Y of OSM tile</param>
    ''' <param name="aZoom">Zoom of OSM tile</param>
    ''' <param name="aNorth">computed meters of northwest corner of tile</param>
    ''' <param name="aWest">computed meters of northwest corner of tile</param>
    ''' <param name="aSouth">computed meters of southeast corner of tile</param>
    ''' <param name="aEast">computed meters of southeast corner of tile</param>
    Public Sub CalcMetersFromTileXY(ByVal aTilePoint As Point, ByVal aZoom As Long, _
                                    ByRef aNorth As Double, ByRef aWest As Double, _
                                    ByRef aSouth As Double, ByRef aEast As Double)
        CalcLatLonFromTileXY(aTilePoint, aZoom, aNorth, aWest, aSouth, aEast)

        aWest = LongitudeToMeters(aWest)
        aEast = LongitudeToMeters(aEast)
        aNorth = LatitudeToMeters(aNorth)
        aSouth = LatitudeToMeters(aSouth)

    End Sub

    ''' <summary>
    ''' Compute the OSM tile (X, Y) that contains the given lat/long point and
    ''' compute the pixel offset within the tile of the given point
    ''' </summary>
    ''' <param name="aLatitude">Latitude of desired point</param>
    ''' <param name="aLongitude">Longitude of desired point</param>
    ''' <param name="aZoom">OSM zoom level (0-17)</param>
    ''' <param name="aOffset">ByRef returns pixel offset of given point within the OSM tile</param>
    ''' <returns>Point containing X and Y of OSM tile containing the given lat/long point</returns>
    Public Function CalcTileXY(ByVal aLatitude As Double, _
                               ByVal aLongitude As Double, _
                               ByVal aZoom As Long, _
                               ByRef aOffset As Point) As Point
        If aLatitude > g_TileServer.LatMax Then aLatitude = g_TileServer.LatMax
        If aLatitude < g_TileServer.LatMin Then aLatitude = g_TileServer.LatMin

        Dim lNumTiles As Integer = 1 << aZoom
        Dim x As Double = (aLongitude + 180) / 360 * lNumTiles
        Dim y As Double = (1 - Math.Log(Math.Tan(aLatitude * Math.PI / 180) _
                                  + 1 / Math.Cos(aLatitude * Math.PI / 180)) / Math.PI) / 2 * lNumTiles
        Dim lPoint As New Point(Math.Floor(x), Math.Floor(y))
        aOffset = New Point((x - lPoint.X) * g_TileServer.TileSize, (y - lPoint.Y) * g_TileServer.TileSize)

        ' TODO Paul Meems Test
        ' From http://wiki.openstreetmap.org/wiki/Slippy_map_tilenames#VB.Net
        'Dim Wiki_X As Double = (aLongitude + 180) / 360 * 2 ^ aZoom
        'Dim Wiki_Y As Double = (1 - Math.Log(Math.Tan(aLatitude * Math.PI / 180) + 1 / Math.Cos(aLatitude * Math.PI / 180)) / Math.PI) / 2 * 2 ^ aZoom

        'Debug.WriteLine("In CalcTileXY")
        'Debug.Write("x: " & x)
        'Debug.WriteLine(" y: " & x)
        'Debug.Write("Wiki_X: " & Wiki_X)
        'Debug.WriteLine(" Wiki_Y: " & Wiki_Y)

        Return lPoint
    End Function

    ''' <summary>
    ''' Remove dangerous characters in filename
    ''' </summary>
    ''' <param name="aOriginalFilename">Filename to be converted</param>
    ''' <param name="aReplaceWith">String to replace non-printable characters with (default=empty string)</param>
    ''' <returns>aOriginalFilename with non-printable and non-allowed file name characters replaced with aReplaceWith</returns>
    Public Function SafeFilename(ByVal aOriginalFilename As String, _
                        Optional ByVal aReplaceWith As String = "") As String
        Dim lRetval As String = "" 'return string
        Dim lChr As String 'individual character in filename
        For i As Integer = 1 To aOriginalFilename.Length
            lChr = Mid(aOriginalFilename, i, 1)
            Select Case Asc(lChr)
                Case 0 : GoTo EndFound
                Case Is < 32, 34, 42, 47, 58, 60, 62, 63, 92, 124, Is > 126 : lRetval &= aReplaceWith
                Case Else : lRetval &= lChr
            End Select
        Next
EndFound:
        Return lRetval.Trim
    End Function

    ''' <summary>
    ''' Return the full path for a cached tile from g_TileCacheFolder and OSM X, Y and Zoom
    ''' </summary>
    ''' <param name="aTilePoint">OSM Tile X and Y</param>
    ''' <param name="aZoom">OSM zoom level (0-17)</param>
    ''' <returns>full path for a cached tile file</returns>
    ''' <remarks>Returns empty string when aTilePoint and aZoom are not a valid tile specification</remarks>
    Public Function TileFilename(ByVal aTilePoint As Point, _
                                 ByVal aZoom As Integer, _
                                 ByVal aUseMarkedTiles As Boolean) As String
        With aTilePoint
            If ValidTilePoint(aTilePoint, aZoom) Then
                Dim lMarked As String = ""
                If aUseMarkedTiles Then lMarked = g_MarkedPrefix
                Return g_TileCacheFolder & lMarked & aZoom _
                     & g_PathChar & .X _
                     & g_PathChar & .Y 'other convention = (1 << aZoom) - .Y
            Else
                ' Most likely zoomed too fast
                Return ""
            End If
        End With
    End Function

    Public Function ValidTilePoint(ByVal aTilePoint As Point, _
                                   ByVal aZoom As Integer) As Boolean
        With aTilePoint
            Return aZoom >= g_TileServer.ZoomMin AndAlso aZoom <= g_TileServer.ZoomMax _
              AndAlso .X >= 0 AndAlso .Y >= 0 _
              AndAlso .X < (1 << aZoom) AndAlso .Y < (1 << aZoom)
        End With
    End Function

    Public Function ReadableFromXML(ByVal aXML As String) As String
        Dim lSB As New System.Text.StringBuilder
        Dim lIndex As Integer = 0
        While lIndex < aXML.Length
            Dim lChar As Char = aXML.Chars(lIndex)
            Select Case lChar
                Case "<"
                    Dim lClose As Integer = aXML.IndexOf(">", lIndex + 1)
                    If aXML.Chars(lIndex + 1) = "/" Then
                        lSB.Append(vbLf)
                    Else
                        lSB.Append(aXML.Substring(lIndex + 1, lClose - lIndex - 1) & ": ")
                    End If
                    lIndex = lClose
                Case Else : lSB.Append(lChar)
            End Select
            lIndex += 1
        End While
        Return lSB.ToString.Replace(vbLf & vbLf, vbLf).Replace(vbLf, vbLf)
    End Function

    'Encode or decode text using the ROT13 algorithm
    Public Function ROT13(ByVal InputText As String) As String
        Dim i As Integer
        Dim CurrentCharacter As Char
        Dim CurrentCharacterCode As Integer
        Dim EncodedText As String = ""

        'Iterate through the length of the input parameter  
        For i = 0 To InputText.Length - 1
            'Convert the current character to a char  
            CurrentCharacter = System.Convert.ToChar(InputText.Substring(i, 1))

            'Get the character code of the current character  
            CurrentCharacterCode = Microsoft.VisualBasic.Asc(CurrentCharacter)

            'Modify the character code of the character, - this  
            'so that "a" becomes "n", "z" becomes "m", "N" becomes "Y" and so on  
            If CurrentCharacterCode >= 97 And CurrentCharacterCode <= 109 Then
                CurrentCharacterCode = CurrentCharacterCode + 13

            Else
                If CurrentCharacterCode >= 110 And CurrentCharacterCode <= 122 Then
                    CurrentCharacterCode = CurrentCharacterCode - 13

                Else
                    If CurrentCharacterCode >= 65 And CurrentCharacterCode <= 77 Then
                        CurrentCharacterCode = CurrentCharacterCode + 13

                    Else
                        If CurrentCharacterCode >= 78 And CurrentCharacterCode <= 90 Then
                            CurrentCharacterCode = CurrentCharacterCode - 13
                        End If
                    End If
                End If 'Add the current character to the string to be returned
            End If
            EncodedText = EncodedText + Microsoft.VisualBasic.ChrW(CurrentCharacterCode)
        Next i

        Return EncodedText
    End Function 'ROT13Encode  

    ''' <summary>
    ''' Set a property or field given its name and a new value
    ''' </summary>
    ''' <param name="aObject">Object whose property or field needs to be set</param>
    ''' <param name="aFieldName">Name of property or field to set</param>
    ''' <param name="aValue">New value to set</param>
    Public Function SetSomething(ByRef aObject As Object, ByVal aFieldName As String, ByVal aValue As Object) As Boolean
        Dim lType As Type = aObject.GetType
        SetSomething = True
        Try
            Dim lProperty As Reflection.PropertyInfo = lType.GetProperty(aFieldName)
            If lProperty IsNot Nothing Then
                Select Case Type.GetTypeCode(lProperty.PropertyType)
                    Case TypeCode.Boolean : lProperty.SetValue(aObject, CBool(aValue), Nothing)
                    Case TypeCode.Byte : lProperty.SetValue(aObject, CByte(aValue), Nothing)
                    Case TypeCode.Char : lProperty.SetValue(aObject, CChar(aValue), Nothing)
                    Case TypeCode.DateTime : lProperty.SetValue(aObject, CDate(aValue), Nothing)
                    Case TypeCode.Decimal : lProperty.SetValue(aObject, CDec(aValue), Nothing)
                    Case TypeCode.Double : lProperty.SetValue(aObject, CDbl(aValue), Nothing)
                    Case TypeCode.Int16 : lProperty.SetValue(aObject, CShort(aValue), Nothing)
                    Case TypeCode.Int32 : lProperty.SetValue(aObject, CInt(aValue), Nothing)
                    Case TypeCode.Int64 : lProperty.SetValue(aObject, CLng(aValue), Nothing)
                    Case TypeCode.SByte : lProperty.SetValue(aObject, CSByte(aValue), Nothing)
                    Case TypeCode.Single : lProperty.SetValue(aObject, CSng(aValue), Nothing)
                    Case TypeCode.String : lProperty.SetValue(aObject, CStr(aValue), Nothing)
                    Case TypeCode.UInt16 : lProperty.SetValue(aObject, CUShort(aValue), Nothing)
                    Case TypeCode.UInt32 : lProperty.SetValue(aObject, CUInt(aValue), Nothing)
                    Case TypeCode.UInt64 : lProperty.SetValue(aObject, CULng(aValue), Nothing)
                    Case Else
                        'Logger.Dbg("Unable to set " & lType.Name & "." & aFieldName & ": unknown type " & lProperty.PropertyType.Name)
                        SetSomething = False
                End Select
            Else
                Dim lField As Reflection.FieldInfo = lType.GetField(aFieldName)
                If lField IsNot Nothing Then
                    Select Case Type.GetTypeCode(lField.FieldType)
                        Case TypeCode.Boolean : lField.SetValue(aObject, CBool(aValue))
                        Case TypeCode.Byte : lField.SetValue(aObject, CByte(aValue))
                        Case TypeCode.Char : lField.SetValue(aObject, CChar(aValue))
                        Case TypeCode.DateTime : lField.SetValue(aObject, CDate(aValue))
                        Case TypeCode.Decimal : lField.SetValue(aObject, CDec(aValue))
                        Case TypeCode.Double : lField.SetValue(aObject, CDbl(aValue))
                        Case TypeCode.Int16 : lField.SetValue(aObject, CShort(aValue))
                        Case TypeCode.Int32 : lField.SetValue(aObject, CInt(aValue))
                        Case TypeCode.Int64 : lField.SetValue(aObject, CLng(aValue))
                        Case TypeCode.SByte : lField.SetValue(aObject, CSByte(aValue))
                        Case TypeCode.Single : lField.SetValue(aObject, CSng(aValue))
                        Case TypeCode.String : lField.SetValue(aObject, CStr(aValue))
                        Case TypeCode.UInt16 : lField.SetValue(aObject, CUShort(aValue))
                        Case TypeCode.UInt32 : lField.SetValue(aObject, CUInt(aValue))
                        Case TypeCode.UInt64 : lField.SetValue(aObject, CULng(aValue))
                        Case Else
                            'Logger.Dbg("Unable to set " & lType.Name & "." & aFieldName & ": unknown type " & lField.FieldType.Name)
                            SetSomething = False
                    End Select
                Else
                    'Logger.Dbg("Unable to set " & lType.Name & "." & aFieldName & ": unknown field or property")
                    SetSomething = False
                End If
            End If
        Catch ex As Exception
            Dbg("SetSomething Exception: " & ex.Message)
            SetSomething = False
        End Try
    End Function

    Public Function GetAppSetting(ByVal aSettingName As String, ByVal aDefaultValue As Object) As Object
        Dim lSoftwareKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software")
        If lSoftwareKey IsNot Nothing Then
            Dim lAppKey As Microsoft.Win32.RegistryKey = lSoftwareKey.OpenSubKey(g_AppName)
            If lAppKey IsNot Nothing Then Return lAppKey.GetValue(aSettingName, aDefaultValue)
        End If
        Return aDefaultValue
    End Function

    Public Sub SaveAppSetting(ByVal aSettingName As String, ByVal aValue As Object)
        Try
            Dim lSoftwareKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software")
            If lSoftwareKey IsNot Nothing Then
                Dim lAppKey As Microsoft.Win32.RegistryKey = lSoftwareKey.CreateSubKey(g_AppName)
                If lAppKey IsNot Nothing Then lAppKey.SetValue(aSettingName, aValue)
            End If
        Catch e As Exception
            Dbg("Exception setting '" & aSettingName & "': " & e.Message)
        End Try
    End Sub

    'Open a file or URL using the default method the system would have used if it was double-clicked
    Public Sub OpenFileOrURL(ByVal aFileOrURL As String, ByVal aWait As Boolean)
        Dim newProcess As New Process
        Try
            If aFileOrURL <> "" Then
                'Use a .NET process() to launch the file or URL
                newProcess.StartInfo.FileName = aFileOrURL
                newProcess.Start()
                If aWait Then newProcess.WaitForExit()
            End If
        Catch ex As Exception
            Dbg("Exception opening '" & aFileOrURL & "': " & ex.Message)
        End Try
    End Sub

    Public Function EnsureDirForFile(ByVal aFilename As String) As Boolean
        Try
            Dim lDirectory As String = IO.Path.GetDirectoryName(aFilename)
            If lDirectory.Length > 1 Then
                If Not IO.Directory.Exists(lDirectory) Then
                    IO.Directory.CreateDirectory(lDirectory)
                    If Not IO.Directory.Exists(lDirectory) Then
                        Return False
                    End If
                End If
            End If
            Return True
        Catch
            Return False
        End Try
    End Function

    Public Sub DrawArrow(ByVal g As Graphics, ByVal aPen As Pen, ByVal aXcenter As Integer, ByVal aYcenter As Integer, ByVal aRadians As Double, ByVal aRadius As Integer)
        Dim ldx As Integer = Math.Sin(aRadians) * aRadius
        Dim ldy As Integer = Math.Cos(aRadians) * aRadius
        DrawArrow(g, aPen, aXcenter - ldx, aYcenter + ldy, aXcenter, aYcenter, aRadius)
    End Sub

    Public Sub DrawArrow(ByVal g As Graphics, ByVal aPen As Pen, _
                         ByVal aTailX As Integer, ByVal aTailY As Integer, _
                         ByVal aHeadX As Integer, ByVal aHeadY As Integer, ByVal aHeadLength As Double)
        'main line of arrow is easy
        'g.DrawLine(aPen, aTailX, aTailY, aHeadX, aHeadY)

        Dim aLeaf1X, aLeaf1Y, aLeaf2X, aLeaf2Y As Integer
        ArrowLeaves(aTailX, aTailY, aHeadX, aHeadY, aHeadLength, aLeaf1X, aLeaf1Y, aLeaf2X, aLeaf2Y)
        g.DrawLine(aPen, aLeaf1X, aLeaf1Y, aHeadX, aHeadY)
        g.DrawLine(aPen, aLeaf2X, aLeaf2Y, aHeadX, aHeadY)
    End Sub

    Public Sub ArrowLeaves(ByVal aTailX As Integer, ByVal aTailY As Integer, _
                           ByVal aHeadX As Integer, ByVal aHeadY As Integer, _
                           ByVal aHeadLength As Double, _
                           ByRef aLeaf1X As Integer, ByRef aLeaf1Y As Integer, _
                           ByRef aLeaf2X As Integer, ByRef aLeaf2Y As Integer)

        Dim psi As Double 'the angle of the vector from the tip to the start
        If aTailY = aHeadY Then
            If aTailX > aHeadX Then
                psi = Math.PI / 2
            Else
                psi = 3 * Math.PI / 2
            End If
        Else
            psi = Math.Atan2(aTailX - aHeadX, aTailY - aHeadY)
        End If
        Dim psi1 As Double = psi + Math.PI / 10
        Dim psi2 As Double = psi - Math.PI / 10
        aLeaf1X = CInt(aHeadX + aHeadLength * Math.Sin(psi1))
        aLeaf1Y = CInt(aHeadY + aHeadLength * Math.Cos(psi1))
        aLeaf2X = CInt(aHeadX + aHeadLength * Math.Sin(psi2))
        aLeaf2Y = CInt(aHeadY + aHeadLength * Math.Cos(psi2))
    End Sub

    Public Function TimeSpanString(ByVal aTimeSpan As TimeSpan) As String
        If aTimeSpan.TotalDays > 1 Then
            Return Math.Floor(aTimeSpan.TotalDays) & "d"
        ElseIf aTimeSpan.TotalHours > 1 Then
            Return Math.Floor(aTimeSpan.TotalHours) & "h"
        ElseIf aTimeSpan.TotalMinutes > 1 Then
            Return Math.Floor(aTimeSpan.TotalMinutes) & "m"
        Else
            Return ""
        End If
    End Function

    Public Function ReadTextFile(ByVal aFilename As String) As String
#If Smartphone Then
        Dim lReader As IO.StreamReader = IO.File.OpenText(aFilename)
        ReadTextFile = lReader.ReadToEnd()
        lReader.Close()
#Else
        Return IO.File.ReadAllText(aFilename)
#End If
    End Function

    Public Sub WriteTextFile(ByVal aFilename As String, ByVal aContents As String)
#If Smartphone Then
        Dim lFile As IO.StreamWriter = IO.File.CreateText(aFilename)
        lFile.Write(aContents)
        lFile.Close()
#Else
        IO.File.WriteAllText(aFilename, aContents)
#End If
    End Sub

    Public Function DoubleTryParse(ByVal aString As String, ByRef aDouble As Double) As Boolean
#If Smartphone Then
        Try
            aDouble = Double.Parse(aString)
            Return True
        Catch ex As Exception
            Return False
        End Try
#Else
        Return Double.TryParse(aString, aDouble)
#End If
    End Function

    Public Function IntegerTryParse(ByVal aString As String, ByRef aInteger As Integer) As Boolean
#If Smartphone Then
        Try
            aInteger = Integer.Parse(aString)
            Return True
        Catch ex As Exception
            Return False
        End Try
#Else
        Return Integer.TryParse(aString, aInteger)
#End If
    End Function

    ' Return a random RGB color.
    Public Function RandomRGBColor(Optional ByVal aAlpha As Integer = 255) As Color
#If Smartphone Then 'Window Mobile does not have FromArgb with Alpha
        Return Color.FromArgb(g_Random.Next(0, 255), _
                              g_Random.Next(0, 255), _
                              g_Random.Next(0, 255))
#Else
        Return Color.FromArgb(aAlpha, _
                              g_Random.Next(0, 255), _
                              g_Random.Next(0, 255), _
                              g_Random.Next(0, 255))
#End If
    End Function

    Public Sub Dbg(ByVal aMessage As String)
        If g_Debug Then
            pDebugMutex.WaitOne()
            Debug.WriteLine(aMessage)
            Try
                If g_DebugWriter Is Nothing Then
                    g_DebugFilename = IO.Path.GetTempPath & "VataviaMapLog" & DateTime.Now.ToString("yyyy-MM-dd_HH-mm.ss") & ".txt"
                    g_DebugWriter = IO.StreamWriter.Synchronized(IO.File.AppendText(g_DebugFilename))
                End If
                g_DebugWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss ") & aMessage)
                g_DebugWriter.Flush()
            Catch e As Exception
                'MsgBox("Could not log message to '" & g_DebugFilename & "'" & vbLf & e.Message & vbLf & aMessage)
            End Try
            pDebugMutex.ReleaseMutex()
        End If
    End Sub

    Public Sub Progress(ByVal aMessage As String, ByVal aCurrent As Integer, ByVal aLast As Integer)
        If g_ShowStatusProgress Then MapWinUtility.Logger.Progress(aMessage, aCurrent, aLast)
        Debug.WriteLine(aMessage)
    End Sub

    ''' <summary>
    ''' Like String.Substring but gracefully return an empty or shorter string when it would have thrown an exception
    ''' </summary>
    ''' <param name="aSourceString">String to get substring of</param>
    ''' <param name="aStartIndex">Zero-based start position of substring</param>
    ''' <param name="aLength">Length of substring</param>
    ''' <remarks>Why could they not have implemented String.Substring more like this?</remarks>
    Public Function SafeSubstring(ByVal aSourceString As String, ByVal aStartIndex As Integer, ByVal aLength As Integer) As String
        If aSourceString Is Nothing OrElse aSourceString.Length <= aStartIndex Then
            Return ""
        ElseIf aSourceString.Length > aStartIndex + aLength Then
            Return aSourceString.Substring(aStartIndex, aLength)
        Else
            Return aSourceString.Substring(aStartIndex)
        End If
    End Function

    ''' <summary>
    ''' Like String.Substring but gracefully return an empty or shorter string when it would have thrown an exception
    ''' </summary>
    ''' <param name="aSourceString">String to get substring of</param>
    ''' <param name="aStartIndex">Zero-based start position of substring</param>
    ''' <remarks>Why could they not have implemented String.Substring more like this?</remarks>
    Public Function SafeSubstring(ByVal aSourceString As String, ByVal aStartIndex As Integer) As String
        If aSourceString Is Nothing OrElse aSourceString.Length <= aStartIndex Then
            Return ""
        Else
            Return aSourceString.Substring(aStartIndex)
        End If
    End Function

End Module