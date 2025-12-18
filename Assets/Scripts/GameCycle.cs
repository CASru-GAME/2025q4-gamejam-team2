using UnityEngine;

public class GameCycle : MonoBehaviour
{
    public enum GameState
    {
        Start,
        Put,
        Delete,
        Processing,
        GameOver
    }
    public GameState currentState;

    void Start()
    {
        currentState = GameState.Put;

    }

    void Update()
    {
        
    }
}
