using UnityEngine;

public class Board : MonoBehaviour
{
    public static int width = 10;
    public static int height = 20;

    //盤面　固定されたブロックだけを記録する
    public static Transform[,] grid = new Transform[width, height];
}