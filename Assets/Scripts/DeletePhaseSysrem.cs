using UnityEngine;
using UnityEngine.InputSystem;

public class DeletePhaseSysrem : MonoBehaviour
{
    [SerializeField] private GameCycle gameCycle;
    [SerializeField] private Board board;//ボードのスクリプトを格納
    [SerializeField] private PutPhaseSystem putPhaseSystem;
    [SerializeField] private Beat beat;//ビートのスクリプトを格納
    private Camera mainCamera;
    [SerializeField] private GameObject highlightPrefab; // インスペクターで設定可能（設定しなくても自動生成します）
    private GameObject currentHighlight;

    private void Start()
    {
        mainCamera = Camera.main;
        InitializeHighlight();
    }
    private void Update()
    {
        HandleHighlight();
    }

    // Input Systemのイベントから呼び出す関数
    public void OnClick(InputAction.CallbackContext context)
    {
        // クリックされた瞬間かつ、Deleteフェーズの時のみ実行
        if (context.performed && gameCycle.currentState == GameCycle.GameState.Delete)
        {
            // タイミングが合っていない場合動かせない
            if (!beat.IsOnBeat()) return;

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

    private void InitializeHighlight()
    {
        if (highlightPrefab != null)
        {
            currentHighlight = Instantiate(highlightPrefab);
        }
        else
        {
            // プレハブが設定されていない場合は、コードで簡易的なハイライト（半透明の黄色い四角）を作成
            currentHighlight = new GameObject("HighlightCursor");
            SpriteRenderer sr = currentHighlight.AddComponent<SpriteRenderer>();
            sr.sprite = CreateSquareSprite();
            sr.color = new Color(1f, 1f, 1f, 0.5f); // 半透明の白
            sr.sortingOrder = 100; // 最前面に表示
        }
        currentHighlight.SetActive(false);
    }

    // ハイライトの表示・移動処理
    private void HandleHighlight()
    {
        // Deleteフェーズでなければ非表示にして終了
        if (gameCycle.currentState != GameCycle.GameState.Delete)
        {
            if (currentHighlight.activeSelf) currentHighlight.SetActive(false);
            return;
        }

        // マウス位置の取得とRaycast
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null)
        {
            // ブロックに当たったら、そのブロックのグリッド位置にハイライトを移動
            Vector2 pos = Rounding.Round(hit.transform.position);
            currentHighlight.transform.position = pos;
            
            if (!currentHighlight.activeSelf) currentHighlight.SetActive(true);
        }
        else
        {
            // 何もない場所なら非表示
            if (currentHighlight.activeSelf) currentHighlight.SetActive(false);
        }
    }

    // 白い正方形のスプライトを生成するヘルパー関数
    private Sprite CreateSquareSprite()
    {
        Texture2D texture = new Texture2D(32, 32);
        Color[] pixels = new Color[32 * 32];
        for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.white;
        texture.SetPixels(pixels);
        texture.Apply();
        // 第4引数(pixelsPerUnit)を32に設定することで、32pxの画像がUnity上で1単位(1m)の大きさになります
        return Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
    }
}