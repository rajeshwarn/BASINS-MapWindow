Public Class Form1
    Dim m As mwTableEditor.frmTableEditor
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sf As New MapWinGIS.Shapefile

        Dim filename As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location()) + "\USStates\states.shp"
        If Not System.IO.File.Exists(Filename) Then
            Dim fod As New OpenFileDialog()
            fod.CheckFileExists = True
            If fod.ShowDialog() = Windows.Forms.DialogResult.OK Then
                filename = fod.FileName
            Else
                Return
            End If
        End If

        sf.Open(filename)
        m = New mwTableEditor.frmTableEditor(sf, Me)
        'm.MapWindowInterface = ... This is a standalone app for testing.
        '                       In an MW plug-in, this could be set to my own IMapWin given to me in my own Initialize()
        m.Show() 'Make sure to show it before using the below functions, or 'No Shapefile Loaded!' error will be shown

        'm.AddField() 'Prompts to add a field
        'MsgBox(m.NumColumns.ToString())
        'MsgBox(m.ColumnName(2))
        'm.RefreshWithSavePrompt()

        Dim statename_idx As Integer = m.ColumnIndex("STATE_NAME")
        m.ConvertColumnToComboBox(statename_idx, New String() {"Alabama", "Alaska", "Arizona", "Arkansas", "California", "Colorado", "Connecticut", "Delaware", "Florida", "Georgia", "Hawaii", "Idaho", "Illinois", "Indiana", "Iowa", "Kansas", "Kentucky", "Louisiana", "Maine", "Maryland", "Massachusetts", "Michigan", "Minnesota", "Mississippi", "Missouri", "Montana", "Nebraska", "Nevada", "New Hampshire", "New Jersey", "New Mexico", "New York", "North Carolina", "North Dakota", "Ohio", "Oklahoma", "Oregon", "Pennsylvania", "Rhode Island", "South Carolina", "South Dakota", "Tennessee", "Texas", "Utah", "Vermont", "Virginia", "Washington", "West Virginia"})

        Dim area_idx As Integer = m.ColumnIndex("AREA")
        m.ConvertColumnToNumericRangeLimited(area_idx, 0, 100000000)

        Dim stateabbr_idx As Integer = m.ColumnIndex("STATE_ABBR")
        m.ColumnHeaderText(stateabbr_idx) = "St. Abbrev"
        m.ConvertColumnToMaskedTextBox(stateabbr_idx, "LL")

        m.ColumnVisible(0) = False 'Hide column 0
        m.ColumnWidth(1) = 100 'Set column 1 wider
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        m.ColumnVisible(0) = CheckBox1.Checked
    End Sub
End Class
