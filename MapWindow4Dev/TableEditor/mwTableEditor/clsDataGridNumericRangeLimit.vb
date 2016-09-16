Imports System.Windows.Forms
Imports System.Drawing

Public Class DataGridNumericRangeLimit
    Inherits DataGridTextBoxColumn
    Private _currencyManager As System.Windows.Forms.CurrencyManager
    Private _Editing As Boolean = False
    Public MinRange As Double = Double.MinValue
    Public MaxRange As Double = Double.MaxValue

    Public Sub New()
        _currencyManager = Nothing
        AddHandler Me.TextBox.TextChanged, AddressOf MarkEdited
    End Sub

    Private Sub MarkEdited(ByVal sender As Object, ByVal e As EventArgs)
        _Editing = True
    End Sub

    Protected Overloads Overrides Sub Edit(ByVal source As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)
        MyBase.Edit(source, rowNum, bounds, [readOnly], instantText, cellIsVisible)
    End Sub

    Protected Overloads Overrides Function Commit(ByVal dataSource As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer) As Boolean
        If Not _Editing Then Return True
        _Editing = False

        Dim val As Double = 0
        If Not Double.TryParse(Me.TextBox.Text, val) Then
            MsgBox("Please enter a valid number.", MsgBoxStyle.Information, "Enter a Valid Number")
            Return False
        Else
            If val < MinRange Then
                MsgBox("Please enter a number greater than " + MinRange.ToString() + ".", MsgBoxStyle.Information, "Value is Too Small")
                Return False
            ElseIf val > MaxRange Then
                MsgBox("Please enter a number less than " + MaxRange.ToString() + ".", MsgBoxStyle.Information, "Value is Too Large")
                Return False
            Else
                SetColumnValueAtRow(dataSource, rowNum, TextBox.Text)
                Return True
            End If
        End If
        Return True
    End Function
End Class
