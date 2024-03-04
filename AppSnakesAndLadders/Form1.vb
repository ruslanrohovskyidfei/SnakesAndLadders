Public Class Form1
    Dim intPlayer1Score As Integer
    Dim intPlayer2Score As Integer
    Dim gameFinished As Boolean
    Dim prevScore As Integer
    Dim fieldLength As Integer = 100
    Dim boardMoves As String() = {
                                   "1-38", "4-14", "9-31", "21-42", "28-84", "51-67", "72-91", "80-99", '  Ladder Moves
                                   "17-7", "54-34", "62-19", "64-60", "87-36", "93-73", "95-75", "98-79" ' Snake Moves
                                 }
    Dim firstAttempt As Boolean = True
    Public Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
        Dim Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function
    Private Sub Game(player As Integer)
        Dim intScore As Integer

        ' Clean message label
        lblDice.Text = ""

        ' Get random number of Dice and show picture of dice with current number
        Dim intValue As Integer = GetRandom(1, 7)
        picDice.Visible = True
        picDice.Image = dices.Images.Item(intValue - 1)

        ' Saving current player score for local iterator
        If player = 1 Then
            intScore = intPlayer1Score
        Else
            intScore = intPlayer2Score
        End If

        ' Condition for checking first player who throw 6 to start game
        If intValue = 6 And firstAttempt Then
            firstAttempt = False
            lblDice.Text = "Player " & player & " starts with " & intValue
        ElseIf firstAttempt And intValue < 6 Then
            lblDice.Text = "Try again"
        End If
        ' Condition for default game
        If intValue <= 6 And firstAttempt = False Then
            firstAttempt = False
            Dim strCounterName As String
            If intScore > 0 Then
                strCounterName = "lblPointer" + intScore.ToString
                Me.Controls(strCounterName).Visible = False
            End If
            ' If was spared point with both players, left behind player which moving is next
            If intPlayer1Score > 0 And intPlayer2Score > 0 And intPlayer1Score = intPlayer2Score Then
                strCounterName = "lblPointer" + intPlayer1Score.ToString
                If (player = 1) Then
                    Me.Controls(strCounterName).BackgroundImage = figures.Images.Item(1)
                Else
                    Me.Controls(strCounterName).BackgroundImage = figures.Images.Item(0)
                End If
                Me.Controls(strCounterName).Visible = True
            End If

            ' Counting next step
            intScore = intScore + intValue

            lblOperation.Text = ""

            ' Checking all moves for possible equality with Snake or Ladder entry point
            snakeAndLadders(intScore, player)

            ' If game still on going move our figure's
            If intScore < fieldLength Then
                lblDice.Text = "Player " & player & " Move"
                strCounterName = "lblPointer" + intScore.ToString
                If player = 1 Then
                    Me.Controls(strCounterName).BackgroundImage = figures.Images.Item(0)
                    intPlayer1Score = intScore
                Else
                    Me.Controls(strCounterName).BackgroundImage = figures.Images.Item(1)
                    intPlayer2Score = intScore
                End If
                If intPlayer1Score = intPlayer2Score Then
                    Me.Controls(strCounterName).BackgroundImage = figures.Images.Item(2)
                End If
                Me.Controls(strCounterName).Visible = True
            ElseIf intScore > fieldLength Then
                If player = 1 Then
                    strCounterName = "lblPointer" + intPlayer1Score.ToString
                Else
                    strCounterName = "lblPointer" + intPlayer2Score.ToString
                End If
                lblDice.Text = "Player " & player & " Stay"
                Me.Controls(strCounterName).Visible = True
                ' If our score 100 or out of order we flash screen for Winner
            ElseIf intScore = fieldLength Then
                lblDice.Text = "Player " & player & " is Winner!"
                Me.Controls("lblPointer100").BackgroundImage = figures.Images.Item(player - 1)
                Me.Controls("lblPointer100").Visible = True
                gameFinished = True
            End If
        End If
    End Sub

    ' Update all statuses
    Private Sub gameUpdateStatus()
        btnPlayer1.Enabled = False
        btnPlayer1.Visible = False
        btnPlayer2.Enabled = False
        btnPlayer2.Visible = False
        btnRepeat.Visible = True
        btnQuit.Visible = True
    End Sub
    Private Sub snakeAndLadders(intScore As Integer, player As Integer)
        For index = 0 To boardMoves.Length - 1
            ' Navigating on array
            Dim line As String = boardMoves(index).ToString()
            ' Splitting line which we choosen by index with separator "-"
            Dim lineArray As String() = line.Split("-")
            ' Saving current position for condition below
            Dim currentItem = Integer.Parse(lineArray(0))
            ' Condition to check equality of current board place and array place
            If intScore = currentItem Then
                'Saving previous score for Moving message
                prevScore = intScore
                'Saving new score for movement
                intScore = Integer.Parse(lineArray(1))
                lblOperation.Text = "Player " & player & " Moved from " & prevScore & " to " & intScore
            End If
        Next
    End Sub
    ' Click event for Player1 button
    Private Sub btnPlayer1_Click(sender As Object, e As EventArgs) Handles btnPlayer1.Click
        btnPlayer1.Enabled = False
        btnPlayer1.Visible = False
        Game(1)
        If gameFinished Then
            gameUpdateStatus()
        Else
            btnPlayer2.Enabled = True
            btnPlayer2.Visible = True
            btnRepeat.Visible = False
            btnQuit.Visible = False
        End If
    End Sub

    ' Click event for Player2 button
    Private Sub btnPlayer2_Click(sender As Object, e As EventArgs) Handles btnPlayer2.Click
        btnPlayer2.Enabled = False
        btnPlayer2.Visible = False
        Game(2)
        If gameFinished Then
            gameUpdateStatus()
        Else
            btnPlayer1.Enabled = True
            btnPlayer1.Visible = True
            btnRepeat.Visible = False
            btnQuit.Visible = False
        End If
    End Sub


    ' Click event for Repeat game button
    Private Sub btnRepeat_Click(sender As Object, e As EventArgs) Handles btnRepeat.Click
        ' Show Player buttons again
        btnPlayer1.Enabled = True
        btnPlayer1.Visible = True
        btnPlayer2.Enabled = True
        btnPlayer2.Visible = True

        ' Clean game fields
        If intPlayer1Score <= 100 Then
            Me.Controls("lblPointer" + intPlayer1Score.ToString).Visible = False
        End If
        If intPlayer2Score <= 100 Then
            Me.Controls("lblPointer" + intPlayer2Score.ToString).Visible = False
        End If
        Me.Controls("lblPointer100").Visible = False
        gameFinished = False
        firstAttempt = True
        intPlayer1Score = 0
        intPlayer2Score = 0
        lblDice.Text = ""
        ' Hide operation buttons
        btnRepeat.Visible = False
        btnQuit.Visible = False
        picDice.Visible = False
    End Sub

    ' Click event for Quit game button
    Private Sub btnQuit_Click(sender As Object, e As EventArgs) Handles btnQuit.Click
        End
    End Sub
End Class
