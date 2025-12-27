using UnityEngine;
using System.Collections.Generic;

public class Beat : MonoBehaviour
{
    //ビートの情報を格納する変数
    private int bps = 135; // Beats Per Second
    public float beatInterval; // ビート間隔（秒）
    public float nextBeatTime; // 次のビートの時間
    [SerializeField] private float judgementWindow = 0.15f;
    [SerializeField] private GameCycle gameCycle;
    public float buffer = -0.1f;

    void Start()
    {
        beatInterval = 60f / bps;
        nextBeatTime = beatInterval + buffer;
    }

    void Update()
    {
        if (gameCycle.currentState != GameCycle.GameState.Put)
        {
            if (Time.time >= nextBeatTime)
            {
                nextBeatTime += beatInterval;
            }
        }
    }

    public bool IsOnBeat()
    {
        float prev = nextBeatTime - beatInterval;

        // 近い方のビートとの差を見る
        bool isWithin = Mathf.Min(Mathf.Abs(Time.time - prev), Mathf.Abs(Time.time - nextBeatTime)) <= judgementWindow;
        Debug.Log("Time: " + Mathf.Min(Mathf.Abs(Time.time - prev), Mathf.Abs(Time.time - nextBeatTime)));
        return isWithin;
    }

}