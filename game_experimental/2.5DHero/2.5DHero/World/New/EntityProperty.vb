''' <summary>
''' The base class for an entity property.
''' </summary>
Public MustInherit Class EntityProperty

    ''' <summary>
    ''' The types of property of an entity.
    ''' </summary>
    Public Enum EntityPropertyType
        Stairs
        Billboard
        Sign
        Warp
        Floor
        Ledge
        CutTree
        Water
        Grass
        BerryPlant
        LoamySoil
        FieldItem
        Script
        Turning
        ApricornPlant
        HeadbuttTree
        SmashRock
        StrengthRock
        NPC
        Waterfall
        Whirlpool
        StrengthTrigger
        SpinTile
        Dive
        RockClimb
    End Enum

    Public Enum EntityPropertyRenderResultType
        Rendered
        RenderedButPassed
        Passed
    End Enum

    Public Enum FunctionResponse
        ValueFalse
        ValueTrue
        NoValue
    End Enum

    Private _parent As Entity
    Private _propertyType As EntityPropertyType

    Protected ReadOnly Property Parent() As Entity
        Get
            Return _parent
        End Get
    End Property

    Public ReadOnly Property PropertyType() As EntityPropertyType
        Get
            Return _propertyType
        End Get
    End Property

    Protected Sub New(ByVal propertyType As EntityPropertyType, ByVal parent As Entity)
        _propertyType = propertyType
        _parent = parent
    End Sub

    Public Overridable Sub Update()

    End Sub

    Public Overridable Function Render() As EntityPropertyRenderResultType
        Return EntityPropertyRenderResultType.Passed
    End Function

    Public Overridable Sub Click()
    End Sub

    Public Overridable Sub WalkOnto()
    End Sub

    Public Overridable Sub ChooseBoxResult(ByVal resultIndex As Integer)
    End Sub

    Public Overridable Function WalkAgainst() As FunctionResponse
        Return FunctionResponse.NoValue
    End Function

    Public Overridable Function WalkInto() As FunctionResponse
        Return FunctionResponse.NoValue
    End Function

    Public Overridable Function LetPlayerMove() As FunctionResponse
        Return FunctionResponse.NoValue
    End Function

End Class