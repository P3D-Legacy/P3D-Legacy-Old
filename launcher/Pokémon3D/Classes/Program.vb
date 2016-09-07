Module Program

    Public Const PROGRAMNAME As String = "Pokémon3D Launcher"
    Public Const VERSION As String = "2.3.0.0"

    Sub Main(ByVal args As String())
        Try
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            Application.Run(New Form1)
        Catch ex As Exception
            Logger.Log(Logger.LogTypes.ErrorMessage, "The program crashed with error ID: -1 (" & ex.Message & ")")
            Logger.LogCrash(ex)
        End Try
    End Sub

    Public ReadOnly Property DecSeparator As String
        Get
            Return My.Application.Culture.NumberFormat.NumberDecimalSeparator
        End Get
    End Property

End Module