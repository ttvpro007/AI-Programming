using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class handling update of game status text
/// </summary>
public class GameStatusTextUpdater : MonoBehaviour
{
    [SerializeField] Text gameStatusText = null;

    private void Update()
    {
        gameStatusText.text = GetText(GameManager.State);
    }

    /// <summary>
    /// Display text based on game state
    /// </summary>
    private string GetText(GameState gameState)
    {
        string text = "";

        switch (gameState)
        {
            case GameState.Playing:
                text = "PLAYING";
                break;
            case GameState.X_Win:
                text = "X WON";
                break;
            case GameState.O_Win:
                text = "O WON";
                break;
            case GameState.Draw:
                text = "DRAW";
                break;
        }

        return text;
    }
}
