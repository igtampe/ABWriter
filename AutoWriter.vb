Public Class AutoWriter

    Public ReturnString As String = ""

    Private Sub Showtime() Handles Me.Shown
        LibBox.Select()
    End Sub

    Private Sub ClickOKtoOK() Handles OKBTN.Click, LibBox.DoubleClick
        Try
            ReturnString = LibBox.SelectedItems(0).Text
        Catch
        End Try
        Close()
    End Sub

    Private Sub LibBox_KeyDown(sender As Object, e As KeyEventArgs) Handles LibBox.KeyUp
        If e.KeyCode = Keys.Enter Then
            ClickOKtoOK()
        ElseIf e.KeyCode = Keys.Escape Then
            Close()
        End If
    End Sub
End Class