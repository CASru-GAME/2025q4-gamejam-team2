using UnityEngine;

public static class Rounding 
{
    public static Vector2 RoundVector2(Vector2 i)
    {
        return new Vector2(Mathf.FloorToInt(i.x), Mathf.FloorToInt(i.y));
    }

    public static Vector3 RoundVector2(Vector3 i)
    {
        return new Vector3(Mathf.FloorToInt(i.x), Mathf.FloorToInt(i.y));
    }

}
