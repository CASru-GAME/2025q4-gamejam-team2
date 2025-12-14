using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public static int height = 20;
    public static int width = 10;
    public static Transform[,] grid = new Transform[width, height];
    public static Vector2 origin = Vector2.zero;

    public float fallTime = 1f;
    private float previousTime;

    private static Spawner spawner;

    void Start()
    {
        previousTime = Time.time;
        if (spawner == null) spawner = Object.FindFirstObjectByType<Spawner>();
        AlignToGridByFirstChild();
    }

    void Update()
    {
        if (spawner != null && spawner.GameOver) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left;
            AlignToGridByFirstChild();
            if (!IsValidMove())
            {
                transform.position += Vector3.right;
                AlignToGridByFirstChild();
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += Vector3.right;
            AlignToGridByFirstChild();
            if (!IsValidMove())
            {
                transform.position += Vector3.left;
                AlignToGridByFirstChild();
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);
            SnapRotation();
            AlignToGridByFirstChild();

            if (!IsValidMove())
            {
                transform.Rotate(0, 0, 90);
                SnapRotation();
                AlignToGridByFirstChild();
            }
        }

        float interval = Input.GetKey(KeyCode.DownArrow) ? fallTime / 10f : fallTime;
        if (Time.time - previousTime > interval)
        {
            transform.position += Vector3.down;
            AlignToGridByFirstChild();

            if (!IsValidMove())
            {
                transform.position += Vector3.up;
                AlignToGridByFirstChild();

                AddToGrid();
                CheckLines();
                enabled = false;

                if (spawner == null) spawner = Object.FindFirstObjectByType<Spawner>();
                if (spawner != null) spawner.NewTetromino();
            }

            previousTime = Time.time;
        }
    }

    public bool IsValidNow() => IsValidMove();

    bool IsValidMove()
    {
        foreach (Transform child in transform)
        {
            Vector2Int c = WorldToCell(child.position);

            if (c.x < 0 || c.x >= width) return false;
            if (c.y < 0) return false;

            if (c.y < height && grid[c.x, c.y] != null) return false;
        }
        return true;
    }

    void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            Vector2Int c = WorldToCell(child.position);
            if (c.x >= 0 && c.x < width && c.y >= 0 && c.y < height)
                grid[c.x, c.y] = child;
        }
    }

    void CheckLines()
    {
        for (int y = 0; y < height; y++)
        {
            if (HasLine(y))
            {
                DeleteLine(y);
                RowDown(y);
                y--;
            }
        }
    }

    bool HasLine(int y)
    {
        for (int x = 0; x < width; x++)
            if (grid[x, y] == null) return false;
        return true;
    }

    void DeleteLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }
    }

    void RowDown(int fromY)
    {
        for (int y = fromY + 1; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;
                    grid[x, y - 1].position += Vector3.down;
                }
            }
        }
    }

    static Vector2Int WorldToCell(Vector3 world)
    {
        int x = Mathf.RoundToInt(world.x - origin.x);
        int y = Mathf.RoundToInt(world.y - origin.y);
        return new Vector2Int(x, y);
    }

    static Vector2 CellToWorld(Vector2Int cell)
    {
        return origin + (Vector2)cell;
    }

    void AlignToGridByFirstChild()
    {
        if (transform.childCount == 0) return;

        Transform child = transform.GetChild(0);
        Vector2Int cell = WorldToCell(child.position);
        Vector2 target = CellToWorld(cell);
        Vector2 delta = target - (Vector2)child.position;

        transform.position += (Vector3)delta;
    }

    void SnapRotation()
    {
        float z = transform.eulerAngles.z;
        z = Mathf.Round(z / 90f) * 90f;
        transform.eulerAngles = new Vector3(0f, 0f, z);
    }
}



