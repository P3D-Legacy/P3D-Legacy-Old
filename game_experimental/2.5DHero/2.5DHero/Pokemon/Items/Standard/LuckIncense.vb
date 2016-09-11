Namespace Items.Standard

    Public Class LuckIncense

        Inherits Item

        Public Sub New()
            MyBase.New("Luck Incense", 9800, ItemTypes.Standard, 290, 1, 0, New Rectangle(240, 264, 24, 24), "An item to be held by a Pokémon. It doubles any prize money received if the holding Pokémon joins a battle.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace