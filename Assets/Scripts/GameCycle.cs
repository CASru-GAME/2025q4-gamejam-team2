using UnityEngine;

public class GameCycle : MonoBehaviour
{
    public enum GameState
    {
        Start,
        Put,
        Delete,
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
