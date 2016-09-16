Public Class frmProgress
    Inherits System.Windows.Forms.Form
    Implements MapWinGIS.ICallback

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
    Friend WithEvents ProgBar As System.Windows.Forms.ProgressBar
    Friend WithEvents lblTaskName As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProgress))
        Me.ProgBar = New System.Windows.Forms.ProgressBar
        Me.lblTaskName = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'ProgBar
        '
        Me.ProgBar.AccessibleDescription = Nothing
        Me.ProgBar.AccessibleName = Nothing
        resources.ApplyResources(Me.ProgBar, "ProgBar")
        Me.ProgBar.BackgroundImage = Nothing
        Me.ProgBar.Font = Nothing
        Me.ProgBar.Name = "ProgBar"
        Me.ProgBar.Step = 1
        '
        'lblTaskName
        '
        Me.lblTaskName.AccessibleDescription = Nothing
        Me.lblTaskName.AccessibleName = Nothing
        resources.ApplyResources(Me.lblTaskName, "lblTaskName")
        Me.lblTaskName.Font = Nothing
        Me.lblTaskName.Name = "lblTaskName"
        '
        'frmProgress
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.ControlBox = False
        Me.Controls.Add(Me.lblTaskName)
        Me.Controls.Add(Me.ProgBar)
        Me.Font = Nothing
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = Nothing
        Me.Name = "frmProgress"
        Me.ShowInTaskbar = False
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private m_NumSteps As Integer = 1
    Private m_CurStep As Integer = 0
    Private m_StepProgress As Integer = 0

    Private m_ShowFilename As Boolean
    Private m_Filename As String

    Public Property Taskname() As String
        Get
            Return lblTaskName.Text
        End Get
        Set(ByVal Value As String)
            lblTaskName.Text = Value
            lblTaskName.Refresh()
        End Set
    End Property

    Public Property Filename() As String
        Get
            Return m_Filename
        End Get
        Set(ByVal Value As String)
            m_Filename = Value
            If Value = Nothing OrElse Value = "" Then
                m_ShowFilename = False
            Else
                m_ShowFilename = True
                DrawFilename()
            End If
        End Set
    End Property

    Public Property NumSteps() As Integer
        Get
            Return m_NumSteps
        End Get
        Set(ByVal Value As Integer)
            m_NumSteps = Value
            ProgBar.Maximum = m_NumSteps * 100
            UpdateProgress()
        End Set
    End Property

    Public Property CurrentStep() As Integer
        Get
            Return m_CurStep
        End Get
        Set(ByVal Value As Integer)
            m_CurStep = Value
            m_StepProgress = 0
            UpdateProgress()
        End Set
    End Property

    Public Property StepProgress() As Integer
        Get
            Return m_StepProgress
        End Get
        Set(ByVal Value As Integer)
            m_StepProgress = Value
            UpdateProgress()
        End Set
    End Property

    Private Sub ErrorOccured(ByVal KeyOfSender As String, ByVal ErrorMsg As String) Implements MapWinGIS.ICallback.Error
        ' Do nothing
    End Sub

    Private Sub Progress(ByVal KeyOfSender As String, ByVal Percent As Integer, ByVal Message As String) Implements MapWinGIS.ICallback.Progress
        m_StepProgress = Percent
        UpdateProgress()
    End Sub

    Private Sub UpdateProgress()
        Try
            ProgBar.Value = 100 * m_CurStep + m_StepProgress
        Catch
            ' OOPS!
        End Try
    End Sub

    Private Sub DrawFilename()
        Dim g As System.Drawing.Graphics = Me.CreateGraphics()
        Dim sf As New System.Drawing.StringFormat(Drawing.StringFormatFlags.NoWrap)
        sf.Trimming = Drawing.StringTrimming.EllipsisPath
        sf.Alignment = Drawing.StringAlignment.Center
        sf.LineAlignment = Drawing.StringAlignment.Center

        Dim bounds As New Drawing.RectangleF(16, 26, 240, 16)
        g.FillRectangle(New System.Drawing.SolidBrush(Me.BackColor), bounds)
        g.DrawString(m_Filename, Me.Font, New System.Drawing.SolidBrush(Drawing.Color.Black), bounds, sf)
    End Sub
End Class
