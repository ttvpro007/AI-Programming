using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class handling camera to center on game board
/// </summary>
public class CameraCentering : MonoBehaviour
{
    [SerializeField] private float smoothFactor = 1;
    private Vector3 newCameraPosition = Vector3.zero;
    
    private void Start()
    {
        BoardSpawner.Instance.OnFinishedSpawning += SetNewCameraPosition;
    }

    private void OnDisable()
    {
        BoardSpawner.Instance.OnFinishedSpawning -= SetNewCameraPosition;
    }

    private void Update()
    {
        MoveCameraToNewPosition();
    }

    /// <summary>
    /// Move camera to position
    /// </summary>
    private void MoveCameraToNewPosition()
    {
        if (newCameraPosition == Vector3.zero) return;
        transform.position = Vector3.Lerp(transform.position, newCameraPosition, smoothFactor * Time.deltaTime);
    }
    /// <summary>
    /// Event: FinishedSpawningEvent
    /// Action: Set new camera position
    /// </summary>
    private void SetNewCameraPosition(object sender, FinishedSpawningEventArgs eventArgs)
    {
        newCameraPosition = GetCameraPosition(eventArgs.BoardTiles, eventArgs.Row, eventArgs.Column);
    }
    /// <summary>
    /// Get new camera position based on game board size
    /// </summary>
    public Vector3 GetCameraPosition(List<BoardTile> boardTiles, int row, int col)
    {
        Vector3 upperLeftCornerPos = boardTiles[0].transform.position;
        Vector3 lowerRightCornerPos = boardTiles[col * row - 1].transform.position;
        Vector3 position = (upperLeftCornerPos + lowerRightCornerPos) / 2;
        float distance = Vector3.Distance(upperLeftCornerPos, lowerRightCornerPos);
        position.y = (row + col) / 3 + distance / 1.5f; // panning

        return position;
    }
}
