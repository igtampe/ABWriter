Public Class WriterError

    Public ReadOnly Message As String
    Public ReadOnly Line As Integer
    Public ReadOnly Type As ErrorType

    Public Enum ErrorType As Integer
        Info = 0
        Warning = 1
        Critical = 2
    End Enum


    Public Sub New(Line As Integer, Message As String, Type As ErrorType)
        Me.Line = Line
        Me.Message = Message
        Me.Type = Type
    End Sub

    Public Overrides Function ToString() As String
        Dim Prefix As String
        Select Case Type
            Case ErrorType.Info
                Prefix = "INFO"
            Case ErrorType.Warning
                Prefix = "WARN"
            Case ErrorType.Critical
                Prefix = "CRIT"
            Case Else
                Prefix = "UNKW"
        End Select

        Return "(" & Line & ") " & Prefix & ": " & Message

    End Function

End Class
