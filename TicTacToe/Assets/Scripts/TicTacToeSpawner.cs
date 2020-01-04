using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum for 4 game pieces
/// </summary>
public enum Piece
{
    Blank,
    X,
    O,
    Invalid
}

/// <summary>
/// Class handling the spawning of game piece
/// </summary>
public class TicTacToeSpawner : MonoBehaviour
{
    // static variables
    private static TicTacToeSpawner instance = null;
    public static TicTacToeSpawner Instance { get { return instance; } }

    // private variables
    [SerializeField] private GameObject XPiece = null;
    [SerializeField] private GameObject OPiece = null;
    [SerializeField] private float spawnOffset = .5f;
    private Dictionary<Piece, GameObject> pieceDictionary = new Dictionary<Piece, GameObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitiatePieceDictionary();
    }

    /// <summary>
    /// Create a dictionary holding enum piece as key and corresponding piece prefabs as value
    /// </summary>
    private void InitiatePieceDictionary()
    {
        pieceDictionary.Add(Piece.X, XPiece);
        pieceDictionary.Add(Piece.O, OPiece);
    }
    /// <summary>
    /// Wrapper for mouse click piece spawning
    /// </summary>
    public void SpawnPieceOnBoard(BoardTile Tile)
    {
        Piece pieceToSpawn = GetPieceToSpawn(GameManager.CurrentTurn);

        if (pieceToSpawn != Piece.Blank)
        {
            if (!Tile.Info.Occupied)
            {
                SpawnPiece(Tile, pieceToSpawn);
            }
        }
        else
        {
            Debug.Log("Invalid piece, please initialize first turn piece in Game Manager");
        }
    }
    /// <summary>
    /// Spawn a particular piece at a particular spot 
    /// then register move and call finish turn in GameManager
    /// </summary>
    private void SpawnPiece(BoardTile boardTile, Piece pieceToSpawn)
    {
        boardTile.Info.GamePiece = Instantiate
        (
            pieceDictionary[pieceToSpawn],
            GetSpawnPosition(boardTile.transform.position, spawnOffset),
            Quaternion.identity
        );

        boardTile.Info.GamePiece.name = pieceDictionary[pieceToSpawn].name;
        boardTile.Info.Occupied = true;
        boardTile.Info.OccupiedPiece = pieceToSpawn;

        GameManager.RegisterMove(boardTile);
        GameManager.Instance.FinishTurn();
    }
    /// <summary>
    /// Wrapper for AI piece spawning
    /// </summary>
    public void SpawnPieceOnBoardAI(BoardTile tile)
    {
        Piece pieceToSpawn = GetPieceToSpawn(GameManager.CurrentTurn);
        SpawnPiece(tile, pieceToSpawn);
    }
    /// <summary>
    /// Wrapper for AI piece spawning with delay
    /// </summary>
    public IEnumerator SpawnPieceWithDelay(BoardTile tile, float delay)
    {
        Piece pieceToSpawn = GetPieceToSpawn(GameManager.CurrentTurn);
        yield return new WaitForSeconds(delay);
        SpawnPiece(tile, pieceToSpawn);
    }
    /// <summary>
    /// Get the right piece to spawn base on current turn
    /// </summary>
    private static Piece GetPieceToSpawn(GameTurn turn)
    {
        switch (turn)
        {
            case GameTurn.Player:
            case GameTurn.AI2:
                return Piece.O;
            case GameTurn.Player2:
            case GameTurn.AI:
                return Piece.X;
        }

        return Piece.Blank;
    }
    /// <summary>
    /// Get spawn position relative to tile
    /// </summary>
    private static Vector3 GetSpawnPosition(Vector3 boardTilePosition, float spawnOffset)
    {
        Vector3 spawnPosition = boardTilePosition;

        spawnPosition.y = boardTilePosition.y + spawnOffset;

        return spawnPosition;
    }
}