using UnityEngine;

/// <summary>
/// Minimax algorithm
/// </summary>
public class Minimax
{
    /// <summary>
    /// Minimax algorithm with unlimited depth
    /// </summary>
    public static int NoDepthLimit(Piece[,] testBoard, int depth, int alpha, int beta, bool isMaximizing)
    {
        int row = testBoard.GetLength(0);
        int col = testBoard.GetLength(1);
        Piece piece = Piece.Blank;
        Position currentPosition = new Position(0, 0);
        int score = EvaluateTestBoard(testBoard);

        if (score != 1) { return score; } // game ended

        if (isMaximizing)
        {
            int maxScore = -100;
            piece = GameManager.MaximizingPiece; // AI maximize

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (testBoard[i, j] == Piece.Blank)
                    {
                        currentPosition.SetPosition(i, j);
                        testBoard = GameBoard.PlaceMove(testBoard, piece, currentPosition);
                        score = NoDepthLimit(testBoard, depth + 1, alpha, beta, false);
                        testBoard = GameBoard.ResetMove(testBoard, currentPosition);

                        maxScore = Mathf.Max(maxScore, score);
                        alpha = Mathf.Max(alpha, score);
                        if (beta <= alpha) break;
                    }
                }
            }

            return maxScore;
        }
        else
        {
            int minScore = 100;
            piece = GameManager.MinimizingPiece; // player minimize

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (testBoard[i, j] == Piece.Blank)
                    {
                        currentPosition.SetPosition(i, j);
                        testBoard = GameBoard.PlaceMove(testBoard, piece, currentPosition);
                        score = NoDepthLimit(testBoard, depth + 1, alpha, beta, true);
                        testBoard = GameBoard.ResetMove(testBoard, currentPosition);

                        minScore = Mathf.Min(minScore, score);
                        beta = Mathf.Min(beta, score);
                        if (beta <= alpha) break;
                    }
                }
            }

            return minScore;
        }
    }
    /// <summary>
    /// Minimax algorithm with unlimited depth - refactored
    /// </summary>
    public static int NoDepthLimitRefactored(Piece[,] testBoard, int depth, int alpha, int beta, bool isMaximizing)
    {
        // local variables
        int row = testBoard.GetLength(0);
        int col = testBoard.GetLength(1);
        Position currentPosition = new Position(0, 0);
        int score = EvaluateTestBoard(testBoard);

        // if is maximizing, defaults to lowest score otherwise, defaults to higest score
        int bestScore = isMaximizing ? -100 : 100;
        
        // if is maximizing, get maximizing piece, otherwise get minimizing piece
        Piece piece = isMaximizing ? GameManager.MaximizingPiece : GameManager.MinimizingPiece;

        if (score != 1) { return score; } // game ended
        
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (testBoard[i, j] == Piece.Blank)
                {
                    currentPosition.SetPosition(i, j);
                    testBoard = GameBoard.PlaceMove(testBoard, piece, currentPosition);
                    score = NoDepthLimitRefactored(testBoard, depth + 1, alpha, beta, !isMaximizing);
                    testBoard = GameBoard.ResetMove(testBoard, currentPosition);

                    bestScore = isMaximizing ? Mathf.Max(bestScore, score) : Mathf.Min(bestScore, score);
                    alpha = isMaximizing ? Mathf.Max(alpha, score) : Mathf.Max(alpha, score);
                    if (beta <= alpha) break;
                }
            }
        }

        return bestScore;
    }
    /// <summary>
    /// Minimax algorithm with unlimited depth and no alpha beta pruning - refactored
    /// </summary>
    public static int NoDepthLimitRefactored(Piece[,] testBoard, int depth, bool isMaximizing)
    {
        // local variables
        int row = testBoard.GetLength(0);
        int col = testBoard.GetLength(1);
        Position currentPosition = new Position(0, 0);
        int score = EvaluateTestBoard(testBoard);

        // if is maximizing, defaults to lowest score otherwise, defaults to higest score
        int bestScore = isMaximizing ? -100 : 100;

        // if is maximizing, get maximizing piece, otherwise get minimizing piece
        Piece piece = isMaximizing ? GameManager.MaximizingPiece : GameManager.MinimizingPiece;

        if (score != 1) { return score; } // game ended

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (testBoard[i, j] == Piece.Blank)
                {
                    currentPosition.SetPosition(i, j);
                    testBoard = GameBoard.PlaceMove(testBoard, piece, currentPosition);
                    score = NoDepthLimitRefactored(testBoard, depth + 1, !isMaximizing);
                    testBoard = GameBoard.ResetMove(testBoard, currentPosition);
                    bestScore = isMaximizing ? Mathf.Max(bestScore, score) : Mathf.Min(bestScore, score);
                }
            }
        }

        return bestScore;
    }
    /// <summary>
    /// Minimax algorithm with user set depth limit
    /// </summary>
    public static int WithDepthLimit(Piece[,] testBoard, int depth, int alpha, int beta, bool isMaximizing)
    {
        int row = testBoard.GetLength(0);
        int col = testBoard.GetLength(1);
        Piece piece = Piece.Blank;
        Position currentPosition = new Position(0, 0);
        int score = EvaluateTestBoard(testBoard);
        
        if (depth == 0 || score != 1) { return score; } // no depth or game ended

        if (isMaximizing)
        {
            int maxScore = -100;
            piece = GameManager.MaximizingPiece; // AI maximize

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (testBoard[i, j] == Piece.Blank)
                    {
                        currentPosition.SetPosition(i, j);
                        testBoard = GameBoard.PlaceMove(testBoard, piece, currentPosition);
                        score = NoDepthLimit(testBoard, depth - 1, alpha, beta, false);
                        testBoard = GameBoard.ResetMove(testBoard, currentPosition);

                        maxScore = Mathf.Max(maxScore, score);
                        alpha = Mathf.Max(alpha, score);
                        if (beta <= alpha) break;
                    }
                }
            }

            return maxScore;
        }
        else
        {
            int minScore = 100;
            piece = GameManager.MinimizingPiece; // player minimize

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (testBoard[i, j] == Piece.Blank)
                    {
                        currentPosition.SetPosition(i, j);
                        testBoard = GameBoard.PlaceMove(testBoard, piece, currentPosition);
                        score = NoDepthLimit(testBoard, depth - 1, alpha, beta, true);
                        testBoard = GameBoard.ResetMove(testBoard, currentPosition);

                        minScore = Mathf.Min(minScore, score);
                        beta = Mathf.Min(beta, score);
                        if (beta <= alpha) break;
                    }
                }
            }

            return minScore;
        }
    }
    /// <summary>
    /// Minimax algorithm with user set depth limit - refactored
    /// </summary>
    public static int WithDepthLimitRefactored(Piece[,] testBoard, int depth, int alpha, int beta, bool isMaximizing)
    {
        // local variables
        int row = testBoard.GetLength(0);
        int col = testBoard.GetLength(1);
        Position currentPosition = new Position(0, 0);
        int score = EvaluateTestBoard(testBoard);

        // if is maximizing, defaults to lowest score otherwise, defaults to higest score
        int bestScore = isMaximizing ? -100 : 100;

        // if is maximizing, get maximizing piece, otherwise get minimizing piece
        Piece piece = isMaximizing ? GameManager.MaximizingPiece : GameManager.MinimizingPiece;

        if (depth == 0 ||score != 1) { return score; } // game ended

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (testBoard[i, j] == Piece.Blank)
                {
                    currentPosition.SetPosition(i, j);
                    testBoard = GameBoard.PlaceMove(testBoard, piece, currentPosition);
                    score = WithDepthLimitRefactored(testBoard, depth - 1, alpha, beta, !isMaximizing);
                    testBoard = GameBoard.ResetMove(testBoard, currentPosition);

                    bestScore = isMaximizing ? Mathf.Max(bestScore, score) : Mathf.Min(bestScore, score);
                    alpha = isMaximizing ? Mathf.Max(alpha, score) : Mathf.Max(alpha, score);
                    if (beta <= alpha) break; // alpha beta pruning
                }
            }
        }

        return bestScore;
    }
    /// <summary>
    /// Minimax algorithm with user set depth limit and no alpha beta pruning - refactored
    /// </summary>
    public static int WithDepthLimitRefactored(Piece[,] testBoard, int depth, bool isMaximizing)
    {
        // local variables
        int row = testBoard.GetLength(0);
        int col = testBoard.GetLength(1);
        Position currentPosition = new Position(0, 0);
        int score = EvaluateTestBoard(testBoard);

        // if is maximizing, defaults to lowest score otherwise, defaults to higest score
        int bestScore = isMaximizing ? -100 : 100;

        // if is maximizing, get maximizing piece, otherwise get minimizing piece
        Piece piece = isMaximizing ? GameManager.MaximizingPiece : GameManager.MinimizingPiece;

        if (depth == 0 || score != 1) { return score; } // game ended

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (testBoard[i, j] == Piece.Blank)
                {
                    currentPosition.SetPosition(i, j);
                    testBoard = GameBoard.PlaceMove(testBoard, piece, currentPosition);
                    score = WithDepthLimitRefactored(testBoard, depth - 1, !isMaximizing);
                    testBoard = GameBoard.ResetMove(testBoard, currentPosition);
                    bestScore = isMaximizing ? Mathf.Max(bestScore, score) : Mathf.Min(bestScore, score);
                }
            }
        }

        return bestScore;
    }
    /// <summary>
    /// Return score for current test board
    /// </summary>
    private static int EvaluateTestBoard(Piece[,] testBoard)
    {
        int movesToWin = GameManager.Instance.MovesToWin;
        GameState state = Rules.CheckGameState(movesToWin, testBoard);
        int baseWinningScore = 50;

        switch (state)
        {
            case GameState.Playing:
                return 1;
            case GameState.X_Win:
                return baseWinningScore;
            case GameState.O_Win:
                return -baseWinningScore;
            case GameState.Draw:
                return 0;
        }

        return 1; // playing
    }
}