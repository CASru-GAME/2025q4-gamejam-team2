using UnityEngine;
using UnityEngine.InputSystem;

public class DeletePhaseSysrem : MonoBehaviour
{
    [SerializeField] private GameCycle gameCycle;
    [SerializeField] private Board board;//ボードのスクリプトを格納
    [SerializeField] private PutPhaseSystem putPhaseSystem;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Input Systemのイベントから呼び出す関数
    public void OnClick(InputAction.CallbackContext context)
    {
        // クリックされた瞬間かつ、Deleteフェーズの時のみ実行
        if (context.performed && gameCycle.currentState == GameCycle.GameState.Delete)
        {
            // マウスのスクリーン座標を取得
            Vector2 mousePos = Mouse.current.position.ReadValue();
            // ワールド座標に変換
            Vector2 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

            // その位置にあるコライダー（ブロック）を検出
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null)
            {
                // 当たったブロックの座標を整数化（Roundingクラスを使用）
                Vector2 pos = Rounding.Round(hit.transform.position);
                int x = (int)pos.x;
                int y = (int)pos.y;

                // 1. 連結ブロックを削除
                board.DeleteConnectedBlocks(x, y);

                // 2. 盤面を整理（重力落下と列詰め）
                board.Sorting();

                // 3. まだ連結があるか確認し、なければPutフェーズへ移行
                board.CheckConnections(gameCycle);
                putPhaseSystem.SpawnBlock();
            }
        }
    }
}