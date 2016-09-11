Namespace Items.Standard

    Public Class FullIncense

        Inherits Item

        Public Sub New()
            MyBase.New("Full Incense", 9800, ItemTypes.Standard, 288, 1, 0, New Rectangle(192, 264, 24, 24), "An item to be held by a Pokémon. This exotic-smelling incense makes the holder bloated and slow moving.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace