Imports System.Net.Sockets
Imports System.IO
Imports System.Net

Namespace Servers

    ''' <summary>
    ''' The class that establishes a connection to a client computer.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ClientConnection

#Region "Enums"

#End Region

#Region "Fields and Constants"

        Private _playerReference As Player

        'Networking:
        Private _client As TcpClient

        Private _stream As NetworkStream
        Private _streamReader As StreamReader
        Private _streamWriter As StreamWriter

        Private _assignedNetworkingValues As Boolean = False

        Private _receiveThread As Threading.Thread
        Private _pingTimer As Timers.Timer

        Private _lastPing As Date
        Private _lastValidPackage As Date
        Private _lastPackageIsInvalid As Boolean = False

#End Region

#Region "Properties"

#End Region

#Region "Delegates"

#End Region

#Region "Constructors"

        Public Sub New(ByVal playerReference As Player)
            Me._playerReference = playerReference

            Me._lastPing = Date.Now
        End Sub

#End Region

#Region "Methods"

        ''' <summary>
        ''' Assign the network streams.
        ''' </summary>
        ''' <param name="TcpClient"></param>
        ''' <remarks></remarks>
        Public Sub AssignNetworkingClient(ByVal TcpClient As TcpClient)
            Me._client = TcpClient

            Me._stream = Me._client.GetStream()
            Me._streamReader = New StreamReader(Me._stream)
            Me._streamWriter = New StreamWriter(Me._stream)

            Me._assignedNetworkingValues = True
        End Sub

        ''' <summary>
        ''' Start receive and sending networking queues.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub StartNetworking()
            If Me._assignedNetworkingValues = True Then
                Me._receiveThread = New Threading.Thread(AddressOf Me.InternalListen)
                Me._receiveThread.IsBackground = True
                Me._receiveThread.Start()

                Me._lastPing = Date.Now
                Me._lastValidPackage = Date.Now
                Me._lastPackageIsInvalid = False

                Me._pingTimer = New Timers.Timer()
                Me._pingTimer.Interval = 1000
                Me._pingTimer.AutoReset = True
                AddHandler Me._pingTimer.Elapsed, AddressOf Me.InternalKickCheckers
                Me._pingTimer.Start()
            End If
        End Sub

        ''' <summary>
        ''' Stops all networking processes.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub StopNetworking()
            Try
                Me._receiveThread.Abort()
            Catch : End Try

            Try
                Me._pingTimer.Stop()
            Catch : End Try

            Try
                Me._client.Close()
            Catch : End Try
        End Sub

        Private _isWritingToStream As Boolean = False

        Private Sub InternalSendPackage(ByVal packageObject As Object)
            If _client.Connected = True Then
                While Me._isWritingToStream = True : End While
                Me._isWritingToStream = True
                Try
                    Dim package As Package = CType(packageObject, Package)

                    Me._streamWriter.WriteLine(package.ToString())
                    Me._streamWriter.Flush()
                    Me._isWritingToStream = False
                Catch ex As Exception
                End Try
            End If
        End Sub

        Private Sub InternalListen()
            Do
                Try
                    Dim receivedData As String = Me._streamReader.ReadLine()
                    Dim p As New Package(receivedData)

                    Me._lastPing = Date.Now

                    If p.IsValid = True Then
                        Me._lastValidPackage = Date.Now
                        p.Handle()
                    End If

                    Me._lastPackageIsInvalid = Not p.IsValid
                Catch ex As Exception 'Receiving an error message / time out -> close the connection.
                    Debug.Print("Listen to client error: " & ex.Message)

                    Me.ListenToClientError()

                    Exit Do
                End Try
            Loop
        End Sub

        Private Sub ListenToClientError()
            Try
                If Basic.ServersManager.PlayerCollection.Contains(Me._playerReference) = True Then
                    Basic.ServersManager.PlayerCollection.Remove(Me._playerReference)
                End If

                Basic.ServersManager.ServerHost.SendToAllPlayers(New Package(Package.PackageTypes.ChatMessage, -1, Servers.Package.ProtocolTypes.TCP, Me._playerReference.Name & " quit the game!"))
                Basic.ServersManager.ServerHost.SendToAllPlayers(New Package(Package.PackageTypes.DestroyPlayer, -1, Servers.Package.ProtocolTypes.TCP, Me._playerReference.ServersID.ToString()))

                Basic.ServersManager.UpdatePlayerList()
            Catch : End Try
        End Sub

        ''' <summary>
        ''' Adds the Package to the player's networking queue.
        ''' </summary>
        ''' <param name="p"></param>
        ''' <remarks></remarks>
        Public Sub SendPackage(ByVal p As Package)
            Dim t As New Threading.Thread(AddressOf Me.InternalSendPackage)
            t.IsBackground = True
            t.Start(p)
        End Sub

        ''' <summary>
        ''' Reads data from an incomming connection without the use of networking threads.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ReadUnthreadedData() As String
            Dim streamr As New StreamReader(New NetworkStream(Me._client.Client))
            Dim data As String = streamr.ReadLine()

            Return data
        End Function

        ''' <summary>
        ''' Sends data through the set data stream without the use of networking threads.
        ''' </summary>
        ''' <param name="data"></param>
        ''' <remarks></remarks>
        Public Function SendUnthreadedData(ByVal data As String) As Boolean
            Try
                Dim streamw As New StreamWriter(New NetworkStream(Me._client.Client))
                streamw.WriteLine(data)
                streamw.Flush()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function SendUnthreadedPackage(ByVal Package As Package) As Boolean
            Return Me.SendUnthreadedData(Package.ToString())
        End Function

        ''' <summary>
        ''' Closes the underlying Tcp Connection of the client.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CloseConnection()
            Try
                Me._client.Close()
            Catch : End Try
        End Sub

        ''' <summary>
        ''' A main sub for controlling the AFK kick timer and No-Ping kick timer
        ''' </summary>
        Private Sub InternalKickCheckers()
            Me.InternalNoPingKickCheck()
            Me.InternalAFKKickCheck()
        End Sub

        ''' <summary>
        ''' This sub checks every second if the player has sent a package in a time frame.
        ''' </summary>
        ''' <remarks>The IdleKickTime property defines the amount of seconds until the next check. 0 indicates that no Ping Check should be conducted.</remarks>
        Private Sub InternalNoPingKickCheck()
            If CInt(Math.Abs(DateDiff(DateInterval.Second, Me._lastValidPackage, Date.Now))) >= 1 And _lastPackageIsInvalid = True Then
                Me._playerReference.Kick(ServerMessages.CLIENT_IDLEKICK)
            End If

            Dim idleKickTime As Integer = CInt(Basic.GetPropertyValue("NoPingKickTime", "0"))
            If idleKickTime > 0 Then
                idleKickTime = idleKickTime.Clamp(20, 120)
                If CInt(Math.Abs(DateDiff(DateInterval.Second, Me._lastPing, Date.Now))) >= idleKickTime Then
                    Me._playerReference.Kick(ServerMessages.CLIENT_IDLEKICK)
                End If
            End If
        End Sub

        Private _secondsAFK As Integer = 0

        Private Sub InternalAFKKickCheck()
            Dim AFKKickTime As Integer = CInt(Basic.GetPropertyValue("AFKKickTime", "600"))

            If AFKKickTime > 0 Then
                If Me._playerReference.BusyType = "3" Then
                    Me._secondsAFK += 1
                    If Me._secondsAFK >= AFKKickTime Then
                        Me._playerReference.Kick(ServerMessages.CLIENT_AFKKICK)
                    End If
                Else
                    If Me._secondsAFK > 0 Then
                        Me._secondsAFK -= 5
                    End If
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace