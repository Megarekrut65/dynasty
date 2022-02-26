using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextResizing : MonoBehaviour{
    private static ResizingData data = new ResizingData(1000);
    [SerializeField]
    private Text text;

    void Awake() {
        ResizingData.OnChanged += ChangeText;
    }
    void OnDestroy(){
        ResizingData.OnChanged -= ChangeText;
    }
    void Start(){
        StartCoroutine(MakeTextSameSize());
    }
     IEnumerator MakeTextSameSize(){
        yield return null;

        int size = text.cachedTextGenerator.fontSizeUsedForBestFit;
        data.MinFontSize = size;
        ChangeText();
     }
     void ChangeText(){
         text.resizeTextMaxSize = data.MinFontSize;
     }
}
class ResizingData{
    private int minFontSize;
    public int MinFontSize{
        get{
            return minFontSize;
        }
        set{
            lock (_lock){
                if(value < minFontSize){
                    minFontSize = value;
                    OnChanged?.Invoke();
                }
            }
        }
    }
    public readonly object _lock = new object();
    public delegate void ChangeFontSize();
    public static event ChangeFontSize OnChanged;
    public ResizingData(int minFontSize){
        this.minFontSize = minFontSize;
    }
}