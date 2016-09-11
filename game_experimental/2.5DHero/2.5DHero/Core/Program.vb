Namespace GameCore

#If WINDOWS Or XBOX Then

    Module Program

        Private _gameCrashed As Boolean = False

        ''' <summary>
        ''' The main entry point for the application.
        ''' </summary>
        Sub Main(ByVal args As String())
            Debug.Print(" ")
            Debug.Print("PROGRAM EXECUTION STARTED")
            Debug.Print("STACK TRACE ENTRY                   | MESSAGE")
            Debug.Print("------------------------------------|------------------------------------")

            GameCore.CommandLineArgHandler.Initialize(args)


            Logger.Debug("000", "---Start game---")

            CheckFutureVersion()

            Using Game As New GameController()
                If GameController.IS_DEBUG_ACTIVE = True And Debugger.IsAttached = True Then
                    Game.Run()
                Else
                    Try
                        Game.Run()
                    Catch ex As Exception
                        _gameCrashed = True
                        Dim informationItem As New Logger.ErrorInformation(ex)

                        Logger.Log("258", Logger.LogTypes.ErrorMessage, "The game crashed with error ID:  " & informationItem.ErrorIDString & " (" & ex.Message & ")")

                        Logger.LogCrash(ex)
                    End Try
                End If
            End Using
        End Sub

        Public ReadOnly Property GameCrashed() As Boolean
            Get
                Return _gameCrashed
            End Get
        End Property

        Private Sub CheckFutureVersion()
            If GameController.IS_FUTURE_VERSION = True And Debugger.IsAttached = False Then
                MsgBox("You are running a future version of Pokémon3D!" & vbNewLine & vbNewLine & "Please handle with extreme caution and do not use for productive actions.", MsgBoxStyle.Exclamation, "WARNING - FUTURE VERSION")
            End If
        End Sub

    End Module

#End If

End Namespace