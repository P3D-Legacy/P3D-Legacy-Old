Namespace Items.Standard

    Public Class RoseIncense

        Inherits Item

        Public Sub New()
            MyBase.New("Rose Incense", 9800, ItemTypes.Standard, 287, 1, 0, New Rectangle(312, 264, 24, 24), "An item to be held by a Pokémon. This exotic-smelling incense boosts the power of Grass-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace