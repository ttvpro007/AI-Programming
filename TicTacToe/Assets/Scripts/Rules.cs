using System.Collections.Generic;

/// <summary>
/// Class handling game state checks
/// </summary>
public static class Rules
{
    /// <summary>
    /// Checks all row for winning condition
    /// </summary>
    public static bool IsHorizontalUniformed(int movesToWin)
    {
        int row = GameManager.Row;
        int col = GameManager.Column;
        Piece piece = Piece.Blank;
        List<Piece> line = new List<Piece>();

        // Horizontal - Check horizontally meaning loop through available column
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j <= col - movesToWin; j++)
            {
                line.Clear();

                for (int k = 0; k < movesToWin; k++)
                {
                    piece = GameManager.Tiles[i * col + j + k].Info.OccupiedPiece;
                    if (piece == Piece.Blank) continue;
                    line.Add(piece);
                }

                if (line.Count == movesToWin && IsLineUniformed(line))
                    return true; // RETURN
            }
        }

        return false;
    }
    /// <summary>
    /// Checks all row for winning piece
    /// </summary>
    public static Piece IsHorizontalUniformed(int movesToWin, Piece[,] testBoard)
    {
        int row = GameManager.Row;
        int col = GameManager.Column;
        Piece piece = Piece.Blank;
        List<Piece> line = new List<Piece>();

        // Horizontal - Check horizontally meaning loop through available column
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j <= col - movesToWin; j++)
            {
                line.Clear();

                for (int k = 0; k < movesToWin; k++)
                {
                    piece = testBoard[i, j + k]; // [i * col + j + k]
                    if (piece == Piece.Blank) continue;
                    line.Add(piece);
                }

                if (line.Count == movesToWin && IsLineUniformed(line))
                    return piece; // RETURN
            }
        }

        return Piece.Blank;
    }
    /// <summary>
    /// Checks all column for winning conditon
    /// </summary>
    public static bool IsVerticalUniformed(int movesToWin)
    {
        int row = GameManager.Row;
        int col = GameManager.Column;
        Piece piece = Piece.Blank;
        List<Piece> line = new List<Piece>();

        // Vertical - Check vertically meaning loop through available row
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j <= row - movesToWin; j++)
            {
                line.Clear();

                for (int k = 0; k < movesToWin; k++)
                {
                    piece = GameManager.Tiles[i + (j + k) * row].Info.OccupiedPiece;
                    if (piece == Piece.Blank) continue;
                    line.Add(piece);
                }

                if (line.Count == movesToWin && IsLineUniformed(line))
                    return true;
            }
        }

        return false;
    }
    /// <summary>
    /// Checks all column for winning piece
    /// </summary>
    public static Piece IsVerticalUniformed(int movesToWin, Piece[,] testBoard)
    {
        int row = GameManager.Row;
        int col = GameManager.Column;
        Piece piece = Piece.Blank;
        List<Piece> line = new List<Piece>();

        // Vertical - Check vertically meaning loop through available row
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j <= row - movesToWin; j++)
            {
                line.Clear();

                for (int k = 0; k < movesToWin; k++)
                {
                    piece = testBoard[j + k, i]; // [i + (j + k) * row]
                    if (piece == Piece.Blank) continue;
                    line.Add(piece);
                }

                if (line.Count == movesToWin && IsLineUniformed(line))
                    return piece;
            }
        }

        return Piece.Blank;
    }
    /// <summary>
    /// Checks all forward down diagonal lines for winning condition
    /// </summary>
    public static bool IsDiagonalForwardDownUniformed(int movesToWin)
    {
        int row = GameManager.Row;
        int col = GameManager.Column;
        Piece piece = Piece.Blank;
        List<Piece> line = new List<Piece>();

        // Diagonal forward down
        for (int i = 0; i <= row - movesToWin; i++)
        {
            for (int j = 0; j <= col - movesToWin; j++)
            {
                line.Clear();

                for (int k = 0; k < movesToWin; k++)
                {
                    piece = GameManager.Tiles[i * row + j + (col + 1) * k].Info.OccupiedPiece;
                    if (piece == Piece.Blank) continue;
                    line.Add(piece);
                }

                if (line.Count == movesToWin && IsLineUniformed(line))
                    return true;
            }
        }

        return false;
    }
    /// <summary>
    /// Checks all forward down diagonal lines for winning piece
    /// </summary>
    public static Piece IsDiagonalForwardDownUniformed(int movesToWin, Piece[,] testBoard)
    {
        int row = GameManager.Row;
        int col = GameManager.Column;
        Piece piece = Piece.Blank;
        List<Piece> line = new List<Piece>();

        // Diagonal forward down
        for (int i = 0; i <= row - movesToWin; i++)
        {
            for (int j = 0; j <= col - movesToWin; j++)
            {
                line.Clear();

                for (int k = 0; k < movesToWin; k++)
                {
                    piece = testBoard[i + k, j + k]; // [i * row + j + (col + 1) * k]
                    if (piece == Piece.Blank) continue;
                    line.Add(piece);
                }

                if (line.Count == movesToWin && IsLineUniformed(line))
                    return piece;
            }
        }

        return Piece.Blank;
    }
    /// <summary>
    /// Checks all forward up diagonal lines for winning condition
    /// </summary>
    public static bool IsDiagonalForwardUpUniformed(int movesToWin)
    {
        int row = GameManager.Row;
        int col = GameManager.Column;
        Piece piece = Piece.Blank;
        List<Piece> line = new List<Piece>();

        // Diagonal forward up
        for (int i = movesToWin - 1; i < row; i++)
        {
            for (int j = 0; j <= col - movesToWin; j++)
            {
                line.Clear();

                for (int k = 0; k < movesToWin; k++)
                {
                    piece = GameManager.Tiles[i * row + j - (col - 1) * k].Info.OccupiedPiece;
                    if (piece == Piece.Blank) continue;
                    line.Add(piece);
                }

                if (line.Count == movesToWin && IsLineUniformed(line))
                    return true;
            }
        }

        return false;
    }
    /// <summary>
    /// Checks all forward up diagonal lines for winning piece
    /// </summary>
    public static Piece IsDiagonalForwardUpUniformed(int movesToWin, Piece[,] testBoard)
    {
        int row = GameManager.Row;
        int col = GameManager.Column;
        Piece piece = Piece.Blank;
        List<Piece> line = new List<Piece>();

        // Diagonal forward up
        for (int i = movesToWin - 1; i < row; i++)
        {
            for (int j = 0; j <= col - movesToWin; j++)
            {
                line.Clear();

                for (int k = 0; k < movesToWin; k++)
                {
                    piece = testBoard[i - k, j + k]; // [i * row + j - (col - 1) * k]
                    if (piece == Piece.Blank) continue;
                    line.Add(piece);
                }

                if (line.Count == movesToWin && IsLineUniformed(line))
                    return piece;
            }
        }

        return Piece.Blank;
    }
    /// <summary>
    /// Checks if has a winner
    /// </summary>
    public static bool IsWin(int movesToWin)
    {
        return
            IsHorizontalUniformed(movesToWin) ||
            IsVerticalUniformed(movesToWin) ||
            IsDiagonalForwardDownUniformed(movesToWin) ||
            IsDiagonalForwardUpUniformed(movesToWin);
    }
    /// <summary>
    /// Checks for winning piece
    /// </summary>
    public static Piece CheckPieceWon(int movesToWin, Piece[,] testBoard)
    {
        Piece piece = Piece.Blank;

        piece = IsHorizontalUniformed(movesToWin, testBoard);
        if (piece != Piece.Blank) return piece;
        piece = IsVerticalUniformed(movesToWin, testBoard);
        if (piece != Piece.Blank) return piece;
        piece = IsDiagonalForwardDownUniformed(movesToWin, testBoard);
        if (piece != Piece.Blank) return piece;
        piece = IsDiagonalForwardUpUniformed(movesToWin, testBoard);
        if (piece != Piece.Blank) return piece;

        return piece;
    }
    /// <summary>
    /// Checks for pieces of same color in a line
    /// </summary>
    public static bool IsLineUniformed(List<Piece> line)
    {
        Piece piece = line[0];

        for (int i = 1; i < line.Count; i++)
        {
            if (piece != line[i]) return false;
        }

        return true;
    }
    /// <summary>
    /// Checks if draw
    /// </summary>
    public static bool IsDraw(Piece[,] testBoard)
    {
        int row = testBoard.GetLength(0);
        int col = testBoard.GetLength(1);

        foreach (Piece piece in testBoard)
        {
            if (piece == Piece.Blank) return false;
        }

        return true;
    }
    /// <summary>
    /// Checks game state of current board
    /// </summary>
    public static GameState CheckGameState(int movesToWin, GameTurn currentTurn, bool isMoveLeft)
    {
        GameState state = GameState.Playing; // default to playing

        if (IsWin(movesToWin))
        {
            switch (currentTurn)
            {
                case GameTurn.Player:
                    return GameState.O_Win;
                case GameTurn.AI:
                    return GameState.X_Win;
            }
        }
        else if (!isMoveLeft)
        {
            return GameState.Draw;
        }

        return state; // RETURN
    }
    /// <summary>
    /// Checks game state of current board
    /// </summary>
    public static GameState CheckGameState(int movesToWin, Piece[,] testBoard)
    {
        Piece piece = CheckPieceWon(movesToWin, testBoard);
        GameState gameState = GameState.Playing;

        switch (piece)
        {
            case Piece.Blank:
                if (!IsDraw(testBoard)) return GameState.Playing;
                else return GameState.Draw;
            case Piece.X:
                return GameState.X_Win;
            case Piece.O:
                return GameState.O_Win;
        }

        return gameState; // RETURN
    }
}