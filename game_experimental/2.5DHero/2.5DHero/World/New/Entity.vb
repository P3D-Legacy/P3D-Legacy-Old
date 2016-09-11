Namespace Entities

    ''' <summary>
    ''' Represents an entity in a level.
    ''' </summary>
    Class Entity

        ''' <summary>
        ''' Possible render modes for an entity.
        ''' </summary>
        Public Enum EntityRenderMode
            Primitives
            Model
        End Enum

        Private _mapOrigin As String = ""
        Private _isOffsetMapContent As Boolean = False
        Private _offset As Vector3 = Vector3.Zero

        Private _properties As New List(Of EntityProperty)

        Private _renderMode As EntityRenderMode = EntityRenderMode.Primitives

        Public Property UseNoneCullMode As Boolean = False

#Region "Texture Render mode"

        ''' <summary>
        ''' The texture file this entity grabs textures from.
        ''' </summary>
        ''' <returns></returns>
        Public Property TexturePath() As String = ""

        ''' <summary>
        ''' The array of textures loaded for this entity.
        ''' </summary>
        ''' <returns></returns>
        Public Property Textures() As Texture2D()

        ''' <summary>
        ''' The index of textures that get mapped to the primitives.
        ''' </summary>
        ''' <returns></returns>
        Public Property TextureIndex() As Integer()

        ''' <summary>
        ''' The primitive model's ID.
        ''' </summary>
        ''' <returns></returns>
        Public Property PrimitiveModelID As Integer = 0

#End Region

#Region "Model render mode"

        ''' <summary>
        ''' The path to the content model.
        ''' </summary>
        ''' <returns></returns>
        Public Property ModelPath() As String = ""

        ''' <summary>
        ''' The content model.
        ''' </summary>
        ''' <returns></returns>
        Public Property Model3D As Model

#End Region

        ''' <summary>
        ''' The entity ID of this entity, used to identify it with scripts.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ID() As Integer = -1

        ''' <summary>
        ''' The position of this entity on the map.
        ''' </summary>
        ''' <returns></returns>
        Public Property Position() As Vector3 = Vector3.Zero

        ''' <summary>
        ''' The visual scale of this entity.
        ''' </summary>
        ''' <returns></returns>
        Public Property Scale() As Vector3 = Vector3.One

        ''' <summary>
        ''' If this entity gets rendered.
        ''' </summary>
        ''' <returns></returns>
        Public Property Visible() As Boolean = True

        ''' <summary>
        ''' If the player can interact with this entity.
        ''' </summary>
        ''' <returns></returns>
        Public Property Collision() As Boolean = True

        Public Sub New(ByVal dataModel As DataModel.Json.Game.EntityModel)

        End Sub

        ''' <summary>
        ''' Updates the entity's properties.
        ''' </summary>
        Public Sub Update()
            For Each p As EntityProperty In _properties
                p.Update()
            Next
        End Sub

        ''' <summary>
        ''' Updates the entity.
        ''' </summary>
        Public Sub UpdateEntity()

        End Sub

#Region "Rendering"

        Private Shared _noneCullRasterizerState As RasterizerState = Nothing
        Private Shared _normalCullRasterizerState As RasterizerState = Nothing

        Private _drawnLastFrame As Boolean = False

        Private ReadOnly Property NoneCullRasterizerState() As RasterizerState
            Get
                If _noneCullRasterizerState Is Nothing Then
                    _noneCullRasterizerState = New RasterizerState()
                    _noneCullRasterizerState.CullMode = CullMode.None
                End If
                Return _noneCullRasterizerState
            End Get
        End Property

        Private ReadOnly Property NormalCullRasterizerState() As RasterizerState
            Get
                If _normalCullRasterizerState Is Nothing Then
                    _normalCullRasterizerState = New RasterizerState()
                    _normalCullRasterizerState.CullMode = CullMode.CullCounterClockwiseFace
                End If
                Return _normalCullRasterizerState
            End Get
        End Property

        ''' <summary>
        ''' Renders this entity.
        ''' </summary>
        Public Sub Render()
            Dim rendered As Boolean = False

            For Each p As EntityProperty In _properties
                Dim renderResult = p.Render()
                Select Case renderResult
                    Case EntityProperty.EntityPropertyRenderResultType.Rendered
                        rendered = True
                        Exit For
                    Case EntityProperty.EntityPropertyRenderResultType.RenderedButPassed
                        rendered = True
                End Select
            Next

            'If none of the entity properties had a special render method, then render this entity with the default settings:
            If rendered = False Then
                DefaultEntityRender()
            End If
        End Sub

        ''' <summary>
        ''' Renders the entity with its default settings, ignoring render settings of properties.
        ''' </summary>
        Public Sub DefaultEntityRender()
            If _Visible = True Then
                If IsInFieldOfView() = True Then
                    _drawnLastFrame = True

                    If _renderMode = EntityRenderMode.Primitives Then
                        If UseNoneCullMode = True Then
                            Core.GraphicsDevice.RasterizerState = NoneCullRasterizerState
                        End If

                        'TODO: Accustom to new entity format.
                        'BaseModel.getModelbyID(_PrimitiveModelID).Draw(Me, _Textures)

                        If UseNoneCullMode = True Then
                            Core.GraphicsDevice.RasterizerState = NormalCullRasterizerState
                        End If
                    Else
                        If Not _Model3D Is Nothing Then
                            '_Model3D.Draw(Me.World, Screen.Camera.View, Screen.Camera.Projection)
                        End If
                    End If
                Else
                    _drawnLastFrame = False
                End If
            Else
                _drawnLastFrame = False
            End If
        End Sub

        Private Function IsInFieldOfView() As Boolean

        End Function

#End Region

#Region "Entity reactions"

        ''' <summary>
        ''' When the player interacts with this entity.
        ''' </summary>
        Public Sub Click()
            For Each p As EntityProperty In _properties
                p.Click()
            Next
        End Sub

        Public Function WalkAgainst() As Boolean
            For Each p As EntityProperty In _properties
                Dim response As EntityProperty.FunctionResponse = p.WalkAgainst()
                If response = EntityProperty.FunctionResponse.ValueFalse Then
                    Return False
                ElseIf response = EntityProperty.FunctionResponse.ValueTrue Then
                    Return True
                End If
            Next
            Return True
        End Function

        Public Function WalkInto() As Boolean
            For Each p As EntityProperty In _properties
                Dim response As EntityProperty.FunctionResponse = p.WalkInto()
                If response = EntityProperty.FunctionResponse.ValueFalse Then
                    Return False
                ElseIf response = EntityProperty.FunctionResponse.ValueTrue Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Sub WalkOnto()
            For Each p As EntityProperty In _properties
                p.WalkOnto()
            Next
        End Sub

        Public Sub ChooseBoxResult(ByVal resultIndex As Integer)
            For Each p As EntityProperty In _properties
                p.ChooseBoxResult(resultIndex)
            Next
        End Sub

        Public Function LetPlayerMove() As Boolean
            For Each p As EntityProperty In _properties
                Dim response As EntityProperty.FunctionResponse = p.LetPlayerMove()
                If response = EntityProperty.FunctionResponse.ValueFalse Then
                    Return False
                ElseIf response = EntityProperty.FunctionResponse.ValueTrue Then
                    Return True
                End If
            Next
            Return True
        End Function

#End Region

    End Class

End Namespace