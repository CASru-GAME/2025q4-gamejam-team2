using UnityEngine;

public class Board : MonoBehaviour
{
    //2次元配列の作成//
    private Transform[,] grid;

    //関数の作成//
    //CheckPositionに追記

    [SerializeField]
    private Transform emptySprite;
    [SerializeField]
    private int height = 30, width = 10, header = 8;

    private void Awake()
    {
        grid = new Transform[width, height];
    }

    void Start()
    {
        CreateBoard();
    }

    //ボードを生成する関数の作成
    void CreateBoard()
    {
        if(emptySprite)
        {
            for (int y = 0; y < height - header; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Transform clone = Instantiate(emptySprite, new Vector3(x, y, 0), Quaternion.identity);

                    clone.transform.parent = this.transform;
                }
            }
        }
    }

    // ブロックが枠内にあるのか判定する関数を呼ぶ関数
    public bool CheckPosition(Block block)
    {
        foreach (Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            if (!BoardOutCheck((int)pos.x, (int)pos.y))
            {
                return false;
            }

            if(BlockCheck((int)pos.x, (int)pos.y,block))
            {
                return false;
            }
        }
        return true;
    }

    //ブロックが枠内にあるのか判定する関数
    bool BoardOutCheck(int x, int y)
    {
        //x軸が0からwidth-1まで、y軸が0以上ならtrueを返す
        return x >= 0 && x < width && y >= 0;
    }

    //移動先にブロックがないか判定する関数
    bool BlockCheck(int x, int y,Block block)
    {
        //2次元配列が空ではないのはほかのブロックがあるとき
        //親が違うのはほかのブロックがあるとき
        return grid[x, y] != null && grid[x, y].parent != block.transform;
    }

    //ブロックが落ちたポジションを記録する関数
    public void SaveBlockInGrid(Block block)
    {
        foreach(Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            grid[(int)pos.x, (int)pos.y] = item;
        }
    }
}
