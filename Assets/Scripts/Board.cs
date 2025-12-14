using UnityEngine;

public class Board : MonoBehaviour
{
    private Transform[,] grid;


    [SerializeField] private int height = 20;
    [SerializeField] private int width = 12;
    public float left, right, bottom, top;

        // ワールド座標→gridインデックス変換
    public Vector2Int WorldToGridIndex(Vector2 pos)
    {
        int x = Mathf.FloorToInt(pos.x - left);
        int y = Mathf.FloorToInt(pos.y - bottom);
        return new Vector2Int(x, y);
    }


    private void Awake()
    {
        Vector3 center = transform.position;
        left   = center.x - width  / 2f;
        right  = center.x + width  / 2f;
        bottom = center.y - height / 2f;
        top    = center.y + height / 2f;

        grid = new Transform[width, height];
    }


    public void Start()
    {
        createBorder();
    }

    public void createBorder()
    {

        var border = gameObject.AddComponent<LineRenderer>();
        border.positionCount = 5;
        border.startWidth = 0.05f;
        border.endWidth = 0.05f;

         // 線を白にする
        border.startColor = Color.white;
        border.endColor = Color.white;
        border.material = new Material(Shader.Find("Sprites/Default"));

        //四隅を設定
        border.SetPosition(0, new Vector3(left, bottom, 0));
        border.SetPosition(1, new Vector3(right, bottom, 0));
        border.SetPosition(2, new Vector3(right, top, 0));
        border.SetPosition(3, new Vector3(left, top, 0));
        border.SetPosition(4, new Vector3(left, bottom, 0));
    }

    public bool CheckPosition(Block block)
    {
        foreach(Transform item in block.transform)
        {
            Vector2 pos = Rounding.RoundVector2(item.position);
            Vector2Int idx = WorldToGridIndex(pos);

            if(!BoardOutCheck(idx.x, idx.y))
            {
                Debug.Log("Out of Board");
                return false;
            }
            if(BlockCheck(idx.x, idx.y, block))
            {
                Debug.Log("Block Collision");
                return false;
            }
        }
        return true;
    }

    //枠の範囲内かどうか判定
    bool BoardOutCheck(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0);
    }

    //移動先にブロックがあるかどうか判定
    bool BlockCheck(int x, int y, Block block)
    {
        // grid配列の範囲外ならfalse（衝突なしとみなす）
        if (x < 0 || x >= width || y < 0 || y >= height) return false;
        return (grid[x, y] != null && grid[x, y].parent != block.transform);
    }


    //ブロックが落ちた場所を記録
    public void SaveBlockInGrid(Block block)
    {
        foreach(Transform item in block.transform)
        {
            Vector2 pos = Rounding.RoundVector2(item.position);
            Vector2Int idx = WorldToGridIndex(pos);
            if (idx.x >= 0 && idx.x < width && idx.y >= 0 && idx.y < height)
            {
                grid[idx.x, idx.y] = item;
            }
        }
    }
}


