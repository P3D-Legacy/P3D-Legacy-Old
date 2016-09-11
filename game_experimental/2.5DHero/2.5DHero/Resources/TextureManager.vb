Public Class TextureManager

    Public Shared DefaultTexture As Texture2D

    Public Shared Sub InitializeTextures()
        DefaultTexture = Content.Load(Of Texture2D)("SharedResources\Textures\no_texture")
    End Sub

    Public Shared TextureList As New Dictionary(Of String, Texture2D)

    ''' <summary>
    ''' Returns a texture.
    ''' </summary>
    ''' <param name="Name">The name of the texture.</param>
    ''' <returns></returns>
        Public Shared Function GetTexture(ByVal Name As String) As Texture2D
        Dim cContent As ContentManager = ContentPackManager.GetContentManager(Name, ".xnb,.png")

        Dim tKey As String = cContent.RootDirectory & "\" & Name & ",FULL_IMAGE"

        If TextureList.ContainsKey(tKey) = False Then
            Dim t As Texture2D = Nothing

            If IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\" & Name & ".xnb") = False Then
                If IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\" & Name & ".png") = True Then
                    Using stream As IO.Stream = IO.File.Open(GameController.GamePath & "\" & cContent.RootDirectory & "\" & Name & ".png", IO.FileMode.OpenOrCreate)
                        Try
                            t = Texture2D.FromStream(GraphicsDevice, stream)
                        Catch ex As Exception
                            Logger.Log("274", Logger.LogTypes.ErrorMessage, "Something went wrong while XNA tried to load a texture. Return default.")
                            Return DefaultTexture
                        End Try
                    End Using
                Else
                    Logger.Log("275", Logger.LogTypes.ErrorMessage, "Textures.vb: Texture """ & GameController.GamePath & "\" & cContent.RootDirectory & "\" & Name & """ was not found!")
                    Return DefaultTexture
                End If
            Else
                t = cContent.Load(Of Texture2D)(Name)
            End If

            TextureList.Add(tKey, TextureRectangle(t, New Rectangle(0, 0, t.Width, t.Height), 1))

            cContent.Unload()
        End If

        Return TextureList(tKey)
    End Function

    ''' <summary>
    ''' Returns a texture.
    ''' </summary>
    ''' <param name="Name">The name of the texture.</param>
    ''' <param name="r">The rectangle to get the texture from.</param>
    ''' <param name="TexturePath">The texturepath to load a texture from.</param>
    ''' <returns></returns>
        Public Shared Function GetTexture(ByVal Name As String, ByVal r As Rectangle, ByVal TexturePath As String) As Texture2D
        Dim tSource As TextureSource = ContentPackManager.GetTextureReplacement(TexturePath & Name, r)

        Dim cContent As ContentManager = ContentPackManager.GetContentManager(tSource.TexturePath, ".xnb,.png")
        Dim resolution As Integer = ContentPackManager.GetTextureResolution(TexturePath & Name)

        Dim tKey As String = cContent.RootDirectory & "\" & TexturePath & Name & "," & r.X & "," & r.Y & "," & r.Width & "," & r.Height & "," & resolution
        If TextureList.ContainsKey(tKey) = False Then
            Dim t As Texture2D = Nothing
            Dim fullImageKey As String = cContent.RootDirectory & "\" & TexturePath & Name & ",FULL_IMAGE"

            If TextureList.ContainsKey(fullImageKey) = True Then
                'Load the full texture if it has already been loaded once:
                t = TextureList(fullImageKey)
            Else
                If IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\" & tSource.TexturePath & ".xnb") = False Then
                    If IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\" & tSource.TexturePath & ".png") = True Then
                        Using stream As IO.Stream = IO.File.Open(GameController.GamePath & "\" & cContent.RootDirectory & "\" & tSource.TexturePath & ".png", IO.FileMode.OpenOrCreate)
                            Try
                                t = Texture2D.FromStream(GraphicsDevice, stream)
                            Catch ex As Exception
                                Logger.Log("276", Logger.LogTypes.ErrorMessage, "Something went wrong while XNA tried to load a texture. Return default.")
                                Return DefaultTexture
                            End Try
                        End Using
                    Else
                        Logger.Log("277", Logger.LogTypes.ErrorMessage, "Textures.vb: Texture """ & GameController.GamePath & "\" & cContent.RootDirectory & "\" & Name & """ was not found!")
                        Return DefaultTexture
                    End If
                Else
                    t = cContent.Load(Of Texture2D)(tSource.TexturePath)
                End If

                TextureList.Add(fullImageKey, t.Copy())
            End If

            If TextureList.ContainsKey(tKey) = False Then TextureList.Add(tKey, TextureRectangle(t, tSource.TextureRectangle, resolution))

            cContent.Unload()
        End If

        Return TextureList(tKey)
    End Function

    ''' <summary>
    ''' Returns the texture. The default texture path is "Textures\".
    ''' </summary>
    ''' <param name="Name">The name of the texture.</param>
    ''' <param name="r">The rectangle to get from the texture.</param>
    ''' <returns></returns>
        Public Shared Function GetTexture(ByVal Name As String, ByVal r As Rectangle) As Texture2D
        Return GetTexture(Name, r, "Textures\")
    End Function

    Public Shared Function TextureRectangle(ByVal Texture As Texture2D, ByVal Rectangle As Rectangle, Optional ByVal Factor As Integer = 1) As Texture2D
        If IsNothing(Rectangle) Then
            Return Texture
        Else
            Rectangle = New Rectangle(Rectangle.X * Factor, Rectangle.Y * Factor, Rectangle.Width * Factor, Rectangle.Height * Factor)

            Dim tRectangle As New Rectangle(0, 0, Texture.Width, Texture.Height)
            If tRectangle.Contains(Rectangle) = False Then
                Logger.Log("278", Logger.LogTypes.ErrorMessage, "Textures.vb: The rectangle for a texture was out of bounds!")
                Return DefaultTexture
            End If

            Dim Data(Rectangle.Width * Rectangle.Height - 1) As Color
            Texture.GetData(0, Rectangle, Data, 0, Rectangle.Width * Rectangle.Height)

            Dim newTex As New Texture2D(GraphicsDevice, Rectangle.Width, Rectangle.Height)
            newTex.SetData(Data)

            Return newTex
        End If
    End Function

    Public Shared Function TextureExist(ByVal Name As String) As Boolean
        Dim cContent As ContentManager = ContentPackManager.GetContentManager(Name, ".xnb,.png")
        If IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\" & Name & ".xnb") = True Then
            Return True
        Else
            If IO.File.Exists(GameController.GamePath & "\" & cContent.RootDirectory & "\" & Name & ".png") = True Then
                Return True
            End If
        End If
        Return False
    End Function

End Class