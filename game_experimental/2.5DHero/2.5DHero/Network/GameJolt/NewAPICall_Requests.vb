Namespace GameJolt.GameAPI

    Partial Public NotInheritable Class GameJoltRequest

        Private Shared Sub ThrowUserNotLoggedInException()
            Throw New ArgumentException("An API request requires the user to be logged in.")
        End Sub

        Private Shared Sub AddUserCredentials(ByRef request As GameJoltRequest)
            If LoggedIn = False Then
                ThrowUserNotLoggedInException()
            Else
                request.AddURLParameter("username", Username)
                request.AddURLParameter("user_token", Token)
            End If
        End Sub

#Region "User"

        ''' <summary>
        ''' Verifies if the input username and token are a correct username/user_token pair.
        ''' </summary>
        ''' <param name="username"></param>
        ''' <param name="token"></param>
        ''' <returns></returns>
        Public Shared ReadOnly Property VerifyUser(ByVal username As String, ByVal token As String) As GameJoltRequest
            Get
                Dim request As New GameJoltRequest(RequestType.GET, "/users/auth/")

                request.AddURLParameter("username", username)
                request.AddURLParameter("user_token", token)

                Return request
            End Get
        End Property

        Public Shared ReadOnly Property FetchUserData(ByVal username As String) As GameJoltRequest
            Get
                Dim request As New GameJoltRequest(RequestType.GET, "/users/")

                request.AddURLParameter("username", username)

                Return request
            End Get
        End Property

        Public Shared ReadOnly Property FetchUserDataById(ByVal user_id As String) As GameJoltRequest
            Get
                Dim request As New GameJoltRequest(RequestType.GET, "/users/")

                request.AddURLParameter("user_id", Username)

                Return request
            End Get
        End Property

#End Region

#Region "Storage"

        Public Shared ReadOnly Property SetStorageData(ByVal key As String, ByVal data As String, ByVal userSpace As Boolean) As GameJoltRequest
            Get
                Dim request As New GameJoltRequest(RequestType.POST, "/data-store/set/")
                If userSpace = True Then
                    AddUserCredentials(request)
                End If

                request.AddURLParameter("key", key)
                request._postData = data

                Return request
            End Get
        End Property

        Public Shared ReadOnly Property SetStorageData(ByVal keys As String(), ByVal dataItems As String(), ByVal userSpaces As Boolean()) As GameJoltRequest
            Get
                Dim request As New GameJoltRequest(RequestType.POST, "/batch/")

                request.AddURLParameter("parallel", "true")

                Dim postData As String = ""

                For i = 0 To keys.Length - 1
                    Dim key As String = keys(i)
                    Dim data As String = dataItems(i)
                    Dim userSpace = userSpaces(i)

                    If userSpace = True And LoggedIn = False Then
                        ThrowUserNotLoggedInException()
                    End If

                    If userSpace = True Then
                        Dim url As String = "/data-store/set/" & "?game_id=" & GAMEID & "&username=" & Username & "&user_token=" & Token & "&key=" & UrlEncoder.Encode(key) & "&data=" & UrlEncoder.Encode(data)
                        url &= "&signature=" & request.getUrlSignature(url)

                        postData &= "&requests[]=" & UrlEncoder.Encode(url)
                    Else
                        Dim url As String = "/data-store/set/" & "?game_id=" & GAMEID & "&key=" & UrlEncoder.Encode(key) & "&data=" & UrlEncoder.Encode(data)
                        url &= "&signature=" & request.getUrlSignature(url)

                        postData &= "&requests[]=" & UrlEncoder.Encode(url)
                    End If
                Next

                request._postData = postData

                Return request
            End Get
        End Property

        Public Shared ReadOnly Property UpdateStorageData(ByVal key As String, ByVal oValue As String, ByVal operation As String, ByVal userSpace As Boolean) As GameJoltRequest
            Get
                Dim request As New GameJoltRequest(RequestType.GET, "/data-store/update/")
                If userSpace = True Then
                    AddUserCredentials(request)
                End If

                request.AddURLParameter("key", key)
                request.AddURLParameter("operation", operation)
                request.AddURLParameter("value", oValue)

                Return request
            End Get
        End Property

        Public Shared ReadOnly Property SetStorageDataRestricted(ByVal key As String, ByVal data As String) As GameJoltRequest
            Get
                Dim request As New GameJoltRequest(RequestType.POST, "/data-store/set/")

                request.AddURLParameter("key", key)
                request.AddURLParameter("restriction_username", Username)
                request.AddURLParameter("restriction_user_token", Token)

                request._postData = data

                Return request
            End Get
        End Property

        Public Shared ReadOnly Property GetStorageData(ByVal key As String, ByVal userSpace As Boolean) As GameJoltRequest
            Get
                Dim request As New GameJoltRequest(RequestType.GET, "/data-store/")
                If userSpace = True Then
                    AddUserCredentials(request)
                End If

                request.AddURLParameter("key", key)

                Return request
            End Get
        End Property

        Public Shared ReadOnly Property GetKeys(ByVal userSpace As Boolean, ByVal pattern As String) As GameJoltRequest
            Get
                Dim request As New GameJoltRequest(RequestType.GET, "/data-store/get-keys/")
                If userSpace = True Then
                    AddUserCredentials(request)
                End If

                request.AddURLParameter("pattern", pattern)

                Return request
            End Get
        End Property

        Public Shared ReadOnly Property RemoveKey(ByVal key As String, ByVal userSpace As Boolean) As GameJoltRequest
            Get
                Dim request As New GameJoltRequest(RequestType.POST, "/data-store/remove/")
                If userSpace = True Then
                    AddUserCredentials(request)
                End If

                request.AddURLParameter("key", key)

                Return request
            End Get
        End Property

#End Region

#Region "Sessions"



#End Region

    End Class

End Namespace