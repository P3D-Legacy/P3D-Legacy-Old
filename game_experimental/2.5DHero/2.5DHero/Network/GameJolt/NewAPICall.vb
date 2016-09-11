Imports System.Threading
Imports System.Security.Cryptography
Imports System.Text
Imports System.Net
Imports System.IO

Namespace GameJolt.GameAPI

#Region "Enums"

    ''' <summary>
    ''' The formats a request can return data in.
    ''' </summary>
    Public Enum RequestFormat
        Json
        Keypair
        Dump
        Xml
    End Enum

    ''' <summary>
    ''' The request type for the API request.
    ''' </summary>
    Public Enum RequestType
        ''' <summary>
        ''' Used when the request serves to get data from the server.
        ''' </summary>
        [GET]
        ''' <summary>
        ''' Used when the request sends data to the server.
        ''' </summary>
        POST
    End Enum

    ''' <summary>
    ''' The status of a request.
    ''' </summary>
    Public Enum RequestStatus
        ''' <summary>
        ''' The API request went through to the server and successfully returned an answer.
        ''' </summary>
        Success
        ''' <summary>
        ''' A problem occured during the request and it failed.
        ''' </summary>
        Failure
    End Enum

#End Region

    ''' <summary>
    ''' A request to the GameJolt API.
    ''' </summary>
    Partial Public NotInheritable Class GameJoltRequest

        Const GAMEID As String = "MTA1NDU="
        Const GAMEKEY As String = "NjY0NDFlMzE2YTc4NjFmNWI4MDVlOWUyM2NhMDU0YTI="
        Const HOST As String = "http://api.gamejolt.com/api/game/"
        Const API_VERSION As String = "v1_1"

        Private ReadOnly Property _GameID() As String
            Get
                Return StringObfuscation.DeObfuscate(GAMEID)
            End Get
        End Property

        Private ReadOnly Property _GameKey() As String
            Get
                Return StringObfuscation.DeObfuscate(GAMEKEY)
            End Get
        End Property

        Private Shared ReadOnly Property Username() As String
            Get
                Return API.username
            End Get
        End Property

        Private Shared ReadOnly Property Token() As String
            Get
                Return API.token
            End Get
        End Property

        Private Shared ReadOnly Property LoggedIn() As Boolean
            Get
                Return API.LoggedIn
            End Get
        End Property

        ''' <summary>
        ''' The event getting raised when an async request finishes.
        ''' </summary>
        ''' <param name="result">The result of the request.</param>
        Public Event Finished(ByVal result As RequestResult)

        Private _requestType As RequestType
        Private _endpoint As String
        Private _postData As String

        Private _urlParameters As New Dictionary(Of String, String)

        Private Sub New(ByVal requestType As RequestType, ByVal endpoint As String)
            _requestType = requestType
            _endpoint = endpoint

            If _endpoint.StartsWith("/") = False Then
                _endpoint = "/" & _endpoint
            End If
            If _endpoint.EndsWith("/") = False Then
                _endpoint = _endpoint & "/"
            End If

            AddURLParameter("game_id", _GameID)
        End Sub

        ''' <summary>
        ''' Adds a URL parameter to the request's URL.
        ''' </summary>
        ''' <param name="key">The key of the parameter.</param>
        ''' <param name="value">The value of the parameter.</param>
        Private Sub AddURLParameter(ByVal key As String, ByVal value As String)
            _urlParameters.Add(key, value)
        End Sub

        ''' <summary>
        ''' Executes the constructed API request.
        ''' </summary>
        Public Function Execute(ByVal returnFormat As RequestFormat) As RequestResult
            _returnFormat = returnFormat

            ExecuteInternal()

            Return _requestResult
        End Function

        ''' <summary>
        ''' Executes the constructed API request asynchronously.
        ''' </summary>
        Public Sub ExecuteAsync(ByVal returnFormat As RequestFormat)
            _returnFormat = returnFormat

            Dim t As New Thread(AddressOf ExecuteInternal)
            t.IsBackground = True
            t.Start()
        End Sub

        Private _requestResult As RequestResult
        Private _returnFormat As RequestFormat

        Private Sub ExecuteInternal()
            Dim url As String = getUrl()

            If _requestType = RequestType.GET Then
                Try
                    Dim getRequest As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
                    getRequest.Method = "GET"

                    Dim getResponse As HttpWebResponse = CType(getRequest.GetResponse(), HttpWebResponse)

                    Dim resultData As String = New StreamReader(getResponse.GetResponseStream()).ReadToEnd()

                    _requestResult = New RequestResult(RequestType.GET, RequestStatus.Success, resultData)
                Catch ex As Exception
                    _requestResult = New RequestResult(RequestType.GET, New RequestException(ex))
                End Try
            ElseIf _requestType = RequestType.POST Then
                Try
                    Dim postContent As String = "data=" & _postData

                    Dim postRequest As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
                    postRequest.AllowWriteStreamBuffering = True
                    postRequest.Method = "POST"
                    postRequest.ContentLength = postContent.Length
                    postRequest.ContentType = "application/x-www-form-urlencoded"
                    postRequest.ServicePoint.Expect100Continue = True

                    Dim postWriter As New StreamWriter(postRequest.GetRequestStream())
                    postWriter.Write(postContent)
                    postWriter.Close()

                    Dim postResponse As HttpWebResponse = CType(postRequest.GetResponse(), HttpWebResponse)

                    Dim resultData As String = New StreamReader(postResponse.GetResponseStream()).ReadToEnd()
                    _requestResult = New RequestResult(RequestType.POST, RequestStatus.Success, resultData)
                Catch ex As Exception
                    _requestResult = New RequestResult(RequestType.POST, New RequestException(ex))
                End Try
            End If

            RaiseEvent Finished(_requestResult)
        End Sub

        Private Function getUrl() As String
            'Construct URL first:
            Dim urlSB As New StringBuilder()
            urlSB.Append(HOST & API_VERSION & _endpoint)

            'append format to url:
            AddURLParameter("format", _returnFormat.ToString().ToLower())

            For i = 0 To _urlParameters.Count - 1
                Dim appendStr As String = ""
                If i = 0 Then
                    urlSB.Append("?")
                Else
                    urlSB.Append("&")
                End If
                urlSB.Append(_urlParameters.Keys(i) & "=" &
                             UrlEncoder.Encode(_urlParameters.Values(i)))
            Next

            Dim url As String = urlSB.ToString()

            'append signature to URL:
            url &= "&signature=" & getUrlSignature(url)

            Return url
        End Function

        Private Function getUrlSignature(ByVal url As String) As String
            'compute hash for signature:
            Dim data As Byte() = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(url & _GameKey))
            Dim signatureSB As New StringBuilder()

            For i = 0 To data.Length - 1
                signatureSB.Append(data(i).ToString("x2"))
            Next

            Return signatureSB.ToString()
        End Function

    End Class

    ''' <summary>
    ''' A class that contains the result of an API request.
    ''' </summary>
    Public NotInheritable Class RequestResult

        Private _requestStatus As RequestStatus
        Private _requestData As String
        Private _requestType As RequestType

        Private _exception As RequestException = Nothing

        ''' <summary>
        ''' Creates a new instance of the RequestResult class.
        ''' </summary>
        ''' <param name="type">The type of the request.</param>
        ''' <param name="status">The status of the request.</param>
        ''' <param name="data">The result data of the request.</param>
        Public Sub New(ByVal type As RequestType, ByVal status As RequestStatus, ByVal data As String)
            _requestStatus = status
            _requestData = data
            _requestType = type
        End Sub

        ''' <summary>
        ''' Creates a new instance of the RequestResult class for when the request failed.
        ''' </summary>
        ''' <param name="type">The type of the request.</param>
        ''' <param name="exception">The exception that occurred.</param>
        Public Sub New(ByVal type As RequestType, ByVal exception As RequestException)
            Me.New(type, RequestStatus.Failure, "")
            _exception = exception
        End Sub

        ''' <summary>
        ''' The result data of the request.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Data() As String
            Get
                Return _requestData
            End Get
        End Property

        ''' <summary>
        ''' The status of the request.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Status() As RequestStatus
            Get
                Return _requestStatus
            End Get
        End Property

        ''' <summary>
        ''' The type of the request.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property RequestType() As RequestType
            Get
                Return _requestType
            End Get
        End Property

        ''' <summary>
        ''' An exception that might have occured during the request. This is null when the request was not a failure.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Exception() As RequestException
            Get
                Return _exception
            End Get
        End Property

    End Class

    ''' <summary>
    ''' An exception that occured during an API request.
    ''' </summary>
    Public NotInheritable Class RequestException

        Inherits Exception

        ''' <summary>
        ''' Creates a new instance of the RequestException class.
        ''' </summary>
        ''' <param name="innerException">The inner exception that caused the problem.</param>
        Public Sub New(ByVal innerException As Exception)
            MyBase.New("A problem occured while making a request to the GameJolt API.", innerException)
        End Sub

    End Class

End Namespace