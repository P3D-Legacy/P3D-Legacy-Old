Public Class ApplicationArguments

    ''' <summary>
    ''' If the server should output API content.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property APIOutput() As Boolean
        Get
            For Each a As String In My.Application.CommandLineArgs.ToList()
                If a.ToLower() = "apioutput" Then
                    Return True
                End If
            Next
            Return False
        End Get
    End Property

End Class