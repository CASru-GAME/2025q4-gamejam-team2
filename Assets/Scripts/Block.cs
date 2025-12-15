using UnityEngine;
using System.Collections.Generic;

public class Block : MonoBehaviour
{
    //変数の作成//
    //回転していいブロックかどうか
    [SerializeField]
    private bool canRotate = true;

    private void Start()
    {
        SetRandomColor();
    }

    //関数の作成//

    // 子オブジェクトの色をランダムに変更する関数
    void SetRandomColor()
    {
        // 辞書が空の場合は処理しない
        if (ColorDictionary.ColorToId.Count == 0) return;

        // 辞書のキー（色）をリストに変換
        List<Color> colors = new List<Color>(ColorDictionary.ColorToId.Keys);

        // ランダムに1色選ぶ
        Color randomColor;
        

        // すべての子オブジェクトに対して処理
        foreach (Transform child in transform)
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {

                randomColor = colors[Random.Range(0, colors.Count)];
                randomColor.a = 1.0f;
                spriteRenderer.color = randomColor;
            }
        }
    }

    //移動用
    void Move(Vector3 moveDirection)
    {
        transform.position += moveDirection;
    }

    //移動関数を呼ぶ関数(4種類)
    public void MoveLeft()
    {
        Move(new Vector3(-1, 0, 0));
    }

    public void MoveRight()
    {
        Move(new Vector3(1, 0, 0));
    }

    public void MoveDown()
    {
        Move(new Vector3(0, -1, 0));
    }

    public void MoveUp()
    {
        Move(new Vector3(0, 1, 0));
    }

    //回転用(2種類)
    public void RotateRight()
    {
        if (canRotate)
        {
            transform.Rotate(0, 0, -90);
        }
    }
    public void RotateLeft()
    {
        if (canRotate)
        {
            transform.Rotate(0, 0, 90);
        }
    }
}
