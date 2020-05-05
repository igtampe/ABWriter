Imports System.IO

Public Class MainForm

    '----------------------------------------------------[Variables]----------------------------------------------------

    ''' <summary>MyValidator is here, so that in the future maybe we can implement validators for WWfiles and other cositas</summary>
    Private MyValidator As IValidator
    Private Modified As Boolean
    Private Filename As String

    '----------------------------------------------------[Initialization]----------------------------------------------------

    Public Sub New()
        InitializeComponent()
        MyValidator = New ABValidator()
        Modified = False
    End Sub

    '----------------------------------------------------[File Operations]----------------------------------------------------

    Private Sub NewFile() Handles NewToolStripMenuItem.Click
        If Modified Then
            Dim result As MsgBoxResult = MsgBox("This file has been modified! Would you want to save?", MsgBoxStyle.Question + MsgBoxStyle.YesNoCancel)
            Select Case result
                Case MsgBoxResult.Yes
                    If Not Save() Then Return
                Case MsgBoxResult.No
                    'Don't save, create a new file
                Case MsgBoxResult.Cancel
                    Return
            End Select
        End If

        Text = "ABWriter - New File"
        MyValidator.SetFileDirectory("")

        Filename = ""
        MainBox.Text = ""
        AllErrorsBox.Text = ""
        Modified = False
    End Sub

    Private Sub OpenFile()
        If String.IsNullOrWhiteSpace(Filename) Then
            OpenFile()
            Return
        End If

        MainBox.Text = ""
        AllErrorsBox.Text = ""

        Dim JustTheFilename As String = Filename.Split("\")(Filename.Split("\").Length - 1)
        Text = "ABWriter - " & JustTheFilename
        MyValidator.SetFileDirectory(Filename.Substring(0, Filename.Length - JustTheFilename.Length))

        FileOpen(1, Filename, OpenMode.Input)
        While Not EOF(1)
            MainBox.AppendText(LineInput(1))
            MainBox.AppendText(vbLf)
        End While
        FileClose(1)

        Modified = False
        Text = Text.TrimEnd("*")
    End Sub

    Private Function Save() As Boolean Handles SaveToolStripMenuItem.Click
        If String.IsNullOrWhiteSpace(Filename) Then
            Return SaveAs()
        End If

        FileOpen(1, Filename, OpenMode.Output)
        For Each Line As String In MainBox.Lines
            PrintLine(1, Line)
        Next
        FileClose(1)

        Dim JustTheFilename As String = Filename.Split("\")(Filename.Split("\").Length - 1)
        Text = "ABWriter - " & JustTheFilename
        MyValidator.SetFileDirectory(Filename.Substring(0, Filename.Length - JustTheFilename.Length))

        Modified = False
        Text = Text.TrimEnd("*")

        Return True
    End Function

    Private Function SaveAs() As Boolean Handles SaveAsToolStripMenuItem.Click
        Dim Result As DialogResult = TheSaver.ShowDialog()
        If Result = DialogResult.OK Then
            Filename = TheSaver.FileName
            Return Save()
        End If

        Return False
    End Function

    Private Sub Open() Handles OpenToolStripMenuItem.Click
        If Modified Then
            Dim result1 As MsgBoxResult = MsgBox("This file has been modified! Would you want to save?", MsgBoxStyle.Question + MsgBoxStyle.YesNoCancel)
            Select Case result1
                Case MsgBoxResult.Yes
                    'if we manage to save the file, continue
                    If Not Save() Then Return
                Case MsgBoxResult.No
                    'Don't save, open the file
                Case MsgBoxResult.Cancel
                    'Do nothing
                    Return
            End Select
        End If

        Dim Result2 As DialogResult = TheOpener.ShowDialog()
        If Result2 = DialogResult.OK Then
            Filename = TheOpener.FileName
            OpenFile()
        End If
    End Sub

    Private Sub Quit() Handles ExitToolStripMenuItem.Click
        Close()
    End Sub

    '----------------------------------------------------[Show Windows]----------------------------------------------------

    Private Sub Preview() Handles PreviewToolStripMenuItem.Click
        If Not File.Exists(MyValidator.GetFileDirectory & "Airportboard.exe") Then
            MsgBox("Could not find Airportboard at this file's directory!" & vbNewLine & vbNewLine & "'" & MyValidator.GetFileDirectory & "'" & vbNewLine & vbNewLine & "Make sure Airportboard is in this directory to be able to preview this file correctly!", MsgBoxStyle.Critical)
            Return
        End If

        Dim PSI As ProcessStartInfo = New ProcessStartInfo(MyValidator.GetFileDirectory & "Airportboard.exe", Filename) With {.WorkingDirectory = MyValidator.GetFileDirectory()}
        Process.Start(PSI)

    End Sub

    Private Sub AutoWrite() Handles FunctionHelperMenuItem.Click
        'attempt to parse it, and if something happens, add an error to allerrors
        SelectCurrentLine()

        'ToUpper it
        Dim UpperLine As String = MainBox.SelectedText.ToUpper

        'God this is so many if-elses I really wonder if some better developer will come after me to fix this.
        'I'm so sorry.

        If String.IsNullOrWhiteSpace(UpperLine) Then
            Dim AW As AutoWriter = New AutoWriter()
            AW.ShowDialog()
            UpperLine = AW.ReturnString
        End If

        If UpperLine.StartsWith("DRAW") Then
            'Draw from file (DRAW FILE LEFT TOP)
            MainBox.SelectedText = "DRAW FILE LEFT TOP"

        ElseIf UpperLine.StartsWith("COLOR") Then
            'Set Screenwriter color (COLOR 0F)
            MainBox.SelectedText = "COLOR 0F"

        ElseIf UpperLine.StartsWith("TEXT") Then
            'Draw text (TEXT~the text~0F~LEFT~TOP)
            MainBox.SelectedText = "TEXT~the text~0F~LEFT~TOP"

        ElseIf UpperLine.StartsWith("RUN") Or UpperLine.StartsWith("NEWSWINDOW") Or UpperLine.StartsWith("INITTICKER") Or UpperLine.StartsWith("NW") Or UpperLine.StartsWith("IT") Then
            'X FILE
            MainBox.SelectedText = UpperLine.Trim(" ").Replace("NW", "NewsWindow").Replace("IT", "INITTICKER") & " FILE"

        ElseIf UpperLine.StartsWith("SLEEP") Then
            'SLEEP 500
            MainBox.SelectedText = "SLEEP 1000"

        ElseIf UpperLine.StartsWith("BOX") Then
            'Draws a box (BOX F LENGTH HEIGHT LEFT TOP)
            MainBox.SelectedText = "BOX F LENGTH HEIGHT LEFT TOP"

        ElseIf UpperLine.StartsWith("CLOCK") Or UpperLine.StartsWith("DATE") Then
            'Draws a clock at the specified position (CLOCK 0F LEFT TOP)
            MainBox.SelectedText = UpperLine.Trim(" ") & " 0F LEFT TOP"

        ElseIf UpperLine.StartsWith("CENTERTEXT") Or UpperLine.StartsWith("CT") Then
            'Centers text on screen (Centertext~text~row)
            MainBox.SelectedText = "CENTERTEXT~TEXT~TOP"

        ElseIf UpperLine.StartsWith("WEATHERWINDOW") Or UpperLine.StartsWith("WW") Then
            'Draws a WeatherWindow using a WeatherWindow File (WeatherWindow Filename Length Height leftpos Toppos)
            MainBox.SelectedText = "WEATHERWINDOW FILE COLUMNS ROWS LEFT TOP"

        ElseIf UpperLine.StartsWith("FLIGHTWINDOW") Or UpperLine.StartsWith("FW") Then
            'Draws a FlightWindow using a Flightwindow file (FlightWindow, DepartureMode)
            MainBox.SelectedText = "FLIGHTWINDOW FILE DEPARTUREMODE"

        ElseIf UpperLine.StartsWith("TICKER") Then
            '(TICKER Colorstring Length leftpos toppos)
            MainBox.SelectedText = "TICKER 0F LENGTH LEFT TOP"

        End If

    End Sub

    Private Sub ShowAbout() Handles AboutToolStripMenuItem.Click
        About.ShowDialog()
    End Sub

    '----------------------------------------------------[The Textbox]----------------------------------------------------

    Private Sub MainBoxModified() Handles MainBox.TextChanged
        If Not Modified Then
            Modified = True
            Text &= "*"
        End If

        ValidateMyself()
    End Sub

    '----------------------------------------------------[Other Operations]----------------------------------------------------

    Private Sub SelectCurrentLine()

        If MainBox.Lines.Length = 0 Then Return

        Dim CurrentCursorPosition As Integer = MainBox.SelectionStart
        Dim CurrentLine As Integer = MainBox.GetLineFromCharIndex(CurrentCursorPosition) + 1
        MainBox.Select(MainBox.Text.Substring(0, CurrentCursorPosition).LastIndexOf(vbLf) + 1, MainBox.Lines(CurrentLine - 1).Length)
    End Sub

    Private Sub ValidateMyself()

        If MainBox.Lines.Length = 0 Then Return

        Dim CurrentCursorPosition As Integer = MainBox.SelectionStart
        Dim CurrentLine As Integer = MainBox.GetLineFromCharIndex(CurrentCursorPosition) + 1
        MainBox.Select(MainBox.Text.Substring(0, CurrentCursorPosition).LastIndexOf(vbLf) + 1, MainBox.Lines(CurrentLine - 1).Length)

        If MainBox.Lines(CurrentLine - 1).StartsWith("'") Then
            MainBox.SelectionColor = Color.Green
        Else
            MainBox.SelectionColor = Color.Black
        End If

        AllErrorsBox.Text = ""
        AllErrorsBox.SelectionBackColor = Color.Black
        AllErrorsBox.SelectionColor = Color.White
        AllErrorsBox.SelectionAlignment = HorizontalAlignment.Center
        AllErrorsBox.AppendText("Errors and Information:" & vbNewLine)
        AllErrorsBox.SelectionAlignment = HorizontalAlignment.Left



        For Each Err As WriterError In MyValidator.ValidateScript(MainBox.Text.Split(vbLf))

            Select Case Err.Type
                Case WriterError.ErrorType.Info
                    AllErrorsBox.SelectionBackColor = Color.Blue
                    AllErrorsBox.SelectionColor = Color.White

                    If Err.Line = CurrentLine And MainBox.SelectionColor = Color.Black Then MainBox.SelectionColor = Color.Blue

                Case WriterError.ErrorType.Warning
                    AllErrorsBox.SelectionBackColor = Color.Yellow
                    AllErrorsBox.SelectionColor = Color.Black

                    If Err.Line = CurrentLine And Not MainBox.SelectionColor = Color.Red Then MainBox.SelectionColor = Color.Orange

                Case WriterError.ErrorType.Critical
                    AllErrorsBox.SelectionBackColor = Color.Red
                    AllErrorsBox.SelectionColor = Color.White

                    If Err.Line = CurrentLine Then MainBox.SelectionColor = Color.Red

            End Select

            AllErrorsBox.AppendText(vbNewLine & Err.ToString)

        Next

        MainBox.Select(CurrentCursorPosition, 0)

    End Sub

End Class
