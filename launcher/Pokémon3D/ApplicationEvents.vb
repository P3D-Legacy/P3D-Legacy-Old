Namespace My
    Partial Friend Class MyApplication
        Private WithEvents MyDomain As AppDomain = AppDomain.CurrentDomain
        Private Function MyDomain_AssemblyResolve(ByVal sender As Object, ByVal args As System.ResolveEventArgs) As System.Reflection.Assembly Handles MyDomain.AssemblyResolve
            If args.Name.Contains("Zimpler") Then
                Return System.Reflection.Assembly.Load(My.Resources.Zimpler)
            Else
                Return Nothing
            End If
        End Function
    End Class
End Namespace