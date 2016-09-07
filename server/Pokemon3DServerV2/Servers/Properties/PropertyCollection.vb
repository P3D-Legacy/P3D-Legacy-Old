Namespace Servers.Properties

    Public Class PropertyCollection

        Inherits System.Collections.Generic.List(Of ServerProperty)

#Region "Fields and Constants"

        Private Const DEFAULTCONTENT = "ServerName=P3D Server" & vbNewLine &
                "IP-Address=127.0.0.1" & vbNewLine &
                "Port=15124" & vbNewLine &
                "MaxPlayers=20" & vbNewLine &
                "BlackList=0" & vbNewLine &
                "WhiteList=0" & vbNewLine &
                "OfflineMode=0" & vbNewLine &
                "ServerMessage=" & vbNewLine &
                "Weather=" & vbNewLine &
                "Season=" & vbNewLine &
                "DoDayCycle=1" & vbNewLine &
                "GameMode=Pokemon 3D" & vbNewLine &
                "NoPingKickTime=20" & vbNewLine &
                "AFKKickTime=600" & vbNewLine &
                "WelcomeMessage=Welcome to my Pokémon3D server" & vbNewLine &
                "AllowOP=1"

#End Region

#Region "Properties"

        ''' <summary>
        ''' Returns the path to the property file.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private ReadOnly Property PropertyFile() As String
            Get
                Return My.Application.Info.DirectoryPath & "\properties.dat"
            End Get
        End Property

        ''' <summary>
        ''' Returns a value for a property.
        ''' </summary>
        ''' <param name="Name">The name of the property.</param>
        ''' <param name="DefaultValue">The default value of the property. If the property doesn't exist, a new one with this value gets created.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GetPropertyValue(ByVal Name As String, ByVal DefaultValue As String) As String
            Get
                For Each p As ServerProperty In Me
                    If p.Name.ToLower() = Name.ToLower() Then
                        Return p.Value
                    End If
                Next

                'Creates a new property and adds it to the collection, then returns it by calling this function again:
                Me.SetPropertyValue(Name, DefaultValue)
                Return GetPropertyValue(Name, DefaultValue)
            End Get
        End Property

#End Region

#Region "Methods"

        ''' <summary>
        ''' Sets the value of a property.
        ''' </summary>
        ''' <param name="Name">The name of the property.</param>
        ''' <param name="Value">The new value of the property.</param>
        ''' <remarks></remarks>
        Public Sub SetPropertyValue(ByVal Name As String, ByVal Value As String)
            For Each p As ServerProperty In Me
                If p.Name.ToLower() = Name.ToLower() Then
                    p.Value = Value
                    Exit Sub
                End If
            Next
            Me.Add(New ServerProperty(Name, Value))
        End Sub

        ''' <summary>
        ''' Loads the property list from the property file.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub LoadFromFile()
            Basic.ServersManager.WriteLine(ServerMessages.PROPERTIES_LOADING)

            'Generate new properties file, if it doesn't exist:
            If System.IO.File.Exists(PropertyFile) = False Then
                Basic.ServersManager.WriteLine(ServerMessages.PROPERTIES_NOFILEFOUND)
                Me.CreateNewPropertiesFile()
            End If

            'Read data from file:
            Dim propertyData() As String = System.IO.File.ReadAllLines(PropertyFile)
            For Each line As String In propertyData
                Me.Add(New ServerProperty(line))
            Next
        End Sub

        ''' <summary>
        ''' Saves all properties to the property file.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SaveToFile()
            Dim fileContent As String = ""

            For Each p As ServerProperty In Me
                If fileContent <> "" Then
                    fileContent &= vbNewLine
                End If
                fileContent &= p.ToString()
            Next

            System.IO.File.WriteAllText(PropertyFile, fileContent)
        End Sub

        ''' <summary>
        ''' Creates a property file with the default properties.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CreateNewPropertiesFile()
            Basic.ServersManager.WriteLine(ServerMessages.PROPERTIES_NEWFILE)
            System.IO.File.WriteAllText(PropertyFile, DEFAULTCONTENT)
        End Sub

#End Region

    End Class

End Namespace