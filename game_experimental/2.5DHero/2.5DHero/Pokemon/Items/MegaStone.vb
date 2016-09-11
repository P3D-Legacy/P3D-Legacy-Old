Namespace Items

    ''' <summary>
    ''' The base class for all Mega Stone items.
    ''' </summary>
    Public Class MegaStone

        Inherits Item

        Private _megaPokemonNumber As Integer = 0

        Public Sub New(ByVal Name As String, ByVal ID As Integer, ByVal TextureRectangle As Rectangle, ByVal MegaPokemonName As String, ByVal MegaPokemonNumber As Integer)
            MyBase.New(Name, 100, ItemTypes.Standard, ID, 1.0F, 0, TextureRectangle, "One variety of the mysterious Mega Stones. Have " & MegaPokemonName & " hold it, and this stone will enable it to Mega Evolve during battle.")

            _canBeHold = True
            _canBeTossed = False
            _canBeTraded = False
            _canBeUsed = False
            _canBeUsedInBattle = False

            _isMegaStone = True
            _megaPokemonNumber = MegaPokemonNumber
        End Sub

        Public ReadOnly Property MegaPokemonNumber() As Integer
            Get
                Return _megaPokemonNumber
            End Get
        End Property

        Protected Overrides Sub LoadTexture()
            _texture = TextureManager.GetTexture("Items\MegaStones", _textureRectangle, "")
        End Sub

    End Class

End Namespace