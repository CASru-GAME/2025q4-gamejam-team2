using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 変数の作成//
    Spawner spawner;//スポナー
    Block activeBlock;//生成されたブロック格納
    [SerializeField]
    private float dropInterval = 0.25f;//次にブロックが落ちるまでのインターバル時間
    private float nextdropTimer = 0.0f;//次にブロックが落ちるまでの時間
    Board board;//ボードのスクリプトを格納

    //入力受付タイマー（3種類）
    float nextKeyDownTimer, nextKeyLeftRightTimer, nextKeyRotateTimer;
    //入力インターバル（3種類）
    [SerializeField]
    private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval;
    
    private void Start()
    {
        //スポナーオブジェクトをスポナー変数に格納する
        spawner = GameObject.FindObjectOfType<Spawner>();

        //ボードオブジェクトをボード変数に格納する
        board = GameObject.FindObjectOfType<Board>();

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
        PlayerInput();
        //Updateで時間の判定をして判定次第で落下関数を呼ぶ
        if(Time.time > nextdropTimer)
        {
            nextdropTimer = Time.time +dropInterval;
            if (activeBlock)
            {
                activeBlock.MoveDown();

                //UpdateでBoardクラスの関数をよ呼び出してボードから出ていないかを確認
                if(!board.CheckPosition(activeBlock))
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
    void PlayerInput()
    {
        if(Input.GetKey(KeyCode.D) && (Time.time > nextKeyLeftRightTimer)||Input.GetKeyDown(KeyCode.D))
        {
            activeBlock.MoveRight();
            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;
            if(!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveLeft();
            }
        }
        else if (Input.GetKey(KeyCode.A) && (Time.time > nextKeyLeftRightTimer)||Input.GetKeyDown(KeyCode.A))
        {
            activeBlock.MoveLeft();
            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;
            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveRight();
            }
        }
        else if (Input.GetKey(KeyCode.E) && (Time.time > nextKeyRotateTimer)||Input.GetKeyDown(KeyCode.E))
        {
            activeBlock.RotateRight();
            nextKeyRotateTimer = Time.time + nextKeyRotateInterval;
            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateLeft();
            }
        }
        else if (Input.GetKey(KeyCode.Q) && (Time.time > nextKeyRotateTimer)||Input.GetKeyDown(KeyCode.Q))
        {
            activeBlock.RotateLeft();
            nextKeyRotateTimer = Time.time + nextKeyRotateInterval;
            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateRight();
            }
        }
        else if (Input.GetKey(KeyCode.S) && (Time.time > nextKeyDownTimer)||Input.GetKeyDown(KeyCode.S))
        {
            activeBlock.MoveDown();

            nextKeyDownTimer = Time.time + nextKeyDownInterval;
            nextdropTimer = Time.time + dropInterval;

            if (!board.CheckPosition(activeBlock))
            {
                //底についた時の処理
                BottomBoard();
            }
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
