Public Class Form1
    Dim intPlayer1Score As Integer
    Dim intPlayer2Score As Integer
    Dim gameFinished As Boolean
    Dim fieldLength As Integer = 100
    Dim intValue As Integer
    Dim Generator As System.Random = New System.Random()
    Dim boardMoves As String() = {
                                   "1-38", "4-14", "9-31", "21-42", "28-84", "51-67", "72-91", "80-99", '  Ladder Moves
                                   "17-7", "54-34", "62-19", "64-60", "87-36", "93-73", "95-75", "98-79" ' Snake Moves
                                 }
    Dim firstAttempt As Boolean = True
    Private Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
        Return Generator.Next(Min, Max)
    End Function
    Private Sub wait(ByVal seconds As Integer)
        For i As Integer = 0 To seconds * 100
            System.Threading.Thread.Sleep(10)
            Application.DoEvents()
        Next
    End Sub
    Private Sub Game(player As Integer)
        Dim intScore As Integer
        Dim prevScore As Integer

        ' Clean message label
        lblDice.Text = ""

        ' Get random number of Dice and show picture of dice with current number
        intValue = GetRandom(1, 7)

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
            lblDice.Text = "Player " & player & " got " & intValue
            firstAttempt = False
            wait(0.8)
            'Repeat dice for starting player
            intValue = GetRandom(1, 7)
            picDice.Image = dices.Images.Item(intValue - 1)
            lblDice.Text = "Player " & player & " starts with " & intValue
        ElseIf firstAttempt And intValue < 6 Then
            lblDice.Text = "Try again"
        End If
        ' Condition for default game
        If firstAttempt = False Then
            Dim strCounterName As String
            If intScore > 0 Then
                strCounterName = "lblPointer" + intScore.ToString
                Me.Controls(strCounterName).Visible = False
            End If
            ' Clean message label
            lblOperation.Text = ""

            ' Counting next step
            intScore = intScore + intValue
            ' Checking all moves for possible equality with Snake or Ladder entry point
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
            ' If game still on going move our figure's
            If intScore <= fieldLength Then
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
                'If someone come's to end point
                If intScore = fieldLength Then
                    lblDice.Text = "Player " & player & " is Winner!"
                    strCounterName = "lblPointer100"
                    Me.Controls(strCounterName).Visible = True
                    If player = 1 Then
                        strCounterName = "lblPointer" + intPlayer2Score.ToString
                        Me.Controls(strCounterName).Visible = True
                    Else
                        strCounterName = "lblPointer" + intPlayer1Score.ToString
                        Me.Controls(strCounterName).Visible = True
                    End If
                    gameFinished = True
                End If
            ElseIf intScore > fieldLength Then
                If player = 1 Then
                    strCounterName = "lblPointer" + intPlayer1Score.ToString
                    lblOperation.Text = "You need " & fieldLength - intPlayer1Score & " on dice"
                Else
                    strCounterName = "lblPointer" + intPlayer2Score.ToString
                    lblOperation.Text = "You need " & fieldLength - intPlayer2Score & " on dice"
                End If
                lblDice.Text = "Player " & player & " Stay"
                Me.Controls(strCounterName).Visible = True
                ' If our score 100 or out of order we flash screen for Winner
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
    ' Click event for Player1 button
    Private Sub btnPlayer1_Click(sender As Object, e As EventArgs) Handles btnPlayer1.Click
        btnPlayer1.Enabled = False
        btnPlayer1.Visible = False
        buttonFigure1.Visible = False
        Game(1)
        If gameFinished Then
            gameUpdateStatus()
        Else
            btnPlayer2.Enabled = True
            btnPlayer2.Visible = True
            buttonFigure2.Visible = True
            btnRepeat.Visible = False
            btnQuit.Visible = False
        End If
    End Sub

    ' Click event for Player2 button
    Private Sub btnPlayer2_Click(sender As Object, e As EventArgs) Handles btnPlayer2.Click
        btnPlayer2.Enabled = False
        btnPlayer2.Visible = False
        buttonFigure2.Visible = False
        Game(2)
        If gameFinished Then
            gameUpdateStatus()
        Else
            btnPlayer1.Enabled = True
            btnPlayer1.Visible = True
            buttonFigure1.Visible = True
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
        buttonFigure1.Visible = True
        buttonFigure2.Visible = True

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
