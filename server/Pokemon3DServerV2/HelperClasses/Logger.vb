Public Class Logger

#Region "Fields and Enums"

    Dim InstanceLogName As String = ""
    Dim CurrentLogContent As String = ""

#End Region

#Region "Constructors"

    Public Sub New()
        With My.Computer.Clock.LocalTime
            Dim month As String = .Month.ToString()
            If month.Length = 1 Then
                month = "0" & month
            End If
            Dim day As String = .Day.ToString()
            If day.Length = 1 Then
                day = "0" & day
            End If
            Dim hour As String = .Hour.ToString()
            If hour.Length = 1 Then
                hour = "0" & hour
            End If
            Dim minute As String = .Minute.ToString()
            If minute.Length = 1 Then
                minute = "0" & minute
            End If
            Dim second As String = .Second.ToString()
            If second.Length = 1 Then
                second = "0" & second
            End If

            Me.InstanceLogName = "Log_" & .Year & "-" & month & "-" & day & "_" & hour & "." & minute & "." & second & ".dat"
        End With
    End Sub

#End Region

#Region "Methods"

    Public Sub LogLine(ByVal Line As String)
        Me.CreateLogEnvironment()

        If Me.CurrentLogContent <> "" Then
            Me.CurrentLogContent &= vbNewLine
        End If

        With My.Computer.Clock.LocalTime
            Dim Hour As String = .Hour.ToString()
            If Hour.Length = 1 Then
                Hour = "0" & Hour
            End If
            Dim minute As String = .Minute.ToString()
            If minute.Length = 1 Then
                minute = "0" & minute
            End If
            Dim second As String = .Second.ToString()
            If second.Length = 1 Then
                second = "0" & second
            End If

            CurrentLogContent &= "[" & Hour & ":" & minute & ":" & second & "] " & Line
        End With

        Try
            System.IO.File.WriteAllText(My.Application.Info.DirectoryPath & "\logs\" & Me.InstanceLogName, CurrentLogContent)
        Catch : End Try
    End Sub

    Private Sub CreateLogEnvironment()
        If System.IO.Directory.Exists(My.Application.Info.DirectoryPath & "\logs") = False Then
            System.IO.Directory.CreateDirectory(My.Application.Info.DirectoryPath & "\logs")
        End If
        If System.IO.File.Exists(My.Application.Info.DirectoryPath & "\logs\" & Me.InstanceLogName) = False Then
            System.IO.File.WriteAllText(My.Application.Info.DirectoryPath & "\logs\" & Me.InstanceLogName, "")
            Me.CurrentLogContent = ""
        End If
    End Sub

#End Region

End Class