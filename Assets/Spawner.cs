using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] tetrominoes;
    public Transform boardOrigin;

    public bool GameOver { get; private set; }

    void Start()
    {
        ClearGrid();

        if (boardOrigin != null)
            TetrisBlock.origin = boardOrigin.position;
        else
            TetrisBlock.origin = Vector2.zero;

        SnapSpawnerToGrid();
        NewTetromino();
    }

    public void NewTetromino()
    {
        if (GameOver) return;

        var go = Instantiate(
            tetrominoes[Random.Range(0, tetrominoes.Length)],
            transform.position,
            Quaternion.identity
        );

        var block = go.GetComponent<TetrisBlock>();
        if (block != null && !block.IsValidNow())
        {
            GameOver = true;
            Debug.Log("GAME OVER");
        }
    }

    void SnapSpawnerToGrid()
    {
        Vector2 p = transform.position;
        p.x = Mathf.Round(p.x);
        p.y = Mathf.Round(p.y);
        transform.position = p;
    }

    static void ClearGrid()
    {
        for (int x = 0; x < TetrisBlock.width; x++)
            for (int y = 0; y < TetrisBlock.height; y++)
                TetrisBlock.grid[x, y] = null;
    }
}



