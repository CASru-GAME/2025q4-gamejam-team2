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

    void Awake()
    {
        currentState = GameState.Put;

    }

    void Update()
    {
        
    }
}
