Public Class SplashScreen

    Inherits Screen

    Private _startedLoading As Boolean = False
    Private _textures As Texture2D()
    Private _game As GameController
    Private _loadThread As Threading.Thread

    Private _delay As Single = 7.0F

    Public Sub New(ByVal GameReference As GameController)
        _game = GameReference
        CanBePaused = False
        CanMuteMusic = False
        CanChat = False
        CanTakeScreenshot = False
        CanDrawDebug = False
        MouseVisible = True
        CanGoFullscreen = False

        _textures = {Content.Load(Of Texture2D)("SharedResources\Textures\UI\Logos\KolbenBrand"),
                     Content.Load(Of Texture2D)("SharedResources\Textures\UI\Logos\KolbenText")}

        Identification = Identifications.SplashScreen
    End Sub

    Public Overrides Sub Draw()
        Canvas.DrawRectangle(windowSize, New Color(164, 27, 27))

        SpriteBatch.Draw(_textures(0), New Rectangle(CInt(windowSize.Width / 2) - CInt(_textures(0).Width / 2), CInt(windowSize.Height / 2) - CInt(_textures(0).Height), _textures(0).Width, _textures(0).Height), Color.White)
        SpriteBatch.Draw(_textures(1), New Rectangle(CInt(windowSize.Width / 2) - CInt(_textures(1).Width / 2), CInt(windowSize.Height / 2) - CInt(_textures(1).Height / 2) + CInt(_textures(0).Height / 2), _textures(1).Width, _textures(1).Height), Color.White)
    End Sub

    Public Overrides Sub Update()
        If _startedLoading = False Then
            _loadThread = New Threading.Thread(AddressOf LoadContent)
            _loadThread.Start()

            _startedLoading = True
        End If

        If _loadThread.IsAlive = False Then
            If _delay <= 0.0F Or GameController.IS_DEBUG_ACTIVE = True Then
                GraphicsManager.ApplyChanges()

                Logger.Debug("142", "---Loading content ready---")

                If MapPreviewScreen.MapViewMode = True Then
                    SetScreen(New MapPreviewScreen())
                Else
                    SetScreen(New PressStartScreen())
                    '   Core.SetScreen(New MainMenuScreen())
                End If
                'Core.SetScreen(New TransitionScreen(Me, New IntroScreen(), Color.Black, False))
            End If
        End If

        _delay -= 0.1F
    End Sub

    Private Sub LoadContent()
        Logger.Debug("143", "---Start loading content---")

        Core.LoadContent()
    End Sub

End Class