using UnityEngine;

public class Spawner : MonoBehaviour
{
    //配列作成
    [SerializeField] Block[] Blocks;

    //ランダムにブロックを1つ選ぶ
    Block GetRandomBlock()
    {
        int i = Random.Range(0, Blocks.Length);

        if (Blocks[i] != null)
        {
            return Blocks[i];
        }
        else
        {
            Debug.LogError("Blocks配列にnullが含まれています。");
            return null;
        }
    }

    //選ばれたブロックを生成
    public Block SpawnBlock()
    {
        Block block = Instantiate(GetRandomBlock(),
            transform.position,
            Quaternion.identity);
        
        if (block != null)
        {
            return block;
        }
        else
        {
            Debug.LogError("ブロックの生成に失敗しました。");
            return null;
        }
    }
}
