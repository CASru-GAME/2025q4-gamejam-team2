using UnityEngine;
using UnityEngine.InputSystem;

public class PutPhaseSystem : MonoBehaviour
{
    // 変数の作成//
    [SerializeField] private Spawner spawner;//スポナー
    private Block activeBlock;//生成されたブロック格納
    [SerializeField] private float dropInterval = 0.25f;//次にブロックが落ちるまでのインターバル時間
    private float nextdropTimer = 0.0f;//次にブロックが落ちるまでの時間
    [SerializeField] private Board board;//ボードのスクリプトを格納
    [SerializeField] private GameCycle gameCycle;//ゲームサイクルのスクリプトを格納
 
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
        if (!activeBlock && gameCycle.currentState == GameCycle.GameState.Put)
        {
            activeBlock = spawner.SpawnBlock();
        }
    }

    private void Update()
    {
        checkBlock();

    }

    private void checkBlock()
    {
        if (Time.time > nextdropTimer && gameCycle.currentState == GameCycle.GameState.Put)
        {
            nextdropTimer = Time.time + dropInterval;
            if (activeBlock)
            {
                //UpdateでBoardクラスの関数を呼び出してボードから出ていないかを確認
                if (!board.CheckPosition(activeBlock))
                {
                    // ボードから出ていたら（床や他のブロックに当たったら）固定処理を行う
                    // BottomBoard() 内で MoveUp, Save, CheckOverflowing, Spawn が行われます
                    BottomBoard();
                }
            }
        }
    }
    //キーの入力を検知してブロックを動かす関数
    public void OnMove(InputAction.CallbackContext context)
    {
        // 入力が確定した瞬間のみ実行
        if (context.performed && gameCycle.currentState == GameCycle.GameState.Put)
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
        if (context.performed && gameCycle.currentState == GameCycle.GameState.Put)
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
        if (context.performed && gameCycle.currentState == GameCycle.GameState.Put)
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

    private void BottomBoard()
    {
        activeBlock.MoveUp();
        board.SaveBlockInGrid(activeBlock);

        if(gameCycle.currentState == GameCycle.GameState.Put) activeBlock = spawner.SpawnBlock();
        // ブロックを固定した直後にオーバーフローチェックを行う
        CheckOverflowing();

        nextdropTimer = Time.time;
        nextKeyLeftRightTimer = Time.time;
        nextKeyRotateTimer = Time.time;
    }

    private void CheckOverflowing()
    {
        // Boardクラスに高さ制限を超えているか確認するメソッドがあると仮定
        // もしBoardクラスにこの機能がない場合は、Boardクラスに `public bool IsOverLimit()` を追加する必要があります
        if (board.IsOverLimit()) 
        {
            gameCycle.currentState = GameCycle.GameState.Delete;
            // 必要であればアクティブなブロックを破棄したり、入力を無効化する処理を追加
            if (activeBlock != null)
            {
                Destroy(activeBlock.gameObject);
            }
        }
    }
}
