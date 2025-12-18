using UnityEngine;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    //2次元配列の作成//
    public Transform[,] grid;

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

    public bool IsOverLimit()
    {
        // 判定を開始する高さ
        int limitY = height - header - 4;

        // limitY 以上のすべての行をチェックする
        for (int y = limitY; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Sorting()
    {
        ApplyGravity();
        SlideColumns();
    }

    private void ApplyGravity()
    {
        for (int x = 0; x < width; x++)
        {
            // 下から上に向かってチェック
            for (int y = 1; y < height; y++)
            {
                if (grid[x, y] != null)
                {
                    int dropY = y;
                    // 下が空いている限り落下位置を下げる
                    while (dropY > 0 && grid[x, dropY - 1] == null)
                    {
                        dropY--;
                    }

                    // 移動が発生する場合
                    if (dropY != y)
                    {
                        // グリッド情報の更新
                        grid[x, dropY] = grid[x, y];
                        grid[x, y] = null;
                        
                        // 実際のオブジェクトの位置を更新
                        grid[x, dropY].position = new Vector3(x, dropY, 0);
                    }
                }
            }
        }
    }

    // 列の一番下のブロックを見て空白なら右の列と内容を列ごと交換する関数
    private void SlideColumns()
    {
        // バブルソートの要領で、空白列を右端へ、中身のある列を左へ移動させる
        // width回繰り返すことで確実に詰める
        for (int i = 0; i < width; i++)
        {
            for (int x = 0; x < width - 1; x++)
            {
                // 現在の列の一番下が空で、右隣の列の一番下が空でない場合（あるいは単に空なら詰める場合）
                if (grid[x, 0] == null)
                {
                    SwapColumn(x, x + 1);
                }
            }
        }
    }

    // 指定された2つの列を入れ替えるヘルパー関数
    private void SwapColumn(int x1, int x2)
    {
        for (int y = 0; y < height; y++)
        {
            // グリッドの中身を入れ替え
            Transform temp = grid[x1, y];
            grid[x1, y] = grid[x2, y];
            grid[x2, y] = temp;

            // 位置情報の更新
            if (grid[x1, y] != null)
            {
                grid[x1, y].position = new Vector3(x1, y, 0);
            }
            if (grid[x2, y] != null)
            {
                grid[x2, y].position = new Vector3(x2, y, 0);
            }
        }
    }

    // 連結を判定し、連結がなければテトリスフェーズ（Put）へ移行する関数
    public void CheckConnections(GameCycle gameCycle)
    {
        bool hasConnection = false;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // ブロックが存在する場合のみチェック
                if (grid[x, y] != null)
                {
                    // 上下左右に同じ色のブロックがあるか確認
                    if (HasSameColorNeighbor(x, y))
                    {
                        hasConnection = true;
                        // 1つでも連結が見つかれば「フェーズ続行」確定なのでループを抜ける
                        break; 
                    }
                }
            }
            if (hasConnection) break;
        }

        // 最後の要素まで見て同じ色がなければ（連結がなければ）、テトリスフェーズに移る
        if (!hasConnection)
        {
            gameCycle.currentState = GameCycle.GameState.Put;
        }
        // 連結がある場合は何もしない（現在のフェーズ、例えばDeleteやProcessingを続行）
    }

    // 指定した座標のブロックの上下左右に同じ色のブロックがあるか判定するヘルパー関数
    private bool HasSameColorNeighbor(int x, int y)
    {
        Transform currentBlock = grid[x, y];
        SpriteRenderer currentRenderer = currentBlock.GetComponent<SpriteRenderer>();
        
        if (currentRenderer == null) return false;

        Color currentColor = currentRenderer.color;

        // 上下左右のオフセット
        int[] dx = { 0, 0, -1, 1 };
        int[] dy = { 1, -1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];

            // グリッドの範囲内かつブロックが存在するか確認
            if (nx >= 0 && nx < width && ny >= 0 && ny < height && grid[nx, ny] != null)
            {
                SpriteRenderer neighborRenderer = grid[nx, ny].GetComponent<SpriteRenderer>();
                
                // 色が同じであれば true を返す
                if (neighborRenderer != null && neighborRenderer.color == currentColor)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void DeleteConnectedBlocks(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height || grid[x, y] == null) return;

        Transform startBlock = grid[x, y];
        SpriteRenderer startRenderer = startBlock.GetComponent<SpriteRenderer>();
        if (startRenderer == null) return;

        Color targetColor = startRenderer.color;
        List<Vector2Int> connectedBlocks = new List<Vector2Int>();
        bool[,] visited = new bool[width, height];

        // 連結ブロックを探索
        FindConnectedBlocks(x, y, targetColor, visited, connectedBlocks);

        // 連結数が2以上なら削除
        if (connectedBlocks.Count >= 2)
        {
            foreach (Vector2Int pos in connectedBlocks)
            {
                if (grid[pos.x, pos.y] != null)
                {
                    Destroy(grid[pos.x, pos.y].gameObject);
                    grid[pos.x, pos.y] = null;
                }
            }
        }
    }

    // 再帰的に連結ブロックを探すヘルパー関数
    private void FindConnectedBlocks(int x, int y, Color targetColor, bool[,] visited, List<Vector2Int> connectedBlocks)
    {
        // 範囲外チェック
        if (x < 0 || x >= width || y < 0 || y >= height) return;
        // 訪問済みチェック
        if (visited[x, y]) return;
        // ブロック存在チェック
        if (grid[x, y] == null) return;

        // 色チェック
        SpriteRenderer renderer = grid[x, y].GetComponent<SpriteRenderer>();
        // 色の比較（厳密な一致が必要な場合は == でOK、誤差を許容する場合は別途実装が必要）
        if (renderer == null || renderer.color != targetColor) return;

        // リストに追加して訪問済みにする
        visited[x, y] = true;
        connectedBlocks.Add(new Vector2Int(x, y));

        // 上下左右を探索
        FindConnectedBlocks(x + 1, y, targetColor, visited, connectedBlocks);
        FindConnectedBlocks(x - 1, y, targetColor, visited, connectedBlocks);
        FindConnectedBlocks(x, y + 1, targetColor, visited, connectedBlocks);
        FindConnectedBlocks(x, y - 1, targetColor, visited, connectedBlocks);
    }
}
