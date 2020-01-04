using System.Collections.Generic;

/// <summary>
/// Struct holding position data
/// </summary>
public struct Position
{
    public Position(int rowIndex, int colIndex) { RowIndex = rowIndex; ColumnIndex = colIndex; }
    public void SetPosition(int rowIndex, int colIndex) { RowIndex = rowIndex; ColumnIndex = colIndex; }
    public void SetPosition(Position position) { RowIndex = position.RowIndex; ColumnIndex = position.ColumnIndex; }
    public int RowIndex;
    public int ColumnIndex;
}

/// <summary>
/// Class that manages game board functionalities
/// </summary>
public static class GameBoard
{
    /// <summary>
    /// Generate game board
    /// </summary>
    public static Piece[,] Generate(List<BoardTile> boardTiles, int row, int col)
    {
        Piece[,] gameBoard = new Piece[row, col];

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (boardTiles[i * col + j].Info.Occupied)
                {
                    gameBoard[i, j] = boardTiles[i * col + j].Info.OccupiedPiece;
                }
            }
        }

        return gameBoard;
    }
    /// <summary>
    /// Place a test move on board
    /// </summary>
    public static Piece[,] PlaceMove(Piece[,] gameBoard, Piece piece, Position position)
    {
        gameBoard[position.RowIndex, position.ColumnIndex] = piece;
        return gameBoard;
    }
    /// <summary>
    /// Reset a test move to default - blank
    /// </summary>
    public static Piece[,] ResetMove(Piece[,] gameBoard, Position position)
    {
        gameBoard[position.RowIndex, position.ColumnIndex] = Piece.Blank;
        return gameBoard;
    }
    /// <summary>
    /// Reset test board to default - blank
    /// </summary>
    public static Piece[,] Reset(Piece[,] gameBoard)
    {
        int row = gameBoard.GetLength(0);
        int col = gameBoard.GetLength(1);

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                gameBoard[i, j] = Piece.Blank;
            }
        }

        return gameBoard;
    }
    /// <summary>
    /// Check if board has blank spot
    /// </summary>
    public static bool HasBlankSpot(Piece[,] gameBoard)
    {
        foreach (Piece piece in gameBoard)
        {
            if (piece == Piece.Blank) return true;
        }

        return false;
    }
    /// <summary>
    /// Return available spots on game board
    /// </summary>
    public static List<BoardTile> GetAvailableSpots(List<BoardTile> gameBoard)
    {
        List<BoardTile> availableSpots = new List<BoardTile>();

        foreach (BoardTile spot in gameBoard)
        {
            if (spot.Info.OccupiedPiece == Piece.Blank) availableSpots.Add(spot);
        }

        return availableSpots;
    }
}
