Public Interface IValidator

    ''' <summary>Validates an entire script</summary>
    Function ValidateScript(Lines As String()) As ArrayList

    ''' <summary>Validates just a specific line</summary>
    Function ValidateLine(LineNumber As Integer, Line As String) As ArrayList

    ''' <summary>Sets the file directory to verify the existence of nearby files</summary>
    Sub SetFileDirectory(FileDirectory As String)

    ''' <summary>Retrieves the stored file directory</summary>
    Function GetFileDirectory() As String

End Interface
