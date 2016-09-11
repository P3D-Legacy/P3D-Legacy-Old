Public Class MapManager

    Private Shared _stubs As New List(Of MapStub)

    Public Shared Function GetMapStub() As MapStub

    End Function

    Public Shared Sub Clear()
        _stubs.Clear()
    End Sub

    Public Shared Sub ApplyMapToLevel(ByVal stub As MapStub, ByVal level As Level)

    End Sub

End Class