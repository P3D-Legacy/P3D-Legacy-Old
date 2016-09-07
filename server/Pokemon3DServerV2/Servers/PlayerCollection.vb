Namespace Servers

    ''' <summary>
    ''' Contains all connected players.
    ''' </summary>
    ''' <remarks>Inherits from List(of )</remarks>
    Public Class PlayerCollection

        Inherits System.Collections.Generic.List(Of Player)

        ''' <summary>
        ''' Removes all players from the collection that have the specified name.
        ''' </summary>
        ''' <param name="Name"></param>
        ''' <remarks></remarks>
        Public Sub RemoveByName(ByVal Name As String)
            For i = 0 To Me.Count - 1
                If i <= Me.Count - 1 Then
                    If Me(i).Name = Name Then
                        Me.RemoveAt(i)
                        i -= 1
                    End If
                Else
                    Exit For
                End If
            Next
        End Sub

        Public Sub RemoveByID(ByVal ID As Integer)
            For i = 0 To Me.Count - 1
                If i <= Me.Count - 1 Then
                    If Me(i).ServersID = ID Then
                        Me.RemoveAt(i)
                        i -= 1
                    End If
                Else
                    Exit For
                End If
            Next
        End Sub

        Public Function HasPlayer(ByVal ID As Integer) As Boolean
            For i = 0 To Me.Count - 1
                If i <= Me.Count - 1 Then
                    If Me(i).ServersID = ID Then
                        Return True
                    End If
                End If
            Next
            Return False
        End Function

        Public Function HasPlayer(ByVal Name As String) As Boolean
            For i = 0 To Me.Count - 1
                If i <= Me.Count - 1 Then
                    If Me(i).Name.ToLower() = Name.ToLower() Then
                        Return True
                    End If
                End If
            Next
            Return False
        End Function

        Public Function GetPlayer(ByVal ID As Integer) As Player
            For Each p As Player In Me
                If p.ServersID = ID Then
                    Return p
                End If
            Next
            Return Nothing
        End Function

        Public Function GetPlayer(ByVal Name As String) As Player
            For i = 0 To Me.Count - 1
                If i <= Me.Count - 1 Then
                    If Me(i).Name = Name Then
                        Return Me(i)
                    End If
                End If
            Next
            Return Nothing
        End Function

        Public Function GetPlayerName(ByVal ID As Integer) As String
            If ID = -1 Then
                Return "[SERVER]"
            End If

            For i = 0 To Me.Count - 1
                If i <= Me.Count - 1 Then
                    If Me(i).ServersID = ID Then
                        Return Me(i).Name
                    End If
                End If
            Next

            Return ""
        End Function

        Public Function GetPlayerID(ByVal Name As String) As Integer
            For i = 0 To Me.Count - 1
                If i <= Me.Count - 1 Then
                    If Me(i).Name.ToLower() = Name.ToLower() Then
                        Return Me(i).ServersID
                    End If
                End If
            Next

            Return -1
        End Function

        Public Sub ApplyPlayerDataPackage(ByVal p As Package)
            If p.DataItems.Count = Player.PLAYERDATAITEMSCOUNT Then
                If p.Origin <> -1 Then
                    Dim targetPlayer = GetPlayer(p.Origin)

                    If Not targetPlayer Is Nothing Then
                        targetPlayer.ApplyNewData(p)
                    End If
                End If
            End If
        End Sub

        Public ReadOnly Property NextFreePlayerID() As Integer
            Get
                If Me.Count = 0 Then
                    Return 1
                Else
                    Dim ID As Integer = 1
                    Dim foundID As Boolean = False

                    While foundID = False
                        foundID = True

                        For i = 0 To Me.Count - 1
                            If i <= Me.Count - 1 Then
                                Dim p As Player = Me(i)
                                If p.ServersID = ID Then
                                    ID += 1
                                    foundID = False
                                    Exit For
                                End If
                            End If
                        Next
                    End While

                    Return ID
                End If
            End Get
        End Property

    End Class

End Namespace