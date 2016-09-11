Namespace Items.Standard

    Public Class RockIncense

        Inherits Item

        Public Sub New()
            MyBase.New("Rock Incense", 9800, ItemTypes.Standard, 286, 1, 0, New Rectangle(288, 264, 24, 24), "An item to be held by a Pokémon. This exotic-smelling incense boosts the power of Rock-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace