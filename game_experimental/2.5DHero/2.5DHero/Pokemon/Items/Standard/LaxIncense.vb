Namespace Items.Standard

    Public Class LaxIncense

        Inherits Item

        Public Sub New()
            MyBase.New("Lax Incense", 9800, ItemTypes.Standard, 289, 1, 0, New Rectangle(216, 264, 24, 24), "An item to be held by a Pokémon. The beguiling aroma of this incense may cause attacks to miss its holder.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace