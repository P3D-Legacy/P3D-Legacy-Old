Public Class ControllerHandler

    Shared OldState As GamePadState
    Shared NewState As GamePadState

    Public Shared Property GamePadState As GamePadState
        Get
            Return NewState
        End Get
        Set(value As GamePadState)
            NewState = value
        End Set
    End Property

    Public Shared Sub Update()
        OldState = NewState
        NewState = GamePad.GetState(PlayerIndex.One)
    End Sub

    Public Shared Function ButtonPressed(ByVal Button As Microsoft.Xna.Framework.Input.Buttons) As Boolean
        Return ButtonPressed(Button, Core.GameOptions.GamePadEnabled)
    End Function

    Public Shared Function ButtonPressed(ByVal Button As Microsoft.Xna.Framework.Input.Buttons, ByVal GamePadEnabled As Boolean) As Boolean
        If GamePadEnabled = True Then
            If OldState.IsButtonDown(Button) = False And NewState.IsButtonDown(Button) = True Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Shared Function ButtonDown(ByVal Button As Microsoft.Xna.Framework.Input.Buttons) As Boolean
        Return ButtonDown(Button, Core.GameOptions.GamePadEnabled)
    End Function

    Public Shared Function ButtonDown(ByVal Button As Microsoft.Xna.Framework.Input.Buttons, ByVal GamePadEnabled As Boolean) As Boolean
        If GamePadEnabled = True Then
            If NewState.IsButtonDown(Button) = True Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Shared Function IsConnected(Optional ByVal index As Integer = 0) As Boolean
        Return (GamePad.GetState(CType(index, PlayerIndex)).IsConnected = True And Core.GameOptions.GamePadEnabled = True)
    End Function

    Public Shared Function HasControlerInput(Optional ByVal index As Integer = 0) As Boolean
        If IsConnected() = False Then
            Return False
        End If

        Dim gPadState As GamePadState = GamePad.GetState(CType(index, PlayerIndex))

        Dim bArr() As Buttons = {Buttons.A, Buttons.B, Buttons.Back, Buttons.BigButton, Buttons.DPadDown, Buttons.DPadLeft, Buttons.DPadRight, Buttons.DPadUp, Buttons.LeftShoulder, Buttons.LeftStick, Buttons.LeftThumbstickDown, Buttons.LeftThumbstickLeft, Buttons.LeftThumbstickRight, Buttons.LeftThumbstickUp, Buttons.LeftTrigger, Buttons.RightShoulder, Buttons.RightStick, Buttons.RightThumbstickDown, Buttons.RightThumbstickLeft, Buttons.RightThumbstickRight, Buttons.RightThumbstickUp, Buttons.RightTrigger, Buttons.Start, Buttons.X, Buttons.Y}

        For Each b As Buttons In bArr
            If gPadState.IsButtonDown(b) = True Then
                Return True
            End If
        Next

        Return False
    End Function

    Public Shared ReadOnly Property GetPressedButtons() As Buttons()
        Get
            Dim pressedButtons As New List(Of Buttons)
            Dim state = NewState.Buttons

            If state.A = ButtonState.Pressed Then
                pressedButtons.Add(Buttons.A)
            End If
            If state.B = ButtonState.Pressed Then
                pressedButtons.Add(Buttons.B)
            End If
            If state.X = ButtonState.Pressed Then
                pressedButtons.Add(Buttons.X)
            End If
            If state.Y = ButtonState.Pressed Then
                pressedButtons.Add(Buttons.Y)
            End If
            If state.Back = ButtonState.Pressed Then
                pressedButtons.Add(Buttons.Back)
            End If
            If state.BigButton = ButtonState.Pressed Then
                pressedButtons.Add(Buttons.BigButton)
            End If
            If state.LeftShoulder = ButtonState.Pressed Then
                pressedButtons.Add(Buttons.LeftShoulder)
            End If
            If state.LeftStick = ButtonState.Pressed Then
                pressedButtons.Add(Buttons.LeftStick)
            End If
            If state.RightShoulder = ButtonState.Pressed Then
                pressedButtons.Add(Buttons.RightShoulder)
            End If
            If state.RightStick = ButtonState.Pressed Then
                pressedButtons.Add(Buttons.RightStick)
            End If
            If state.Start = ButtonState.Pressed Then
                pressedButtons.Add(Buttons.Start)
            End If

            If NewState.DPad.Down = ButtonState.Pressed Then
                pressedButtons.Add(Buttons.DPadDown)
            End If
            If NewState.DPad.Up = ButtonState.Pressed Then
                pressedButtons.Add(Buttons.DPadUp)
            End If
            If NewState.DPad.Left = ButtonState.Pressed Then
                pressedButtons.Add(Buttons.DPadLeft)
            End If
            If NewState.DPad.Right = ButtonState.Pressed Then
                pressedButtons.Add(Buttons.DPadRight)
            End If

            If NewState.ThumbSticks.Left.X <> 0F Or NewState.ThumbSticks.Left.Y <> 0F Then
                If NewState.ThumbSticks.Left.X > 0F Then
                    pressedButtons.Add(Buttons.LeftThumbstickRight)
                ElseIf NewState.ThumbSticks.Left.X < 0F Then
                    pressedButtons.Add(Buttons.LeftThumbstickLeft)
                End If
                If NewState.ThumbSticks.Left.Y > 0F Then
                    pressedButtons.Add(Buttons.LeftThumbstickDown)
                ElseIf NewState.ThumbSticks.Left.Y < 0F Then
                    pressedButtons.Add(Buttons.LeftThumbstickUp)
                End If
            End If
            If NewState.ThumbSticks.Right.X <> 0F Or NewState.ThumbSticks.Right.Y <> 0F Then
                If NewState.ThumbSticks.Right.X > 0F Then
                    pressedButtons.Add(Buttons.RightThumbstickRight)
                ElseIf NewState.ThumbSticks.Right.X < 0F Then
                    pressedButtons.Add(Buttons.RightThumbstickLeft)
                End If
                If NewState.ThumbSticks.Right.Y > 0F Then
                    pressedButtons.Add(Buttons.RightThumbstickDown)
                ElseIf NewState.ThumbSticks.Right.Y < 0F Then
                    pressedButtons.Add(Buttons.RightThumbstickUp)
                End If
            End If

            Return pressedButtons.ToArray()
        End Get
    End Property

End Class