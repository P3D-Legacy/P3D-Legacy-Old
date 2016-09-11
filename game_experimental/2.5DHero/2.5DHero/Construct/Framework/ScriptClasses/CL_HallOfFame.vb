Namespace Construct.Framework.Classes

    <ScriptClass("HallOfFame")>
    <ScriptDescription("A class to access the Hall of Fame of the game.")>
    Public Class CL_HallOfFame

        Inherits ScriptClass

        <ScriptCommand("Register")>
        <ScriptDescription("Registers a new Hall Of Fame entry.")>
        Private Function M_Register(ByVal argument As String) As String
            Dim count As Integer = -1

            If Pokemon3D.Core.Player.HallOfFameData <> "" Then
                Dim data() As String = Pokemon3D.Core.Player.HallOfFameData.SplitAtNewline()

                For Each l As String In data
                    Dim id As Integer = CInt(l.Remove(l.IndexOf(",")))
                    If id > count Then
                        count = id
                    End If
                Next
            End If

            count += 1

            Dim time As String = TimeHelpers.GetDisplayTime(TimeHelpers.GetCurrentPlayTime(), True)

            Dim newData As String

            If Pokemon3D.Core.Player.IsGameJoltSave Then
                newData = count & ",(" & Pokemon3D.Core.Player.Name & "|" & time & "|" & GameJoltSave.Points & "|" & Pokemon3D.Core.Player.OT & "|" & Pokemon3D.Core.Player.Skin & ")"
            Else
                newData = count & ",(" & Pokemon3D.Core.Player.Name & "|" & time & "|" & Pokemon3D.Core.Player.Points & "|" & Pokemon3D.Core.Player.OT & "|" & Pokemon3D.Core.Player.Skin & ")"
            End If

            For Each p As Pokemon In Pokemon3D.Core.Player.Pokemons
                If p.IsEgg() = False Then
                    Dim pData As String = p.GetSaveData()
                    newData &= vbNewLine & count & "," & pData
                End If
            Next

            If Pokemon3D.Core.Player.HallOfFameData <> "" Then
                Pokemon3D.Core.Player.HallOfFameData &= vbNewLine
            End If

            Pokemon3D.Core.Player.HallOfFameData &= newData

            Return Core.Null
        End Function

        <ScriptConstruct("Count")>
        <ScriptDescription("Returns the amount of hall of fame entries.")>
        Private Function F_Count(ByVal argument As String) As String
            Return ToString(HallOfFameScreen.GetHallOfFameCount())
        End Function

    End Class

End Namespace