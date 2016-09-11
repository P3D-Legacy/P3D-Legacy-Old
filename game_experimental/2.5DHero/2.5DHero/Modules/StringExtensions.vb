Imports System.Runtime.CompilerServices

Module StringExtensions

    ''' <summary>
    ''' Returns a part of a string.
    ''' </summary>
    ''' <param name="fullString">The full string.</param>
    ''' <param name="valueIndex">The index of the part.</param>
    ''' <param name="seperator">The separator to separate the parts.</param>
    ''' <returns></returns>
    <Extension()>
    Public Function GetSplit(ByVal fullString As String, ByVal valueIndex As Integer, ByVal seperator As String) As String
        If fullString.Contains(seperator) = False Then
            Return fullString
        Else
            Dim parts() As String = fullString.Split({seperator}, StringSplitOptions.None)
            If parts.Count - 1 >= valueIndex Then
                Return parts(valueIndex)
            Else
                Return fullString
            End If
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="fullString"></param>
    ''' <param name="valueIndex"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function GetSplit(ByVal fullString As String, ByVal valueIndex As Integer) As String
        Return GetSplit(fullString, valueIndex, ",")
    End Function

    <Extension()>
    Public Function ToNumberString(ByVal bool As Boolean) As String
        If bool = True Then
            Return "1"
        Else
            Return "0"
        End If
    End Function

    <Extension()>
    Public Function Crop(ByVal s As String, ByVal font As SpriteFont, ByVal width As Integer) As String
        Return Crop(s, font, 1.0F, width)
    End Function

    <Extension()>
    Public Function Crop(ByVal s As String, ByVal font As SpriteFont, ByVal scale As Single, ByVal width As Integer) As String
        Dim fulltext As String = s

        If (font.MeasureString(fulltext).X * scale) <= width Then
            Return fulltext
        Else
            If fulltext.Contains(" ") = False Then
                Dim newText As String = ""
                While fulltext.Length > 0
                    If (font.MeasureString(newText & fulltext(0).ToString()).X * scale) > width Then
                        newText &= vbNewLine
                        newText &= fulltext(0).ToString()
                        fulltext.Remove(0, 1)
                    Else
                        newText &= fulltext(0).ToString()
                        fulltext.Remove(0, 1)
                    End If
                End While
                Return newText
            End If
        End If

        Dim output As String = ""
        Dim currentLine As String = ""
        Dim currentWord As String = ""

        While fulltext.Length > 0
            If fulltext.StartsWith(vbNewLine) = True Then
                If currentLine <> "" Then
                    currentLine &= " "
                End If
                currentLine &= currentWord
                output &= currentLine & vbNewLine
                currentLine = ""
                currentWord = ""
                fulltext = fulltext.Remove(0, 2)
            ElseIf fulltext.StartsWith(" ") = True Then
                If currentLine <> "" Then
                    currentLine &= " "
                End If
                currentLine &= currentWord
                currentWord = ""
                fulltext = fulltext.Remove(0, 1)
            Else
                currentWord &= fulltext(0)
                If (font.MeasureString(currentLine & currentWord).X * scale) >= width Then
                    If currentLine = "" Then
                        output &= currentWord & vbNewLine
                        currentWord = ""
                        currentLine = ""
                    Else
                        output &= currentLine & vbNewLine
                        currentLine = ""
                    End If
                End If
                fulltext = fulltext.Remove(0, 1)
            End If
        End While

        If currentWord <> "" Then
            If currentLine <> "" Then
                currentLine &= " "
            End If
            currentLine &= currentWord
        End If
        If currentLine <> "" Then
            output &= currentLine
        End If

        Return output
    End Function

End Module