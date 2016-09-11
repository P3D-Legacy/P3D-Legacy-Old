''' <summary>
''' Represents a Pokémon.
''' </summary>
Public Class Pokemon

    ''' <summary>
    ''' Defines which Pokémon in the default GameMode are considered "legendary".
    ''' </summary>
        Public Shared ReadOnly Legendaries() As Integer = {144, 145, 146, 150, 151, 243, 244, 245, 249, 250, 251, 377, 378, 379, 380, 381, 382, 383, 384, 385, 386, 480, 481, 482, 483, 484, 485, 486, 487, 488, 489, 490, 491, 492, 493, 494, 638, 639, 640, 641, 642, 643, 644, 645, 646, 647, 648, 649, 716, 717, 718, 719, 720, 721}

    Private _definitionModel As DataModel.Json.GameModeData.PokemonDefinitionModel

#Region "Events"

    Public Event TexturesCleared(ByVal sender As Object, ByVal e As EventArgs)

#End Region

#Region "Enums"

    ''' <summary>
    ''' The different experience types a Pokémon can have.
    ''' </summary>
        Public Enum ExperienceTypes
        Fast
        MediumFast
        MediumSlow
        Slow
    End Enum

    ''' <summary>
    ''' EggGroups a Pokémon can have to define its breeding compatibility.
    ''' </summary>
        Public Enum EggGroups
        Monster
        Water1
        Water2
        Water3
        Bug
        Flying
        Field
        Fairy
        Grass
        Undiscovered
        HumanLike
        Mineral
        Amorphous
        Ditto
        Dragon
        GenderUnknown
        None
    End Enum

    ''' <summary>
    ''' Genders of a Pokémon.
    ''' </summary>
        Public Enum Genders
        Male
        Female
        Genderless
    End Enum

    ''' <summary>
    ''' The status problems a Pokémon can have.
    ''' </summary>
        Public Enum StatusProblems
        None
        Burn
        Freeze
        Paralyzed
        Poison
        BadPoison
        Sleep
        Fainted
    End Enum

    ''' <summary>
    ''' The volatile status a Pokémon can have.
    ''' </summary>
        Public Enum VolatileStatus
        Confusion
        Flinch
        Infatuation
        Trapped
    End Enum

    ''' <summary>
    ''' Different natures of a Pokémon.
    ''' </summary>
        Public Enum Natures As Integer
        Hardy = 0
        Lonely
        Brave
        Adamant
        Naughty
        Bold
        Docile
        Relaxed
        Impish
        Lax
        Timid
        Hasty
        Serious
        Jolly
        Naive
        Modest
        Mild
        Quiet
        Bashful
        Rash
        Calm
        Gentle
        Sassy
        Careful
        Quirky
    End Enum

    ''' <summary>
    ''' Ways to change the Friendship value of a Pokémon.
    ''' </summary>
        Public Enum FriendShipCauses
        Walking
        LevelUp
        Fainting
        EnergyPowder
        HealPowder
        EnergyRoot
        RevivalHerb
        Trading
        Vitamin
        EVBerry
    End Enum

#End Region

#Region "Properties"

    ''' <summary>
    ''' Returns the name to reference to the animation/model of this Pokémon.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public ReadOnly Property AnimationName() As String
        Get
            Return PokemonForms.GetAnimationName(Me)
        End Get
    End Property

    Public Property AdditionalData() As String
        Get
            Return _additionalData
        End Get
        Set(value As String)
            _additionalData = value

            ClearTextures()
        End Set
    End Property

    Public Property Experience() As Integer
        Get
            Return _experience
        End Get
        Set(value As Integer)
            _experience = value
        End Set
    End Property

    Public Property Gender() As Genders
        Get
            Return _gender
        End Get
        Set(value As Genders)
            _gender = value
        End Set
    End Property

    Public Property EggSteps() As Integer
        Get
            Return _eggSteps
        End Get
        Set(value As Integer)
            _eggSteps = value
        End Set
    End Property

    Public Property NickName() As String
        Get
            Return _nickName
        End Get
        Set(value As String)
            _nickName = value
        End Set
    End Property

    Public Property Level() As Integer
        Get
            Return _level
        End Get
        Set(value As Integer)
            _level = value
        End Set
    End Property

    Public Property OT() As String
        Get
            Return _OT
        End Get
        Set(value As String)
            _OT = value
        End Set
    End Property

    Public Property Status() As StatusProblems
        Get
            Return _status
        End Get
        Set(value As StatusProblems)
            _status = value
        End Set
    End Property

    Public Property Nature() As Natures
        Get
            Return _nature
        End Get
        Set(value As Natures)
            _nature = value
        End Set
    End Property

    Public Property CatchLocation() As String
        Get
            Return _catchLocation
        End Get
        Set(value As String)
            _catchLocation = value
        End Set
    End Property

    Public Property CatchTrainerName() As String
        Get
            Return _catchTrainerName
        End Get
        Set(value As String)
            _catchTrainerName = value
        End Set
    End Property

    Public Property CatchMethod() As String
        Get
            Return _catchMethod
        End Get
        Set(value As String)
            _catchMethod = value
        End Set
    End Property

    Public Property Friendship() As Integer
        Get
            Return _friendship
        End Get
        Set(value As Integer)
            _friendship = value
        End Set
    End Property

    Public Property IsShiny() As Boolean
        Get
            Return _isShiny
        End Get
        Set(value As Boolean)
            _isShiny = value
        End Set
    End Property

    Public Property IndividualValue() As String
        Get
            Return _individualValue
        End Get
        Set(value As String)
            _individualValue = value
        End Set
    End Property

#End Region

#Region "Definition"

#Region "Properties"

    Public Property Number() As Integer
        Get
            Return _definitionModel.Number
        End Get
        Set(value As Integer)
            _definitionModel.Number = value
        End Set
    End Property

    Public Property ExperienceType() As ExperienceTypes
        Get
            Return _definitionModel.ExperienceType
        End Get
        Set(value As ExperienceTypes)
            _definitionModel.ExperienceType = value
        End Set
    End Property

    Public Property BaseExperience() As Integer
        Get
            Return _definitionModel.BaseExperience
        End Get
        Set(value As Integer)
            _definitionModel.BaseExperience = value
        End Set
    End Property

    Private Property Name() As String
        Get
            Return _definitionModel.Name
        End Get
        Set(value As String)
            _definitionModel.Name = value
        End Set
    End Property

    Public Property CatchRate() As Integer
        Get
            Return _definitionModel.CatchRate
        End Get
        Set(value As Integer)
            _definitionModel.CatchRate = value
        End Set
    End Property

    Public Property BaseFriendship() As Integer
        Get
            Return _definitionModel.BaseFriendship
        End Get
        Set(value As Integer)
            _definitionModel.BaseFriendship = value
        End Set
    End Property

    Public Property BaseEggSteps() As Integer
        Get
            Return _definitionModel.BaseEggSteps
        End Get
        Set(value As Integer)
            _definitionModel.BaseEggSteps = value
        End Set
    End Property

    Public Property EggGroup1() As EggGroups
        Get
            Return _definitionModel.EggGroup1
        End Get
        Set(value As EggGroups)
            _definitionModel.EggGroup1 = value
        End Set
    End Property

    Public Property EggGroup2() As EggGroups
        Get
            Return _definitionModel.EggGroup2
        End Get
        Set(value As EggGroups)
            _definitionModel.EggGroup2 = value
        End Set
    End Property

    Public Property IsMale() As Decimal
        Get
            Return _definitionModel.IsMale
        End Get
        Set(value As Decimal)
            _definitionModel.IsMale = value
        End Set
    End Property

    Public Property IsGenderless() As Boolean
        Get
            Return _definitionModel.IsGenderless
        End Get
        Set(value As Boolean)
            _definitionModel.IsGenderless = value
        End Set
    End Property

    Public Property Devolution() As Integer
        Get
            Return _definitionModel.Devolution
        End Get
        Set(value As Integer)
            _definitionModel.Devolution = value
        End Set
    End Property

    Public Property CanLearnAllMachines() As Boolean
        Get
            Return _canLearnAllMachines
        End Get
        Set(value As Boolean)
            _canLearnAllMachines = value
        End Set
    End Property

    Public Property CanSwim() As Boolean
        Get
            Return _definitionModel.CanSwim
        End Get
        Set(value As Boolean)
            _definitionModel.CanSwim = value
        End Set
    End Property

    Public Property CanFly() As Boolean
        Get
            Return _definitionModel.CanFly
        End Get
        Set(value As Boolean)
            _definitionModel.CanFly = value
        End Set
    End Property

    Public Property EggPokemon() As Integer
        Get
            Return _definitionModel.EggPokemon
        End Get
        Set(value As Integer)
            _definitionModel.EggPokemon = value
        End Set
    End Property

    Public Property TradeValue() As Integer
        Get
            Return _definitionModel.TradeValue
        End Get
        Set(value As Integer)
            _definitionModel.TradeValue = value
        End Set
    End Property

    Public Property CanBreed() As Boolean
        Get
            Return _definitionModel.CanBreed
        End Get
        Set(value As Boolean)
            _definitionModel.CanBreed = value
        End Set
    End Property

#End Region

#Region "Base Stats"

    Public Property BaseHP() As Integer
        Get
            Return _definitionModel.BaseStats.HP
        End Get
        Set(value As Integer)
            _definitionModel.BaseStats.HP = value
        End Set
    End Property

    Public Property BaseAttack() As Integer
        Get
            Return _definitionModel.BaseStats.Atk
        End Get
        Set(value As Integer)
            _definitionModel.BaseStats.Atk = value
        End Set
    End Property

    Public Property BaseDefense() As Integer
        Get
            Return _definitionModel.BaseStats.Def
        End Get
        Set(value As Integer)
            _definitionModel.BaseStats.Def = value
        End Set
    End Property

    Public Property BaseSpAttack() As Integer
        Get
            Return _definitionModel.BaseStats.SpAtk
        End Get
        Set(value As Integer)
            _definitionModel.BaseStats.SpAtk = value
        End Set
    End Property

    Public Property BaseSpDefense() As Integer
        Get
            Return _definitionModel.BaseStats.SpDef
        End Get
        Set(value As Integer)
            _definitionModel.BaseStats.SpDef = value
        End Set
    End Property

    Public Property BaseSpeed() As Integer
        Get
            Return _definitionModel.BaseStats.Speed
        End Get
        Set(value As Integer)
            _definitionModel.BaseStats.Speed = value
        End Set
    End Property

#End Region

#Region "GiveEVStats"

    Public Property GiveEVHP() As Integer
        Get
            Return _definitionModel.RewardEV.HP
        End Get
        Set(value As Integer)
            _definitionModel.RewardEV.HP = value
        End Set
    End Property

    Public Property GiveEVAttack() As Integer
        Get
            Return _definitionModel.RewardEV.Atk
        End Get
        Set(value As Integer)
            _definitionModel.RewardEV.Atk = value
        End Set
    End Property

    Public Property GiveEVDefense() As Integer
        Get
            Return _definitionModel.RewardEV.Def
        End Get
        Set(value As Integer)
            _definitionModel.RewardEV.Def = value
        End Set
    End Property

    Public Property GiveEVSpAttack() As Integer
        Get
            Return _definitionModel.RewardEV.SpAtk
        End Get
        Set(value As Integer)
            _definitionModel.RewardEV.SpAtk = value
        End Set
    End Property

    Public Property GiveEVSpDefense() As Integer
        Get
            Return _definitionModel.RewardEV.SpDef
        End Get
        Set(value As Integer)
            _definitionModel.RewardEV.SpDef = value
        End Set
    End Property

    Public Property GiveEVSpeed() As Integer
        Get
            Return _definitionModel.RewardEV.Speed
        End Get
        Set(value As Integer)
            _definitionModel.RewardEV.Speed = value
        End Set
    End Property

#End Region

    Public AttackLearns As New Dictionary(Of Integer, BattleSystem.Attack)
    Public EvolutionConditions As New List(Of EvolutionCondition)
    Public HiddenAbility As Ability = Nothing
    Public NewAbilities As New List(Of Ability)

    Private _wildItems As New Dictionary(Of Integer, Integer)
    Private _canLearnAllMachines As Boolean = False

    Public ReadOnly Property PokedexEntry() As PokedexEntry
        Get
            Return _definitionModel.PokedexEntry.GetPokedexEntry()
        End Get
    End Property

    Public ReadOnly Property Scale() As Vector3
        Get
            Return _definitionModel.Scale.ToVector3()
        End Get
    End Property

    Public Property Type1() As Element
        Get
            Return _definitionModel.Type1
        End Get
        Set(value As Element)
            _definitionModel.Type1 = value
        End Set
    End Property

    Public Property Type2() As Element
        Get
            Return _definitionModel.Type2
        End Get
        Set(value As Element)
            _definitionModel.Type2 = value
        End Set
    End Property

    Public Property EggMoves() As Integer()
        Get
            Return _definitionModel.EggMoves
        End Get
        Set(value As Integer())
            _definitionModel.EggMoves = value
        End Set
    End Property

    Public Property TutorAttacks() As Integer()
        Get
            Return _definitionModel.TutorMoves
        End Get
        Set(value As Integer())
            _definitionModel.TutorMoves = value
        End Set
    End Property

    Public Property Machines() As Integer()
        Get
            Return _definitionModel.Machines
        End Get
        Set(value As Integer())
            _definitionModel.Machines = value
        End Set
    End Property

#End Region

#Region "SavedStats"

#Region "Stats"

    Private _HP As Integer
    Private _maxHP As Integer
    Private _attack As Integer
    Private _defense As Integer
    Private _spAttack As Integer
    Private _spDefense As Integer
    Private _speed As Integer

    ''' <summary>
    ''' The HP of this Pokémon.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property HP() As Integer
        Get
            Return _HP
        End Get
        Set(value As Integer)
            _HP = value
        End Set
    End Property

    ''' <summary>
    ''' The maximal HP of this Pokémon.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property MaxHP() As Integer
        Get
            Return _maxHP
        End Get
        Set(value As Integer)
            _maxHP = value
        End Set
    End Property

    ''' <summary>
    ''' The Attack of this Pokémon.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property Attack() As Integer
        Get
            Return _attack
        End Get
        Set(value As Integer)
            _attack = value
        End Set
    End Property

    ''' <summary>
    ''' The Defense of this Pokémon.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property Defense() As Integer
        Get
            Return _defense
        End Get
        Set(value As Integer)
            _defense = value
        End Set
    End Property

    ''' <summary>
    ''' The Special Attack of this Pokémon.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property SpAttack() As Integer
        Get
            Return _spAttack
        End Get
        Set(value As Integer)
            _spAttack = value
        End Set
    End Property

    ''' <summary>
    ''' The Special Defense of this Pokémon.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property SpDefense() As Integer
        Get
            Return _spDefense
        End Get
        Set(value As Integer)
            _spDefense = value
        End Set
    End Property

    ''' <summary>
    ''' The Speed of this Pokémon.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property Speed() As Integer
        Get
            Return _speed
        End Get
        Set(value As Integer)
            _speed = value
        End Set
    End Property

#End Region

#Region "EVStats"

    Private _EVHP As Integer
    Private _EVAttack As Integer
    Private _EVDefense As Integer
    Private _EVSpAttack As Integer
    Private _EVSpDefense As Integer
    Private _EVSpeed As Integer

    ''' <summary>
    ''' The HP EV this Pokémon got.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property EVHP() As Integer
        Get
            Return _EVHP
        End Get
        Set(value As Integer)
            _EVHP = value

            CalculateStats()
        End Set
    End Property

    ''' <summary>
    ''' The Attack EV this Pokémon got.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property EVAttack() As Integer
        Get
            Return _EVAttack
        End Get
        Set(value As Integer)
            _EVAttack = value

            CalculateStats()
        End Set
    End Property

    ''' <summary>
    ''' The Defense EV this Pokémon got.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property EVDefense() As Integer
        Get
            Return _EVDefense
        End Get
        Set(value As Integer)
            _EVDefense = value

            CalculateStats()
        End Set
    End Property

    ''' <summary>
    ''' The Special Attack EV this Pokémon got.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property EVSpAttack() As Integer
        Get
            Return _EVSpAttack
        End Get
        Set(value As Integer)
            _EVSpAttack = value

            CalculateStats()
        End Set
    End Property

    ''' <summary>
    ''' The Special Defense EV this Pokémon got.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property EVSpDefense() As Integer
        Get
            Return _EVSpDefense
        End Get
        Set(value As Integer)
            _EVSpDefense = value

            CalculateStats()
        End Set
    End Property

    ''' <summary>
    ''' The Speed EV this Pokémon got.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property EVSpeed() As Integer
        Get
            Return _EVSpeed
        End Get
        Set(value As Integer)
            _EVSpeed = value

            CalculateStats()
        End Set
    End Property

#End Region

#Region "IVStats"

    Private _IVHP As Integer
    Private _IVAttack As Integer
    Private _IVDefense As Integer
    Private _IVSpAttack As Integer
    Private _IVSpDefense As Integer
    Private _IVSpeed As Integer

    ''' <summary>
    ''' The HP IV this Pokémon got.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property IVHP() As Integer
        Get
            Return _IVHP
        End Get
        Set(value As Integer)
            _IVHP = value

        End Set
    End Property

    ''' <summary>
    ''' The Attack IV this Pokémon got.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property IVAttack() As Integer
        Get
            Return _IVAttack
        End Get
        Set(value As Integer)
            _IVAttack = value

        End Set
    End Property

    ''' <summary>
    ''' The Defense IV this Pokémon got.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property IVDefense() As Integer
        Get
            Return _IVDefense
        End Get
        Set(value As Integer)
            _IVDefense = value

        End Set
    End Property

    ''' <summary>
    ''' The Special Attack IV this Pokémon got.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property IVSpAttack() As Integer
        Get
            Return _IVSpAttack
        End Get
        Set(value As Integer)
            _IVSpAttack = value

        End Set
    End Property

    ''' <summary>
    ''' The Special Defense IV this Pokémon got.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property IVSpDefense() As Integer
        Get
            Return _IVSpDefense
        End Get
        Set(value As Integer)
            _IVSpDefense = value

        End Set
    End Property

    ''' <summary>
    ''' The Speed IV this Pokémon got.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property IVSpeed() As Integer
        Get
            Return _IVSpeed
        End Get
        Set(value As Integer)
            _IVSpeed = value

        End Set
    End Property

#End Region

    Private _item As Item
    Private _additionalData As String = ""

    Public Property Item() As Item
        Get
            Return _item
        End Get
        Set(value As Item)
            _item = value
            ClearTextures()
        End Set
    End Property

    Public Attacks As New List(Of BattleSystem.Attack)
    Public Ability As Ability
    Public CatchBall As Item = Item.GetItemByID(5)

    Private _experience As Integer
    Private _gender As Genders
    Private _eggSteps As Integer
    Private _nickName As String
    Private _level As Integer
    Private _OT As String = "00000"
    Private _status As StatusProblems = StatusProblems.None
    Private _nature As Natures
    Private _catchLocation As String = "at unknown place"
    Private _catchTrainerName As String = "???"
    Private _catchMethod As String = "somehow obtained"
    Private _friendship As Integer
    Private _isShiny As Boolean
    Private _individualValue As String = ""

#End Region

#Region "Temp"

    Private _volatiles As New List(Of VolatileStatus)

    ''' <summary>
    ''' Returns if this Pokémon is affected by a Volatile Status effect.
    ''' </summary>
    ''' <param name="VolatileStatus">The Volatile Status effect to test for.</param>
    ''' <returns></returns>
        Public Function HasVolatileStatus(ByVal VolatileStatus As VolatileStatus) As Boolean
        Return _volatiles.Contains(VolatileStatus)
    End Function

    ''' <summary>
    ''' Affects this Pokémon with a Volatile Status.
    ''' </summary>
    ''' <param name="VolatileStatus">The Volatile Status to affect this Pokémon with.</param>
        Public Sub AddVolatileStatus(ByVal VolatileStatus As VolatileStatus)
        If _volatiles.Contains(VolatileStatus) = False Then
            _volatiles.Add(VolatileStatus)
        End If
    End Sub

    ''' <summary>
    ''' Removes a Volatile Status effect this Pokémon is affected by.
    ''' </summary>
    ''' <param name="VolatileStatus">The Volatile Status effect to remove.</param>
        Public Sub RemoveVolatileStatus(ByVal VolatileStatus As VolatileStatus)
        If _volatiles.Contains(VolatileStatus) = True Then
            _volatiles.Remove(VolatileStatus)
        End If
    End Sub

    ''' <summary>
    ''' Clears all Volatile Status effects affecting this Pokémon.
    ''' </summary>
        Public Sub ClearAllVolatiles()
        _volatiles.Clear()
    End Sub

    Public StatAttack As Integer = 0
    Public StatDefense As Integer = 0
    Public StatSpAttack As Integer = 0
    Public StatSpDefense As Integer = 0
    Public StatSpeed As Integer = 0

    Public Accuracy As Integer = 0
    Public Evasion As Integer = 0

    Public hasLeveledUp As Boolean = False

    Public SleepTurns As Integer = -1
    Public ConfusionTurns As Integer = -1

    Public LastHitByMove As BattleSystem.Attack
    Public LastDamageReceived As Integer = 0
    Public LastHitPhysical As Integer = -1

    ''' <summary>
    ''' Resets the temp storages of the Pokémon.
    ''' </summary>
        Public Sub ResetTemp()
        _volatiles.Clear()

        StatAttack = 0
        StatDefense = 0
        StatSpAttack = 0
        StatSpDefense = 0
        StatSpeed = 0

        Accuracy = 0
        Evasion = 0

        If _originalNumber > -1 Then
            Number = _originalNumber
            _originalNumber = -1
        End If

        If Not _originalType1 Is Nothing Then
            Type1 = _originalType1
            _originalType1 = Nothing
        End If

        If Not _originalType2 Is Nothing Then
            Type2 = _originalType2
            _originalType2 = Nothing
        End If

        If _originalStats(0) > -1 Then
            Attack = _originalStats(0)
            _originalStats(0) = -1
        End If
        If _originalStats(1) > -1 Then
            Defense = _originalStats(1)
            _originalStats(1) = -1
        End If
        If _originalStats(2) > -1 Then
            SpAttack = _originalStats(2)
            _originalStats(2) = -1
        End If
        If _originalStats(3) > -1 Then
            SpDefense = _originalStats(3)
            _originalStats(3) = -1
        End If
        If _originalStats(4) > -1 Then
            Speed = _originalStats(4)
            _originalStats(4) = -1
        End If

        If _originalShiny > -1 Then
            If _originalShiny = 0 Then
                IsShiny = False
            ElseIf _originalShiny = 1 Then
                IsShiny = True
            End If
            _originalShiny = -1
        End If

        If Not _originalMoves Is Nothing Then
            Attacks.Clear()
            For Each a As BattleSystem.Attack In _originalMoves
                Attacks.Add(a)
            Next
            _originalMoves = Nothing
        End If

        If Not _originalAbility Is Nothing Then
            Ability = _originalAbility
            _originalAbility = Nothing
        End If

        If Not _originalItem Is Nothing Then
            Item = Item.GetItemByID(_originalItem.ID)
            Item.AdditionalData = _originalItem.AdditionalData
            _originalItem = Nothing
        End If

        IsTransformed = False

        CalculateStats()
    End Sub

#End Region

#Region "OriginalStats"

    'Original Stats store the stats that the Pokémon has by default. When they get overwritten (for example by Dittos Transform move), the original values get stored in the "Original_X" value.
    'All these properties ensure that no part of the original Pokémon gets overwritten once the original value got set into place.

    ''' <summary>
    ''' The Pokémon's original primary type.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property OriginalType1() As Element
        Get
            Return _originalType1
        End Get
        Set(value As Element)
            If _originalType1 Is Nothing Then
                _originalType1 = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' The Pokémon's original secondary type.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property OriginalType2() As Element
        Get
            Return _originalType2
        End Get
        Set(value As Element)
            If _originalType2 Is Nothing Then
                _originalType2 = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' The Pokémon's original national Pokédex number.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property OriginalNumber() As Integer
        Get
            Return _originalNumber
        End Get
        Set(value As Integer)
            If _originalNumber = -1 Then
                _originalNumber = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' The Pokémon's original shiny state.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property OriginalShiny() As Integer
        Get
            Return _originalShiny
        End Get
        Set(value As Integer)
            If _originalShiny = -1 Then
                _originalShiny = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' The Pokémon's original stats.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property OriginalStats() As Integer()
        Get
            Return _originalStats
        End Get
        Set(value As Integer())
            If _originalStats Is {-1, -1, -1, -1, -1} Then
                _originalStats = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' The Pokémon's original ability.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property OriginalAbility() As Ability
        Get
            Return _originalAbility
        End Get
        Set(value As Ability)
            If _originalAbility Is Nothing Then
                _originalAbility = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' The Pokémon's original hold item.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property OriginalItem() As Item
        Get
            Return _originalItem
        End Get
        Set(value As Item)
            If _originalItem Is Nothing Then
                _originalItem = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' The Pokémon's original moveset.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property OriginalMoves() As List(Of BattleSystem.Attack)
        Get
            Return _originalMoves
        End Get
        Set(value As List(Of BattleSystem.Attack))
            If _originalMoves Is Nothing Then
                _originalMoves = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' If this Pokémon has been using the Transform move (or any other move/ability that causes similar effects).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property IsTransformed() As Boolean
        Get
            Return _isTransformed
        End Get
        Set(value As Boolean)
            _isTransformed = value
        End Set
    End Property

    Private _originalType1 As Element = Nothing
    Private _originalType2 As Element = Nothing
    Private _originalNumber As Integer = -1

    Private _originalStats() As Integer = {-1, -1, -1, -1, -1}
    Private _originalShiny As Integer = -1
    Private _originalMoves As List(Of BattleSystem.Attack) = Nothing
    Private _originalAbility As Ability = Nothing

    Private _originalItem As Item = Nothing

    Private _isTransformed As Boolean = False

#End Region

    Private Textures As New List(Of Texture2D)

    ''' <summary>
    ''' Empties the cached textures.
    ''' </summary>
        Private Sub ClearTextures()
        Textures.Clear()
        Textures.AddRange({Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing})
        RaiseEvent TexturesCleared(Me, New EventArgs())
    End Sub

#Region "Constructors and Data Handlers"

    ''' <summary>
    ''' Creates a new instance of the Pokémon class.
    ''' </summary>
        Private Sub New()
        MyBase.New()
        ClearTextures()
    End Sub

    ''' <summary>
    ''' Returns a new Pokémon class instance.
    ''' </summary>
    ''' <param name="Number">The number of the Pokémon in the national Pokédex.</param>
    ''' <returns></returns>
        Public Shared Function GetPokemonByID(ByVal Number As Integer) As Pokemon
        Return GetPokemonByID(Number, "")
    End Function

    Public Shared Function GetPokemonByID(ByVal Number As Integer, ByVal AdditionalData As String) As Pokemon
        Dim p As New Pokemon()
        p.LoadDefinitions(Number, AdditionalData)
        Return p
    End Function

    ''' <summary>
    ''' Checks if a requested Pokémon data file exists.
    ''' </summary>
    ''' <param name="Number"></param>
    ''' <returns></returns>
        Public Shared Function PokemonDataExists(ByVal Number As Integer) As Boolean
        Return IO.File.Exists(GameModeManager.GetDataFilePath("Pokemon\" & Number.ToString() & ".dat"))
    End Function

    ''' <summary>
    ''' Returns a new Pokémon class instance defined by data.
    ''' </summary>
    ''' <param name="InputData">The data that defines the Pokémon.</param>
    ''' <returns></returns>
        Public Shared Function GetPokemonByData(ByVal InputData As String) As Pokemon
        Dim Tags As New Dictionary(Of String, String)
        Dim Data() As String = InputData.Split(CChar("}"))
        For Each Tag As String In Data
            If Tag.Contains("{") = True And Tag.Contains("[") = True Then
                Dim TagName As String = Tag.Remove(0, 2)
                TagName = TagName.Remove(TagName.IndexOf(""""))

                Dim TagContent As String = Tag.Remove(0, Tag.IndexOf("[") + 1)
                TagContent = TagContent.Remove(TagContent.IndexOf("]"))

                If Tags.ContainsKey(TagName) = False Then
                    Tags.Add(TagName, TagContent)
                End If
            End If
        Next

        Dim NewAdditionalData As String = ""
        If Tags.ContainsKey("AdditionalData") = True Then
            NewAdditionalData = CStr(Tags("AdditionalData"))
        End If

        Dim PokemonID As Integer = 10
        If Tags.ContainsKey("Pokemon") = True Then
            PokemonID = CInt(Tags("Pokemon"))
        End If

        Dim p As Pokemon = GetPokemonByID(PokemonID, NewAdditionalData)
        p.LoadData(InputData)

        Return p
    End Function

    ''' <summary>
    ''' Loads definition data from the data files and empties the temp textures.
    ''' </summary>
        Public Sub ReloadDefinitions()
        LoadDefinitions(Number, AdditionalData)
        ClearTextures()
    End Sub

    ''' <summary>
    ''' Loads definition data from the data file.
    ''' </summary>
    ''' <param name="Number">The number of the Pokémon in the national Pokédex.</param>
    ''' <param name="AdditionalData">The additional data.</param>
        Public Sub LoadDefinitions(ByVal Number As Integer, ByVal AdditionalData As String)
        Dim path As String = PokemonForms.GetPokemonDataFile(Number, AdditionalData)
        Security.FileValidation.CheckFileValid(path, False, "Pokemon.vb")

        Dim jsonData As String = IO.File.ReadAllText(path)
        _definitionModel = DataModel.Json.JsonDataModel.FromString(Of DataModel.Json.GameModeData.PokemonDefinitionModel)(jsonData)

        For Each evolutionConditionModel In _definitionModel.EvolutionConditions
            Dim Evolution As Integer = evolutionConditionModel.Evolution
            Dim Type As String = evolutionConditionModel.ConditionType
            Dim Argument As String = evolutionConditionModel.Condition
            Dim Trigger As String = evolutionConditionModel.Trigger

            Dim EvolutionExists As Boolean = False
            Dim e As EvolutionCondition = New EvolutionCondition()

            For Each oldE As EvolutionCondition In EvolutionConditions
                If Evolution = oldE.Evolution Then
                    e = oldE
                    EvolutionExists = True
                End If
            Next

            e.SetEvolution(Evolution)
            e.AddCondition(Type, Argument, Trigger)

            If EvolutionExists = False Then
                EvolutionConditions.Add(e)
            End If
        Next

        _definitionModel.Moves = _definitionModel.Moves.OrderBy(Function(move) move.Level).ToArray()
        For Each m In _definitionModel.Moves
            If AttackLearns.Keys.Contains(m.Level) = False Then
                AttackLearns.Add(m.Level, BattleSystem.Attack.GetAttackByID(m.ID))
            End If
        Next

        For Each wildItem In _definitionModel.Items
            If _wildItems.Keys.Contains(wildItem.Chance) = False Then
                _wildItems.Add(wildItem.Chance, wildItem.Id)
            End If
        Next

        For Each abilityId As Integer In _definitionModel.Abilities
            NewAbilities.Add(Ability.GetAbilityByID(abilityId))
        Next

        If _definitionModel.HiddenAbilityID > -1 Then
            HiddenAbility = Ability.GetAbilityByID(_definitionModel.HiddenAbilityID)
        End If

        If _definitionModel.Machines.Length = 1 Then
            If _definitionModel.Machines(0) = -1 Then
                _canLearnAllMachines = True
            End If
        End If

        If EggPokemon = 0 Then
            EggPokemon = Me.Number
        End If

        If Devolution = 0 Then
            If EggPokemon > 0 And EggPokemon <> Me.Number Then
                If Me.Number - EggPokemon = 2 Then
                    Devolution = Me.Number - 1
                ElseIf Me.Number - EggPokemon = 1 Then
                    Devolution = EggPokemon
                End If
            End If
        End If

        If AdditionalData = "" Then
            Me.AdditionalData = PokemonForms.GetInitialAdditionalData(Me)
        End If
    End Sub

    ''' <summary>
    ''' Applies data to the Pokémon.
    ''' </summary>
    ''' <param name="InputData">The input data.</param>
        Public Sub LoadData(ByVal InputData As String)
        Dim Tags As New Dictionary(Of String, String)
        Dim Data() As String = InputData.Split(CChar("}"))
        For Each Tag As String In Data
            If Tag.Contains("{") = True And Tag.Contains("[") = True Then
                Dim TagName As String = Tag.Remove(0, 2)
                TagName = TagName.Remove(TagName.IndexOf(""""))

                Dim TagContent As String = Tag.Remove(0, Tag.IndexOf("[") + 1)
                TagContent = TagContent.Remove(TagContent.IndexOf("]"))

                If Tags.ContainsKey(TagName) = False Then
                    Tags.Add(TagName, TagContent)
                End If
            End If
        Next

        Dim loadedHP As Boolean = False

        For i = 0 To Tags.Count - 1
            Dim tagName As String = Tags.Keys(i)
            Dim tagValue As String = Tags.Values(i)

            Select Case tagName.ToLower()
                Case "experience"
                    Experience = CInt(tagValue)
                Case "gender"
                    Select Case CInt(tagValue)
                        Case 0
                            Gender = Genders.Male
                        Case 1
                            Gender = Genders.Female
                        Case 2
                            Gender = Genders.Genderless
                    End Select
                Case "eggsteps"
                    EggSteps = CInt(tagValue)
                Case "item"
                    If IsNumeric(tagValue) = True Then
                        Item = Item.GetItemByID(CInt(tagValue))
                    End If
                Case "itemdata"
                    If Not Item Is Nothing Then
                        Item.AdditionalData = tagValue
                    End If
                Case "nickname"
                    NickName = tagValue
                Case "level"
                    Level = CInt(tagValue).Clamp(1, CInt(GameModeManager.GetGameRuleValue("MaxLevel", "100")))
                Case "ot"
                    OT = tagValue
                Case "ability"
                    Ability = Ability.GetAbilityByID(CInt(tagValue))
                Case "status"
                    Select Case tagValue
                        Case "BRN"
                            Status = StatusProblems.Burn
                        Case "PSN"
                            Status = StatusProblems.Poison
                        Case "PRZ"
                            Status = StatusProblems.Paralyzed
                        Case "SLP"
                            Status = StatusProblems.Sleep
                        Case "FNT"
                            Status = StatusProblems.Fainted
                        Case "FRZ"
                            Status = StatusProblems.Freeze
                        Case "BPSN"
                            Status = StatusProblems.BadPoison
                        Case Else
                            Status = StatusProblems.None
                    End Select
                Case "nature"
                    Nature = ConvertIDToNature(CInt(tagValue))
                Case "catchlocation"
                    CatchLocation = tagValue
                Case "catchtrainer"
                    CatchTrainerName = tagValue
                Case "catchball"
                    CatchBall = Item.GetItemByID(CInt(tagValue))
                Case "catchmethod"
                    CatchMethod = tagValue
                Case "friendship"
                    Friendship = CInt(tagValue)
                Case "isshiny"
                    IsShiny = CBool(tagValue)
                Case "attack1", "attack2", "attack3", "attack4"
                    If Not BattleSystem.Attack.ConvertStringToAttack(tagValue) Is Nothing Then
                        Attacks.Add(BattleSystem.Attack.ConvertStringToAttack(tagValue))
                    End If
                Case "hp"
                    HP = CInt(tagValue)
                    loadedHP = True
                Case "fps", "evs"
                    Dim EVs() As String = tagValue.Split(CChar(","))
                    EVHP = CInt(EVs(0)).Clamp(0, 255)
                    EVAttack = CInt(EVs(1)).Clamp(0, 255)
                    EVDefense = CInt(EVs(2)).Clamp(0, 255)
                    EVSpAttack = CInt(EVs(3)).Clamp(0, 255)
                    EVSpDefense = CInt(EVs(4)).Clamp(0, 255)
                    EVSpeed = CInt(EVs(5)).Clamp(0, 255)
                Case "dvs", "ivs"
                    Dim IVs() As String = tagValue.Split(CChar(","))
                    IVHP = CInt(IVs(0))
                    IVAttack = CInt(IVs(1))
                    IVDefense = CInt(IVs(2))
                    IVSpAttack = CInt(IVs(3))
                    IVSpDefense = CInt(IVs(4))
                    IVSpeed = CInt(IVs(5))
                Case "additionaldata"
                    AdditionalData = tagValue
                Case "idvalue"
                    IndividualValue = tagValue
            End Select
        Next

        If loadedHP = False Then
            HP = MaxHP
        End If

        If IndividualValue = "" Then
            GenerateIndividualValue()
        End If

        CalculateStats()
    End Sub

    ''' <summary>
    ''' Returns the save data from the Pokémon.
    ''' </summary>
    ''' <returns></returns>
        Public Function GetSaveData() As String
        Dim SaveGender As Integer = 0
        If Gender = Genders.Female Then
            SaveGender = 1
        End If
        If IsGenderless = True Then
            SaveGender = 2
        End If

        Dim SaveStatus As String = ""
        Select Case Status
            Case StatusProblems.Burn
                SaveStatus = "BRN"
            Case StatusProblems.Poison
                SaveStatus = "PSN"
            Case StatusProblems.Paralyzed
                SaveStatus = "PRZ"
            Case StatusProblems.Sleep
                SaveStatus = "SLP"
            Case StatusProblems.Fainted
                SaveStatus = "FNT"
            Case StatusProblems.Freeze
                SaveStatus = "FRZ"
            Case StatusProblems.BadPoison
                SaveStatus = "BPSN"
        End Select

        Dim A1 As String = ""
        If Attacks.Count > 0 Then
            If Not Attacks(0) Is Nothing Then
                A1 = Attacks(0).ToString()
            End If
        End If

        Dim A2 As String = ""
        If Attacks.Count > 1 Then
            If Not Attacks(1) Is Nothing Then
                A2 = Attacks(1).ToString()
            End If
        End If

        Dim A3 As String = ""
        If Attacks.Count > 2 Then
            If Not Attacks(2) Is Nothing Then
                A3 = Attacks(2).ToString()
            End If
        End If

        Dim A4 As String = ""
        If Attacks.Count > 3 Then
            If Not Attacks(3) Is Nothing Then
                A4 = Attacks(3).ToString()
            End If
        End If

        Dim SaveItemID As String = "0"
        If Not Item Is Nothing Then
            SaveItemID = Item.ID.ToString()
        End If

        Dim ItemData As String = ""
        If Not Item Is Nothing Then
            ItemData = Item.AdditionalData
        End If

        Dim EVSave As String = EVHP & "," & EVAttack & "," & EVDefense & "," & EVSpAttack & "," & EVSpDefense & "," & EVSpeed
        Dim IVSave As String = IVHP & "," & IVAttack & "," & IVDefense & "," & IVSpAttack & "," & IVSpDefense & "," & IVSpeed

        Dim shinyString As String = "0"
        If IsShiny = True Then
            shinyString = "1"
        End If

        If Ability Is Nothing Then
            Ability = NewAbilities(Random.Next(0, NewAbilities.Count))
        End If

        Dim Data As String = "{""Pokemon""[" & Number & "]}" &
        "{""Experience""[" & Experience & "]}" &
        "{""Gender""[" & SaveGender & "]}" &
        "{""EggSteps""[" & EggSteps & "]}" &
        "{""Item""[" & SaveItemID & "]}" &
        "{""ItemData""[" & ItemData & "]}" &
        "{""NickName""[" & NickName & "]}" &
        "{""Level""[" & Level & "]}" &
        "{""OT""[" & OT & "]}" &
        "{""Ability""[" & Ability.ID & "]}" &
        "{""Status""[" & SaveStatus & "]}" &
        "{""Nature""[" & Nature & "]}" &
        "{""CatchLocation""[" & CatchLocation & "]}" &
        "{""CatchTrainer""[" & CatchTrainerName & "]}" &
        "{""CatchBall""[" & CatchBall.ID & "]}" &
        "{""CatchMethod""[" & CatchMethod & "]}" &
        "{""Friendship""[" & Friendship & "]}" &
        "{""isShiny""[" & shinyString & "]}" &
        "{""Attack1""[" & A1 & "]}" &
        "{""Attack2""[" & A2 & "]}" &
        "{""Attack3""[" & A3 & "]}" &
        "{""Attack4""[" & A4 & "]}" &
        "{""HP""[" & HP & "]}" &
        "{""EVs""[" & EVSave & "]}" &
        "{""IVs""[" & IVSave & "]}" &
        "{""AdditionalData""[" & AdditionalData & "]}" &
        "{""IDValue""[" & IndividualValue & "]}"

        Return Data
    End Function

    ''' <summary>
    ''' Generates a Pokémon's initial values.
    ''' </summary>
    ''' <param name="newLevel">The level to set the Pokémon's level to.</param>
    ''' <param name="SetParameters">If the parameters like Nature and Ability should be set. Otherwise, it just loads the attacks and sets the level.</param>
        Public Sub Generate(ByVal newLevel As Integer, ByVal SetParameters As Boolean)
        Level = 0

        If SetParameters = True Then
            GenerateIndividualValue()
            AdditionalData = PokemonForms.GetInitialAdditionalData(Me)

            Nature = CType(Random.Next(0, 25), Natures)

            'Synchronize ability:
            If Core.Player.Pokemons.Count > 0 Then
                If Core.Player.Pokemons(0).Ability.Name.ToLower() = "synchronize" Then
                    If Random.Next(0, 100) < 50 Then
                        Nature = Core.Player.Pokemons(0).Nature
                    End If
                End If
            End If

            If Screen.Level IsNot Nothing Then
                If Screen.Level.HiddenAbilityChance > Random.Next(0, 100) And HasHiddenAbility = True Then
                    Ability = Ability.GetAbilityByID(HiddenAbility.ID)
                Else
                    Ability = Ability.GetAbilityByID(NewAbilities(Random.Next(0, NewAbilities.Count)).ID)
                End If
            End If

            Dim shinyRate As Integer = 8192

            For Each mysteryEvent As MysteryEventScreen.MysteryEvent In MysteryEventScreen.ActivatedMysteryEvents
                If mysteryEvent.EventType = MysteryEventScreen.EventTypes.ShinyMultiplier Then
                    shinyRate = CInt(shinyRate / CSng(mysteryEvent.Value.Replace(".", GameController.DecSeparator)))
                End If
            Next

            'ShinyCharm
            If Core.Player.Inventory.GetItemAmount(242) > 0 Then
                shinyRate = CInt(shinyRate * 0.75F)
            End If

            If Random.Next(0, shinyRate) = 0 And Legendaries.Contains(Number) = False Then
                IsShiny = True
            End If

            If IsGenderless = True Then
                Gender = Genders.Genderless
            Else
                'Determine if Pokemon is male or female depending on the rate defined in the data file:
                If Random.Next(1, 101) > IsMale Then
                    Gender = Genders.Female
                Else
                    Gender = Genders.Male
                End If

                'Cute Charm ability:
                If Core.Player.Pokemons.Count > 0 Then
                    If Core.Player.Pokemons(0).Gender <> Genders.Genderless And Core.Player.Pokemons(0).Ability.Name.ToLower() = "cute charm" Then
                        If Random.Next(0, 100) < 67 Then
                            If Core.Player.Pokemons(0).Gender = Genders.Female Then
                                Gender = Genders.Male
                            Else
                                Gender = Genders.Female
                            End If
                        Else
                            Gender = Core.Player.Pokemons(0).Gender
                        End If
                    End If
                End If
            End If

            'Set the IV values of the Pokémon randomly, range 0-31.
            IVHP = Random.Next(0, 32)
            IVAttack = Random.Next(0, 32)
            IVDefense = Random.Next(0, 32)
            IVSpAttack = Random.Next(0, 32)
            IVSpDefense = Random.Next(0, 32)
            IVSpeed = Random.Next(0, 32)

            Friendship = BaseFriendship

            If _wildItems.Count > 0 Then
                Dim has100 As Boolean = False
                Dim ChosenItemID As Integer = 0
                For i = 0 To _wildItems.Count - 1
                    If _wildItems.Keys(i) = 100 Then
                        has100 = True
                        ChosenItemID = _wildItems.Values(i)
                        Exit For
                    End If
                Next
                If has100 = True Then
                    Item = Item.GetItemByID(ChosenItemID)
                Else
                    Dim usedWildItems As Dictionary(Of Integer, Integer) = _wildItems

                    'Compound eyes ability:
                    If Core.Player.Pokemons.Count > 0 Then
                        If Core.Player.Pokemons(0).Ability.Name.ToLower() = "compound eyes" Then
                            usedWildItems = Abilities.Compoundeyes.ConvertItemChances(usedWildItems)
                        End If
                    End If

                    For i = 0 To usedWildItems.Count - 1
                        Dim v As Integer = Random.Next(0, 100)
                        If v < usedWildItems.Keys(i) Then
                            ChosenItemID = usedWildItems.Values(i)
                            Exit For
                        End If
                    Next
                    Item = Item.GetItemByID(ChosenItemID)
                End If
            End If
        End If

        'Level the Pokémon up and give the appropriate move set for its new level:
        While newLevel > Level
            LevelUp(False)
            Experience = NeedExperience(Level)
        End While

        Dim canLearnMoves As New List(Of BattleSystem.Attack)
        For i = 0 To AttackLearns.Count - 1
            If AttackLearns.Keys(i) <= Level Then

                Dim hasMove As Boolean = False
                For Each m As BattleSystem.Attack In Attacks
                    If m.ID = AttackLearns.Values(i).ID Then
                        hasMove = True
                        Exit For
                    End If
                Next
                If hasMove = False Then
                    For Each m As BattleSystem.Attack In canLearnMoves
                        If m.ID = AttackLearns.Values(i).ID Then
                            hasMove = True
                            Exit For
                        End If
                    Next
                End If

                If hasMove = False Then
                    canLearnMoves.Add(AttackLearns.Values(i))
                End If
            End If
        Next

        If canLearnMoves.Count > 0 Then
            Attacks.Clear()

            Dim startIndex As Integer = canLearnMoves.Count - 4

            If startIndex < 0 Then
                startIndex = 0
            End If

            For t = startIndex To canLearnMoves.Count - 1
                Attacks.Add(canLearnMoves(t))
            Next
        End If

        HP = MaxHP
    End Sub

#End Region

#Region "Converters"

    ''' <summary>
    ''' Converts an EggGroup ID string to the EggGroup enum item.
    ''' </summary>
    ''' <param name="ID">The ID string.</param>
    ''' <returns></returns>
        Public Shared Function ConvertIDToEggGroup(ByVal ID As String) As EggGroups
        Select Case ID.ToLower()
            Case "monster"
                Return EggGroups.Monster
            Case "water1"
                Return EggGroups.Water1
            Case "water2"
                Return EggGroups.Water2
            Case "water3"
                Return EggGroups.Water3
            Case "bug"
                Return EggGroups.Bug
            Case "flying"
                Return EggGroups.Flying
            Case "field"
                Return EggGroups.Field
            Case "fairy"
                Return EggGroups.Fairy
            Case "grass"
                Return EggGroups.Grass
            Case "undiscovered"
                Return EggGroups.Undiscovered
            Case "humanlike"
                Return EggGroups.HumanLike
            Case "mineral"
                Return EggGroups.Mineral
            Case "amorphous"
                Return EggGroups.Amorphous
            Case "ditto"
                Return EggGroups.Ditto
            Case "dragon"
                Return EggGroups.Dragon
            Case "genderunknown"
                Return EggGroups.GenderUnknown
            Case "none", "", "0", "nothing", "null"
                Return EggGroups.None
        End Select

        Return EggGroups.None
    End Function

    ''' <summary>
    ''' Converts a Nature ID to a Nature enum item.
    ''' </summary>
    ''' <param name="ID">The nature ID.</param>
    ''' <returns></returns>
        Public Shared Function ConvertIDToNature(ByVal ID As Integer) As Natures
        'Catch out of range arg:
        If ID < 0 Or ID > 24 Then
            ID = 0
        End If
        Return CType(ID, Natures)
    End Function

#End Region

    ''' <summary>
    ''' Generates a new individual value for this Pokémon.
    ''' </summary>
        Private Sub GenerateIndividualValue()
        Dim chars As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"

        Dim s As String = ""
        For x = 0 To 10
            s &= chars(Random.Next(0, chars.Length)).ToString()
        Next

        IndividualValue = s
    End Sub

    ''' <summary>
    ''' Returns the Display Name of this Pokémon.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Returns "Egg" when the Pokémon is in an egg. Returns the properly translated name if it exists. Returns the nickname if set.</remarks>
    Public Function GetDisplayName() As String
        If EggSteps > 0 Then
            Return "Egg"
        Else
            If NickName = "" Then
                If Localization.TokenExists("pokemon_name_" & Name) = True Then
                    Return Localization.GetString("pokemon_name_" & Name)
                Else
                    Return Name
                End If
            Else
                Return NickName
            End If
        End If
    End Function

    ''' <summary>
    ''' Returns the properly translated name of a Pokémon if defined in the language files.
    ''' </summary>
    ''' <returns></returns>
        Public Function GetName() As String
        If Localization.TokenExists("pokemon_name_" & Name) = True Then
            Return Localization.GetString("pokemon_name_" & Name)
        Else
            Return Name
        End If
    End Function

    ''' <summary>
    ''' Returns the English name of the Pokémon.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public Property OriginalName As String
        Get
            Return Name
        End Get
        Set(value As String)
            Name = value
        End Set
    End Property

#Region "Experience, Level Up and Stats"

    ''' <summary>
    ''' Gives the Pokémon experience points and levels it up.
    ''' </summary>
    ''' <param name="Exp">The amount of EXP.</param>
    ''' <param name="LearnRandomAttack">If the Pokémon should learn an attack if it could learn one at level up.</param>
        Public Sub GetExperience(ByVal Exp As Integer, ByVal LearnRandomAttack As Boolean)
        Experience += Exp
        While Experience >= NeedExperience(Level + 1)
            LevelUp(LearnRandomAttack)
        End While
        Level = Level.Clamp(1, CInt(GameModeManager.GetGameRuleValue("MaxLevel", "100")))
    End Sub

    ''' <summary>
    ''' Rasies the Pokémon's level by one.
    ''' </summary>
    ''' <param name="LearnRandomAttack">If one attack of the Pokémon should be replaced by an attack potentially learned on the new level.</param>
        Public Sub LevelUp(ByVal LearnRandomAttack As Boolean)
        Level += 1

        Dim currentMaxHP As Integer = MaxHP

        CalculateStats()

        'Heals the Pokémon by the HP difference.
        Dim HPDifference As Integer = MaxHP - currentMaxHP
        If HPDifference > 0 Then
            Heal(HPDifference)
        End If

        If LearnRandomAttack = True Then
            LearnAttack(Level)
        End If
    End Sub

    ''' <summary>
    ''' Recalculates all stats for this Pokémon using its current EVs, IVs and level.
    ''' </summary>
        Public Sub CalculateStats()
        MaxHP = CalcStatus(Level, True, BaseHP, EVHP, IVHP, "HP")
        Attack = CalcStatus(Level, False, BaseAttack, EVAttack, IVAttack, "Attack")
        Defense = CalcStatus(Level, False, BaseDefense, EVDefense, IVDefense, "Defense")
        SpAttack = CalcStatus(Level, False, BaseSpAttack, EVSpAttack, IVSpAttack, "SpAttack")
        SpDefense = CalcStatus(Level, False, BaseSpDefense, EVSpDefense, IVSpDefense, "SpDefense")
        Speed = CalcStatus(Level, False, BaseSpeed, EVSpeed, IVSpeed, "Speed")
    End Sub

    ''' <summary>
    ''' Gets the value of a status.
    ''' </summary>
    ''' <param name="calcLevel">The level of the Pokémon.</param>
    ''' <param name="DoHP">If the requested stat is HP.</param>
    ''' <param name="baseStat">The base stat of the Pokémon.</param>
    ''' <param name="EVStat">The EV stat of the Pokémon.</param>
    ''' <param name="IVStat">The IV stat of the Pokémon.</param>
    ''' <param name="StatName">The name of the stat.</param>
    ''' <returns></returns>
        Private Function CalcStatus(ByVal calcLevel As Integer, ByVal DoHP As Boolean, ByVal baseStat As Integer, ByVal EVStat As Integer, ByVal IVStat As Integer, ByVal StatName As String) As Integer
        If DoHP = True Then
            If Number = 292 Then
                Return 1
            Else
                Return CInt(Math.Floor((((IVStat + (2 * baseStat) + (EVStat / 4) + 100) * calcLevel) / 100) + 10))
            End If
        Else
            Return CInt(Math.Floor(((((IVStat + (2 * baseStat) + (EVStat / 4)) * calcLevel) / 100) + 5) * Pokemon3D.Nature.GetMultiplier(Nature, StatName)))
        End If
    End Function

    ''' <summary>
    ''' Replaces a random move of the Pokémon by one that it learns on a given level.
    ''' </summary>
    ''' <param name="learnLevel">The level the Pokémon learns the desired move on.</param>
        Public Sub LearnAttack(ByVal learnLevel As Integer)
        If AttackLearns.ContainsKey(learnLevel) = True Then
            Dim a As BattleSystem.Attack = AttackLearns(learnLevel)

            For Each la As BattleSystem.Attack In Attacks
                If la.ID = a.ID Then
                    Exit Sub 'Pokémon already knows that attack.
                End If
            Next

            Attacks.Add(a)

            If Attacks.Count = 5 Then
                Attacks.RemoveAt(Random.Next(0, 5))
            End If
        End If
    End Sub

    ''' <summary>
    ''' Returns the EXP needed for the given level.
    ''' </summary>
    ''' <param name="EXPLevel">The level this function should return the exp amount for.</param>
    ''' <returns></returns>
        Public Function NeedExperience(ByVal EXPLevel As Integer) As Integer
        Dim n As Integer = EXPLevel
        Dim i As Integer = 0

        Select Case ExperienceType
            Case ExperienceTypes.Fast
                i = CInt((4 * n * n * n) / 5)
            Case ExperienceTypes.MediumFast
                i = CInt(n * n * n)
            Case ExperienceTypes.MediumSlow
                i = CInt(((6 * n * n * n) / 5) - (15 * n * n) + (100 * n) - 140)
            Case ExperienceTypes.Slow
                i = CInt((5 * n * n * n) / 4)
            Case Else
                i = CInt(n * n * n)
        End Select

        If i < 0 Then
            i = 0
        End If

        Return i
    End Function

    ''' <summary>
    ''' Returns the cummulative PP count of all moves.
    ''' </summary>
    ''' <returns></returns>
        Public Function CountPP() As Integer
        Dim AllPP As Integer = 0
        For Each Attack As BattleSystem.Attack In Attacks
            AllPP += Attack.CurrentPP
        Next
        Return AllPP
    End Function

    ''' <summary>
    ''' Fully heals this Pokémon.
    ''' </summary>
        Public Sub FullRestore()
        Status = StatusProblems.None
        Heal(MaxHP)
        _volatiles.Clear()
        If Attacks.Count > 0 Then
            For d = 0 To Attacks.Count - 1
                Attacks(d).CurrentPP = Attacks(d).MaxPP
            Next
        End If
    End Sub

    ''' <summary>
    ''' Heals the Pokémon.
    ''' </summary>
    ''' <param name="HealHP">The amount of HP to heal the Pokémon by.</param>
        Public Sub Heal(ByVal HealHP As Integer)
        HP = (HP + HealHP).Clamp(0, MaxHP)
    End Sub

    ''' <summary>
    ''' Changes the Friendship value of this Pokémon.
    ''' </summary>
    ''' <param name="cause">The cause as to why the Friendship value should change.</param>
        Public Sub ChangeFriendShip(ByVal cause As FriendShipCauses)
        Dim add As Integer = 0

        Select Case cause
            Case FriendShipCauses.Walking
                add = 1
            Case FriendShipCauses.LevelUp
                If Friendship <= 99 Then
                    add = 5
                ElseIf Friendship > 99 And Friendship <= 199 Then
                    add = 3
                Else
                    add = 2
                End If
            Case FriendShipCauses.Fainting
                Friendship -= 1
            Case FriendShipCauses.EnergyPowder, FriendShipCauses.HealPowder
                If Friendship <= 99 Then
                    add = -5
                ElseIf Friendship > 99 And Friendship <= 199 Then
                    add = -5
                Else
                    add = -10
                End If
            Case FriendShipCauses.EnergyRoot
                If Friendship <= 99 Then
                    add = -10
                ElseIf Friendship > 99 And Friendship <= 199 Then
                    add = -10
                Else
                    add = -15
                End If
            Case FriendShipCauses.RevivalHerb
                If Friendship <= 99 Then
                    add = -15
                ElseIf Friendship > 99 And Friendship <= 199 Then
                    add = -15
                Else
                    add = -20
                End If
            Case FriendShipCauses.Trading
                Friendship = BaseFriendship
            Case FriendShipCauses.Vitamin
                If Friendship <= 99 Then
                    add = 5
                ElseIf Friendship > 99 And Friendship <= 199 Then
                    add = 3
                Else
                    add = 2
                End If
            Case FriendShipCauses.EVBerry
                If Friendship <= 99 Then
                    add = 10
                ElseIf Friendship > 99 And Friendship <= 199 Then
                    add = 5
                Else
                    add = 2
                End If
        End Select

        If add > 0 Then
            If CatchBall.ID = 174 Then
                add += 1
            End If
            If Not Item Is Nothing Then
                If Item.Name.ToLower() = "soothe bell" Then
                    add *= 2
                End If
            End If
        End If

        If add <> 0 Then
            Friendship += add
        End If

        Friendship = CInt(MathHelper.Clamp(Friendship, 0, 255))
    End Sub

#End Region

#Region "Textures/Models"

    ''' <summary>
    ''' Returns a texture for this Pokémon.
    ''' </summary>
    ''' <param name="index">0=normal,front
    ''' 1=normal,back
    ''' 2=shiny,front
    ''' 3=shiny,back
    ''' 4=menu sprite
    ''' 5=egg menu sprite
    ''' 6=Egg front sprite
    ''' 7=Egg back sprite
    ''' 8=normal overworld
    ''' 9=shiny overworld
    ''' 10=normal,front,animation</param>
    ''' <returns></returns>
        Private Function GetTexture(ByVal index As Integer) As Texture2D
        If Textures(index) Is Nothing Then
            Select Case index
                Case 0
                    Textures(index) = TextureManager.GetTexture("Pokemon\Sprites\" & AnimationName, New Rectangle(0, 0, 128, 128), "")
                Case 1
                    Textures(index) = TextureManager.GetTexture("Pokemon\Sprites\" & AnimationName, New Rectangle(128, 0, 128, 128), "")
                Case 2
                    Textures(index) = TextureManager.GetTexture("Pokemon\Sprites\" & AnimationName, New Rectangle(0, 128, 128, 128), "")
                Case 3
                    Textures(index) = TextureManager.GetTexture("Pokemon\Sprites\" & AnimationName, New Rectangle(128, 128, 128, 128), "")
                Case 4
                    Dim v As Vector2 = PokemonForms.GetMenuImagePosition(Me)
                    Dim s As Size = PokemonForms.GetMenuImageSize(Me)

                    Dim shiny As String = ""
                    If IsShiny = True Then
                        shiny = "Shiny"
                    End If

                    Textures(index) = TextureManager.GetTexture("GUI\PokemonMenu" & shiny, New Rectangle(CInt(v.X) * 32, CInt(v.Y) * 32, s.Width, s.Height), "")
                Case 5
                    Textures(index) = EggCreator.CreateEggSprite(Me, TextureManager.GetTexture("GUI\PokemonMenu", New Rectangle(992, 992, 32, 32), ""), TextureManager.GetTexture("Pokemon\Egg\Templates\Menu"))
                Case 6
                    Textures(index) = EggCreator.CreateEggSprite(Me, TextureManager.GetTexture("Pokemon\Egg\Egg_front"), TextureManager.GetTexture("Pokemon\Egg\Templates\Front"))
                Case 7
                    Textures(index) = EggCreator.CreateEggSprite(Me, TextureManager.GetTexture("Pokemon\Egg\Egg_back"), TextureManager.GetTexture("Pokemon\Egg\Templates\Back"))
                Case 8
                    Dim addition As String = PokemonForms.GetOverworldAddition(Me)
                    Textures(index) = TextureManager.GetTexture("Pokemon\Overworld\Normal\" & Number & addition)
                Case 9
                    Dim addition As String = PokemonForms.GetOverworldAddition(Me)
                    Textures(index) = TextureManager.GetTexture("Pokemon\Overworld\Shiny\" & Number & addition)
            End Select
        End If

        Return Textures(index)
    End Function

    ''' <summary>
    ''' Returns the Overworld texture of this Pokémon.
    ''' </summary>
    ''' <returns></returns>
        Public Function GetOverworldTexture() As Texture2D
        If IsShiny = False Then
            Return GetTexture(8)
        Else
            Return GetTexture(9)
        End If
    End Function

    ''' <summary>
    ''' Returns the Menu Texture of this Pokémon.
    ''' </summary>
    ''' <param name="CanGetEgg">If the texture returned can represent the Pokémon in its egg.</param>
    ''' <returns></returns>
        Public Function GetMenuTexture(ByVal CanGetEgg As Boolean) As Texture2D
        If EggSteps > 0 And CanGetEgg = True Then
            Return GetTexture(5)
        Else
            Return GetTexture(4)
        End If
    End Function

    ''' <summary>
    ''' Returns the Menu Texture of this Pokémon.
    ''' </summary>
    ''' <returns></returns>
        Public Function GetMenuTexture() As Texture2D
        Return GetMenuTexture(True)
    End Function

    ''' <summary>
    ''' Returns the display texture of this Pokémon.
    ''' </summary>
    ''' <param name="FrontView">If this Pokémon should be viewed from the front.</param>
    ''' <returns></returns>
        Public Function GetTexture(ByVal FrontView As Boolean) As Texture2D
        If FrontView = True Then
            If IsEgg() = True Then
                Return GetTexture(6)
            Else
                If IsShiny = True Then
                    Return GetTexture(2)
                Else
                    Return GetTexture(0)
                End If
            End If
        Else
            If IsEgg() = True Then
                Return GetTexture(7)
            Else
                If IsShiny = True Then
                    Return GetTexture(3)
                Else
                    Return GetTexture(1)
                End If
            End If
        End If
    End Function

    ''' <summary>
    ''' Returns properties to display models on a 2D GUI. Data structure: scale sng,posX sng,posY sng,posZ sng,roll sng
    ''' </summary>
    ''' <returns></returns>
        Public Function GetModelProperties() As Tuple(Of Single, Single, Single, Single, Single)
        Dim scale As Single = CSng(0.6 / PokedexEntry.Height)
        Dim x As Single = 0.0F
        Dim y As Single = 0.0F
        Dim z As Single = 0.0F

        Dim roll As Single = 0.3F

        Select Case Number
            Case 6
                scale = 0.55
            Case 9
                scale = 0.7F
            Case 15
                z = 4.0F
            Case 19
                scale = 1.1
            Case 20
                scale = 1.3
            Case 23
                scale = 1
            Case 24
                scale = 0.5
            Case 41
                z = 5.0F
            Case 55
                scale = 0.7
            Case 63
                z = -4.0F
            Case 74
                scale = 0.75
            Case 81
                z = 6.0F
            Case 82
                scale = 0.9
                z = 6.0F
            Case 95
                x = -6
                scale = 0.3
            Case 98
                scale = 1
            Case 102
                scale = 0.9
            Case 103
                scale = 0.45
            Case 115
                scale = 0.45
            Case 129
                z = -4.0F
            Case 130
                scale = 0.25
            Case 131
                z = -8
            Case 143
                scale = 0.5
                roll = 1.2F
            Case 144
                z = -9
                scale = 0.35
            Case 147
                scale = 0.7
            Case 148
                x = 5.0F
                scale = 0.4
            Case 149, 150
                scale = 0.42F
            Case 151
                z = 5
            Case 157
                scale = 0.6
            Case 160
                scale = 0.5
            Case 162
                scale = 0.8
            Case 164
                z = -3
            Case 168
                scale = 0.8
            Case 180
                scale = 0.5
            Case 181
                scale = 0.75
            Case 184, 185
                scale = 0.8
            Case 187
                scale = 0.65
            Case 196, 197
                scale = 0.900000036F
            Case 206
                scale = 0.9F
            Case 208
                scale = 0.4
            Case 211
                z = 5
            Case 212
                scale = 0.7
            Case 214
                scale = 1.2
            Case 217
                scale = 0.55
            Case 223
                z = -5
            Case 229
                scale = 0.8
            Case 230
                scale = 0.6
                z = 3
            Case 235
                scale = 0.7
            Case 241
                scale = 0.7
            Case 247
                scale = 0.7
            Case 248
                scale = 0.6
            Case 249
                scale = 0.3
            Case 250
                scale = 0.2
            Case 336
                scale = 0.8
        End Select

        Return New Tuple(Of Single, Single, Single, Single, Single)(scale, x, y, z, roll)
    End Function

#End Region

    ''' <summary>
    ''' Checks if this Pokémon can evolve by a given EvolutionTrigger and EvolutionArgument.
    ''' </summary>
    ''' <param name="trigger">The trigger of the evolution.</param>
    ''' <param name="argument">An argument that specifies the evolution.</param>
    ''' <returns></returns>
        Public Function CanEvolve(ByVal trigger As EvolutionCondition.EvolutionTrigger, ByVal argument As String) As Boolean
        Dim n As Integer = EvolutionCondition.EvolutionNumber(Me, trigger, argument)
        If n = 0 Then
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Returns the evolution ID of this Pokémon.
    ''' </summary>
    ''' <param name="trigger">The trigger of the evolution.</param>
    ''' <param name="argument">An argument that specifies the evolution.</param>
    ''' <returns></returns>
        Public Function GetEvolutionID(ByVal trigger As EvolutionCondition.EvolutionTrigger, ByVal argument As String) As Integer
        Return EvolutionCondition.EvolutionNumber(Me, trigger, argument)
    End Function

    ''' <summary>
    ''' Sets the catch infos of the Pokémon. Uses the current map name and player name + OT.
    ''' </summary>
    ''' <param name="Ball">The Pokéball this Pokémon got captured in.</param>
    ''' <param name="Method">The capture method.</param>
        Public Sub SetCatchInfos(ByVal Ball As Item, ByVal Method As String)
        CatchLocation = Screen.Level.MapName
        CatchTrainerName = Core.Player.Name
        OT = Core.Player.OT

        CatchMethod = Method
        CatchBall = Ball
    End Sub

    ''' <summary>
    ''' Checks if the Pokémon is of a certain type.
    ''' </summary>
    ''' <param name="CheckType">The type to check.</param>
    ''' <returns></returns>
        Public Function IsType(ByVal CheckType As Element.Types) As Boolean
        If Type1.Type = CheckType Or Type2.Type = CheckType Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Plays the cry of this Pokémon.
    ''' </summary>
        Public Sub PlayCry()
        Dim Pitch As Single = 0.0F
        Dim percent As Integer = 100
        If HP > 0 And MaxHP > 0 Then
            percent = CInt(Math.Ceiling(HP / MaxHP) * 100)
        End If

        If percent <= 50 Then
            Pitch = -0.4F
        End If
        If percent <= 15 Then
            Pitch = -0.8F
        End If
        If percent = 0 Then
            Pitch = -1.0F
        End If

        SoundManager.PlayPokemonCry(Number, Pitch, 0F)
    End Sub

    ''' <summary>
    ''' Checks if this Pokémon knows a certain move.
    ''' </summary>
    ''' <param name="Move">The move to check for.</param>
    ''' <returns></returns>
        Public Function KnowsMove(ByVal Move As BattleSystem.Attack) As Boolean
        For Each a As BattleSystem.Attack In Attacks
            If a.ID = Move.ID Then
                Return True
            End If
        Next

        Return False
    End Function

    ''' <summary>
    ''' Checks if this Pokémon is inside an Egg.
    ''' </summary>
    ''' <returns></returns>
        Public Function IsEgg() As Boolean
        If EggSteps > 0 Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Adds Effort values (EV) to this Pokémon after defeated another Pokémon, if possible.
    ''' </summary>
    ''' <param name="DefeatedPokemon">The defeated Pokémon.</param>
        Public Sub GainEffort(ByVal DefeatedPokemon As Pokemon)
        Dim allEV As Integer = EVHP + EVAttack + EVDefense + EVSpeed + EVSpAttack + EVSpDefense
        If allEV < 510 Then
            Dim maxGainEV As Integer = 0
            If allEV < 510 Then
                maxGainEV = 510 - allEV
            End If
            If maxGainEV > 0 Then
                maxGainEV = CInt(MathHelper.Clamp(maxGainEV, 1, 3))

                If EVHP < 255 And DefeatedPokemon.GiveEVHP > 0 Then
                    Dim gainHPEV As Integer = DefeatedPokemon.GiveEVHP
                    gainHPEV = CInt(MathHelper.Clamp(gainHPEV, 0, 255 - EVHP))
                    EVHP += gainHPEV
                End If

                If EVAttack < 255 And DefeatedPokemon.GiveEVAttack > 0 Then
                    Dim gainAttackEV As Integer = DefeatedPokemon.GiveEVAttack
                    gainAttackEV = CInt(MathHelper.Clamp(gainAttackEV, 0, 255 - EVAttack))
                    EVAttack += gainAttackEV
                End If

                If EVDefense < 255 And DefeatedPokemon.GiveEVDefense > 0 Then
                    Dim gainDefenseEV As Integer = DefeatedPokemon.GiveEVDefense
                    gainDefenseEV = CInt(MathHelper.Clamp(gainDefenseEV, 0, 255 - EVDefense))
                    EVDefense += gainDefenseEV
                End If

                If EVSpeed < 255 And DefeatedPokemon.GiveEVSpeed > 0 Then
                    Dim gainSpeedEV As Integer = DefeatedPokemon.GiveEVSpeed
                    gainSpeedEV = CInt(MathHelper.Clamp(gainSpeedEV, 0, 255 - EVSpeed))
                    EVSpeed += gainSpeedEV
                End If

                If EVSpAttack < 255 And DefeatedPokemon.GiveEVSpAttack > 0 Then
                    Dim gainSpAttackEV As Integer = DefeatedPokemon.GiveEVSpAttack
                    gainSpAttackEV = CInt(MathHelper.Clamp(gainSpAttackEV, 0, 255 - EVSpAttack))
                    EVSpAttack += gainSpAttackEV
                End If

                If EVSpDefense < 255 And DefeatedPokemon.GiveEVSpDefense > 0 Then
                    Dim gainSpDefenseEV As Integer = DefeatedPokemon.GiveEVSpDefense
                    gainSpDefenseEV = CInt(MathHelper.Clamp(gainSpDefenseEV, 0, 255 - EVSpDefense))
                    EVSpDefense += gainSpDefenseEV
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Returns if this Pokémon knows an HM move.
    ''' </summary>
    ''' <returns></returns>
        Public Function HasHMMove() As Boolean
        For Each m As BattleSystem.Attack In Attacks
            If m.IsHMMove = True Then
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' Returns if this Pokémon is fully evolved.
    ''' </summary>
    ''' <returns></returns>
        Public Function IsFullyEvolved() As Boolean
        Return EvolutionConditions.Count = 0
    End Function

    ''' <summary>
    ''' Checks if this Pokémon has a Hidden Ability assigend to it.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public ReadOnly Property HasHiddenAbility() As Boolean
        Get
            Return Not HiddenAbility Is Nothing
        End Get
    End Property

    ''' <summary>
    ''' Checks if the Pokémon has its Hidden Ability set as its current ability.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
        Public ReadOnly Property IsUsingHiddenAbility() As Boolean
        Get
            If HasHiddenAbility() = True Then
                Return HiddenAbility.ID = Ability.ID
            End If
            Return False
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return GetSaveData()
    End Function

End Class