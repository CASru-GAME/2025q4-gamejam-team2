using System.Collections.Generic;
using UnityEngine;

public class ColorDictionaryInitializer : MonoBehaviour
{
    [System.Serializable]
    public struct ColorIdPair
    {
        public Color color;
        public int id;
    }

    // インスペクターで編集するためのリスト
    [SerializeField]
    private List<ColorIdPair> colorSettings = new List<ColorIdPair>();

    private void Awake()
    {
        // ゲーム開始時にリストの内容を static な辞書に登録する
        ColorDictionary.ColorToId.Clear();
        
        foreach (var pair in colorSettings)
        {
            if (!ColorDictionary.ColorToId.ContainsKey(pair.color))
            {
                ColorDictionary.ColorToId.Add(pair.color, pair.id);
            }
        }
    }
}