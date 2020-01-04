using UnityEngine;

/// <summary>
/// Class holding tile data
/// </summary>
public class BoardTile : MonoBehaviour
{
    // public variables
    public TileInfo Info = new TileInfo();

    /// <summary>
    /// Reset tile and destroy occupied piece
    /// </summary>
    public void Reset()
    {
        Destroy(Info.GamePiece);
        Info.Reset();
    }
}

[System.Serializable]
public class TileInfo
{
    // public variables
    public Piece OccupiedPiece = Piece.Blank;
    public bool Occupied = false;
    public GameObject GamePiece = null;
    public Position Position = new Position(-1, -1);

    /// <summary>
    /// Reset tile info to defaults
    /// </summary>
    public void Reset()
    {
        OccupiedPiece = Piece.Blank;
        Occupied = false;
        GamePiece = null;
    }
}