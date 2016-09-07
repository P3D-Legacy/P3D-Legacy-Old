Imports System.Data.SqlClient

Public Class CrashlogHandler

    Public Shared Sub SendCrashlog(ByVal parameter As Object)
        With My.Computer.Clock.LocalTime
            Dim sendDate As String = .Year & "-" & .Month & "-" & .Day & " " & .Hour & ":" & .Minute & ":" & .Second & "." & .Millisecond

            Dim sqlConn As New SqlConnection()
            sqlConn.ConnectionString = "" ' CLASSIFIED

            sqlConn.Open()

            Dim query As String = "" ' CLASSIFIED

            Dim command As SqlCommand = New SqlCommand(query, sqlConn)
            command.Parameters.AddWithValue("@crashlogdata", CStr(parameter))

            Dim reader = command.ExecuteReader()

            sqlConn.Close()
        End With
    End Sub

End Class