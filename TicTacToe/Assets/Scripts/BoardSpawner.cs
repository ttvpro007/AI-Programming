using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class spawning game board
/// </summary>
public class BoardSpawner : MonoBehaviour
{
    // event delegate
    [System.Serializable]
    public delegate void OnFinishedSpawningEvent(object sender, FinishedSpawningEventArgs eventArgs);
    public event OnFinishedSpawningEvent OnFinishedSpawning;

    // static instance
    private static BoardSpawner instance = null;
    public static BoardSpawner Instance { get { return instance; } }

    // private variables
    private int row = 3;
    private int column = 3;
    [Range(.1f, 1)]
    [SerializeField] private float delay = .5f;
    [SerializeField] private GameObject boardTile = null;
    [SerializeField] private GameObject gameBoard = null;
    [SerializeField] private LayerMask layerToSpawnOn = new LayerMask();
    [Range(.1f, .3f)]
    [SerializeField] private float distanceBetweenTiles = .1f;
    private int layerToSpawnOnIndex = 0;
    private List<BoardTile> boardTiles = new List<BoardTile>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // convert bitmask value bit to layer index log2(256) = 8
        layerToSpawnOnIndex = Mathf.RoundToInt(Mathf.Log(layerToSpawnOn.value, 2));

        if (delay > 0) StartCoroutine(SpawnBoard(delay));
    }

    /// <summary>
    /// Spawn board with delay between each tile
    /// </summary>
    private IEnumerator SpawnBoard(float Delay)
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                SpawnTile(i, j);
                yield return new WaitForSeconds(Delay);
            }
        }

        OnFinishedSpawning.Invoke(this, new FinishedSpawningEventArgs(boardTiles, row, column));
    }
    /// <summary>
    /// Spawn tile at row and column
    /// </summary>
    private void SpawnTile(int row, int col)
    {
        string name = (row + 1) + "x" + (col + 1);
        GameObject spawnedTile = null;
        spawnedTile = Instantiate(boardTile, GetSpawnLocation(row, col), Quaternion.identity, gameBoard.transform);
        spawnedTile.name = name;
        spawnedTile.layer = layerToSpawnOnIndex;
        spawnedTile.GetComponent<BoardTile>().Info.Position.SetPosition(row, col);
        boardTiles.Add(spawnedTile.GetComponent<BoardTile>());
    }
    /// <summary>
    /// Get spawn location relative to scale of tile
    /// </summary>
    private Vector3 GetSpawnLocation(int row, int col)
    {
        Vector3 originalSpawnPoint = transform.position;

        float x = originalSpawnPoint.x + (boardTile.transform.localScale.x + distanceBetweenTiles) * col;
        float y = originalSpawnPoint.y;
        float z = originalSpawnPoint.z - (boardTile.transform.localScale.z + distanceBetweenTiles) * row;

        return new Vector3(x, y, z);
    }
}

/// <summary>
/// Class that stores data to sent to all subscribers of OnFinishedSpawningEvent
/// </summary>
public class FinishedSpawningEventArgs
{
    /// <summary>
    /// OnFinishedSpawningEvent event argument constructor
    /// </summary>
    public FinishedSpawningEventArgs(List<BoardTile> boardTiles, int row, int column)
    {
        BoardTiles = boardTiles;
        Row = row;
        Column = column;
    }

    public List<BoardTile> BoardTiles { get; }
    public int Row { get; }
    public int Column { get; }
}