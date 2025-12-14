using UnityEngine;

public class Block : MonoBehaviour
{
    //回転していいかどうか
    [SerializeField] private bool canRotate = true;

    //移動
    public void Move(Vector3 moveDirection)
    {
        transform.position += moveDirection;
    }

    public void Moveleft()
    {
        Move(new Vector3(-1, 0, 0));
    }
    public void Moveright()
    {
        Move(new Vector3(1, 0, 0));
    }
    public void Movedown()
    {
        Move(new Vector3(0, -1, 0));
    }
    public void Moveup()
    {
        Move(new Vector3(0, 1, 0));
        Debug.Log("Moveup");
    }

    //回転
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
