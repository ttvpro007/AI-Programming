using UnityEngine;

/// <summary>
/// Class handling mouse click event
/// </summary>
public class MouseClickHandler : MonoBehaviour
{
    // event delegate
    [System.Serializable]
    public delegate void OnHitGameBoardEvent(object sender, OnHitGameBoardEventArgs eventArgs);
    public event OnHitGameBoardEvent OnHitGameBoard;

    // static instance
    private static MouseClickHandler instance = null;
    public static MouseClickHandler Instance { get { return instance; } }

    // private variables
    [SerializeField] private LayerMask layerToHit = new LayerMask();
    private BoardTile hitBoardTile = null;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        HandlingMouseClick();
    }

    /// <summary>
    /// Handling mouse click event
    /// </summary>
    private void HandlingMouseClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (IsMouseClickHit(out hitBoardTile))
            {
                if (GameManager.State != GameState.Playing) return;

                OnHitGameBoard.Invoke(this, new OnHitGameBoardEventArgs(hitBoardTile));
            }
        }
    }
    /// <summary>
    /// Check if mouse click on game board
    /// </summary>
    private bool IsMouseClickHit(out BoardTile hitBoardTile)
    {
        hitBoardTile = null; // private variable
        Ray mouseClickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(mouseClickRay, out hit, Mathf.Infinity, layerToHit))
        {
            hitBoardTile = hit.transform.GetComponent<BoardTile>();
            return true;
        }

        return false;
    }
}

/// <summary>
/// Class that stores data to sent to all subscribers of OnHitGameBoardEvent
/// </summary>
public class OnHitGameBoardEventArgs
{
    public OnHitGameBoardEventArgs(BoardTile bt) { boardTile = bt; }
    public BoardTile boardTile { get; }
}