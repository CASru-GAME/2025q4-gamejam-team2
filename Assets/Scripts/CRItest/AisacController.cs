using UnityEngine;
using UnityEngine.UI;
using CriWare;

// Sliderコンポーネントが必須であることを明示（アタッチ忘れ防止）
[RequireComponent(typeof(Slider))]
public class AisacController : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("制御したい音を鳴らすCriAtomSource")]
    public CriAtomSource targetAtomSource;

    [Tooltip("Atom Craft側のAISACコントロール名")]
    public string aisacControlName = "AisacControl_01";

    private void Start()
    {
        // 自分自身(Slider1)のSliderコンポーネントを取得
        var slider = GetComponent<Slider>();

        if (slider != null)
        {
            // 起動時に現在のスライダー値を一度反映（初期化）
            ChangeAisacValue(slider.value);

            // スライダーの値が変わった時に呼ばれる関数をコードから登録
            // ※これでInspectorのOnValueChanged設定は不要になる
            slider.onValueChanged.AddListener(ChangeAisacValue);
        }
    }

    // 実際にAISAC値を変更する処理
    private void ChangeAisacValue(float value)
    {
        if (targetAtomSource != null)
        {
            targetAtomSource.SetAisacControl(aisacControlName, value);
        }
    }
}