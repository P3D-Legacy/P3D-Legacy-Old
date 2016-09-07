Imports System.Runtime.CompilerServices

Public Module Extensions

    <Extension()>
    Public Function ToNumberString(ByVal b As Boolean) As String
        If b = True Then
            Return "1"
        Else
            Return "0"
        End If
    End Function

    <Extension()>
    Public Function Clamp(ByVal i As Integer, ByVal min As Integer, ByVal max As Integer) As Integer
        If i < min Then
            Return min
        ElseIf i > max Then
            Return max
        Else
            Return i
        End If
    End Function

End Module