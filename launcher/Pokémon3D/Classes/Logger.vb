Public Class Logger

    Public Enum LogTypes
        Message
        Debug
        ErrorMessage
        Warning
        Entry
    End Enum

    Public Shared History As New List(Of String)
    Const LOGVERSION As String = "1.3"

    Public Shared Sub Log(ByVal LogType As LogTypes, ByVal Message As String)
        Try
            Dim currentTime As String = ""
            With My.Computer.Clock.LocalTime
                currentTime = .Hour & ":" & .Minute & ":" & .Second
            End With

            Dim LogString As String
            If LogType = LogTypes.Entry Then
                LogString = "]" & Message
            Else
                LogString = LogType.ToString() & " (" & currentTime & "): " & Message
            End If

            Debug.Print("Logger: " & LogString)

            Dim Log As String = ""

            If System.IO.File.Exists(My.Application.Info.DirectoryPath & "\log.dat") = True Then
                Log = System.IO.File.ReadAllText(My.Application.Info.DirectoryPath & "\log.dat")
            End If

            If Log = "" Then
                Log = LogString
            Else
                Log &= vbNewLine & LogString
            End If

            System.IO.File.WriteAllText(My.Application.Info.DirectoryPath & "\log.dat", Log)
            History.Add(LogString)
        Catch ex As Exception : End Try
    End Sub

    Public Shared Sub LogCrash(ByVal ex As Exception)
        Try
            Dim logName As String = ""
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
                logName = .Year & "-" & month & "-" & day & "_" & hour & "." & minute & "." & second & "_crash.dat"
            End With

            Dim specs As String = Program.PROGRAMNAME & " version: " & Program.VERSION & vbNewLine &
                "Operating system: " & My.Computer.Info.OSFullName & " [" & My.Computer.Info.OSVersion & "]" & vbNewLine &
                "System time: " & My.Computer.Clock.LocalTime.ToString() & vbNewLine &
                "System language: " & System.Globalization.CultureInfo.CurrentCulture.EnglishName & "(" & System.Globalization.CultureInfo.CurrentCulture.ThreeLetterWindowsLanguageName & ")" & vbNewLine &
                "Decimal seperator: " & Program.DecSeparator

            Dim innerException As String = "NOTHING"
            If Not ex.InnerException Is Nothing Then
                innerException = ex.InnerException.Message
            End If
            Dim message As String = "NOTHING"
            If Not ex.Message Is Nothing Then
                message = ex.Message
            End If
            Dim source As String = "NOTHING"
            If Not ex.Source Is Nothing Then
                source = ex.Source
            End If
            Dim StackTrace As String = "NOTHING"
            If Not ex.StackTrace Is Nothing Then
                StackTrace = ex.StackTrace
            End If
            Dim targetSite As String = "NOTHING"
            If Not ex.TargetSite Is Nothing Then
                targetSite = "Name: " & ex.TargetSite.Name & " [" &
                    vbNewLine & "   Attributes: " & ex.TargetSite.Attributes.ToString() &
                    vbNewLine & "   CallingConvention: " & ex.TargetSite.CallingConvention.ToString() &
                    vbNewLine & "   ContainsGenericParameters: " & ex.TargetSite.ContainsGenericParameters &
                    vbNewLine & "   DeclaringType: " & ex.TargetSite.DeclaringType.ToString() &
                    vbNewLine & "   IsAbstract: " & ex.TargetSite.IsAbstract &
                    vbNewLine & "   IsAssembly: " & ex.TargetSite.IsAssembly &
                    vbNewLine & "   IsConstructor: " & ex.TargetSite.IsConstructor &
                    vbNewLine & "   IsFamily: " & ex.TargetSite.IsFamily &
                    vbNewLine & "   IsFamilyAndAssembly: " & ex.TargetSite.IsFamilyAndAssembly &
                    vbNewLine & "   IsFamilyOrAssembly: " & ex.TargetSite.IsFamilyOrAssembly &
                    vbNewLine & "   IsFinal: " & ex.TargetSite.IsFinal &
                    vbNewLine & "   IsGenericMethod: " & ex.TargetSite.IsGenericMethod &
                    vbNewLine & "   IsGenericMethodDefinition: " & ex.TargetSite.IsGenericMethodDefinition &
                    vbNewLine & "   IsHideBySig: " & ex.TargetSite.IsHideBySig &
                    vbNewLine & "   IsPrivate: " & ex.TargetSite.IsPrivate &
                    vbNewLine & "   IsPublic: " & ex.TargetSite.IsPublic &
                    vbNewLine & "   IsSecurityCritical: " & ex.TargetSite.IsSecurityCritical &
                    vbNewLine & "   IsSecuritySafeCritical: " & ex.TargetSite.IsSecuritySafeCritical &
                    vbNewLine & "   IsSecurityTransparent: " & ex.TargetSite.IsSecurityTransparent &
                    vbNewLine & "   IsSpecialName: " & ex.TargetSite.IsSpecialName &
                    vbNewLine & "   IsStatic: " & ex.TargetSite.IsStatic &
                    vbNewLine & "   IsVirtual: " & ex.TargetSite.IsVirtual &
                    vbNewLine & "   MemberType: " & ex.TargetSite.MemberType.ToString() &
                    vbNewLine & "   MetadataToken: " & ex.TargetSite.MetadataToken.ToString() &
                    vbNewLine & "   MethodHandle: " & ex.TargetSite.MethodHandle.ToString() &
                    vbNewLine & "   Module: " & ex.TargetSite.Module.ToString() &
                    vbNewLine & "   ReflectedType: " & ex.TargetSite.ReflectedType.ToString() &
                    vbNewLine & "   ]" & vbNewLine
            End If

            Dim helpLink As String = "No helplink available."
            If Not ex.HelpLink Is Nothing Then
                helpLink = ex.HelpLink
            End If

            Dim BaseException As Exception = ex.GetBaseException()

            Dim data As String = "NOTHING"
            If Not ex.Data Is Nothing Then
                data = "Items: " & ex.Data.Count
                If ex.Data.Count > 0 Then
                    data = ""
                    For i = 0 To ex.Data.Count - 1
                        If data <> "" Then
                            data &= vbNewLine
                        End If
                        data &= "[" & ex.Data.Keys(i).ToString() & ": """ & ex.Data.Values(i).ToString() & """]"
                    Next
                End If
            End If

            Dim content As String = "Kolben Games Crash Log V " & LOGVERSION & vbNewLine &
                            Program.PROGRAMNAME & " has crashed!" & vbNewLine & "---------------------------" & vbNewLine & vbNewLine &
                            "System specifications: " & vbNewLine & specs & vbNewLine & "---------------------------" & vbNewLine & vbNewLine &
                            "Here is further information:" &
                           vbNewLine & "Message: " & message &
                           vbNewLine & "InnerException: " & innerException &
                           vbNewLine & "BaseException: " & BaseException.Message &
                           vbNewLine & "HelpLink: " & helpLink &
                           vbNewLine & "Data: " & data &
                           vbNewLine & "Source: " & source &
                           vbNewLine & "TargetSite: " & targetSite &
                           vbNewLine & "CallStack: " & StackTrace &
                           vbNewLine & vbNewLine & "You should report this error." & vbNewLine & vbNewLine & "Go to ""http://pokemon3d.net/forum/forums/6/create-thread"" to report this crash there."

            System.IO.File.WriteAllText(My.Application.Info.DirectoryPath & "\" & LogName, content)

            MsgBox(Program.PROGRAMNAME & " has crashed!" & vbNewLine & "---------------------------" & vbNewLine & vbNewLine & "Here is further information:" &
                           vbNewLine & "Message: " & ex.Message &
                           vbNewLine & vbNewLine & "You should report this error. When you do this, please attach the crash log to the report. You can find the file in your ""Pokemon"" folder." & vbNewLine & vbNewLine & "The name of the file is: """ & logName & """.", MsgBoxStyle.Critical, "Pokémon3D Launcher crashed!")

            Process.Start("explorer.exe", "/select,""" & My.Application.Info.DirectoryPath & "\" & logName & """")
        Catch : End Try
    End Sub

End Class