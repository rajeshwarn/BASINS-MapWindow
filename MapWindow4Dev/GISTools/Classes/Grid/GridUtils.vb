Imports System.Threading

Public Class GridUtils
    Private m_Ready As Boolean
    Private m_Grid As MapWinGIS.Grid
    Private m_Thread As Thread

    Private Sub InitThread()
        m_Grid = New MapWinGIS.Grid()
        m_Ready = True
    End Sub

    Private Sub StartThread()
        'Due to funny conflicts with ESRI's grid stuff, we have to create the grid in a new thread 
        m_Ready = False
        m_Thread = New Thread(New ThreadStart(AddressOf InitThread))
        m_Thread.SetApartmentState(ApartmentState.STA)
        m_Thread.Start()

        While (m_Ready = False)
            System.Threading.Thread.Sleep(50)
        End While
    End Sub

    Private Sub StopThread()
        m_Thread.Abort()
        m_Thread = Nothing
    End Sub

    <CLSCompliant(False)> _
    Public Function CreateSafeGrid() As MapWinGIS.Grid
        Try
            StartThread()
            StopThread()
            'mapwinutility.logger.msg(m_Thread.ThreadState.ToString())

        Catch ex As System.Exception

        End Try
        Return m_Grid
    End Function

    Public Function GridCdlgFilter() As String
        StartThread()
        StopThread()
        Return m_Grid.CdlgFilter
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
