using UnityEngine;

public class Spawner : MonoBehaviour
{
    //配列の作成（生成するブロックすべてを格納する）
    [SerializeField]
    Block[] blocks;
    
    Block GetRandomBlock()
    {
        int i = Random.Range(0, blocks.Length);

        if (blocks[i])
        {
            return blocks[i];
        }
        else
        {
            return null;
        }
    }
    public Block SpawnBlock()
    {
        Block block = Instantiate(GetRandomBlock(), transform.position, Quaternion.identity);

        if (block)
        {
            return block;
        }
        else
        {
            return null;
        }
    }
}
