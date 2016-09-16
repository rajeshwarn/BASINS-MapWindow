' Christopher Michaelis
' cmichaelis@happysquirrel.com
'
' 2/24/2007
'
' This code is released under GPL 1.1.

Imports ICSharpCode.SharpZipLib.Zip
Imports System.IO
Imports MapWinUtility

Public Class frmArchiveToolForm

    Private m_MapWin As MapWindow.Interfaces.IMapWin
    Private m_Notes As String = ""
    Private m_RestoringProjectFilename As String = "zzz"

    Public Sub New(ByRef MapWin As MapWindow.Interfaces.IMapWin)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        m_MapWin = MapWin
    End Sub

    Private Sub frmArchiveToolForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If (m_MapWin.Layers.NumLayers = 0) Then
            grpArchive.Enabled = False
        End If
        MapWinUtility.Logger.Dbg("Archive Tool Form: Displayed")
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Me.Close()
    End Sub

    Private Sub AddFiles(ByRef FileList As ArrayList, ByVal path As String)
        'Files:
        For Each s As String In System.IO.Directory.GetFiles(path)
            If Not FileList.Contains(s) Then AddFileAndAssociates(FileList, s)
        Next
        'Subdirs:
        For Each s As String In System.IO.Directory.GetDirectories(path)
            AddFiles(FileList, s)
        Next
    End Sub

    Private Sub btnArchive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnArchive.Click
        If m_MapWin.Project.Modified Then
            If Logger.Msg("The project has been modified but not saved." + vbCrLf + vbCrLf + "Save it first?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Continue Without Notes?") = MsgBoxResult.Yes Then
                If Not m_MapWin.Project.FileName = "" Then
                    m_MapWin.Project.Save(m_MapWin.Project.FileName)
                    Logger.Dbg("ArchiveProject: Saved project to file " + m_MapWin.Project.FileName)
                Else
                    Dim psfd As New System.Windows.Forms.SaveFileDialog
                    psfd.Filter = "MapWindow Projects (*.mwprj)|*.mwprj"
                    If Not psfd.ShowDialog() = Windows.Forms.DialogResult.OK Then Return
                    m_MapWin.Project.Save(psfd.FileName)
                    Logger.Dbg("ArchiveProject: Saved project to file " + psfd.FileName)
                End If
            End If
        End If

        If txtNotes.Text = "" Then
            If Logger.Msg("Are you sure you wish to continue without notes?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Continue Without Notes?") = MsgBoxResult.No Then Exit Sub
            Logger.Dbg("ArchiveProject: Warning: Continuing with no notes")
        End If

        Dim sfd As New System.Windows.Forms.SaveFileDialog
        sfd.Filter = "Archive Project Files (*.mwa)|*.mwa"
        If sfd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            MapWinUtility.Logger.Dbg("ArchiveProject: Attempting to archive project to: " + sfd.FileName)

            Me.Cursor = Windows.Forms.Cursors.WaitCursor
            grpArchive.Enabled = False
            grpRestore.Enabled = False

            Dim FileList As New ArrayList
            Dim DeleteList As New ArrayList

            If Not m_MapWin.Project.FileName = "" Then
                'If they elected to preserve original pathnames, no need to change the project
                'file, since it already contains relative paths.
                If chbPreserveLocations.Checked Then
                    'Project file paths are already relative
                    FileList.Add(m_MapWin.Project.FileName)
                Else
                    'Will need to correct the project file
                    Dim newPrj As String = System.IO.Path.GetTempFileName()
                    System.IO.File.Delete(newPrj)
                    newPrj = System.IO.Path.GetDirectoryName(newPrj) + "\" + System.IO.Path.GetFileName(m_MapWin.Project.FileName)
                    System.IO.File.Copy(m_MapWin.Project.FileName, newPrj, True)

                    Dim xmldoc As New Xml.XmlDocument
                    xmldoc.Load(newPrj)
                    For Each xn As Xml.XmlNode In xmldoc.GetElementsByTagName("Layer")
                        If Not xn.Attributes Is Nothing Then
                            If Not xn.Attributes("Path") Is Nothing Then
                                If Not xn.Attributes("Path").Value = "" Then
                                    xn.Attributes("Path").Value = System.IO.Path.GetFileName(xn.Attributes("Path").Value)
                                End If
                            End If
                        End If

                    Next
                    xmldoc.Save(newPrj)

                    FileList.Add(newPrj)
                    DeleteList.Add(newPrj)
                End If

                If chbAllFiles.Checked Then
                    For Each s As String In System.IO.Directory.GetDirectories(System.IO.Path.GetDirectoryName(m_MapWin.Project.FileName))
                        AddFiles(FileList, s)
                    Next
                    AddFiles(FileList, System.IO.Path.GetDirectoryName(m_MapWin.Project.FileName))
                End If
            End If

            For i As Integer = 0 To m_MapWin.Layers.NumLayers - 1
                Dim s As String = m_MapWin.Layers(m_MapWin.Layers.GetHandle(i)).FileName
                If Not FileList.Contains(s) Then AddFileAndAssociates(FileList, s)
            Next

            prg1.Visible = True
            prg1.Maximum = FileList.Count + 1
            prg1.Value = 0
            Me.Refresh()

            Dim notesfile As String
            If m_MapWin.Project.FileName = "" Then
                notesfile = "\mwarknotes.txt" 'Use current drive from CWD
            Else
                notesfile = System.IO.Path.GetDirectoryName(m_MapWin.Project.FileName) + "\mwarknotes.txt"
            End If

            Dim tw As System.IO.TextWriter = New System.IO.StreamWriter(notesfile)
            tw.Write("Project Filename: " + System.IO.Path.GetFileName(m_MapWin.Project.FileName) + vbCrLf)
            tw.Write("Project Path: " + System.IO.Path.GetDirectoryName(m_MapWin.Project.FileName) + vbCrLf)
            tw.Write("Date Archived: " + DateTime.Now.ToLongDateString() + vbCrLf)
            tw.Write("Time Archived: " + DateTime.Now.ToLongTimeString() + vbCrLf)
            If chkIncludeUserCompName.Checked Then
                tw.Write("Computer Name: " + Environment.MachineName + vbCrLf)
                tw.Write("User Name: " + Environment.UserName + vbCrLf)
            End If
            tw.Write("Notes: " + Environment.NewLine + Environment.NewLine + txtNotes.Text + vbCrLf)
            tw.Close()

            Logger.Dbg("Project Filename: " + System.IO.Path.GetFileName(m_MapWin.Project.FileName))
            Logger.Dbg("Project Path: " + System.IO.Path.GetDirectoryName(m_MapWin.Project.FileName))
            Logger.Dbg("Date Archived: " + DateTime.Today.ToLongDateString())
            Logger.Dbg("Computer Name: " + Environment.MachineName)
            Logger.Dbg("User Name: " + Environment.UserName)
            Logger.Dbg("Notes: " + txtNotes.Text)

            FileList.Add(notesfile)
            DeleteList.Add(notesfile)

            CompressAllFiles(sfd.FileName, FileList, Not chbPreserveLocations.Checked)

            For i As Integer = 0 To DeleteList.Count - 1
                System.IO.File.Delete(DeleteList(i).ToString())
            Next

            prg1.Visible = False
            Me.Cursor = Windows.Forms.Cursors.Default
            Logger.Dbg("ArchiveProject: Compression Finished")
            Logger.Msg("Finished Archiving!", MsgBoxStyle.Information, "Finished!")
            Me.Close()
        Else
            MapWinUtility.Logger.Dbg("ArchiveProject: Cancelled open file dialog")
        End If
    End Sub

    Private Sub AddFileAndAssociates(ByRef FileList As ArrayList, ByVal filename As String)
        If Not FileList.Contains(filename) Then FileList.Add(filename)
        Dim base As String = System.IO.Path.GetDirectoryName(filename) + "\" + System.IO.Path.GetFileNameWithoutExtension(filename)

        If System.IO.Path.GetExtension(filename).ToLower().EndsWith(".shp") Then
            If System.IO.File.Exists(base + ".dbf") And Not FileList.Contains(base + ".dbf") Then FileList.Add(base + ".dbf")
            If System.IO.File.Exists(base + ".shx") And Not FileList.Contains(base + ".shx") Then FileList.Add(base + ".shx")
            If System.IO.File.Exists(base + ".shp.xml") And Not FileList.Contains(base + ".shp.xml") Then FileList.Add(base + ".shp.xml")
            If System.IO.File.Exists(base + ".lbl") And Not FileList.Contains(base + ".lbl") Then FileList.Add(base + ".lbl")
        End If

        ' General metadata
        If System.IO.File.Exists(base + ".prj") And Not FileList.Contains(base + ".prj") Then FileList.Add(base + ".prj")
        If System.IO.File.Exists(base + ".xml") And Not FileList.Contains(base + ".xml") Then FileList.Add(base + ".xml")
        If System.IO.File.Exists(base + ".mwsr") And Not FileList.Contains(base + ".mwsr") Then FileList.Add(base + ".mwsr")
        If System.IO.File.Exists(base + ".mwleg") And Not FileList.Contains(base + ".mwleg") Then FileList.Add(base + ".mwleg")
        If System.IO.File.Exists(base + ".bmp") And Not FileList.Contains(base + ".bmp") Then FileList.Add(base + ".bmp")
        If System.IO.File.Exists(base + ".bpw") And Not FileList.Contains(base + ".bpw") Then FileList.Add(base + ".bpw")
        If System.IO.File.Exists(base + ".jgw") And Not FileList.Contains(base + ".jgw") Then FileList.Add(base + ".jgw")
        If System.IO.File.Exists(base + ".gfw") And Not FileList.Contains(base + ".gfw") Then FileList.Add(base + ".gfw")
        If System.IO.File.Exists(base + ".aux") And Not FileList.Contains(base + ".aux") Then FileList.Add(base + ".aux")
        If System.IO.File.Exists(base + ".rrd") And Not FileList.Contains(base + ".rrd") Then FileList.Add(base + ".rrd")
    End Sub

    Private Sub CompressAllFiles(ByVal ZipFile As String, ByRef FileList As ArrayList, ByVal StripPaths As Boolean)
        Logger.Dbg("Compressing to file: " + ZipFile)
        ' No duplicates will be in FileList (exact case)
        ' Now, use a second arraylist to ensure no duplicates with differing case
        Dim LowercaseAdded As New ArrayList

        Dim zip As ZipOutputStream
        zip = New ZipOutputStream(System.IO.File.Create(ZipFile))

        ' Compression Level: 0-9
        ' 0: no compression
        ' 9: maximum compression
        zip.SetLevel(tbCompression.Value)
        Logger.Dbg("Compressing with level " + tbCompression.Value.ToString())

        For i As Integer = 0 To FileList.Count - 1
            Dim entryname As String
            If (StripPaths) Then
                entryname = System.IO.Path.GetFileName(FileList(i))
            Else
                'Keep relative paths
                entryname = GetRelativePath(FileList(i).ToString(), m_MapWin.Project.FileName)
            End If

            If Not LowercaseAdded.Contains(entryname.ToLower()) Then
                Logger.Dbg("Compressing file: " + entryname.ToLower())
                LowercaseAdded.Add(entryname.ToLower())

                Dim strmFile As FileStream = New System.IO.FileStream(FileList(i).ToString(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                Dim buffer(2048) As Byte
                Dim c As Integer = 1 'nonzero

                Dim objZipEntry As ZipEntry = New ZipEntry(entryname)

                Dim fi As New System.IO.FileInfo(FileList(i).ToString())
                objZipEntry.DateTime = fi.CreationTime
                objZipEntry.Size = strmFile.Length
                zip.PutNextEntry(objZipEntry)

                While (c > 0)
                    c = strmFile.Read(buffer, 0, buffer.Length)
                    zip.Write(buffer, 0, c)
                End While

                strmFile.Close()

                prg1.Value = i + 1
                prg1.Refresh()
                System.Windows.Forms.Application.DoEvents()
            End If
        Next

        zip.Finish()
        zip.Close()
    End Sub

    Private Sub btnRestore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestore.Click
        If txtRestoreTo.Text = "" Then
            Logger.Msg("Please select a location to restore to by clicking the Browse button.", MsgBoxStyle.Information, "Select Location")
            Return
        End If

        If Not System.IO.Directory.Exists(txtRestoreTo.Text) Then
            If Logger.Msg("The directory selected for the restore location doesn't exist." + vbCrLf + vbCrLf + "Create this directory?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Create Restore Directory?") = MsgBoxResult.Yes Then
                Try
                    System.IO.Directory.CreateDirectory(txtRestoreTo.Text)
                Catch
                    Logger.Msg("The directory could not be created. Please check for invalid characters like / or ?.", MsgBoxStyle.Exclamation, "Could Not Create Directory")
                    Return
                End Try
            Else
                Return
            End If
        End If

        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        prg1.Maximum = 100
        prg1.Minimum = 0
        prg1.Value = 0
        prg1.Visible = True

        Dim ProjectFilename As String = ""

        Logger.Dbg("Archived Project Restore: Restoring " + txtArchive.Text)

        'Actual Restore
        Dim totsize As Long = New FileInfo(txtArchive.Text).Length
        Dim zip As ZipInputStream = New ZipInputStream(File.OpenRead(txtArchive.Text))
        If Not zip Is Nothing Then
            Dim entry As ZipEntry = zip.GetNextEntry()
            While Not entry Is Nothing
                prg1.Value = Math.Min((zip.Position / totsize) * 100, 100)
                prg1.Refresh()
                System.Windows.Forms.Application.DoEvents()

                'No need to extract notes
                If Not entry.Name.EndsWith("mwarknotes.txt") Then
                    Logger.Dbg("Archived Project Restore: Restoring " + entry.Name)
                    Dim outpath As String = txtRestoreTo.Text + "\" + GetNoDrive(entry.Name)

                    Dim buf(2048) As Byte
                    Dim n As Integer = 1 'nonzero
                    EnsureDirectoriesExist(outpath)
                    Dim output As FileStream = New FileStream(outpath, FileMode.Create)
                    While (n > 0)
                        n = zip.Read(buf, 0, buf.Length)
                        output.Write(buf, 0, n)
                    End While
                    output.Close()

                    Dim fi As New FileInfo(outpath)
                    fi.CreationTime = entry.DateTime
                    fi.LastAccessTime = entry.DateTime
                    fi.LastWriteTime = entry.DateTime

                    If entry.Name.ToLower().EndsWith(m_RestoringProjectFilename.ToLower()) Then ProjectFilename = outpath
                End If

                entry = zip.GetNextEntry()
            End While
            zip.Close()
        End If
        'Done with Actual Restore

        Logger.Dbg("Archived Project Restore: Finished")
        prg1.Visible = False

        If ProjectFilename = "" Then
            Logger.Msg("The project data have been restored, but the project file (.mwprj) cannot be found to be opened.", MsgBoxStyle.Exclamation, "No Project File Found")
        Else
            If chbIntoThisProject.Checked Then
                m_MapWin.Project.LoadIntoCurrentProject(ProjectFilename)
                Logger.Msg("The project has been restored and loaded into the current project.", MsgBoxStyle.Information, "Finished")
            Else
                If Logger.Msg("The project has been restored. Open it now?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Open Project?") = MsgBoxResult.Yes Then
                    If m_MapWin.Project.Modified Then
                        If Logger.Msg("Save the current project first?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Open Project?") = MsgBoxResult.Yes Then
                            m_MapWin.Project.Save(m_MapWin.Project.FileName)
                        End If
                    End If

                    Logger.Dbg("Archived Project Restore: Opening Project " + ProjectFilename)
                    m_MapWin.Project.Load(ProjectFilename)
                End If
            End If
        End If

        Me.Cursor = Windows.Forms.Cursors.Default
        Me.Close()
    End Sub

    Private Function GetNoDrive(ByVal inp As String) As String
        If Not inp.Contains(":") Then Return inp

        Return inp.Substring(inp.IndexOf(":") + 2)
    End Function

    Public Function GetRelativePath(ByVal Filename As String, ByVal ProjectFile As String) As String
        If Filename = ProjectFile Then Return System.IO.Path.GetFileName(Filename)

        GetRelativePath = ""
        Dim a() As String, b() As String
        Dim i As Integer, j As Integer, k As Integer, Offset As Integer

        If Len(Filename) = 0 Or Len(ProjectFile) = 0 Then
            Return ""
        End If

        Try
            'If the drive is different then use the full path
            If System.IO.Path.GetPathRoot(Filename).ToLower() <> System.IO.Path.GetPathRoot(ProjectFile).ToLower() Then
                GetRelativePath = Filename
                Exit Function
            End If
            '
            'load a()
            ReDim a(0)
            a(0) = Filename
            i = 0
            Do
                i = i + 1
                ReDim Preserve a(i)
                Try
                    a(i) = System.IO.Directory.GetParent(a(i - 1)).FullName.ToLower()
                Catch
                End Try
            Loop Until a(i) = ""
            '
            'load b()
            ReDim b(0)
            b(0) = ProjectFile
            i = 0
            Do
                i = i + 1
                ReDim Preserve b(i)
                Try
                    b(i) = System.IO.Directory.GetParent(b(i - 1)).FullName.ToLower()
                Catch
                End Try
            Loop Until b(i) = ""
            '
            'look for match
            For i = 0 To UBound(a)
                For j = 0 To UBound(b)
                    If a(i) = b(j) Then
                        'found match
                        GoTo [CONTINUE]
                    End If
                Next j
            Next i
[CONTINUE]:
            ' j is num steps to get from BasePath to common path
            ' so I need this many of "..\"
            For k = 1 To j - 1
                GetRelativePath = GetRelativePath & "..\"
            Next k

            'everything past a(i) needs to be appended now.
            If a(i).EndsWith("\") Then
                Offset = 0
            Else
                Offset = 1
            End If
            GetRelativePath = GetRelativePath & Filename.Substring(Len(a(i)) + Offset)
        Catch e As System.Exception
            Return ""
        End Try
    End Function

    Private Sub btnOpenArchive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenArchive.Click
        Dim ofd As New System.Windows.Forms.OpenFileDialog
        ofd.Filter = "Archive Project Files (*.mwa)|*.mwa"
        If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtArchive.Text = ofd.FileName

            If LoadDetails() Then
                lnkDetails.Enabled = True
                btnRestoreTo.Enabled = True
                txtRestoreTo.Enabled = True
                chbIntoThisProject.Enabled = True
                btnRestore.Enabled = True
            End If
        End If
    End Sub

    Private Sub btnRestoreTo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestoreTo.Click
        Dim ofd As New System.Windows.Forms.FolderBrowserDialog
        If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtRestoreTo.Text = ofd.SelectedPath
        End If
    End Sub

    Private Sub lnkDetails_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkDetails.LinkClicked
        Logger.Msg(m_Notes, MsgBoxStyle.Information, "Archive Details")
    End Sub

    Private Sub EnsureDirectoriesExist(ByVal outpath As String)
        If Not System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(outpath)) Then System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outpath))
    End Sub

    Private Function LoadDetails() As Boolean
        Dim zip As ZipInputStream = New ZipInputStream(File.OpenRead(txtArchive.Text))
        If Not zip Is Nothing Then
            Dim entry As ZipEntry = zip.GetNextEntry()
            While Not entry Is Nothing
                'No need to extract notes
                If entry.Name.EndsWith("mwarknotes.txt") Then
                    Dim buf(2048) As Byte
                    Dim n As Integer = 1 'nonzero
                    Dim output As MemoryStream = New MemoryStream()
                    While (n > 0)
                        n = zip.Read(buf, 0, buf.Length)
                        output.Write(buf, 0, n)
                    End While

                    output.Seek(0, SeekOrigin.Begin)

                    If output.Length = 0 Then
                        Logger.Msg("Could not find archive details! Are you sure this is a MapWindow Archive?", MsgBoxStyle.Exclamation, "Could Not Find Archive Details")
                    Else
                        Dim tr As IO.TextReader = New IO.StreamReader(output)
                        Dim notes As String = tr.ReadToEnd()
                        m_Notes = notes
                        For Each s As String In notes.Split(vbCrLf)
                            If s.Contains("Project Path: ") Then
                                txtRestoreTo.Text = s.Replace("Project Path: ", "").Trim()
                            ElseIf s.Contains("Project Filename: ") Then
                                m_RestoringProjectFilename = s.Replace("Project Filename: ", "").Trim()
                            End If
                        Next
                        tr.Close()
                    End If

                    zip.Close()
                    Return True
                End If

                entry = zip.GetNextEntry()
            End While

            zip.Close()
        End If

        Logger.Msg("Could not find archive details! Are you sure this is a MapWindow Archive?", MsgBoxStyle.Exclamation, "Could Not Find Archive Details")
        Return False
    End Function

    Private Sub tbCompression_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbCompression.Scroll
        lblCompression.Text = "Compression: " + tbCompression.Value.ToString()
    End Sub
End Class