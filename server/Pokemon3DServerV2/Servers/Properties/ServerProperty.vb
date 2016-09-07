Namespace Servers.Properties

    Public Class ServerProperty

#Region "Fields"

        Private _name As String
        Private _value As String

#End Region

#Region "Properties"

        Public ReadOnly Property Name() As String
            Get
                Return Me._name
            End Get
        End Property

        Public Property Value() As String
            Get
                Return Me._value
            End Get
            Set(value As String)
                Me._value = value
            End Set
        End Property

#End Region

#Region "Constructors"

        Public Sub New(ByVal Data As String)
            Me._name = Data.Remove(Data.IndexOf("="))
            Me._value = Data.Remove(0, Data.IndexOf("=") + 1)
        End Sub

        Public Sub New(ByVal Name As String, ByVal Value As String)
            Me._name = Name
            Me._value = Value
        End Sub

#End Region

#Region "Methods"

        ''' <summary>
        ''' Returns saveable data of this property.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function ToString() As String
            Return Me._name & "=" & Me._value
        End Function

#End Region

    End Class

End Namespace