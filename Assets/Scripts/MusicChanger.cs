using UnityEngine;
using CriWare;

public class MusicChanger : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("制御したい音を鳴らすCriAtomSource")]
    public CriAtomSource targetAtomSource;

    [Tooltip("Atom Craft側のAISACコントロール名")]
    private string aisacControlName = "AisacControl_01";
    [SerializeField] private float musicFeder = 0.0f;
    [SerializeField] private float verosity = 0.05f;
    [SerializeField] private GameCycle gameCycle;

    // 実際にAISAC値を変更する処理
    private void FixedUpdate()
    {
        if (gameCycle.currentState == GameCycle.GameState.Put)
        {
            musicFeder -= verosity;
            if (musicFeder < 0.0f)
            {
                musicFeder = 0.0f;
            }

            targetAtomSource.SetAisacControl(aisacControlName, musicFeder);
        }
        if (gameCycle.currentState == GameCycle.GameState.Delete)
        {
            musicFeder += verosity;
            if (musicFeder > 1.0f)
            {
                musicFeder = 1.0f;
            }
            targetAtomSource.SetAisacControl(aisacControlName, musicFeder);
        }
    }
}
