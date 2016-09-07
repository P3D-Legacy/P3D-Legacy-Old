Namespace Servers

    ''' <summary>
    ''' A class to handle command inputs.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ServerCommands

#Region "Enums"

#End Region

#Region "Fields and Constants"

#End Region

#Region "Properties"

#End Region

#Region "Delegates"

#End Region

#Region "Constructors"

#End Region

#Region "Methods"

        ''' <summary>
        ''' Executes a client command if the client is an OP on the server.
        ''' </summary>
        ''' <param name="p"></param>
        ''' <remarks></remarks>
        Public Shared Sub ExecuteClientCommand(ByVal p As Package)
            'Check if the client has operator permissions when using the command:
            If Basic.ServersManager.ListManager.Operators.ContainsPlayer(Basic.ServersManager.PlayerCollection.GetPlayer(p.Origin).ListName) = True Then
                If CBool(Basic.GetPropertyValue("AllowOP", "1")) = True Then 'If the server allows operators (other than the server itself), the command goes through.
                    Execute(p.DataItems(0), p.Origin, True)
                End If
            Else
                'Send messages to the server and client because client doesn't have operator permissions.
                Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_NOOPWARNING, Basic.ServersManager.PlayerCollection.GetPlayerName(p.Origin)))
                Basic.ServersManager.PlayerCollection.GetPlayer(p.Origin).Networking.SendPackage(New Package(Package.PackageTypes.ChatMessage, -1, Servers.Package.ProtocolTypes.TCP, ServerMessages.CLIENT_NOOPWARNING))
            End If
        End Sub

        ''' <summary>
        ''' Executes a command on the server.
        ''' </summary>
        ''' <param name="Command">The command string to execute, starting with "/".</param>
        ''' <param name="Origin">The Origin of the client (or -1 for the server) that used the command.</param>
        ''' <remarks></remarks>
        Public Shared Sub Execute(ByVal Command As String, ByVal Origin As Integer, ByVal IsClientInput As Boolean)
            'remove the "/"
            If Command.StartsWith("/") = True Then
                Command = Command.Remove(0, 1)
            End If

            Dim mainCommand As String = Command
            Dim subCommand As String = ""
            If Command.Contains(" ") = True Then
                mainCommand = Command.Remove(Command.IndexOf(" "))
                subCommand = Command.Remove(0, Command.IndexOf(" ") + 1)
            End If

            Select Case mainCommand.ToLower()
                Case "kick" '/kick <playername> : Kicks the player from the server : Client = true
                    Dim playerID As Integer = Basic.ServersManager.PlayerCollection.GetPlayerID(subCommand)
                    If playerID > -1 Then 'Ensure that the targeted player exists and that it's not the player that sent the command.
                        If playerID <> Origin Then
                            Dim p As Player = Basic.ServersManager.PlayerCollection.GetPlayer(playerID)
                            p.Kick(ServerMessages.CLIENT_KICKED)
                        End If
                    End If
                Case "stop" '/stop : Stops the server : Client = false
                    If IsClientInput = False Then
                        Basic.ServersManager.ServerHost.SendToAllPlayers(New Package(Package.PackageTypes.ServerClose, -1, Servers.Package.ProtocolTypes.TCP, ServerMessages.CLIENT_SERVERCLOSED))
                        Basic.ServersManager.WriteLine(ServerMessages.SERVER_STOP)
                        Application.Exit()
                    End If
                Case "restart" '/restart : Restarts the server : Client = true
                    Basic.ServersManager.ServerHost.SendToAllPlayers(New Package(Package.PackageTypes.ServerClose, -1, Servers.Package.ProtocolTypes.TCP, ServerMessages.CLIENT_SERVERRESTART))
                    Basic.ServersManager.WriteLine(ServerMessages.SERVER_RESTART)
                    Application.Restart()
                Case "servermessage" '/servermessage <message> : sets the servermessage and sends the new message to all clients. : Client = true
                    Dim message As String = subCommand
                    Basic.ServersManager.PropertyCollection.SetPropertyValue("ServerMessage", message)

                    Basic.ServersManager.ServerHost.SendToAllPlayers(New Package(Package.PackageTypes.ServerMessage, -1, Servers.Package.ProtocolTypes.TCP, message))
                    Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_NEWSERVERMESSAGE, message))
                Case "weather" '/weather <weatherID> : sets the weather : Client = true
                    Dim weatherID As Integer = CInt(subCommand)

                    Basic.ServersManager.World.CurrentWeather = CType(weatherID, Game.World.WeatherTypes)
                    Basic.ServersManager.World.PrintWorldInformation()
                    Basic.ServersManager.World.DistributeWorldData()
                Case "season" '/season <seasonID> : sets the season : Client = true
                    Dim seasonID As Integer = CInt(subCommand)

                    Basic.ServersManager.PropertyCollection.SetPropertyValue("Season", seasonID.ToString())
                    Basic.ServersManager.World.PrintWorldInformation()
                    Basic.ServersManager.World.DistributeWorldData()
                Case "list" '/list <add|remove|toggle|check> <whitelist|blacklist|operators|mutelist> [playername] : Adds or removes the player to/from a list. Can also enable/disable use of lists (white/black). : Client = true
                    Dim argList As List(Of String) = subCommand.Split(CChar(" ")).ToList()

                    If argList.Count = 2 Then
                        Dim listCommand As String = argList(0)
                        Dim listName As String = argList(1)

                        Select Case listName.ToLower()
                            Case "whitelist"
                                Select Case listCommand.ToLower()
                                    Case "enable"
                                        Basic.ServersManager.PropertyCollection.SetPropertyValue("WhiteList", "1")
                                        Basic.ServersManager.WriteLine(ServerMessages.SERVER_WHITELIST_ON)
                                    Case "disable"
                                        Basic.ServersManager.PropertyCollection.SetPropertyValue("WhiteList", "0")
                                        Basic.ServersManager.WriteLine(ServerMessages.SERVER_WHITELIST_OFF)
                                End Select
                            Case "blacklist"
                                Select Case listCommand.ToLower()
                                    Case "enable"
                                        Basic.ServersManager.PropertyCollection.SetPropertyValue("BlackList", "1")
                                        Basic.ServersManager.WriteLine(ServerMessages.SERVER_BLACKLIST_ON)
                                    Case "disable"
                                        Basic.ServersManager.PropertyCollection.SetPropertyValue("BlackList", "0")
                                        Basic.ServersManager.WriteLine(ServerMessages.SERVER_BLACKLIST_OFF)
                                End Select
                        End Select
                    End If

                    If argList.Count >= 3 Then
                        Dim listCommand As String = argList(0)
                        Dim listName As String = argList(1)

                        Dim playerName As String = argList(2)
                        If argList.Count > 3 Then
                            For i = 3 To argList.Count - 1
                                playerName &= " " & argList(i)
                            Next
                        End If

                        Dim displayName As String = playerName
                        playerName = Basic.ServersManager.PlayerCollection.GetPlayer(playerName).ListName

                        Dim list As PlayerList = Basic.ServersManager.ListManager.GetListFromName(listName)

                        If Not list Is Nothing Then
                            Select Case listCommand.ToLower()
                                Case "add"
                                    If list.AddPlayer(playerName) = False Then
                                        Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_LIST_EXISTS, displayName, list.ListName))
                                    Else
                                        Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_LIST_ADD, displayName, list.ListName))
                                    End If
                                Case "remove"
                                    If list.RemovePlayer(playerName) = False Then
                                        Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_LIST_NOT_EXISTS, displayName, list.ListName))
                                    Else
                                        Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_LIST_REMOVE, displayName, list.ListName))
                                    End If
                                Case "toggle"
                                    If list.ContainsPlayer(playerName) = True Then
                                        list.RemovePlayer(playerName)
                                        Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_LIST_REMOVE, displayName, list.ListName))
                                    Else
                                        list.AddPlayer(playerName)
                                        Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_LIST_ADD, displayName, list.ListName))
                                    End If
                                Case "check"
                                    If list.ContainsPlayer(playerName) = True Then
                                        Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_LIST_EXISTS, displayName, list.ListName))
                                    Else
                                        Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_LIST_NOT_EXISTS, displayName, list.ListName))
                                    End If
                            End Select
                        End If
                    End If

                Case "pm" '/pm <playername> <message> : Sends a private message to a player. : Client = false
                    If IsClientInput = False Then
                        Dim message As String = subCommand

                        Dim playerName As String = message
                        While Basic.ServersManager.PlayerCollection.HasPlayer(playerName) = False And playerName.Contains(" ") = True
                            playerName = playerName.Remove(playerName.LastIndexOf(" "))
                        End While

                        If playerName <> "" And Basic.ServersManager.PlayerCollection.HasPlayer(playerName) = True Then
                            message = message.Remove(0, playerName.Length + 1)

                            Basic.ServersManager.PlayerCollection.GetPlayer(playerName).Networking.SendPackage(New Package(Package.PackageTypes.PrivateMessage, -1, Servers.Package.ProtocolTypes.TCP, message))
                            Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_PM_SENT, playerName))
                        Else
                            Basic.ServersManager.WriteLine(ServerMessages.SERVER_PM_NOTARGET)
                        End If
                    End If

                Case "allowop" '/allowop <value> : allows or disallows operators : Client = false
                    If IsClientInput = False Then
                        Select Case subCommand.ToLower()
                            Case "true", "1"
                                Basic.ServersManager.PropertyCollection.SetPropertyValue("AllowOP", "1")
                                Basic.ServersManager.WriteLine(ServerMessages.SERVER_OP_ON)
                            Case "false", "0"
                                Basic.ServersManager.PropertyCollection.SetPropertyValue("AllowOP", "0")
                                Basic.ServersManager.WriteLine(ServerMessages.SERVER_OP_OFF)
                        End Select
                    End If

                Case "say" '/say <message> : Posts a normal chat message. : Client = false
                    If IsClientInput = False Then
                        Basic.ServersManager.ServerHost.SendToAllPlayers(New Package(Package.PackageTypes.ChatMessage, -1, Servers.Package.ProtocolTypes.TCP, subCommand))
                    End If

                Case "world" '/world update : Updates the world. : Client = true
                    If subCommand.ToLower() = "update" Then
                        Basic.ServersManager.World.UpdateWorld()
                    End If

                Case "clear" '/clear : Clears the console output : Client = false
                    If IsClientInput = False Then
                        Basic.ServersManager.ClearOutput()
                    End If

                Case Else
                    Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_INVALIDCOMMAND, Command))
            End Select
        End Sub

#End Region

    End Class

End Namespace