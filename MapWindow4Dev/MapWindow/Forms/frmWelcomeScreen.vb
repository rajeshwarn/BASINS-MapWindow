'********************************************************************************************************
'File Name: frmWelcomeScreen.vb
'Description: MapWindow Welcome Screen
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
'2/3/2005 - made spacing of text relative if no recent projexts, added TODOs (jlk)
'8/9/2006 - Paul Meems (pm) - Started Duth translation
'1/28/2008 - Jiri Kadlec - changed ResourceManager (message strings moved to GlobalResource.resx)
'10/1/2008 - Earljon Hidalgo (ejh) Modify icons modern look. Icons provided by famfamfam
'********************************************************************************************************
Public Class frmWelcomeScreen
    Inherits System.Windows.Forms.Form

#Region "Declarations"
    'changed by Jiri Kadlec
    Private resources As System.Resources.ResourceManager = _
    New System.Resources.ResourceManager("MapWindow.GlobalResource", System.Reflection.Assembly.GetExecutingAssembly())
#End Region

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        'May/12/2008 Jiri Kadlec - load icon from shared resources to reduce size of the program
        'Me.Icon = My.Resources.MapWindow_new
        ' Paul Meems, The above does not seem to work.
        ' This does:
        If frmMain IsNot Nothing AndAlso frmMain.Icon IsNot Nothing Then Me.Icon = frmMain.Icon

        'September/28/2009 Paul Meems - load icon from shared resources to reduce size of the program
        'pbMapWindow.Image = My.Resources.MapWindow_new.ToBitmap
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
    Friend WithEvents lbAddData As System.Windows.Forms.LinkLabel
    Friend WithEvents lbOpenProject As System.Windows.Forms.LinkLabel
    Friend WithEvents lbGettingStarted As System.Windows.Forms.LinkLabel
    Friend WithEvents lbMapWindow As System.Windows.Forms.LinkLabel
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox5 As System.Windows.Forms.PictureBox
    Friend WithEvents pbMapWindow As System.Windows.Forms.PictureBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents cbShowDlg As System.Windows.Forms.CheckBox
    Friend WithEvents lbProject1 As System.Windows.Forms.LinkLabel
    Friend WithEvents lbProject2 As System.Windows.Forms.LinkLabel
    Friend WithEvents lbProject3 As System.Windows.Forms.LinkLabel
    Friend WithEvents PictureBox6 As System.Windows.Forms.PictureBox
    Friend WithEvents lbHelpFile As System.Windows.Forms.LinkLabel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lbPaypal As System.Windows.Forms.LinkLabel
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWelcomeScreen))
        Me.lbAddData = New System.Windows.Forms.LinkLabel
        Me.lbOpenProject = New System.Windows.Forms.LinkLabel
        Me.lbGettingStarted = New System.Windows.Forms.LinkLabel
        Me.lbMapWindow = New System.Windows.Forms.LinkLabel
        Me.cbShowDlg = New System.Windows.Forms.CheckBox
        Me.btnClose = New System.Windows.Forms.Button
        Me.lbProject1 = New System.Windows.Forms.LinkLabel
        Me.lbProject2 = New System.Windows.Forms.LinkLabel
        Me.lbProject3 = New System.Windows.Forms.LinkLabel
        Me.lblVersion = New System.Windows.Forms.Label
        Me.pbMapWindow = New System.Windows.Forms.PictureBox
        Me.PictureBox5 = New System.Windows.Forms.PictureBox
        Me.PictureBox4 = New System.Windows.Forms.PictureBox
        Me.PictureBox3 = New System.Windows.Forms.PictureBox
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.PictureBox6 = New System.Windows.Forms.PictureBox
        Me.lbHelpFile = New System.Windows.Forms.LinkLabel
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.lbPaypal = New System.Windows.Forms.LinkLabel
        CType(Me.pbMapWindow, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lbAddData
        '
        resources.ApplyResources(Me.lbAddData, "lbAddData")
        Me.lbAddData.Name = "lbAddData"
        Me.lbAddData.TabStop = True
        '
        'lbOpenProject
        '
        resources.ApplyResources(Me.lbOpenProject, "lbOpenProject")
        Me.lbOpenProject.Name = "lbOpenProject"
        Me.lbOpenProject.TabStop = True
        Me.lbOpenProject.UseCompatibleTextRendering = True
        '
        'lbGettingStarted
        '
        resources.ApplyResources(Me.lbGettingStarted, "lbGettingStarted")
        Me.lbGettingStarted.Name = "lbGettingStarted"
        Me.lbGettingStarted.TabStop = True
        Me.lbGettingStarted.UseCompatibleTextRendering = True
        '
        'lbMapWindow
        '
        resources.ApplyResources(Me.lbMapWindow, "lbMapWindow")
        Me.lbMapWindow.Name = "lbMapWindow"
        Me.lbMapWindow.TabStop = True
        '
        'cbShowDlg
        '
        resources.ApplyResources(Me.cbShowDlg, "cbShowDlg")
        Me.cbShowDlg.Name = "cbShowDlg"
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.SystemColors.Control
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK
        resources.ApplyResources(Me.btnClose, "btnClose")
        Me.btnClose.Name = "btnClose"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'lbProject1
        '
        resources.ApplyResources(Me.lbProject1, "lbProject1")
        Me.lbProject1.Name = "lbProject1"
        Me.lbProject1.TabStop = True
        '
        'lbProject2
        '
        resources.ApplyResources(Me.lbProject2, "lbProject2")
        Me.lbProject2.Name = "lbProject2"
        Me.lbProject2.TabStop = True
        '
        'lbProject3
        '
        resources.ApplyResources(Me.lbProject3, "lbProject3")
        Me.lbProject3.Name = "lbProject3"
        Me.lbProject3.TabStop = True
        '
        'lblVersion
        '
        resources.ApplyResources(Me.lblVersion, "lblVersion")
        Me.lblVersion.Name = "lblVersion"
        '
        'pbMapWindow
        '
        resources.ApplyResources(Me.pbMapWindow, "pbMapWindow")
        Me.pbMapWindow.Name = "pbMapWindow"
        Me.pbMapWindow.TabStop = False
        '
        'PictureBox5
        '
        Me.PictureBox5.Image = Global.MapWindow.GlobalResource.tutorials
        resources.ApplyResources(Me.PictureBox5, "PictureBox5")
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.TabStop = False
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = Global.MapWindow.GlobalResource.documentation
        resources.ApplyResources(Me.PictureBox4, "PictureBox4")
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.TabStop = False
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = Global.MapWindow.GlobalResource.open
        resources.ApplyResources(Me.PictureBox3, "PictureBox3")
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.MapWindow.GlobalResource.layer_add
        resources.ApplyResources(Me.PictureBox2, "PictureBox2")
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.TabStop = False
        '
        'PictureBox6
        '
        resources.ApplyResources(Me.PictureBox6, "PictureBox6")
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.TabStop = False
        '
        'lbHelpFile
        '
        resources.ApplyResources(Me.lbHelpFile, "lbHelpFile")
        Me.lbHelpFile.Name = "lbHelpFile"
        Me.lbHelpFile.TabStop = True
        Me.lbHelpFile.UseCompatibleTextRendering = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.MapWindow.GlobalResource.paypal
        resources.ApplyResources(Me.PictureBox1, "PictureBox1")
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.TabStop = False
        '
        'lbPaypal
        '
        resources.ApplyResources(Me.lbPaypal, "lbPaypal")
        Me.lbPaypal.Name = "lbPaypal"
        Me.lbPaypal.TabStop = True
        '
        'frmWelcomeScreen
        '
        Me.AcceptButton = Me.btnClose
        resources.ApplyResources(Me, "$this")
        Me.BackColor = System.Drawing.Color.White
        Me.CancelButton = Me.btnClose
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lbPaypal)
        Me.Controls.Add(Me.PictureBox6)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.lbProject3)
        Me.Controls.Add(Me.lbProject2)
        Me.Controls.Add(Me.lbProject1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.cbShowDlg)
        Me.Controls.Add(Me.pbMapWindow)
        Me.Controls.Add(Me.PictureBox5)
        Me.Controls.Add(Me.PictureBox4)
        Me.Controls.Add(Me.PictureBox3)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.lbMapWindow)
        Me.Controls.Add(Me.lbHelpFile)
        Me.Controls.Add(Me.lbGettingStarted)
        Me.Controls.Add(Me.lbOpenProject)
        Me.Controls.Add(Me.lbAddData)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmWelcomeScreen"
        Me.ShowInTaskbar = False
        CType(Me.pbMapWindow, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub lbAddData_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lbAddData.LinkClicked
        If (Not frmMain.m_layers.Add() Is Nothing) Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub lbOpenProject_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lbOpenProject.LinkClicked
        Dim dlg As New OpenFileDialog

        dlg.Filter = "MapWindow Project Files (*.mwprj)|*.mwprj"
        dlg.CheckFileExists = True
        dlg.InitialDirectory = AppInfo.DefaultDir
        If dlg.ShowDialog(Me) = DialogResult.OK Then
            frmMain.Project.Load(dlg.FileName)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub lbGettingStarted_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lbGettingStarted.LinkClicked
        'If MapWinUtility.MiscUtils.CheckInternetConnection("http://www.mapwindow.org/apps/wiki/doku.php?id=getting_started") Then
        '    System.Diagnostics.Process.Start("http://www.mapwindow.org/apps/wiki/doku.php?id=getting_started")
        'Else
        If MapWinUtility.MiscUtils.CheckInternetConnection("http://www.mapwindow.org/apps/wiki/doku.php?id=mapwindow4") Then
            System.Diagnostics.Process.Start("http://www.mapwindow.org/apps/wiki/doku.php?id=mapwindow4")
        Else

            'PM
            'mapwinutility.logger.msg("You don't appear to have an active internet connection. We have tutorials available at this URL:" + vbCrLf + vbCrLf + "http://www.MapWindow.org/tutorials/", MsgBoxStyle.Information, "No Internet Connection")
            MapWinUtility.Logger.Msg(resources.GetString("msgFindTutorials.Text"), MsgBoxStyle.Information, "No Internet Connection")
        End If
    End Sub

    Private Sub lbHelpFile_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lbHelpFile.LinkClicked
        If MapWinUtility.MiscUtils.CheckInternetConnection("http://www.mapwindow.org/apps/wiki/") Then
            System.Diagnostics.Process.Start("http://www.mapwindow.org/apps/wiki/")
            'ElseIf System.IO.File.Exists(BinFolder & "\OfflineDocs\index.html") Then
            '    System.Diagnostics.Process.Start(BinFolder & "\OfflineDocs\index.html")
            'Else
            '    'PM
            '    'mapwinutility.logger.msg("You don't appear to have an active internet connection. Our documentation is available online at this URL:" + vbCrLf + vbCrLf + "http://www.MapWindow.org/wiki/", MsgBoxStyle.Information, "No Internet Connection")
            '    MapWinUtility.Logger.Msg(resources.GetString("msgFindDocumentation.Text"), MsgBoxStyle.Information, "No Internet Connection")
        End If
    End Sub

    Private Sub lbMapWindow_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lbMapWindow.LinkClicked
        'PM Everything is .org so this one should also:
        'System.Diagnostics.Process.Start("http://www.MapWindow.com")
        System.Diagnostics.Process.Start("http://www.MapWindow.org")
    End Sub

    Private Sub cbShowDlg_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbShowDlg.CheckedChanged
        AppInfo.ShowWelcomeScreen = cbShowDlg.Checked
    End Sub

    Private Sub openLinkedProject(ByVal recentProjectID As Short)
        Dim fileName As String = CType(ProjInfo.RecentProjects(recentProjectID), String)
        If (System.IO.File.Exists(fileName)) Then
            'Paul Meems
            '6/12/2009
            'Save if necessary:
            If Not frmMain.m_HasBeenSaved Or ProjInfo.Modified Then
                If frmMain.PromptToSaveProject() = MsgBoxResult.Cancel Then
                    Exit Sub
                End If
            End If

            ' August 5, 2011 -  Paul Meems: Clear current project first:
            frmMain.DoNew()

            frmMain.Project.Load(fileName)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Else
            'TODO - 2/3/2005 - jlk - need a findFile here 
            'pm
            'mapwinutility.logger.msg("Could not find " & fileName, MsgBoxStyle.Exclamation)
            Dim sMsg As String = String.Format(resources.GetString("msgFileNotFound.Text"), fileName)
            MapWinUtility.Logger.Msg(sMsg, MsgBoxStyle.Exclamation)
        End If

    End Sub
    Private Sub lbProject1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lbProject1.LinkClicked
        openLinkedProject(0)
    End Sub

    Private Sub lbProject2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lbProject2.LinkClicked
        openLinkedProject(1)
    End Sub

    Private Sub lbProject3_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lbProject3.LinkClicked
        openLinkedProject(2)
    End Sub

    Private Sub frmWelcomeScreen_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblVersion.Text = "MapWindow Open Source " + App.VersionString
        cbShowDlg.Checked = AppInfo.ShowWelcomeScreen

        'check to see if there were any recent projects
        If (ProjInfo.RecentProjects.Count = 1) Then
            lbProject1.Text = System.IO.Path.GetFileName(CType(ProjInfo.RecentProjects(0), String))
            lbProject2.Visible = False
            lbProject3.Visible = False
            ' Fix for Bug #2041 Me.Height = Me.Height - (2 * (lbProject2.Top - lbProject1.Top))
        ElseIf (ProjInfo.RecentProjects.Count = 2) Then
            lbProject1.Text = System.IO.Path.GetFileName(CType(ProjInfo.RecentProjects(0), String))
            lbProject2.Text = System.IO.Path.GetFileName(CType(ProjInfo.RecentProjects(1), String))
            lbProject3.Visible = False
            ' Fix for Bug #2041 Me.Height = Me.Height - (lbProject2.Top - lbProject1.Top)
        ElseIf (ProjInfo.RecentProjects.Count >= 3) Then
            lbProject1.Text = System.IO.Path.GetFileName(CType(ProjInfo.RecentProjects(0), String))
            lbProject2.Text = System.IO.Path.GetFileName(CType(ProjInfo.RecentProjects(1), String))
            lbProject3.Text = System.IO.Path.GetFileName(CType(ProjInfo.RecentProjects(2), String))
        Else
            lbProject1.Visible = False
            lbProject2.Visible = False
            lbProject3.Visible = False
            ' Fix for Bug #2041 Me.Height = Me.Height - (3 * (lbProject2.Top - lbProject1.Top))
        End If

        'Chris M May 2006 - This makes the underline portion extend to the end of the line.
        If lbProject1.Visible Then lbProject1.LinkArea = New LinkArea(0, lbProject1.Text.Length)
        If lbProject2.Visible Then lbProject2.LinkArea = New LinkArea(0, lbProject2.Text.Length)
        If lbProject3.Visible Then lbProject3.LinkArea = New LinkArea(0, lbProject3.Text.Length)
        'PM August 2006 - After translation the size of the linkarea might be changed so create them on run-time

        lbAddData.LinkArea = New LinkArea(0, lbAddData.Text.Length)
        lbOpenProject.LinkArea = New LinkArea(0, lbOpenProject.Text.Length)
        lbGettingStarted.LinkArea = New LinkArea(0, lbGettingStarted.Text.Length)
        lbHelpFile.LinkArea = New LinkArea(0, lbHelpFile.Text.Length)
        lbMapWindow.LinkArea = New LinkArea(0, lbMapWindow.Text.Length)

    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        '2/3/2005 - jlk - is this the right call?
        'Me.finalize
        '2/4/2005 - dpa - Finalize is considered a "protected" method that the garbage collector uses to get rid of objects.
        'so normally we just use me.close().  This seems to be different than in VB6 where Close would close the form
        'but keep an object in memory.  In .NET, Close releases the form and then the object is goes away when it is out
        'of context in the calling function or when the calling function ends.
        Me.Close()
    End Sub

    Private Sub lbPaypal_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lbPaypal.LinkClicked
        System.Diagnostics.Process.Start("http://www.mapwindow.org/pages/donate.php")
    End Sub
End Class
