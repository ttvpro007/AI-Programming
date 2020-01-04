using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class contains AI actions
/// </summary>
public class AI : MonoBehaviour
{
    [SerializeField] private bool usingMinimaxAlgorithm = true;
    [SerializeField] private bool usingAlphaBetaPruning = true;
    [SerializeField] private bool limitedDepth = true;
    [SerializeField] private int depth = 1;
    [SerializeField] private bool spawnDelay = true;
    [SerializeField] private float delay = .5f;
    [SerializeField] private GameTurn myTurn = new GameTurn();

    private void Start()
    {
        // register action for On AI Turn event - On AI turn -> Place a piece on board
        GameManager.Instance.OnAITurn += PlacePieceOnBoard;
    }
    /// <summary>
    /// Event: OnAITurnEvent
    /// Action: Spawn a piece on board with best or random position with or without delay
    /// </summary>
    private void PlacePieceOnBoard(object sender, AITurnEventArgs eventArgs)
    {
        if (myTurn != eventArgs.CurrentTurn) return; // if is not turn, don't move

        if (usingMinimaxAlgorithm) // run minimax algorithm to find best move
        {
            if (spawnDelay) // spawn after delay
            {
                StartCoroutine(TicTacToeSpawner.Instance.SpawnPieceWithDelay(BestPosition(limitedDepth, depth), delay));
            }
            else // spawn immediately
            {
                TicTacToeSpawner.Instance.SpawnPieceOnBoardAI(BestPosition(limitedDepth, depth));
            }
        }
        else // find random position
        {
            if (spawnDelay) // spawn after delay
            {
                StartCoroutine(TicTacToeSpawner.Instance.SpawnPieceWithDelay(GetRandomUnoccupiedTile(), delay));
            }
            else // spawn immediately
            {
                TicTacToeSpawner.Instance.SpawnPieceOnBoardAI(GetRandomUnoccupiedTile());
            }
        }
    }
    /// <summary>
    /// Find a random unoccupied spot on board
    /// </summary>
    private static BoardTile GetRandomUnoccupiedTile()
    {
        List<BoardTile> availableSpots = GameBoard.GetAvailableSpots(GameManager.Tiles);
        int randomIndex = Random.Range(0, availableSpots.Count);
        if (availableSpots.Count == 0)
        {
            return null;
        }
        else
        {
            return availableSpots[randomIndex];
        }
    }
    /// <summary>
    /// Find best move using minimax algorithm
    /// </summary>
    private BoardTile BestPosition(bool isLimitedDepth, int depth)
    {
        // reference values from GameManager
        List<BoardTile> mainBoard = GameManager.Tiles;
        Piece[,] testBoard = GameManager.TestBoard;
        Piece maximizingPiece = GameManager.MaximizingPiece;
        Piece testPiece = GameManager.Instance.GetAIPiece();

        BoardTile bestMove = GetRandomUnoccupiedTile();
        Position position = new Position(0,0);

        int row = testBoard.GetLength(0);
        int col = testBoard.GetLength(1);

        bool isMaximizing = testPiece == maximizingPiece; // check if current piece is maximizing piece
        int bestScore = isMaximizing ? -100 : 100; // flipping value base on the current piece
        int score = bestScore;

        if (GameManager.Turn == 1) return bestMove; // return random move on first turn

        // loop through all positions
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (testBoard[i, j] == Piece.Blank) // if current spot is unoccupied
                {
                    position.SetPosition(i, j);
                    testBoard = GameBoard.PlaceMove(testBoard, testPiece, position); // place test move

                    if (!isLimitedDepth)
                    {
                        if (usingAlphaBetaPruning)
                            // minimizing if maximizing and vice versa using algorithm with depth priority
                            score = Minimax.NoDepthLimitRefactored(testBoard, 0, -100, 100, !isMaximizing);
                        else
                            score = Minimax.NoDepthLimitRefactored(testBoard, 0, !isMaximizing);
                    }
                    else
                    {
                        if (usingAlphaBetaPruning)
                            // minimizing if maximizing and vice versa using algorithm with depth priority
                            score = Minimax.WithDepthLimitRefactored(testBoard, depth, -100, 100, !isMaximizing);
                        else
                            score = Minimax.WithDepthLimitRefactored(testBoard, depth, !isMaximizing);
                    }

                    testBoard = GameBoard.ResetMove(testBoard, position); // reset the placed move

                    if (isMaximizing)
                    {
                        if (score > bestScore) { bestScore = score; bestMove = mainBoard[i * col + j]; }
                    }
                    else
                    {
                        if (score < bestScore) { bestScore = score; bestMove = mainBoard[i * col + j]; }
                    }
                }
            }
        }

        return bestMove;
    }
}