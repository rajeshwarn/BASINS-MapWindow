Public Class frmColoringSchemeStylePicker
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblFilename As System.Windows.Forms.Label
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmColoringSchemeStylePicker))
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblFilename = New System.Windows.Forms.Label
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AccessibleDescription = Nothing
        Me.Label1.AccessibleName = Nothing
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Font = Nothing
        Me.Label1.Name = "Label1"
        '
        'lblFilename
        '
        Me.lblFilename.AccessibleDescription = Nothing
        Me.lblFilename.AccessibleName = Nothing
        resources.ApplyResources(Me.lblFilename, "lblFilename")
        Me.lblFilename.BackColor = System.Drawing.SystemColors.Window
        Me.lblFilename.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFilename.Font = Nothing
        Me.lblFilename.Name = "lblFilename"
        '
        'ListBox1
        '
        Me.ListBox1.AccessibleDescription = Nothing
        Me.ListBox1.AccessibleName = Nothing
        resources.ApplyResources(Me.ListBox1, "ListBox1")
        Me.ListBox1.BackgroundImage = Nothing
        Me.ListBox1.Font = Nothing
        Me.ListBox1.Items.AddRange(New Object() {resources.GetString("ListBox1.Items"), resources.GetString("ListBox1.Items1"), resources.GetString("ListBox1.Items2"), resources.GetString("ListBox1.Items3"), resources.GetString("ListBox1.Items4"), resources.GetString("ListBox1.Items5"), resources.GetString("ListBox1.Items6"), resources.GetString("ListBox1.Items7")})
        Me.ListBox1.Name = "ListBox1"
        '
        'Button1
        '
        Me.Button1.AccessibleDescription = Nothing
        Me.Button1.AccessibleName = Nothing
        resources.ApplyResources(Me.Button1, "Button1")
        Me.Button1.BackgroundImage = Nothing
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button1.Font = Nothing
        Me.Button1.Name = "Button1"
        '
        'frmColoringSchemeStylePicker
        '
        Me.AcceptButton = Me.Button1
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.lblFilename)
        Me.Controls.Add(Me.Label1)
        Me.Font = Nothing
        Me.Icon = Nothing
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmColoringSchemeStylePicker"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Dim m_grd As MapWinGIS.Grid

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        g_Scheme = New MapWinGIS.GridColorScheme
        Dim pdf As New MapWinGIS.PredefinedColorScheme
        Select Case ListBox1.Items(ListBox1.SelectedIndex).ToString()
            Case "Dead Sea"
                pdf = MapWinGIS.PredefinedColorScheme.DeadSea
            Case "Desert"
                pdf = MapWinGIS.PredefinedColorScheme.Desert
            Case "Fall Leaves"
                pdf = MapWinGIS.PredefinedColorScheme.FallLeaves
            Case "Glaciers"
                pdf = MapWinGIS.PredefinedColorScheme.Glaciers
            Case "Highway"
                pdf = MapWinGIS.PredefinedColorScheme.Highway1
            Case "Meadow"
                pdf = MapWinGIS.PredefinedColorScheme.Meadow
            Case "Summer Mountains"
                pdf = MapWinGIS.PredefinedColorScheme.SummerMountains
            Case "Valley Fires"
                pdf = MapWinGIS.PredefinedColorScheme.ValleyFires
        End Select

        g_Scheme.UsePredefined(m_grd.Minimum, m_grd.Maximum, pdf)
        Me.Close()
    End Sub

    <CLSCompliant(False)> _
    Public Sub SetGridObject(ByRef grd As MapWinGIS.Grid)
        m_grd = grd
        lblFilename.Text = System.IO.Path.GetFileName(grd.Filename)
    End Sub

    Private Sub frmColoringSchemeStylePicker_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ListBox1.SelectedIndex = 0
    End Sub
End Class
