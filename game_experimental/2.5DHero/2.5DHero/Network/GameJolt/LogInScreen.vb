Namespace GameJolt

    Public Class LogInScreen

        Inherits Screen

        Public Shared BanList As String = ""
        Public Shared BanReasons As String = ""

        Public Shared LoadedGameJoltID As String = ""

        Dim UserName As JoltTextBox
        Dim Token As JoltTextBox

        Dim LogInButton As JoltButton
        Dim CloseButton As JoltButton
        Dim CreateAccountButton As JoltButton
        Dim OkButton As JoltButton

        Dim WaitingForResponse As Boolean = False
        Dim WaitingMessage As String = "Please wait..."
        Dim ShowokButton As Boolean = True
        Dim TimeOut As Integer = 0
        Const TimeOutVar As Integer = 500

        Dim DownloadedBanList As Boolean = False

        Dim _tempCloseScreen As Boolean = False 'To prevent the screen closing and so mouse visibility change from a different thread than main.

        Public Sub New(ByVal currentScreen As Screen)
            PreScreen = currentScreen

            Identification = Identifications.GameJoltLoginScreen
            MouseVisible = True
            CanBePaused = False
            CanChat = False
            CanMuteMusic = False

            UserName = New JoltTextBox(FontManager.MainFont, Color.Black, Color.White)
            UserName.Size = New Size(400, 30)

            Token = New JoltTextBox(FontManager.MainFont, Color.Black, Color.White)
            Token.Size = New Size(400, 30)
            Token.IsPassword = True

            LogInButton = New JoltButton("Log in", FontManager.MainFont, New Color(68, 68, 68), New Color(204, 255, 0))
            LogInButton.Size = New Size(100, 30)
            LogInButton.SetDelegate(AddressOf LogIn)

            CloseButton = New JoltButton("Close", FontManager.MainFont, New Color(68, 68, 68), New Color(204, 255, 0))
            CloseButton.Size = New Size(100, 30)
            CloseButton.SetDelegate(AddressOf Close)

            CreateAccountButton = New JoltButton("Create Account", FontManager.MainFont, New Color(68, 68, 68), New Color(204, 255, 0))
            CreateAccountButton.Size = New Size(180, 30)
            CreateAccountButton.SetDelegate(AddressOf CreateAccount)

            OkButton = New JoltButton("OK", FontManager.MainFont, New Color(68, 68, 68), New Color(204, 255, 0))
            OkButton.Size = New Size(100, 30)
            OkButton.SetDelegate(AddressOf PressOK)

            UpdatePosition()

            UserName.IsActive = True

            If API.LoggedIn = True Then
                UserName.Text = API.username
                Token.Text = API.token

                LogInButton.Text = "Log out"
            Else
                LoadSettings()
            End If

            Dim t As New Threading.Thread(AddressOf DownloadBanList)
            t.IsBackground = True
            t.Start()
        End Sub

        Private Sub SaveSettings()
            If API.LoggedIn = True Then
                Dim cUsername As String = Encryption.EncryptString(UserName.Text, "")  ' CLASSIFIED
                Dim cToken As String = Encryption.EncryptString(Token.Text, "")  ' CLASSIFIED

                IO.File.WriteAllText(GameController.GamePath & "\Save\gamejoltAcc.dat", cUsername & vbNewLine & cToken)
            End If
        End Sub

        Private Sub LoadSettings()
            If IO.File.Exists(GameController.GamePath & "\Save\gamejoltAcc.dat") = True Then
                Dim content() As String = IO.File.ReadAllLines(GameController.GamePath & "\Save\gamejoltAcc.dat")

                If content.Length >= 2 Then
                    Try
                        UserName.Text = Encryption.DecryptString(content(0), "")  ' CLASSIFIED
                        Token.Text = Encryption.DecryptString(content(1), "") ' CLASSIFIED

                        Deactivate()
                        LogInButton.IsActive = True
                    Catch ex As Exception
                        IO.File.Delete(GameController.GamePath & "\Save\gamejoltAcc.dat")
                        Logger.Log("241", Logger.LogTypes.Warning, "Cannot read GameJolt account settings!")
                    End Try
                End If
            End If
        End Sub

        Public Overrides Sub Draw()
            PreScreen.Draw()

            Canvas.DrawRectangle(ScreenSize, New Color(0, 0, 0, 150), True)
            Canvas.DrawRectangle(New Rectangle(CInt(ScreenSize.Width / 2 - 310), 90, 620, 420), New Color(16, 16, 16), True)
            Canvas.DrawRectangle(New Rectangle(CInt(ScreenSize.Width / 2 - 300), 100, 600, 400), New Color(39, 39, 39), True)

            If DownloadedBanList = True Then
                SpriteBatch.DrawInterfaceString(FontManager.InGameFont, "Sign in with", New Vector2(CSng(ScreenSize.Width / 2 - 280), 130), Color.White)
                SpriteBatch.DrawInterface(Content.Load(Of Texture2D)("SharedResources\Textures\Logos\GameJolt"), New Rectangle(CInt(ScreenSize.Width / 2 - 120), 130, 328, 36), Color.White)

                If WaitingForResponse = True Then
                    Dim textSize As Vector2 = FontManager.MainFont.MeasureString(WaitingMessage)

                    SpriteBatch.DrawInterfaceString(FontManager.MainFont, WaitingMessage, New Vector2(CSng(ScreenSize.Width / 2 - textSize.X / 2), 310 - textSize.Y / 2), Color.White)

                    If ShowokButton = True Then
                        OkButton.Draw()
                    End If
                Else
                    SpriteBatch.DrawInterfaceString(FontManager.MiniFont, "Username:", New Vector2(CSng(ScreenSize.Width / 2) - 200, 195), Color.White)
                    SpriteBatch.DrawInterfaceString(FontManager.MiniFont, "Token:", New Vector2(CSng(ScreenSize.Width / 2) - 200, 275), Color.White)

                    UserName.Draw()
                    Token.Draw()
                    LogInButton.Draw()
                    CloseButton.Draw()
                    CreateAccountButton.Draw()
                End If
            Else
                SpriteBatch.DrawInterfaceString(FontManager.MiniFont, "Please wait" & LoadingDots.Dots, New Vector2(CSng(ScreenSize.Width / 2) - 200, 195), Color.White)
            End If
        End Sub

        Public Overrides Sub Update()
            If _tempCloseScreen = True Then
                SetScreen(PreScreen)
            End If

            If DownloadedBanList = True Then
                UpdatePosition()

                If WaitingForResponse = True Then
                    If ShowokButton = True Then
                        If Controls.Accept(True, False) = True Then
                            Select Case True
                                Case ScaleScreenRec(OkButton.GetRectangle()).Contains(MouseHandler.MousePosition)
                                    If OkButton.IsActive = True Then
                                        OkButton.DoPress()
                                    Else
                                        Deactivate()
                                        OkButton.IsActive = True
                                    End If
                            End Select
                        End If
                    Else
                        TimeOut -= 1
                        If TimeOut <= 0 Then
                            ShowokButton = True
                            WaitingMessage = "Error: Server timeout."
                        End If
                        If Not API.Exception Is Nothing Then
                            ShowokButton = True
                            WaitingMessage = "Error: " & API.Exception.Message
                        End If
                    End If
                Else
                    PressTab()

                    UserName.Update()
                    Token.Update()

                    LogInButton.Update()
                    CloseButton.Update()
                    CreateAccountButton.Update()

                    If Controls.Accept(True, False, False) = True Then
                        Select Case True
                            Case ScaleScreenRec(UserName.GetRectangle()).Contains(MouseHandler.MousePosition)
                                Deactivate()
                                UserName.IsActive = True
                            Case ScaleScreenRec(Token.GetRectangle()).Contains(MouseHandler.MousePosition)
                                Deactivate()
                                Token.IsActive = True
                            Case ScaleScreenRec(LogInButton.GetRectangle()).Contains(MouseHandler.MousePosition)
                                If LogInButton.IsActive = True Then
                                    LogInButton.DoPress()
                                Else
                                    Deactivate()
                                    LogInButton.IsActive = True
                                End If
                            Case ScaleScreenRec(CloseButton.GetRectangle()).Contains(MouseHandler.MousePosition)
                                If CloseButton.IsActive = True Then
                                    CloseButton.DoPress()
                                Else
                                    Deactivate()
                                    CloseButton.IsActive = True
                                End If
                            Case ScaleScreenRec(CreateAccountButton.GetRectangle()).Contains(MouseHandler.MousePosition)
                                If CreateAccountButton.IsActive = True Then
                                    CreateAccountButton.DoPress()
                                Else
                                    Deactivate()
                                    CreateAccountButton.IsActive = True
                                End If
                        End Select
                    End If
                    If Controls.Accept(False, False, True) = True Or KeyBoardHandler.KeyPressed(KeyBindings.EnterKey1) = True Then
                        Select Case True
                            Case UserName.IsActive
                                UserName.DoPress()
                            Case Token.IsActive
                                Token.DoPress()
                            Case LogInButton.IsActive
                                LogInButton.DoPress()
                            Case CloseButton.IsActive
                                CloseButton.DoPress()
                            Case CreateAccountButton.IsActive
                                CreateAccountButton.DoPress()
                        End Select
                    End If
                    If Controls.Dismiss(True, False, True) = True Then
                        SetScreen(PreScreen)
                    End If
                End If
            Else
                If Controls.Dismiss() = True Then
                    SetScreen(PreScreen)
                End If
            End If
        End Sub

        Private Sub PressTab()
            Dim direction As Integer = 0

            If KeyBoardHandler.KeyPressed(Keys.Tab) = True Or Controls.Down(True, True, True, False, True, True) = True Then
                direction = 1
            End If
            If Controls.Up(True, True, True, False, True, True) = True Then
                direction = -1
            End If

            If direction <> 0 Then
                Dim l As JoltControl() = {UserName, Token, LogInButton, CreateAccountButton, CloseButton}

                For i = 0 To l.Length - 1
                    If l(i).IsActive = True Then
                        Deactivate()
                        Dim activateIndex As Integer = i + direction
                        If activateIndex > l.Length - 1 Then
                            activateIndex = 0
                        End If
                        If activateIndex < 0 Then
                            activateIndex = l.Length - 1
                        End If
                        l(activateIndex).IsActive = True
                        Exit For
                    End If
                Next
            End If
        End Sub

        Private Sub UpdatePosition()
            UserName.Position = New Vector2(CSng(ScreenSize.Width / 2) - CSng(UserName.Size.Width / 2), 220)
            Token.Position = New Vector2(CSng(ScreenSize.Width / 2) - CSng(Token.Size.Width / 2), 300)

            LogInButton.Position = New Vector2(CSng(ScreenSize.Width / 2) - CSng(LogInButton.Size.Width / 2) - 150, 380)
            CreateAccountButton.Position = New Vector2(CSng(ScreenSize.Width / 2) - CSng(CreateAccountButton.Size.Width / 2), 380)
            CloseButton.Position = New Vector2(CSng(ScreenSize.Width / 2) - CSng(CloseButton.Size.Width / 2) + 150, 380)
            OkButton.Position = New Vector2(CSng(ScreenSize.Width / 2) - CSng(OkButton.Size.Width / 2), 400)
        End Sub

        Private Sub Deactivate()
            UserName.IsActive = False
            Token.IsActive = False
            LogInButton.IsActive = False
            CloseButton.IsActive = False
            CreateAccountButton.IsActive = False
            OkButton.IsActive = False
        End Sub

        Private Sub Close()
            SaveSettings()
            SetScreen(PreScreen)
        End Sub

        Private Sub CreateAccount()
            Process.Start("http://www.gamejolt.com/auth/sign-up/")
        End Sub

        Private Sub LogIn()
            If API.LoggedIn = True Then
                SessionManager.Close()

                API.LoggedIn = False
                API.username = ""
                API.token = ""

                UserName.Text = ""
                Token.Text = ""

                LogInButton.Text = "Log in"

                If IO.File.Exists(GameController.GamePath & "\Save\gamejoltAcc.dat") = True Then
                    IO.File.Delete(GameController.GamePath & "\Save\gamejoltAcc.dat")
                End If
            Else
                Dim APICall As New APICall(AddressOf VerifyVersion)
                APICall.GetStorageData("ONLINEVERSION", False)

                WaitingMessage = "Please wait..."

                WaitingForResponse = True
                ShowokButton = False
                TimeOut = TimeOutVar
            End If
        End Sub

        Private Sub VerifyVersion(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)
            If CBool(list(0).Value) = True Then
                If list(1).Value = GameController.GAMEVERSION Or GameController.IS_DEBUG_ACTIVE = True Then
                    Dim APICall As New APICall(AddressOf VerifyResult)
                    APICall.VerifyUser(UserName.Text, Token.Text)
                Else
                    WaitingForResponse = True
                    API.LoggedIn = False

                    WaitingMessage = "The version of your game does not match with" & vbNewLine & "the version required to play online. If you have" & vbNewLine & "the lastest version of the game, the game is" & vbNewLine & "getting updated right now." & vbNewLine & vbNewLine & vbNewLine & "Your version: " & GameController.GAMEVERSION & vbNewLine & "Required version: " & list(1).Value
                    ShowokButton = True

                    LogInButton.Text = "Log in"
                End If
            End If
        End Sub

        Private Sub VerifyResult(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)
            If CBool(list(0).Value) = True Then
                API.LoggedIn = True

                Dim APICall As New APICall(AddressOf HandleUserData)
                APICall.FetchUserdata(API.username)
            Else
                WaitingForResponse = True
                API.LoggedIn = False

                WaitingMessage = "Cannot connect to account!" & vbNewLine & "You have to use your Token," & vbNewLine & "not your Password."
                ShowokButton = True

                LogInButton.Text = "Log in"
            End If
        End Sub

        Private Sub HandleUserData(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)
            For Each Item As API.JoltValue In list
                If Item.Name = "id" Then
                    LoadedGameJoltID = Item.Value 'set the public shared field to the GameJolt ID.

                    If GameController.UPDATEONLINEVERSION = True And GameController.IS_DEBUG_ACTIVE = True Then
                        Dim APICall As New APICall
                        APICall.SetStorageDataRestricted("ONLINEVERSION", GameController.GAMEVERSION)
                        Logger.Debug("140", "UPDATED ONLINE VERSION TO: " & GameController.GAMEVERSION)
                    End If

                    LogInButton.Text = "Log out"
                    SaveSettings()
                    _tempCloseScreen = True

                    Exit For
                End If
            Next
        End Sub

        Private Sub PressOK()
            WaitingForResponse = False
            OkButton.IsActive = False
            ShowokButton = False
        End Sub

        Public Shared ReadOnly Property UserBanned(ByVal GameJoltID As String) As Boolean
            Get
                Dim ID_list() As String = BanList.SplitAtNewline()

                For i As Integer = 0 To ID_list.Count() - 1
                    If ID_list(i).GetSplit(0, "|") = GameJoltID Then
                        Return True
                    End If
                Next

                Return False
            End Get
        End Property

        Public Shared ReadOnly Property BanReasonIDForUser(ByVal User_ID As String) As String
            Get
                Dim ID_list() As String = BanList.SplitAtNewline()

                For i As Integer = 0 To ID_list.Count() - 1
                    If ID_list(i).GetSplit(0, "|") = User_ID Then
                        Return ID_list(i).GetSplit(1, "|")
                    End If
                Next

                Return "0"
            End Get
        End Property

        Public Shared ReadOnly Property GetBanReasonByID(ByVal banReasonID As String) As String
            Get
                For Each reasonString As String In BanReasons.SplitAtNewline()
                    Dim reason As String = reasonString.GetSplit(1, "|")
                    Dim reasonID As String = reasonString.GetSplit(0, "|")

                    If reasonID = banReasonID Then
                        Return reason
                    End If
                Next

                Return ""
            End Get
        End Property

        Private Sub DownloadBanList()
            Try
                Dim w As New Net.WebClient
                BanList = w.DownloadString("") ' CLASSIFIED
                BanReasons = w.DownloadString("") ' CLASSIFIED
                Logger.Log("242", Logger.LogTypes.Message, "Retrieved ban list data.")
                DownloadedBanList = True
            Catch ex As Exception
                Logger.Log("243", Logger.LogTypes.ErrorMessage, "Failed to fetch ban list data!")
            End Try
        End Sub

        ''' <summary>
        ''' This gets called from all GameJolt screens. If the player is no longer connected to GameJolt, it opens up the login screen.
        ''' </summary>
        ''' <param name="SetToScreen"></param>
        Public Shared Sub KickFromOnlineScreen(ByVal SetToScreen As Screen)
            If Core.Player.IsGameJoltSave = True AndAlso API.LoggedIn = False Then
                SetScreen(New LogInScreen(SetToScreen))
            End If
        End Sub

        Private Class JoltControl

            Public IsActive As Boolean = False

        End Class

        Private Class JoltTextBox

            Inherits JoltControl

            Dim _text As String = ""
            Dim _password As Boolean = False

            Dim _backcolor As Color
            Dim _forecolor As Color

            Dim _font As SpriteFont

            Public Position As New Vector2(0)
            Public Size As New Size(0, 0)
            Public MaxChars As Integer = -1

            Public Sub New(ByVal Font As SpriteFont, ByVal BackColor As Color, ByVal FontColor As Color)
                _font = Font
                _backcolor = BackColor
                _forecolor = FontColor
            End Sub

            Public Property IsPassword() As Boolean
                Get
                    Return _password
                End Get
                Set(value As Boolean)
                    _password = value
                End Set
            End Property

            Public Property Text() As String
                Get
                    Return _text
                End Get
                Set(value As String)
                    _text = value
                End Set
            End Property

            Public Property BackColor() As Color
                Get
                    Return _backcolor
                End Get
                Set(value As Color)
                    _backcolor = value
                End Set
            End Property

            Public Property FontColor() As Color
                Get
                    Return _forecolor
                End Get
                Set(value As Color)
                    _forecolor = value
                End Set
            End Property

            Public Property Font() As SpriteFont
                Get
                    Return _font
                End Get
                Set(value As SpriteFont)
                    _font = value
                End Set
            End Property

            Public Sub Draw()
                Dim useColor As Color = _backcolor
                Dim useFontColor As Color = _forecolor
                If IsActive = True Then
                    useColor = _forecolor
                    useFontColor = _backcolor
                End If

                Canvas.DrawRectangle(New Rectangle(CInt(Position.X), CInt(Position.Y), Size.Width, Size.Height), useColor, True)

                Dim useText As String = _text
                If _password = True Then
                    useText = ""
                    For i = 0 To _text.Length - 1
                        useText &= "x"
                    Next
                End If

                If IsActive = True Then
                    If MaxChars < 0 Or MaxChars > _text.Length Then
                        useText &= "_"
                    End If
                End If

                SpriteBatch.DrawInterfaceString(_font, useText, Position, useFontColor)
            End Sub

            Public Sub Update()
                If IsActive = True Then
                    KeyBindings.GetInput(_text, MaxChars, True, True)
                End If
            End Sub

            Public Function GetRectangle() As Rectangle
                Return New Rectangle(CInt(Position.X), CInt(Position.Y), Size.Width, Size.Height)
            End Function

            Public Sub DoPress()
                SetScreen(New InputScreen(CurrentScreen, "", InputScreen.InputModes.Text, _text, 32, New List(Of Texture2D), New InputScreen.ConfirmInput(AddressOf ReturnSetText)) With {.PasswordMode = _password})
            End Sub

            Private Sub ReturnSetText(ByVal result As String)
                _text = result
            End Sub

        End Class

        Private Class JoltButton

            Inherits JoltControl

            Dim _text As String
            Dim _backColor As Color
            Dim _textColor As Color
            Dim _font As SpriteFont

            Public Position As Vector2 = New Vector2(0)
            Public Size As Size = New Size(0, 0)

            Public Visible As Boolean = True

            Public Delegate Sub Press()

            Public DoPress As Press

            Public Sub New(ByVal Text As String, ByVal Font As SpriteFont, ByVal BackColor As Color, ByVal TextColor As Color)
                _text = Text
                _backColor = BackColor
                _textColor = TextColor
                _font = Font
            End Sub

            Public Sub SetDelegate(ByVal DelegateSub As Press)
                DoPress = DelegateSub
            End Sub

            Public Sub Update()
            End Sub

            Public Sub Draw()
                If Visible = True Then
                    Dim useColor As Color = _backColor
                    Dim useFontColor As Color = _textColor
                    If IsActive = True Then
                        useColor = _textColor
                        useFontColor = _backColor
                    End If

                    Canvas.DrawRectangle(New Rectangle(CInt(Position.X), CInt(Position.Y), Size.Width, Size.Height), useColor, True)

                    SpriteBatch.DrawInterfaceString(_font, _text, New Vector2(Position.X + CSng(Size.Width / 2 - _font.MeasureString(_text).X / 2), Position.Y + CSng(Size.Height / 2 - _font.MeasureString(_text).Y / 2)), useFontColor)
                End If
            End Sub

            Public Property Text As String
                Get
                    Return _text
                End Get
                Set(value As String)
                    _text = value
                End Set
            End Property

            Public Function GetRectangle() As Rectangle
                Return New Rectangle(CInt(Position.X), CInt(Position.Y), Size.Width, Size.Height)
            End Function

        End Class

    End Class

    ''' <summary>
    ''' The screen to prompt the user to log in to GameJolt.
    ''' </summary>
    Public Class NewLogInScreen

        'TODO: Replace old Loginscreen.

        Inherits Screen

        Private Shared _banList As DataModel.Json.GameJolt.BanListEntryModel()
        Private Shared _banReasons As DataModel.Json.GameJolt.BanReasonEntryModel()

        Const BANLIST_URL As String = "" ' CLASSIFIED
        Const BANREASONS_URL As String = "" ' CLASSIFIED

        Private ReadOnly LOADING_STRINGS As String() = {"Projecting hoopla matrix...", "Deploying amusement modules...", "Connecting to mainframe..."}

        Private ReadOnly LEFT_GRADIENT_COLOR As Color = New Color(241, 153, 184, 120)
        Private ReadOnly RIGHT_GRADIENT_COLOR As Color = New Color(183, 241, 255, 120)

        Dim _usernameInitial As Boolean = True
        Dim _passwordInitial As Boolean = True

        Private _gameJoltLogo As Texture2D = Nothing
        Private _loadingTextures As Texture2D()
        Private _gameJoltLogin As Texture2D = Nothing
        Private _gameJoltX As Texture2D = Nothing
        Private _gameJoltQuestion As Texture2D = Nothing

        Private _fadeIn As Single = 0F
        Private _closing As Boolean = False

        Private _loadingIndex As Integer = 0
        Private _loadingDelay As Single = 1.0F
        Private _isLoading As Boolean = True
        Private _loadingString As String = ""

        Private _showMessage As Boolean = False
        Private _isError As Boolean = True
        Private _message As String = ""
        Private _canCloseMessage As Boolean = False

        Private WithEvents _usernameBox As New UI.GameControls.Textbox(Me, FontManager.GameJoltFont)
        Private WithEvents _passwordBox As New UI.GameControls.Textbox(Me, FontManager.GameJoltFont)
        Private WithEvents _loginButton As New UI.GameControls.Button(Me, FontManager.GameJoltFont)
        Private WithEvents _closeButton As New UI.GameControls.Button(Me, FontManager.GameJoltFont)

        Private _messageBox As New UI.MessageBox(Me)

        Private _controls As New UI.GameControls.ControlList()

        Public Sub New(ByVal currentScreen As Screen)
            PreScreen = currentScreen
            Identification = Identifications.GameJoltLoginScreen
            MouseVisible = True
            CanBePaused = False
            CanChat = False
            CanMuteMusic = False

            With Content
                _loadingTextures = { .Load(Of Texture2D)("SharedResources\Textures\UI\GameJolt\Loading\0"),
                                     .Load(Of Texture2D)("SharedResources\Textures\UI\GameJolt\Loading\1"),
                                     .Load(Of Texture2D)("SharedResources\Textures\UI\GameJolt\Loading\2"),
                                     .Load(Of Texture2D)("SharedResources\Textures\UI\GameJolt\Loading\3")}

                _gameJoltLogo = .Load(Of Texture2D)("SharedResources\Textures\UI\GameJolt\logo")
                _gameJoltLogin = .Load(Of Texture2D)("SharedResources\Textures\UI\GameJolt\login")
                _gameJoltX = .Load(Of Texture2D)("SharedResources\Textures\UI\GameJolt\x")
                _gameJoltQuestion = .Load(Of Texture2D)("SharedResources\Textures\UI\GameJolt\question")
            End With
            _loadingString = LOADING_STRINGS(Random.Next(0, LOADING_STRINGS.Length))

            _usernameBox.Text = "Username"
            _usernameBox.FontSize = 0.5F
            _usernameBox.FontColor = New Color(100, 100, 100)
            _usernameBox.BorderWidth = 2
            _usernameBox.BorderColor = New Color(193, 193, 193)
            _usernameBox.Width = 350
            _usernameBox.Height = 40
            _usernameBox.HorizonzalTextPadding = 10
            _usernameBox.VerticalTextPadding = 10
            _usernameBox.InputType = KeyboardInput.InputModifier.GameJolt

            _passwordBox.Text = "Token"
            _passwordBox.FontSize = 0.5F
            _passwordBox.FontColor = New Color(100, 100, 100)
            _passwordBox.BorderWidth = 2
            _passwordBox.BorderColor = New Color(193, 193, 193)
            _passwordBox.Width = 350
            _passwordBox.Height = 40
            _passwordBox.HorizonzalTextPadding = 10
            _passwordBox.VerticalTextPadding = 10

            _loginButton.Text = "LOG IN"
            _loginButton.Image = _gameJoltLogin
            _loginButton.Font = FontManager.GameJoltFont
            _loginButton.FontColor = Color.White
            _loginButton.BackColor = New Color(85, 85, 85)
            _loginButton.SelectedBackColor = New Color(47, 127, 111)
            _loginButton.SelectedFontColor = New Color(204, 255, 0)
            _loginButton.Width = 350
            _loginButton.FontSize = 0.5F

            _closeButton.Text = "CLOSE"
            _closeButton.Image = _gameJoltX
            _closeButton.Font = FontManager.GameJoltFont
            _closeButton.FontColor = Color.White
            _closeButton.BackColor = New Color(85, 85, 85)
            _closeButton.SelectedBackColor = New Color(47, 127, 111)
            _closeButton.SelectedFontColor = New Color(204, 255, 0)
            _closeButton.Width = 350
            _closeButton.FontSize = 0.5F

            _controls.AddRange({_usernameBox, _passwordBox, _loginButton, _closeButton})

            SetControlPosition()

            'Start downloading ban list data:
            Dim t As New Threading.Thread(AddressOf DownloadBanList)
            t.IsBackground = True
            t.Start()
        End Sub

        Public Overrides Sub Draw()
            PreScreen.Draw()

            Canvas.DrawRectangle(windowSize, New Color(0, 0, 0, CInt(140 * _fadeIn)))

            Dim _leftColor As Color = LEFT_GRADIENT_COLOR
            _leftColor.A = CByte(_leftColor.A * _fadeIn)

            Dim _rightColor As Color = RIGHT_GRADIENT_COLOR
            _rightColor.A = CByte(_rightColor.A * _fadeIn)

            Canvas.DrawGradient(New Rectangle(0, 0, CInt(windowSize.Width / 2), windowSize.Height), _leftColor, New Color(255, 255, 255, 0), True, -1)
            Canvas.DrawGradient(New Rectangle(CInt(windowSize.Width / 2), 0, CInt(windowSize.Width / 2), windowSize.Height), New Color(255, 255, 255, 0), _rightColor, True, -1)

            Canvas.DrawRectangle(New Rectangle(CInt(windowSize.Width / 2 - 200), CInt(160 + 20 * _fadeIn), 400, 350), New Color(25, 25, 25, CInt(255 * _fadeIn)))

            SpriteBatch.Draw(_gameJoltLogo, New Rectangle(CInt(windowSize.Width / 2 - 164), CInt(190 + 20 * _fadeIn), 328, 36), New Color(255, 255, 255, CInt(255 * _fadeIn)))

            If _isLoading Then
                DrawLoading()
            Else
                If _showMessage Then
                    DrawMessage()
                ElseIf _closing = False Then
                    _usernameBox.Draw()
                    _passwordBox.Draw()
                    _loginButton.Draw()
                    _closeButton.Draw()
                End If
            End If
        End Sub

        Private Sub DrawLoading()
            SpriteBatch.Draw(_loadingTextures(_loadingIndex), New Rectangle(CInt(windowSize.Width / 2 - 88), CInt(300 + 20 * _fadeIn), 176, 56), New Color(255, 255, 255, CInt(255 * _fadeIn)))

            GetFontRenderer().DrawString(FontManager.GameJoltFont, _loadingString, New Vector2(windowSize.Width / 2.0F - FontManager.GameJoltFont.MeasureString(_loadingString).X / 4.0F, CInt(390 + 20 * _fadeIn)), New Color(255, 255, 255, CInt(255 * _fadeIn)), 0F, Vector2.Zero, 0.5F, SpriteEffects.None, 0F)
        End Sub

        Private Sub DrawMessage()
            Canvas.DrawRectangle(New Rectangle(CInt(windowSize.Width / 2 - 164), CInt(310 + 20 * _fadeIn), 328, 56), New Color(255, 63, 172, CInt(255 * _fadeIn)))

            If _isError = True Then
                SpriteBatch.Draw(_gameJoltQuestion, New Rectangle(CInt(windowSize.Width / 2 - FontManager.TextFont.MeasureString(_message).X / 2 - 14), CInt(328 + 20 * _fadeIn), 20, 20), New Color(255, 255, 255, CInt(255 * _fadeIn)))

                GetFontRenderer().DrawString(FontManager.TextFont, _message, New Vector2(windowSize.Width / 2.0F - FontManager.TextFont.MeasureString(_message).X / 2.0F + 14, CInt(330 + 20 * _fadeIn)), New Color(255, 255, 255, CInt(255 * _fadeIn)))
            Else
                GetFontRenderer().DrawString(FontManager.TextFont, _message, New Vector2(windowSize.Width / 2.0F - FontManager.TextFont.MeasureString(_message).X / 2.0F, CInt(330 + 20 * _fadeIn)), New Color(255, 255, 255, CInt(255 * _fadeIn)))
            End If
        End Sub

        Public Overrides Sub Update()
            If _closing Then
                If _fadeIn > 0.0F Then
                    _fadeIn = MathHelper.Lerp(0.0F, _fadeIn, 0.8F)
                    If _fadeIn - 0.01F <= 0.0F Then
                        _fadeIn = 0.0F
                    End If
                Else
                    SetScreen(PreScreen)
                End If
            Else
                If _fadeIn < 1.0F Then
                    _fadeIn = MathHelper.Lerp(1.0F, _fadeIn, 0.95F)
                    If _fadeIn > 1.0F Then
                        _fadeIn = 1.0F
                    End If
                End If

                SetControlPosition()

                If _isLoading = False And _showMessage = False Then
                    _messageBox.Update()

                    _controls.Update()

                    _usernameBox.Update()
                    _passwordBox.Update()
                    _loginButton.Update()
                    _closeButton.Update()
                End If

                _loadingDelay -= 0.1F
                If _loadingDelay <= 0F Then
                    _loadingDelay = 1.0F
                    _loadingIndex += 1
                    If _loadingIndex = 4 Then
                        _loadingIndex = 0
                    End If
                End If

                If Controls.Dismiss(True, False, True) OrElse (_isError = False AndAlso Controls.Accept()) Then
                    If _showMessage And _canCloseMessage Then
                        _showMessage = False
                    Else
                        CloseScreen()
                    End If
                End If
            End If
        End Sub

        Protected Overrides Function GetFontRenderer() As SpriteBatch
            If IsCurrentScreen() Then
                If _closing Then
                    Return SpriteBatch
                Else
                    Return FontRenderer
                End If
            Else
                Return SpriteBatch
            End If
        End Function

        Private Sub CloseScreen()
            _closing = True
            _fadeIn = 1.0F
        End Sub

        Private Sub SetControlPosition()
            _usernameBox.Position = New Drawing.Point(CInt(windowSize.Width / 2.0 - 175), CInt(250 + 20 * _fadeIn))
            _passwordBox.Position = New Drawing.Point(CInt(windowSize.Width / 2.0 - 175), CInt(304 + 20 * _fadeIn))
            _loginButton.Position = New Drawing.Point(CInt(windowSize.Width / 2.0 - 175), CInt(380 + 20 * _fadeIn))
            _closeButton.Position = New Drawing.Point(CInt(windowSize.Width / 2.0 - 175), CInt(430 + 20 * _fadeIn))
        End Sub

        Private Sub Username_Focus(ByVal sender As Object, ByVal e As EventArgs) Handles _usernameBox.Focused
            If _usernameInitial Then
                _usernameInitial = False
                _usernameBox.Text = ""
            End If
        End Sub

        Private Sub Username_DeFocus(ByVal sender As Object, ByVal e As EventArgs) Handles _usernameBox.DeFocused
            If _usernameBox.Text = "" Then
                _usernameInitial = True
                _usernameBox.Text = "Username"
            End If
        End Sub

        Private Sub FocusedPassword(ByVal sender As Object, ByVal e As EventArgs) Handles _passwordBox.Focused
            If _passwordInitial Then
                _passwordInitial = False
                _passwordBox.Text = ""
                _passwordBox.IsPassword = True
            End If
        End Sub

        Private Sub Password_DeFocus(ByVal sender As Object, ByVal e As EventArgs) Handles _passwordBox.DeFocused
            If _passwordBox.Text = "" Then
                _passwordInitial = True
                _passwordBox.Text = "Token"
                _passwordBox.IsPassword = False
            End If
        End Sub

        Private Sub CloseButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles _closeButton.Click
            CloseScreen()
        End Sub

        Private Sub LoginButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles _loginButton.Click
            If _passwordBox.Text.Length > 0 And _usernameBox.Text.Length > 0 And _passwordInitial = False And _usernameInitial = False Then
                _isLoading = True

                Dim APIRequest = GameAPI.GameJoltRequest.VerifyUser(_usernameBox.Text, _passwordBox.Text)
                AddHandler APIRequest.Finished, AddressOf VerifyUserResult
                APIRequest.ExecuteAsync(GameAPI.RequestFormat.Json)
            End If
        End Sub

        Private Sub SaveSettings()
            If API.LoggedIn = True Then
                Dim cUsername As String = Encryption.EncryptString(_usernameBox.Text, "") ' CLASSIFIED
                Dim cToken As String = Encryption.EncryptString(_passwordBox.Text, "") ' CLASSIFIED

                IO.File.WriteAllText(GameController.GamePath & "\Save\gamejoltAcc.dat", cUsername & vbNewLine & cToken)
            End If
        End Sub

        Private Sub LoadSettings()
            If IO.File.Exists(GameController.GamePath & "\Save\gamejoltAcc.dat") = True Then
                Dim content() As String = IO.File.ReadAllLines(GameController.GamePath & "\Save\gamejoltAcc.dat")

                If content.Length >= 2 Then
                    Try
                        _usernameBox.Text = Encryption.DecryptString(content(0), "") ' CLASSIFIED
                        _passwordBox.Text = Encryption.DecryptString(content(1), "") ' CLASSIFIED

                        _loginButton.IsFocused = True
                    Catch ex As Exception
                        IO.File.Delete(GameController.GamePath & "\Save\gamejoltAcc.dat")
                        Logger.Log("241", Logger.LogTypes.Warning, "Cannot read GameJolt account settings!")
                    End Try
                End If
            End If
        End Sub

#Region "Networking"

        ''' <summary>
        ''' Callback function for verifying if the user input was correct.
        ''' </summary>
        ''' <param name="result"></param>
        Private Sub VerifyUserResult(ByVal result As GameAPI.RequestResult)
            _canCloseMessage = False
            If result.Status = GameAPI.RequestStatus.Success Then
                Try
                    Dim responseModel = DataModel.Json.JsonDataModel.FromString(Of DataModel.Json.GameJolt.VerifyUserResponseModel)(result.Data)

                    _canCloseMessage = True
                    If responseModel.response.success = "true" Then
                        GetGameJoltId()
                    Else
                        _message = "Wrong user credentials entered."
                        _showMessage = True
                    End If
                Catch ex As Exception
                    _message = "Failed to connect to GameJolt server."
                    _showMessage = True
                End Try
            Else
                _message = "Failed to connect to GameJolt server."
                _showMessage = True
            End If

            _isLoading = False
        End Sub

        Private Sub GetGameJoltId()
            Dim result = GameAPI.GameJoltRequest.FetchUserData(_usernameBox.Text).Execute(GameAPI.RequestFormat.Json)

            If result.Status = GameAPI.RequestStatus.Success Then
                Dim responseModel = DataModel.Json.JsonDataModel.FromString(Of DataModel.Json.GameJolt.UserDataResponseModel)(result.Data)

                If responseModel.response.success = "true" Then
                    API.LoggedIn = True
                    API.username = _usernameBox.Text
                    API.token = _passwordBox.Text
                    API.gameJoltId = responseModel.response.users(0).id

                    _showMessage = True
                    _message = "Successfully logged in."
                    _canCloseMessage = False
                    _isError = False

                    SaveSettings()
                Else
                    _message = "Unable to find user in database."
                    _showMessage = True
                End If
            Else
                _message = "Wrong user credentials entered."
                _showMessage = True
            End If
        End Sub

        Private Sub DownloadBanList()
            Try
                Dim webClient As New Net.WebClient
                Dim jsonData As String = ""

                jsonData = webClient.DownloadString(BANLIST_URL)
                _banList = DataModel.Json.JsonDataModel.FromString(Of DataModel.Json.GameJolt.BanListEntryModel())(jsonData)

                jsonData = webClient.DownloadString(BANREASONS_URL)
                _banReasons = DataModel.Json.JsonDataModel.FromString(Of DataModel.Json.GameJolt.BanReasonEntryModel())(jsonData)

                'Cascade to GameJolt connection test:
                TestGameJoltConnection()
            Catch ex As Exception
                _showMessage = True
                _message = "Failed to load data from server."
            End Try
            _isLoading = False
        End Sub

        Private Sub TestGameJoltConnection()
            Try
                Dim APIRequest = GameAPI.GameJoltRequest.GetStorageData("ONLINEVERSION", False)

                Dim result = APIRequest.Execute(GameAPI.RequestFormat.Json)
                If result.Status = GameAPI.RequestStatus.Success Then
                    Dim responseModel = DataModel.Json.JsonDataModel.FromString(Of DataModel.Json.GameJolt.DataStorageResponseModel)(result.Data)

                    If responseModel.response.success = "true" Then
                        Dim version As String = responseModel.response.data

                        If version = GameController.GAMEVERSION Then
                            _showMessage = False

                            LoadPreviousGameJoltAccount()
                        Else
                            _showMessage = True
                            _message = String.Format("Version {0} is required to play.", version)
                        End If
                    Else
                        _showMessage = True
                        _message = "Failed to connect to GameJolt server" & vbNewLine & "(API unsuccessful)."
                    End If
                Else
                    _showMessage = True
                    _message = "Failed to connect to GameJolt server" & vbNewLine & "(API unavailable)."
                End If
            Catch ex As Exception
                _showMessage = True
                _message = "Failed to connect to GameJolt server" & vbNewLine & "(misc error)."
            End Try
        End Sub

        Private Sub LoadPreviousGameJoltAccount()
            If IO.File.Exists(GameController.GamePath & "\Save\gamejoltAcc.dat") Then
                Dim content() As String = IO.File.ReadAllLines(GameController.GamePath & "\Save\gamejoltAcc.dat")

                If content.Length >= 2 Then
                    Try
                        Dim loadedUsername As String = Encryption.DecryptString(content(0), "") ' CLASSIFIED
                        Dim loadedToken As String = Encryption.DecryptString(content(1), "") ' CLASSIFIED

                        _usernameInitial = False
                        _passwordInitial = False

                        _usernameBox.Text = loadedUsername
                        _passwordBox.Text = loadedToken
                        _passwordBox.IsPassword = True

                        _loginButton.IsFocused = True

                        LoadSettings()
                    Catch ex As Exception
                        IO.File.Delete(GameController.GamePath & "\Save\gamejoltAcc.dat")
                        Logger.Log("241", Logger.LogTypes.Warning, "Failed to read GameJolt account settings!")
                        _canCloseMessage = True
                    End Try
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace