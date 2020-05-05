Imports System.IO
Imports ABWriter.WriterError

Class ABValidator
    Implements IValidator

    '----------------------------------------------------[Variables]----------------------------------------------------

    Private AllErrors As ArrayList
    Private Const HexNumbers As String = "0123456789ABCDEF"
    Private Filedirectory As String = ""

    Public Sub SetFileDirectory(FileDir As String) Implements IValidator.SetFileDirectory
        Filedirectory = FileDir
    End Sub

    Public Function GetFileDirectory() As String Implements IValidator.GetFileDirectory
        Return Filedirectory
    End Function

    '----------------------------------------------------[Other Operations]----------------------------------------------------

    Public Function ValidateScript(Lines As String()) As ArrayList Implements IValidator.ValidateScript

        If IsNothing(AllErrors) Then AllErrors = New ArrayList Else AllErrors.Clear()

        Dim currentline As Integer = 1

        For Each Line As String In Lines
            InternalValidateLine(currentline, Line)
            currentline += 1
        Next

        Return AllErrors
    End Function

    Public Function ValidateLine(LineNumber As Integer, Line As String) As ArrayList Implements IValidator.ValidateLine
        If IsNothing(AllErrors) Then AllErrors = New ArrayList Else AllErrors.Clear()
        Return AllErrors
    End Function

    ''' <summary>Validates a line with a giant array of if-elses, just like Airportboard would</summary>
    Private Sub InternalValidateLine(LineNumber As Integer, Line As String)
        Try
            'attempt to parse it, and if something happens, add an error to allerrors

            'ToUpper it
            Dim UpperLine As String = Line.ToUpper
            Dim currentcommand As String()

            'God this is so many if-elses I really wonder if some better developer will come after me to fix this.
            'I'm so sorry.

            If String.IsNullOrWhiteSpace(Line) Or UpperLine.StartsWith("'") Or UpperLine.StartsWith("SCREENTEST") Or UpperLine.StartsWith("CLEAR") Or UpperLine.StartsWith("PAUSE") Then
                'These all get a pass because iether they are nothing, a comment, or commands that have no arguements

            ElseIf UpperLine.StartsWith("DRAW") Then

                'Draw from file (DRAW FILE LEFT TOP)
                currentcommand = Line.Split(" ")
                If Not currentcommand.Length >= 4 Then
                    AddError(LineNumber, "Draw command does not have the 3 arguements it needs (File, Left, Top)", ErrorType.Critical)
                Else

                    If Not File.Exists(Filedirectory & currentcommand(1)) Then AddError(LineNumber, "File " & Filedirectory & currentcommand(1) & " does not exist", ErrorType.Info)
                    ValidateLeft(LineNumber, currentcommand(2))
                    ValidateTop(LineNumber, currentcommand(3))

                End If

            ElseIf UpperLine.StartsWith("COLOR") Then
                'Set Screenwriter color (COLOR 0F)
                currentcommand = Line.Split(" ")
                If Not currentcommand.Length >= 2 Then
                    AddError(LineNumber, "Color command does not have the arguement it needs (Color (0F))", ErrorType.Critical)
                Else
                    ValidateColor(LineNumber, currentcommand(1))
                End If

            ElseIf UpperLine.StartsWith("TEXT") Then
                'Draw text (TEXT~the text~0F~LEFT~TOP
                currentcommand = Line.Split("~")

                If Not currentcommand.Length >= 5 Then
                    AddError(LineNumber, "Text doesn't have the 4 arguements it needs (Text, Color (0F), Left, Top), or is not separated by '~'", ErrorType.Critical)
                Else
                    ValidateColor(LineNumber, currentcommand(2))
                    Dim Left As Integer = ValidateLeft(LineNumber, currentcommand(3))
                    Dim top As Integer = ValidateTop(LineNumber, currentcommand(4))

                    Dim Length As Integer = currentcommand(1).Length
                    If Left + Length > 79 Then AddError(LineNumber, "Length of this text will run-on to next line with default Airportboard dimensions", ErrorType.Warning)
                    If ((Left + Length) / 79) + top - 1 > 24 Then AddError(LineNumber, "text will cause Airportboard console to scroll with default Airportboard dimensions", ErrorType.Warning)

                End If

            ElseIf UpperLine.StartsWith("RUN") Or UpperLine.StartsWith("NEWSWINDOW") Or UpperLine.StartsWith("INITTICKER") Then
                currentcommand = Line.Split(" ")
                If Not currentcommand.Length >= 2 Then
                    AddError(LineNumber, currentcommand(0) & " command does not have the arguement it needs (File)", ErrorType.Critical)
                Else
                    If Not File.Exists(Filedirectory & currentcommand(1)) Then AddError(LineNumber, "File " & Filedirectory & currentcommand(1) & " does not exist", ErrorType.Info)
                End If

            ElseIf UpperLine.StartsWith("SLEEP") Then
                currentcommand = Line.Split(" ")
                If Not currentcommand.Length >= 2 Then
                    AddError(LineNumber, "Sleep command does not have the arguement it needs (Time)", ErrorType.Critical)
                Else
                    If Not IsInteger(currentcommand(1)) Then AddError(LineNumber, "Sleep's time could not be parsed as an integer", ErrorType.Info)
                End If

            ElseIf UpperLine.StartsWith("BOX") Then
                'Draws a box (BOX F LENGTH HEIGHT LEFT TOP)
                currentcommand = Line.Split(" ")
                If Not currentcommand.Length >= 6 Then
                    AddError(LineNumber, currentcommand(0) & " command does not have the 5 arguements it needs (Single color (F), Length, Height, Left, Top)", ErrorType.Critical)
                Else

                    If Not HexNumbers.Contains(currentcommand(1)) Then AddError(LineNumber, currentcommand(1) & " is not a valid color", ErrorType.Critical)

                    Dim Left As Integer = ValidateLeft(LineNumber, currentcommand(4))
                    Dim Top As Integer = ValidateTop(LineNumber, currentcommand(5))

                    If Not IsInteger(currentcommand(2)) Then
                        AddError(LineNumber, "Length of the box could not be parsed as integer", ErrorType.Critical)
                    ElseIf Not IsInteger(currentcommand(3)) Then
                        AddError(LineNumber, "Height of the box could not be parsed as integer", ErrorType.Critical)
                    Else
                        Dim Length As Integer = currentcommand(2)
                        Dim Height As Integer = currentcommand(3)

                        If Length + Left > 79 Then AddError(LineNumber, "Length of the box is out of bounds on default Airportboard dimensions", ErrorType.Warning)
                        If Top + Height > 24 Then AddError(LineNumber, "Height of the box is out of bounds on default Airportboard dimensions", ErrorType.Warning)
                        If Length + Left = 79 And Height + Top = 24 Then AddError(LineNumber, "Length and height of the box may cause Airportboard console to scroll", ErrorType.Info)

                    End If
                End If


            ElseIf UpperLine.StartsWith("CLOCK") Or UpperLine.StartsWith("DATE") Then
                'Draws a clock at the specified position (CLOCK 0F LEFT TOP)
                currentcommand = Line.Split(" ")

                If Not currentcommand.Length >= 4 Then
                    AddError(LineNumber, currentcommand(0) & " command does not have the 3 arguements it needs (Color (0F), Left, Top)", ErrorType.Critical)
                Else
                    ValidateColor(LineNumber, currentcommand(1))
                    ValidateLeft(LineNumber, currentcommand(2))
                    ValidateTop(LineNumber, currentcommand(3))
                End If

            ElseIf UpperLine.StartsWith("CENTERTEXT") Then
                'Centers text on screen (Centertext~text~row)
                currentcommand = Line.Split("~")
                If Not currentcommand.Length >= 3 Then
                    AddError(LineNumber, currentcommand(0) & " command does not have the 2 arguements it needs (Text, Top), or is not split by '~'", ErrorType.Critical)
                Else
                    If currentcommand(1).Length > 79 Then AddError(LineNumber, "Centertext will be unable to properly render this text with the default Airportboard dimensions", ErrorType.Warning)
                    ValidateTop(LineNumber, currentcommand(2))
                End If

            ElseIf UpperLine.StartsWith("WEATHERWINDOW") Then
                'Draws a WeatherWindow using a WeatherWindow File (WeatherWindow Filename Length Height leftpos Toppos)
                currentcommand = Line.Split(" ")
                If Not currentcommand.Length >= 6 Then
                    AddError(LineNumber, currentcommand(0) & " command does not have the 5 arguements it needs (File, Length (in cells), Height (In cells, Left, Top)", ErrorType.Critical)
                Else

                    If Not File.Exists(Filedirectory & currentcommand(1)) Then AddError(LineNumber, "File " & Filedirectory & currentcommand(1) & " does not exist", ErrorType.Info)

                    Dim Left As Integer = ValidateLeft(LineNumber, currentcommand(4))
                    Dim Top As Integer = ValidateTop(LineNumber, currentcommand(5))

                    If Not IsInteger(currentcommand(2)) Then
                        AddError(LineNumber, "Length of the WWindow could not be parsed as integer", ErrorType.Critical)
                    ElseIf Not IsInteger(currentcommand(3)) Then
                        AddError(LineNumber, "Height of the WWindow could not be parsed as integer", ErrorType.Critical)
                    Else
                        Dim Length As Integer = currentcommand(2) * 30
                        Dim Height As Integer = currentcommand(3) * 5

                        If Length + Left > 79 Then AddError(LineNumber, "Length of the WWindow is out of bounds on default Airportboard dimensions", ErrorType.Warning)
                        If Top + Height > 24 Then AddError(LineNumber, "Height of the WWindow is out of bounds on default Airportboard dimensions", ErrorType.Warning)
                        If Length + Left = 79 And Height + Top = 24 Then AddError(LineNumber, "Length and height of the WWindow may cause Airportboard console to scroll", ErrorType.Info)
                        If Length < 1 Then AddError(LineNumber, "WWindow length must be at least 1", ErrorType.Critical)
                        If Height < 1 Then AddError(LineNumber, "WWindow height must be at least 1", ErrorType.Critical)


                    End If
                End If

            ElseIf UpperLine.StartsWith("FLIGHTWINDOW") Then
                'Draws a FlightWindow using a Flightwindow file (FlightWindow, DepartureMode)
                currentcommand = Line.Split(" ")
                If Not currentcommand.Length >= 3 Then
                    AddError(LineNumber, currentcommand(0) & " command does not have the 2 arguements it needs (File, DepartureMode)", ErrorType.Critical)
                Else
                    If Not File.Exists(Filedirectory & currentcommand(1)) Then AddError(LineNumber, "File " & Filedirectory & currentcommand(1) & " does not exist", ErrorType.Info)
                    If Not IsBoolean(currentcommand(2)) Then AddError(LineNumber, "Could not parse DepartureMode as boolean", ErrorType.Critical)
                End If

            ElseIf UpperLine.StartsWith("TICKER") Then
                '(TICKER Colorstring Length leftpos toppos)
                currentcommand = Line.Split(" ")
                If Not currentcommand.Length >= 5 Then
                    AddError(LineNumber, currentcommand(0) & " command does not have the 4 arguements it needs (Color (0F), Length, Left, Top)", ErrorType.Critical)
                Else
                    ValidateColor(LineNumber, currentcommand(1))
                    Dim Left As Integer = ValidateLeft(LineNumber, currentcommand(3))
                    Dim Top As Integer = ValidateTop(LineNumber, currentcommand(4))

                    If Not IsInteger(currentcommand(2)) Then
                        AddError(LineNumber, "Could not parse Length as integer", ErrorType.Critical)
                    Else
                        Dim Length As Integer = currentcommand(2)
                        If Left + Length > 79 Then AddError(LineNumber, "Length of this Ticker will run-on to next line with default Airportboard dimensions", ErrorType.Warning)
                        If ((Left + Length) / 79) + Top - 1 > 24 Then AddError(LineNumber, "Ticker will cause Airportboard console to scroll with default Airportboard dimensions", ErrorType.Warning)
                    End If
                End If


            Else
                AddError(LineNumber, "Unparsable Line", ErrorType.Critical)
            End If

        Catch ex As Exception
            AddError(LineNumber, "Unknown error while parsing this line: " & ex.Message & vbNewLine & vbNewLine & ex.StackTrace, ErrorType.Critical)

        End Try


    End Sub

    Private Function ValidateColor(Line As Integer, Colorstring As String) As Boolean
        Colorstring = Colorstring.ToUpper()
        If Not Colorstring.Length = 2 Then
            AddError(Line, "Color is not two numbers long", ErrorType.Critical)
            Return False
        ElseIf Not HexNumbers.Contains(Colorstring(0)) Or Not HexNumbers.Contains(Colorstring(1)) Then
            AddError(Line, "Color string unparsable", ErrorType.Critical)
            Return False
        End If

        Return True
    End Function

    Private Function ValidateLeft(Line As Integer, Left As String) As Integer
        If Not IsInteger(Left) Then
            AddError(Line, "Could not parse left coordinate as integer", ErrorType.Critical)
            Return 0
        Else
            Dim I As Integer = Left
            If I < 0 Or I > 79 Then AddError(Line, "Left coordinate is out of bounds for default AirportBoard dimensions", ErrorType.Warning)
            Return I
        End If
    End Function

    Private Function ValidateTop(Line As Integer, Top As String) As Integer
        If Not IsInteger(Top) Then
            AddError(Line, "Could not parse Top coordinate as integer", ErrorType.Critical)
            Return 0
        Else
            Dim I As Integer = Top
            If I < 0 Or I > 24 Then AddError(Line, "Top coordinate is out of bounds for default AirportBoard dimensions", ErrorType.Warning)
            If I = 24 Then AddError(Line, "Top coordinate is 24. This may cause the screen to scroll if the text reaches the edge!", ErrorType.Warning)
            Return I
        End If
    End Function

    Private Function IsInteger(N As String) As Boolean
        Try
            Dim I As Integer = N
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function IsBoolean(N As String) As Boolean
        Try
            Dim I As Boolean = N
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub AddError(Line As Integer, Message As String, Errortype As ErrorType)
        AllErrors.Add(New WriterError(Line, Message, Errortype))
    End Sub


End Class
