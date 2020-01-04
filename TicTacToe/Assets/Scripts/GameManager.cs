using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum for 3 game modes
/// </summary>
public enum GameMode
{
    Two_Player,
    Single_Player,
    Observer
}

/// <summary>
/// Enum for 4 game states
/// </summary>
public enum GameState
{
    Playing,
    X_Win,
    O_Win,
    Draw
}

/// <summary>
/// Enum for 5 game turns
/// </summary>
public enum GameTurn
{
    Player,
    Player2,
    AI,
    AI2,
    Invalid
}

/// <summary>
/// Class managing game info, settings, and call AI actions
/// </summary>
public class GameManager : MonoBehaviour
{
    // event delegate
    [System.Serializable]
    public delegate void OnAITurnEvent(object sender, AITurnEventArgs eventArgs);
    public event OnAITurnEvent OnAITurn;

    // static instance
    private static GameManager instance = null;
    public static GameManager Instance { get { return instance; } }

    // static variables
    public static GameState State = new GameState();
    public static Piece MinimizingPiece { get { return Piece.O; } }
    public static Piece MaximizingPiece { get { return Piece.X; } }
    private static GameTurn currentTurn = new GameTurn();
    public static GameTurn CurrentTurn { get { return currentTurn; } }
    private static Piece[,] testBoard;
    public static Piece[,] TestBoard { get { return testBoard; } }
    private static List<BoardTile> tiles = new List<BoardTile>();
    public static List<BoardTile> Tiles { get { return tiles; } }
    private static int row = 0;
    public static int Row { get { return row; } }
    private static int col = 0;
    public static int Column { get { return col; } }

    // public variables
    public int MovesToWin { get { return movesToWin; } } // readonly

    // private variables
    [SerializeField] private GameMode gameMode = new GameMode();
    [SerializeField] private GameTurn firstToPlay = new GameTurn();
    [Range(3, 5)]
    [SerializeField] private int movesToWin = 3;
    private static List<BoardTile> moves = new List<BoardTile>();
    private static List<GameTurn> turns = new List<GameTurn>();
    private static int turn = 1;
    public static int Turn { get { return turn; } }

    public GameTurn curTurn = currentTurn;

    private void Awake()
    {
        SingularityInstantiate();
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        curTurn = currentTurn;
    }

    /// <summary>
    /// Unregistered all event listeners
    /// </summary>
    private void OnDisable()
    {
        BoardSpawner.Instance.OnFinishedSpawning -= SetupTestBoard;
        BoardSpawner.Instance.OnFinishedSpawning -= AdjustWinningPiece;
        BoardSpawner.Instance.OnFinishedSpawning -= CallAIMove;
        MouseClickHandler.Instance.OnHitGameBoard -= SpawnTicTacToeOnBoard;
    }

    /// <summary>
    /// Setup GameManager as singleton
    /// </summary>
    private void SingularityInstantiate()
    {
        if (instance && instance != this) Destroy(gameObject);
        else instance = this;

        DontDestroyOnLoad(this);
    }
    /// <summary>
    /// Setup essential game variables and register actions to events
    /// </summary>
    private void Initialize()
    {
        State = GameState.Playing;
        firstToPlay = GetFirstToPlay(firstToPlay, gameMode); // in case did not set in imspector properly
        currentTurn = firstToPlay;
        BoardSpawner.Instance.OnFinishedSpawning += SetupTestBoard;
        BoardSpawner.Instance.OnFinishedSpawning += AdjustWinningPiece; // register for on board finished spawning event
        BoardSpawner.Instance.OnFinishedSpawning += CallAIMove;
        MouseClickHandler.Instance.OnHitGameBoard += SpawnTicTacToeOnBoard;
    }
    /// <summary>
    /// Event: OnFinishedSpawningEvent
    /// Action: Setup test board reference
    /// </summary>
    private void SetupTestBoard(object sender, FinishedSpawningEventArgs eventArgs)
    {
        tiles = eventArgs.BoardTiles;
        row = eventArgs.Row;
        col = eventArgs.Column;
        testBoard = GameBoard.Generate(tiles, row, col);
    }
    /// <summary>
    /// Event: OnFinishedSpawningEvent
    /// Action: Call AI actions with conditions
    /// </summary>
    private void CallAIMove(object sender, FinishedSpawningEventArgs eventArgs)
    {
        ExecuteAIMoveConditionally();
    }
    /// <summary>
    /// Execute AI moves with conditions
    /// </summary>
    private void ExecuteAIMoveConditionally()
    {
        if (State == GameState.Playing &&
            (gameMode == GameMode.Single_Player || gameMode == GameMode.Observer) &&
            (currentTurn == GameTurn.AI || currentTurn == GameTurn.AI2))
        {
            CheckState();
            if (State != GameState.Playing) return;
            OnAITurn.Invoke(this, new AITurnEventArgs(testBoard, currentTurn));
        }
    }
    /// <summary>
    /// Event: OnFinishedSpawningEvent
    /// Action: Adjusting changable values in inspector 
    /// in favor of game rules in case some configurations 
    /// cause conflict in game logic
    /// </summary>
    private void AdjustWinningPiece(object sender, FinishedSpawningEventArgs eventArgs)
    {
        int row = eventArgs.Row;
        int col = eventArgs.Column;

        if (movesToWin > row || movesToWin > col)
        {
            Debug.Log("Current winning pieces [" + movesToWin + "]" +
                " is larger than board size [" + row + "x" + col + "]" +
                " , winning pieces will be adjusted to " +
                "[" + Mathf.Min(Mathf.Min(row, col)) + "]");

            movesToWin = Mathf.Min(Mathf.Min(row, col));
        }
    }
    /// <summary>
    /// Event: OnMouseClickEvent
    /// Action: Spawn playing piece on to game board
    /// </summary>
    private void SpawnTicTacToeOnBoard(object sender, OnHitGameBoardEventArgs eventArgs)
    {
        if (currentTurn == GameTurn.AI) return;

        TicTacToeSpawner.Instance.SpawnPieceOnBoard(eventArgs.boardTile);
    }
    /// <summary>
    /// Add move to a moves list and test board
    /// </summary>
    public static void RegisterMove(BoardTile boardTile)
    {
        moves.Add(boardTile);
        testBoard = GameBoard.PlaceMove(testBoard, boardTile.Info.OccupiedPiece, boardTile.Info.Position);
    }
    /// <summary>
    /// Check board's current state
    /// </summary>
    private void CheckState()
    {
        State = Rules.CheckGameState(movesToWin, testBoard);
    }
    /// <summary>
    /// Check state, set current turn, and execute AI move if met conditions
    /// </summary>
    public void FinishTurn()
    {
        turn++;
        turns.Add(currentTurn);
        CheckState();
        if (State != GameState.Playing) return;
        currentTurn = GetNextTurn(currentTurn, gameMode);
        ExecuteAIMoveConditionally();
    }
    /// <summary>
    /// Undo past moves
    /// </summary>
    public void UndoLastMove()
    {
        if (moves.Count == 0 || State != GameState.Playing) return;

        int lastMoveIndex = moves.Count - 1;
        BoardTile move = moves[lastMoveIndex];
        int row = moves[lastMoveIndex].Info.Position.RowIndex;
        int col = moves[lastMoveIndex].Info.Position.ColumnIndex;
        
        moves[lastMoveIndex].Reset();
        moves.RemoveAt(lastMoveIndex);
        currentTurn = turns[lastMoveIndex];
        turns.RemoveAt(lastMoveIndex);

        testBoard = GameBoard.ResetMove(testBoard, new Position(row, col));
    }
    /// <summary>
    /// Reset to defaults
    /// </summary>
    private void Reset()
    {
        turn = 1;
        moves.Clear();
        turns.Clear();
        testBoard = GameBoard.Reset(testBoard);
        currentTurn = GetFirstToPlay(firstToPlay, gameMode);
        State = GameState.Playing;
        ExecuteAIMoveConditionally();
    }
    /// <summary>
    /// Wrapper for (class)BoardTile.Reset and Reset
    /// </summary>
    public void ResetBoard()
    {
        foreach (BoardTile move in moves)
        {
            move.Reset();
        }

        Reset();
    }
    /// <summary>
    /// Return a piece for AI based on current game mode and turn 
    /// </summary>
    public Piece GetAIPiece()
    {
        if (gameMode == GameMode.Observer)
        {
            switch (currentTurn)
            {
                case GameTurn.AI:
                    return MaximizingPiece;
                case GameTurn.AI2:
                    return MinimizingPiece;
            }
        }

        return MaximizingPiece;
    }
    /// <summary>
    /// Adjusting default first to play turn based on 
    /// current game mode in case forgot to set in inspector
    /// </summary>
    private GameTurn GetFirstToPlay(GameTurn firstToPlay, GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.Two_Player:
                if (firstToPlay != GameTurn.Player && firstToPlay != GameTurn.Player2)
                    return GameTurn.Player;
                else break;
            case GameMode.Single_Player:
                if (firstToPlay != GameTurn.Player && firstToPlay != GameTurn.AI)
                    return GameTurn.Player;
                else break;
            case GameMode.Observer:
                if (firstToPlay != GameTurn.AI && firstToPlay != GameTurn.AI2)
                    return GameTurn.AI;
                else break;
        }

        return firstToPlay;
    }
    /// <summary>
    /// Get next turn based on current turn and current mode
    /// </summary>
    private GameTurn GetNextTurn(GameTurn currentTurn, GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.Two_Player:
                return (currentTurn == GameTurn.Player) ? GameTurn.Player2 : GameTurn.Player;
            case GameMode.Single_Player:
                return (currentTurn == GameTurn.Player) ? GameTurn.AI : GameTurn.Player;
            case GameMode.Observer:
                return (currentTurn == GameTurn.AI) ? GameTurn.AI2 : GameTurn.AI;
        }

        return GameTurn.Invalid;
    }
}

/// <summary>
/// Class that stores data to sent to all subscribers of OnAITurnEvent
/// </summary>
public class AITurnEventArgs
{
    /// <summary>
    /// OnAITurn event argument constructor
    /// </summary>
    public AITurnEventArgs(Piece[,] testBoard, GameTurn currentTurn)
    {
        TestBoard = testBoard;
        CurrentTurn = currentTurn;
    }

    public Piece[,] TestBoard { get; }
    public GameTurn CurrentTurn { get; }
}