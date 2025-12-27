using UnityEngine;
using UnityEngine.UI;
using CriWare;

public class AdxInteractiveDemo : MonoBehaviour
{
    [Header("--- 必要なコンポーネントをここにセット ---")]
    [Tooltip("音を鳴らすCriAtomSourceコンポーネント")]
    public CriAtomSource targetAtomSource;

    [Header("--- UIスライダー ---")]
    [Tooltip("AISAC_01と背景色を操作するスライダー")]
    public Slider slider1;
    [Tooltip("AISAC_02を操作するスライダー")]
    public Slider slider2;

    [Header("--- 背景色演出の設定 ---")]
    [Tooltip("色を変えたいカメラ（通常はMain Camera）")]
    public Camera targetCamera;
    [Tooltip("スライダー1が「0」の時の色")]
    public Color color1 = Color.black;
    [Tooltip("スライダー1が「1」の時の色")]
    public Color color2 = Color.white;

    // Atom Craft側で設定したAISACコントロール名
    private const string AisacName01 = "AisacControl_01";
    private const string AisacName02 = "AisacControl_02";

    void Start()
    {
        // --- 初期設定チェック ---
        if (targetAtomSource == null || slider1 == null || slider2 == null)
        {
            Debug.LogError("AdxInteractiveDemo: インスペクターでコンポーネントが設定されていません");
            return;
        }

        // カメラが設定されてなければ、勝手にメインカメラを探す
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        // スライダーの値の範囲を0～1に強制設定
        slider1.minValue = 0f; slider1.maxValue = 1f;
        slider2.minValue = 0f; slider2.maxValue = 1f;

        // --- スライダー操作時のイベント登録 ---
        // スライダーを動かした時に、指定した関数が呼ばれるようにする
        slider1.onValueChanged.AddListener(OnSlider1Changed);
        slider2.onValueChanged.AddListener(OnSlider2Changed);

        // --- 開始時の初期状態を適用 ---
        // ゲーム開始時点のスライダーの位置に合わせて、音と色をセットする
        OnSlider1Changed(slider1.value);
        OnSlider2Changed(slider2.value);

        // 再生開始
        targetAtomSource.Play();
    }

    // スライダー1が動いた時に呼ばれる関数
    // value には 0.0 ～ 1.0 の値が入ってくる
    void OnSlider1Changed(float value)
    {
        // 1. ADXのAISAC_01を制御
        if (targetAtomSource != null)
        {
            targetAtomSource.SetAisacControl(AisacName01, value);
        }

        // 2. 背景色を滑らかに変化させる（Lerp：線形補間）
        if (targetCamera != null)
        {
            targetCamera.backgroundColor = Color.Lerp(color1, color2, value);
        }
    }

    // スライダー2が動いた時に呼ばれる関数
    void OnSlider2Changed(float value)
    {
        // ADXのAISAC_02を制御
        if (targetAtomSource != null)
        {
            targetAtomSource.SetAisacControl(AisacName02, value);
        }
    }
}