using UnityEngine;

public class Tetromino : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Tetoromino Start called");
        PrintCells();
    }

    void PrintCells()
    {
        foreach (Transform block in transform) //親オブジェクトの子を1つずつ見る
        {
            int x = Mathf.RoundToInt(block.position.x);　//小数点以下をマス番号に変換する
            int y = Mathf.RoundToInt(block.position.y);

            Debug.Log($"block at grid ({x},{y})");
        }
    }

    bool IsValidPosition()
    {
        foreach (Transform block in transform)
        {
            int x = Mathf.RoundToInt(block.position.x);
            int y = Mathf.RoundToInt(block.position.y);

            if (x < 0 || x >= Board.width || y < 0)
                return false;

            if (Board.grid[x, y] != null)
                return false;
        }
        return true;
    }

    private void Update()
    {
        
        //左
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            TryMove(Vector3.left);

        //右
        if (Input.GetKeyDown(KeyCode.RightArrow))
            TryMove(Vector3.right);

        //自動落下
        transform.position += Vector3.down;

        if (!IsValidPosition())
        {
            transform.position += Vector3.up;
            AddToGrid();
            FindObjectOfType<Spawner>().Spawn();
            enabled = false;
        }

        if (!IsValidPosition())
        {
            //戻す
            transform.position += Vector3.up;

            //固定
            AddToGrid();

            //不動にする
            enabled = false;
        }
    }

    void AddToGrid()
    {
        foreach (Transform block in transform)
        {
            int x = Mathf.RoundToInt(block.position.x);
            int y = Mathf.RoundToInt(block.position.y);

            Board.grid[x, y] = block;
        }
    }


        


    void TryMove(Vector3 direction)
    {
        transform.position += direction;

        if (!IsValidPosition())
        {
            transform.position -= direction;

        }
    }
}