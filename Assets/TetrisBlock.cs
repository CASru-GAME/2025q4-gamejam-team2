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
        SnapAll();
    }

    void Update()
    {
        if (spawner != null && spawner.GameOver) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left;
            SnapAll();
            if (!IsValidMove())
            {
                transform.position += Vector3.right;
                SnapAll();
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += Vector3.right;
            SnapAll();
            if (!IsValidMove())
            {
                transform.position += Vector3.left;
                SnapAll();
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);
            SnapRotation();
            SnapAll();
            if (!IsValidMove())
            {
                transform.Rotate(0, 0, 90);
                SnapRotation();
                SnapAll();
            }
        }

        float interval = Input.GetKey(KeyCode.DownArrow) ? fallTime / 10f : fallTime;
        if (Time.time - previousTime > interval)
        {
            transform.position += Vector3.down;
            SnapAll();

            if (!IsValidMove())
            {
                transform.position += Vector3.up;
                SnapAll();

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

    public void SnapAll()
    {
        Vector3 p = transform.position;
        p.x = Mathf.Round(p.x);
        p.y = Mathf.Round(p.y);
        transform.position = p;

        foreach (Transform child in transform)
        {
            Vector3 lp = child.localPosition;
            lp.x = Mathf.Round(lp.x);
            lp.y = Mathf.Round(lp.y);
            lp.z = 0f;
            child.localPosition = lp;
        }
    }

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

    void SnapRotation()
    {
        float z = transform.eulerAngles.z;
        z = Mathf.Round(z / 90f) * 90f;
        transform.eulerAngles = new Vector3(0f, 0f, z);
    }
}




