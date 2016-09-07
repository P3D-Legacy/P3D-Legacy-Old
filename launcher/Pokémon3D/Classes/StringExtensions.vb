Imports System.Runtime.CompilerServices

Module StringExtensions

    <Extension()>
    Public Function GetSplit(ByVal fullString As String, ByVal valueIndex As Integer, ByVal seperator As String) As String
        If valueIndex = 0 Then
            Return fullString.Remove(fullString.IndexOf(seperator))
        Else
            For x = 0 To valueIndex - 1
                fullString = fullString.Remove(0, fullString.IndexOf(seperator) + 1)
            Next
            If fullString.Contains(seperator) = True Then
                While fullString.Contains(seperator) = True
                    fullString = fullString.Remove(fullString.IndexOf(seperator))
                End While
            End If
            Return fullString
        End If
    End Function

    <Extension()>
    Public Function GetSplit(ByVal fullString As String, ByVal valueIndex As Integer) As String
        Return GetSplit(fullString, valueIndex, ",")
    End Function

    <Extension()>
    Public Function SetSplit(ByVal fullString As String, ByVal valueIndex As Integer, ByVal newValue As String, ByVal seperator As String, ByVal replace As Boolean) As String
        Dim s() As String = fullString.Split(CChar(seperator))

        fullString = ""

        For x = 0 To s.Count - 1
            If x = valueIndex Then
                If replace = True Then
                    fullString &= newValue
                Else
                    fullString &= newValue & seperator & s(x)
                End If
            Else
                fullString &= s(x)
            End If

            If x <> s.Count - 1 Then
                fullString &= seperator
            End If
        Next

        Return fullString
    End Function

    <Extension()>
    Public Function SetSplit(ByVal fullString As String, ByVal valueIndex As Integer, ByVal newValue As String, ByVal replace As Boolean) As String
        Return SetSplit(fullString, valueIndex, newValue, ",", replace)
    End Function

    <Extension()>
    Public Function SetSplit(ByVal fullString As String, ByVal valueIndex As Integer, ByVal newValue As String, ByVal seperator As String) As String
        Return SetSplit(fullString, valueIndex, newValue, seperator, True)
    End Function

    <Extension()>
    Public Function SetSplit(ByVal fullString As String, ByVal valueIndex As Integer, ByVal newValue As String) As String
        Return SetSplit(fullString, valueIndex, newValue, ",", True)
    End Function

    <Extension()>
    Public Function ToInteger(ByVal bool As Boolean) As Integer
        If bool = True Then
            Return "1"
        Else
            Return "0"
        End If
    End Function

End Module