Namespace Servers

    Public Class ListManager

#Region "Fields"

        Private _blackList As PlayerList
        Private _whitelist As PlayerList
        Private _ops As PlayerList
        Private _mutelist As PlayerList

#End Region

        Public Sub New()
            Me._blackList = New PlayerList("Blacklist", "blacklist.dat")
            Me._whitelist = New PlayerList("Whitelist", "whitelist.dat")
            Me._ops = New PlayerList("Operators", "ops.dat")
            Me._mutelist = New PlayerList("Mutelist", "mutelist.dat")
        End Sub

        ''' <summary>
        ''' Loads all lists from the available files or creates new files.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Load()
            Me._blackList.LoadFromFile()
            Me._whitelist.LoadFromFile()
            Me._ops.LoadFromFile()
            Me._mutelist.LoadFromFile()
        End Sub

        ''' <summary>
        ''' Saves all lists to files.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Save()
            Me._blackList.SaveToFile()
            Me._whitelist.SaveToFile()
            Me._ops.SaveToFile()
            Me._mutelist.SaveToFile()
        End Sub

        Public ReadOnly Property BlackList() As PlayerList
            Get
                Return Me._blackList
            End Get
        End Property

        Public ReadOnly Property WhiteList() As PlayerList
            Get
                Return Me._whitelist
            End Get
        End Property

        Public ReadOnly Property Operators() As PlayerList
            Get
                Return Me._ops
            End Get
        End Property

        Public ReadOnly Property Mutelist() As PlayerList
            Get
                Return Me._mutelist
            End Get
        End Property

        ''' <summary>
        ''' Returns a PlayerList depending on its name.
        ''' </summary>
        ''' <param name="name">The name of the PlayerList.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetListFromName(ByVal name As String) As PlayerList
            Select Case name.ToLower()
                Case "whitelist"
                    Return Me._whitelist
                Case "blacklist"
                    Return Me._blackList
                Case "operators"
                    Return Me._ops
                Case "mutelist"
                    Return Me._mutelist
            End Select

            Return Nothing
        End Function

    End Class

End Namespace