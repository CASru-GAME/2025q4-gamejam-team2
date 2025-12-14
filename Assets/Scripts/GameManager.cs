using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    Spawner spawner;
    Block activeBlock;
    Board board;

    // float nextKeydownTimer, nextKeyLeftRightTime, nextKeyRotateTimer;
    // [SerializeField] float keyDownInterval = 0.1f;
    // [SerializeField] float keyLeftRightInterval = 0.1f;
    // [SerializeField] float keyRotateInterval = 0.1f;

    private void Start()
    {
        board = Object.FindFirstObjectByType<Board>();
        // Spawnerコンポーネントを取得
        spawner = Object.FindFirstObjectByType<Spawner>();

        // // タイマー初期設定
        // nextKeydownTimer = Time.time + keyDownInterval;
        // nextKeyLeftRightTime = Time.time + keyLeftRightInterval;   
        // nextKeyRotateTimer = Time.time + keyRotateInterval;

        if(activeBlock == null)
        {
            activeBlock = spawner.SpawnBlock();
        }
    }

    private void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    { 
        if(Keyboard.current.dKey.wasPressedThisFrame)
        {
            activeBlock.Moveright();
            if(!board.CheckPosition(activeBlock))
            {
                activeBlock.Moveleft(); // 枠外なら元に戻す
            }
        }
        else if(Keyboard.current.aKey.wasPressedThisFrame)
        {
            activeBlock.Moveleft();
            if(!board.CheckPosition(activeBlock))
            {
                activeBlock.Moveright(); // 枠外なら元に戻す
            }
        }
        else if(Keyboard.current.eKey.wasPressedThisFrame)
        {
            activeBlock.RotateRight();
            if(!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateLeft(); // 枠外なら元に戻す
            }
        }
        else if(Keyboard.current.qKey.wasPressedThisFrame)
        {
            activeBlock.RotateLeft();
            if(!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateRight(); // 枠外なら元に戻す
            }
        }

        else if(Keyboard.current.sKey.wasPressedThisFrame)
            {

                    activeBlock.Movedown();

                    if(!board.CheckPosition(activeBlock))
                    {
                        activeBlock.Moveup(); // 枠外なら元に戻す
                        board.SaveBlockInGrid(activeBlock);
                        activeBlock = spawner.SpawnBlock();
                    }
            }

    }
}
