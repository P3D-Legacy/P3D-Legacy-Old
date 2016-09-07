Namespace Servers

    Public Class PackageHandler

        Public Shared Sub HandlePackage(ByVal p As Package)
            Select Case p.PackageType
                Case Package.PackageTypes.ChatMessage
                    HandleChatMessage(p)
                Case Package.PackageTypes.PrivateMessage
                    HandlePrivateMessage(p)
                Case Package.PackageTypes.GameData
                    HandleGameData(p)
                Case Package.PackageTypes.Ping
                    HandlePing(p)
                Case Package.PackageTypes.GamestateMessage
                    HandleGamestateMessage(p)

                    'Trades:
                Case Package.PackageTypes.TradeJoin
                    HandleTradeJoin(p)
                Case Package.PackageTypes.TradeOffer
                    HandleTradeOffer(p)
                Case Package.PackageTypes.TradeQuit
                    HandleTradeQuit(p)
                Case Package.PackageTypes.TradeStart
                    HandleTradeStart(p)
                Case Package.PackageTypes.TradeRequest
                    HandleTradeRequest(p)

                    'Battle:
                Case Package.PackageTypes.BattleJoin
                    HandleBattleJoin(p)
                Case Package.PackageTypes.BattleQuit
                    HandleBattleQuit(p)
                Case Package.PackageTypes.BattleOffer
                    HandleBattleOffer(p)
                Case Package.PackageTypes.BattleStart
                    HandleBattleStart(p)
                Case Package.PackageTypes.BattleClientData
                    HandleBattleClientData(p)
                Case Package.PackageTypes.BattleHostData
                    HandleBattleHostData(p)
                Case Package.PackageTypes.BattlePokemonData
                    HandleBattlePokemonData(p)
                Case Package.PackageTypes.BattleRequest
                    HandleBattleRequest(p)
            End Select
        End Sub

#Region "GameData"

        Private Shared Sub HandleGameData(ByVal p As Package)
            Dim player = Basic.ServersManager.PlayerCollection.GetPlayer(p.Origin)
            If Not player Is Nothing Then
                'Apply the new player data and send the updated player package to all clients:
                player.ApplyNewData(p)
                Basic.ServersManager.ServerHost.SendToAllPlayers(player.PlayerDataPackage(False))
            End If
        End Sub

#End Region

#Region "BaseFunctions"

        Private Shared Sub HandleChatMessage(ByVal p As Package)
            'Check if the player is muted on the server:
            If Basic.ServersManager.ListManager.Mutelist.ContainsPlayer(Basic.ServersManager.PlayerCollection.GetPlayer(p.Origin).ListName) = False Then
                If p.DataItems(0).StartsWith("/") = True Then
                    'This is a command sent by a client.
                    ServerCommands.ExecuteClientCommand(p)
                Else
                    'Send chat message to all clients:
                    Basic.ServersManager.ServerHost.SendToAllPlayers(p)
                End If
            Else
                'The player is muted on the server, send message to client:
                Basic.ServersManager.ServerHost.SendToPlayer(p.Origin, New Package(Package.PackageTypes.ChatMessage, -1, Servers.Package.ProtocolTypes.TCP, ServerMessages.CLIENT_ISMUTED))
            End If
        End Sub

        Private Shared Sub HandlePrivateMessage(ByVal p As Package)
            'Private message data structure:
            '0: destination playername
            '1: message

            'Check if the player is muted on the server:
            If Basic.ServersManager.ListManager.Mutelist.ContainsPlayer(Basic.ServersManager.PlayerCollection.GetPlayerName(p.Origin)) = False Then
                'Check if the destination player exists:
                If Basic.ServersManager.PlayerCollection.HasPlayer(p.DataItems(0)) = True Then
                    'Send a private message to the destination player and the normal chat message to the player that sent the message:
                    Basic.ServersManager.ServerHost.SendToPlayer(Basic.ServersManager.PlayerCollection.GetPlayerID(p.DataItems(0)), New Package(Package.PackageTypes.PrivateMessage, p.Origin, Servers.Package.ProtocolTypes.TCP, p.DataItems(1)))
                    Basic.ServersManager.ServerHost.SendToPlayer(p.Origin, New Package(Package.PackageTypes.PrivateMessage, p.Origin, Servers.Package.ProtocolTypes.TCP, p.DataItems))
                Else
                    Basic.ServersManager.ServerHost.SendToPlayer(p.Origin, New Package(Package.PackageTypes.ChatMessage, -1, Servers.Package.ProtocolTypes.TCP, String.Format(ServerMessages.CLIENT_NODESTINATIONPLAYER, p.DataItems(0))))
                End If
            Else
                'The player is muted on the server, send message to client:
                Basic.ServersManager.ServerHost.SendToPlayer(p.Origin, New Package(Package.PackageTypes.ChatMessage, -1, Servers.Package.ProtocolTypes.TCP, ServerMessages.CLIENT_ISMUTED))
            End If
        End Sub

        Private Shared Sub HandlePing(ByVal p As Package)
            'Don't handle the ping package, because every arriving package from a player counts as ping and gets handled in ClientConnection.vb
        End Sub

        Private Shared Sub HandleGamestateMessage(ByVal p As Package)
            Dim playerName As String = Basic.ServersManager.PlayerCollection.GetPlayerName(p.Origin)
            If playerName <> "" Then
                If Basic.ServersManager.ListManager.Mutelist.ContainsPlayer(playerName) = False Then
                    Basic.ServersManager.ServerHost.SendToAllPlayers(New Package(Package.PackageTypes.ChatMessage, -1, Servers.Package.ProtocolTypes.TCP, String.Format(ServerMessages.CLIENT_GAMESTATEMESSAGE, playerName, p.DataItems(0))))
                End If
            End If
        End Sub

#End Region

#Region "Trade"

        ''' <summary>
        ''' A player joins a trade requested by another player.
        ''' </summary>
        ''' <param name="p"></param>
        ''' <remarks></remarks>
        Private Shared Sub HandleTradeJoin(ByVal p As Package)
            Basic.ServersManager.ServerHost.SendToPlayer(CInt(p.DataItems(0)), New Package(Package.PackageTypes.TradeJoin, p.Origin, Servers.Package.ProtocolTypes.TCP))
        End Sub

        ''' <summary>
        ''' Offers another Pokémon to a trade partner.
        ''' </summary>
        ''' <param name="p"></param>
        ''' <remarks></remarks>
        Private Shared Sub HandleTradeOffer(ByVal p As Package)
            'Data structure:
            '0: Destination ID
            '1: PokemonData

            Basic.ServersManager.ServerHost.SendToPlayer(CInt(p.DataItems(0)), New Package(Package.PackageTypes.TradeOffer, p.Origin, Servers.Package.ProtocolTypes.TCP, p.DataItems(1)))
        End Sub

        ''' <summary>
        ''' A player quits a trade.
        ''' </summary>
        ''' <param name="p"></param>
        ''' <remarks></remarks>
        Private Shared Sub HandleTradeQuit(ByVal p As Package)
            Basic.ServersManager.ServerHost.SendToPlayer(CInt(p.DataItems(0)), New Package(Package.PackageTypes.TradeQuit, p.Origin, Servers.Package.ProtocolTypes.TCP))
        End Sub

        ''' <summary>
        ''' A player accepts the and wants to start the trade.
        ''' </summary>
        ''' <param name="p"></param>
        ''' <remarks></remarks>
        Private Shared Sub HandleTradeStart(ByVal p As Package)
            Basic.ServersManager.ServerHost.SendToPlayer(CInt(p.DataItems(0)), New Package(Package.PackageTypes.TradeStart, p.Origin, Servers.Package.ProtocolTypes.TCP))
        End Sub

        ''' <summary>
        ''' A player requests another player to trade.
        ''' </summary>
        ''' <param name="p"></param>
        ''' <remarks></remarks>
        Private Shared Sub HandleTradeRequest(ByVal p As Package)
            If Basic.ServersManager.ListManager.Mutelist.ContainsPlayer(Basic.ServersManager.PlayerCollection.GetPlayerName(p.Origin)) = False Then
                Basic.ServersManager.ServerHost.SendToPlayer(CInt(p.DataItems(0)), New Package(Package.PackageTypes.TradeRequest, p.Origin, Servers.Package.ProtocolTypes.TCP))
            End If
        End Sub

#End Region

#Region "Battle"

        Private Shared Sub HandleBattleJoin(ByVal p As Package)
            Basic.ServersManager.ServerHost.SendToPlayer(CInt(p.DataItems(0)), New Package(Package.PackageTypes.BattleJoin, p.Origin, Servers.Package.ProtocolTypes.TCP))
        End Sub

        Private Shared Sub HandleBattleQuit(ByVal p As Package)
            Basic.ServersManager.ServerHost.SendToPlayer(CInt(p.DataItems(0)), New Package(Package.PackageTypes.BattleQuit, p.Origin, Servers.Package.ProtocolTypes.TCP))
        End Sub

        Private Shared Sub HandleBattleOffer(ByVal p As Package)
            'Data structure:
            '0: Destination ID
            '1: PokemonData

            Basic.ServersManager.ServerHost.SendToPlayer(CInt(p.DataItems(0)), New Package(Package.PackageTypes.BattleOffer, p.Origin, Servers.Package.ProtocolTypes.TCP, p.DataItems(1)))
        End Sub

        Private Shared Sub HandleBattleStart(ByVal p As Package)
            Basic.ServersManager.ServerHost.SendToPlayer(CInt(p.DataItems(0)), New Package(Package.PackageTypes.BattleStart, p.Origin, Servers.Package.ProtocolTypes.TCP))
        End Sub

        Private Shared Sub HandleBattleClientData(ByVal p As Package)
            'Data structure:
            '0: Destination ID
            '1: Client Data

            Basic.ServersManager.ServerHost.SendToPlayer(CInt(p.DataItems(0)), New Package(Package.PackageTypes.BattleClientData, p.Origin, Servers.Package.ProtocolTypes.TCP, p.DataItems(1)))
        End Sub

        Private Shared Sub HandleBattleHostData(ByVal p As Package)
            'Data structure:
            '0: Destination ID
            '1: Host Data

            Basic.ServersManager.ServerHost.SendToPlayer(CInt(p.DataItems(0)), New Package(Package.PackageTypes.BattleHostData, p.Origin, Servers.Package.ProtocolTypes.TCP, p.DataItems(1)))
        End Sub

        Private Shared Sub HandleBattlePokemonData(ByVal p As Package)
            'Data structure:
            '0: Destination ID
            '1: Pokemon Data

            Basic.ServersManager.ServerHost.SendToPlayer(CInt(p.DataItems(0)), New Package(Package.PackageTypes.BattlePokemonData, p.Origin, Servers.Package.ProtocolTypes.TCP, p.DataItems(1)))
        End Sub

        Private Shared Sub HandleBattleRequest(ByVal p As Package)
            If Basic.ServersManager.ListManager.Mutelist.ContainsPlayer(Basic.ServersManager.PlayerCollection.GetPlayerName(p.Origin)) = False Then
                Basic.ServersManager.ServerHost.SendToPlayer(CInt(p.DataItems(0)), New Package(Package.PackageTypes.BattleRequest, p.Origin, Servers.Package.ProtocolTypes.TCP))
            End If
        End Sub

#End Region

    End Class

End Namespace