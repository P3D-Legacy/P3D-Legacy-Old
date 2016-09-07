''' <summary>
''' Holds thread-safe global objects.
''' </summary>
''' <remarks></remarks>
Public Class Basic

    Private Shared _MainformReference As Mainform

    Public Shared Sub SetMainform(ByVal MainformReference As Mainform)
        _MainformReference = MainformReference
    End Sub

    ''' <summary>
    ''' The currently active instance of the ServersManager from the Mainform class instance.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property ServersManager() As Servers.ServersManager
        Get
            Return _MainformReference.ServersManager
        End Get
    End Property

    ''' <summary>
    ''' The currently active Mainform instance.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property MainformReference() As Mainform
        Get
            Return _MainformReference
        End Get
    End Property

    Public Shared ReadOnly Property GetPropertyValue(ByVal Name As String, ByVal DefaultValue As String) As String
        Get
            Return _MainformReference.ServersManager.PropertyCollection.GetPropertyValue(Name, DefaultValue)
        End Get
    End Property

End Class