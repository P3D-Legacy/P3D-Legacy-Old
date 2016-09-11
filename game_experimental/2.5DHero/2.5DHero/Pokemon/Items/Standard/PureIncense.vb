Namespace Items.Standard

    Public Class PureIncense

        Inherits Item

        Public Sub New()
            MyBase.New("Pure Incense", 9800, ItemTypes.Standard, 291, 1, 0, New Rectangle(264, 264, 24, 24), "An item to be held by a Pokémon. It helps keep wild Pokémon away if the holder is the head of the party.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace