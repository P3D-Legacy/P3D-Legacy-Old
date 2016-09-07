Namespace Servers

    ''' <summary>
    ''' A base class for player lists like a blacklist.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PlayerList

        Inherits System.Collections.Generic.List(Of String)

#Region "Fields"

        Private _listName As String = ""
        Private _fileName As String = ""

#End Region

#Region "Properties"

        ''' <summary>
        ''' Checks if this list contains a certain player.
        ''' </summary>
        ''' <param name="PlayerName">The playername to check the list for.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ContainsPlayer(ByVal PlayerName As String) As Boolean
            Get
                For Each s As String In Me
                    If s.ToLower() = PlayerName.ToLower() Then
                        Return True
                    End If
                Next
                Return False
            End Get
        End Property

        ''' <summary>
        ''' Gets the save data of this list.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SaveData() As String
            Get
                Dim data As String = ""
                For Each playerName As String In Me
                    If data <> "" Then
                        data &= vbNewLine
                    End If
                    data &= playerName
                Next
                Return data
            End Get
        End Property

        ''' <summary>
        ''' Returns this list's name.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ListName() As String
            Get
                Return Me._listName
            End Get
        End Property

        ''' <summary>
        ''' Returns the filename associated with this list.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FileName() As String
            Get
                Return Me._fileName
            End Get
        End Property

        ''' <summary>
        ''' Returns the full path to the list's file.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FullPath() As String
            Get
                Return My.Application.Info.DirectoryPath & "\" & Me._fileName
            End Get
        End Property

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Creates a new instance of a PlayerList.
        ''' </summary>
        ''' <param name="Name">The name of the new PlayerList.</param>
        ''' <param name="FileName">The filename of the new PlayerList. This is used to save and load the list.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal Name As String, ByVal FileName As String)
            Me._listName = Name
            Me._fileName = FileName
        End Sub

#End Region

#Region "Methods"

        ''' <summary>
        ''' Add a player to the list.
        ''' </summary>
        ''' <param name="PlayerName">The player to add.</param>
        ''' <returns>True, if the player got added succesfully.</returns>
        ''' <remarks>This method checks if the player is already added to the list.</remarks>
        Public Function AddPlayer(ByVal PlayerName As String) As Boolean
            If ContainsPlayer(PlayerName) = False Then
                Me.Add(PlayerName)
                Return True
            End If
            Return False
        End Function

        ''' <summary>
        ''' Removes a player from the list.
        ''' </summary>
        ''' <param name="PlayerName">The player to remove.</param>
        ''' <returns>True, if the player was succesfully removed.</returns>
        ''' <remarks></remarks>
        Public Function RemovePlayer(ByVal PlayerName As String) As Boolean
            If ContainsPlayer(PlayerName) = True Then
                For i = 0 To Me.Count - 1
                    If Me(i).ToLower() = PlayerName.ToLower() Then
                        Me.RemoveAt(i)
                        Return True
                    End If
                Next
            End If
            Return False
        End Function

        ''' <summary>
        ''' Saves the content of the list to the associated file.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SaveToFile()
            System.IO.File.WriteAllText(Me.FullPath, Me.SaveData)
        End Sub

        ''' <summary>
        ''' Loads the list's content from the associated file.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub LoadFromFile()
            Me.Clear()

            If System.IO.File.Exists(Me.FullPath) = False Then
                Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_LIST_NEW, Me._listName, Me._fileName))
                System.IO.File.WriteAllText(Me.FullPath, "")
            Else
                Dim data() As String = System.IO.File.ReadAllLines(Me.FullPath)
                For Each playerName As String In data
                    Me.Add(playerName)
                Next
            End If
        End Sub

#End Region

    End Class

End Namespace