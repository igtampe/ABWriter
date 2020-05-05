<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AutoWriter
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"DRAW", " FILE LEFT TOP", "Draws a DF File starting at the Left and Top coordinate"}, -1)
        Dim ListViewItem2 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"COLOR", " 0F", "Sets color of the console writer (Using hexadecimal codes)"}, -1)
        Dim ListViewItem3 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"RUN", " FILE", "Runs another ABScript file"}, -1)
        Dim ListViewItem4 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"SLEEP", "TimeInMS", "Waits the specified time (in MS) before continuing execution"}, -1)
        Dim ListViewItem5 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"BOX", " F LENGTH HEIGHT LEFT TOP", "Draws a box of the specified color of specified length and height at specified Le" &
                "ft and Top coordinate"}, -1)
        Dim ListViewItem6 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"CLOCK", " 0F Left Top", "Draws the current time in the specified color at specified left and top coordinat" &
                "e"}, -1)
        Dim ListViewItem7 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"DATE", " 0F Left Top", "Draws the current date in the specified color at specified left and top coordinat" &
                "e"}, -1)
        Dim ListViewItem8 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"TEXT", "~text~0F~Left~Top", "Draws the specified text in the specified color at specified left and top coordin" &
                "ate"}, -1)
        Dim ListViewItem9 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"CENTERTEXT", "~text~top", "Centers the text at the specified top coordinate"}, -1)
        Dim ListViewItem10 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"WEATHERWINDOW", " FILE COLUMNS ROWS LEFT TOP", "Draws a WeatherWindow from the specified file with specified columns and rows at " &
                "specified Left and Top coordinate"}, -1)
        Dim ListViewItem11 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"NEWSWINDOW", " File", "Draws a NewsWindow from the specified file"}, -1)
        Dim ListViewItem12 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"FLIGHTWINDOW", " File DepartureMode", "Draws a FlightWindow from the specified file as Arrivals (Departuremode false) or" &
                " Departures (Departuremode true)"}, -1)
        Dim ListViewItem13 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"INITTICKER", " File", " Initializes the AirportBoard Ticker from the specified text file"}, -1)
        Dim ListViewItem14 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"TICKER", " 0F LENGTH LEFT TOP", "Draws a previously initialized ticker of specified length in specified color at s" &
                "pecified left and top coordinate"}, -1)
        Me.LibBox = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.OKBTN = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'LibBox
        '
        Me.LibBox.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.LibBox.FullRowSelect = True
        Me.LibBox.HideSelection = False
        Me.LibBox.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1, ListViewItem2, ListViewItem3, ListViewItem4, ListViewItem5, ListViewItem6, ListViewItem7, ListViewItem8, ListViewItem9, ListViewItem10, ListViewItem11, ListViewItem12, ListViewItem13, ListViewItem14})
        Me.LibBox.Location = New System.Drawing.Point(12, 12)
        Me.LibBox.Name = "LibBox"
        Me.LibBox.Size = New System.Drawing.Size(899, 273)
        Me.LibBox.TabIndex = 0
        Me.LibBox.UseCompatibleStateImageBehavior = False
        Me.LibBox.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Command"
        Me.ColumnHeader1.Width = 113
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Arguements"
        Me.ColumnHeader2.Width = 189
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Description"
        Me.ColumnHeader3.Width = 574
        '
        'OKBTN
        '
        Me.OKBTN.Location = New System.Drawing.Point(836, 291)
        Me.OKBTN.Name = "OKBTN"
        Me.OKBTN.Size = New System.Drawing.Size(75, 23)
        Me.OKBTN.TabIndex = 1
        Me.OKBTN.Text = "OK"
        Me.OKBTN.UseVisualStyleBackColor = True
        '
        'AutoWriter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(923, 326)
        Me.Controls.Add(Me.OKBTN)
        Me.Controls.Add(Me.LibBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "AutoWriter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Function Library"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LibBox As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents OKBTN As Button
End Class
