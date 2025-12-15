using UnityEngine;
using UnityEngine.InputSystem;

public class PutPhaseSystem : MonoBehaviour
{
    // 変数の作成//
    [SerializeField] private Spawner spawner;//スポナー
    private Block activeBlock;//生成されたブロック格納
    [SerializeField]
    private float dropInterval = 0.25f;//次にブロックが落ちるまでのインターバル時間
    private float nextdropTimer = 0.0f;//次にブロックが落ちるまでの時間
    [SerializeField] private Board board;//ボードのスクリプトを格納

    //入力受付タイマー（3種類）
    private  float nextKeyDownTimer, nextKeyLeftRightTimer, nextKeyRotateTimer;
    //入力インターバル（3種類）
    [SerializeField]
    private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval;

    private void Start()
    {
        spawner.transform.position = Rounding.Round(spawner.transform.position);

        //タイマー初期化
        nextKeyDownTimer = Time.time + nextKeyDownInterval;
        nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;
        nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

        //スポナークラスからブロック生成関数を呼んで変数に格納する
        if (!activeBlock)
        {
            activeBlock = spawner.SpawnBlock();
        }
    }

    private void Update()
    {
        //Updateで時間の判定をして判定次第で落下関数を呼ぶ
        if (Time.time > nextdropTimer)
        {
            nextdropTimer = Time.time + dropInterval;
            if (activeBlock)
            {
                //activeBlock.MoveDown();

                //UpdateでBoardクラスの関数を呼び出してボードから出ていないかを確認
                if (!board.CheckPosition(activeBlock))
                {
                    //ボードから出ていたら元の位置に戻す
                    activeBlock.MoveUp();

                    board.SaveBlockInGrid(activeBlock);

                    //新しいブロックを生成する
                    activeBlock = spawner.SpawnBlock();
                }
            }
        }

    }
    //キーの入力を検知してブロックを動かす関数
    public void OnMove(InputAction.CallbackContext context)
    {
        // 入力が確定した瞬間のみ実行
        if (context.performed)
        {
            Vector2 value = context.ReadValue<Vector2>();

            if (value.x > 0) // 右移動
            {
                activeBlock.MoveRight();
                if (!board.CheckPosition(activeBlock))
                {
                    activeBlock.MoveLeft();
                }
            }
            else if (value.x < 0) // 左移動
            {
                activeBlock.MoveLeft();
                if (!board.CheckPosition(activeBlock))
                {
                    activeBlock.MoveRight();
                }
            }
        }
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float value = context.ReadValue<float>();

            if (value > 0) // 右回転
            {
                activeBlock.RotateRight();
                if (!board.CheckPosition(activeBlock))
                {
                    activeBlock.RotateLeft();
                }
            }
            else if (value < 0) // 左回転
            {
                activeBlock.RotateLeft();
                if (!board.CheckPosition(activeBlock))
                {
                    activeBlock.RotateRight();
                }
            }
        }
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // 有効な位置にある限り、下へ移動し続ける
            while (board.CheckPosition(activeBlock))
            {
                activeBlock.MoveDown();
            }

            // ループを抜けた時点でブロックは無効な位置（床や他のブロックの中）にあるため、
            // BottomBoard() を呼んで 1つ戻して固定する
            BottomBoard();
        }
    }
    void BottomBoard()
    {
        activeBlock.MoveUp();
        board.SaveBlockInGrid(activeBlock);

        activeBlock = spawner.SpawnBlock();

        nextdropTimer = Time.time;
        nextKeyLeftRightTimer = Time.time;
        nextKeyRotateTimer = Time.time;
    }
}
