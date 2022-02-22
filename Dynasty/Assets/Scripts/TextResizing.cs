using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextResizing : MonoBehaviour{
    private static int minFontSize = 1000;
    [SerializeField]
    private Text text;
    public delegate void ChangeFontSize();
    public static event ChangeFontSize OnChanged;
    private static readonly object _lock = new object();
    void Awake() {
        OnChanged += ChangeText;
    }
    void OnDestroy(){
        OnChanged -= ChangeText;
    }
    void Start(){
       
        StartCoroutine(MakeTextSameSize());
    }

     IEnumerator MakeTextSameSize(){
        yield return null;

        int size = text.cachedTextGenerator.fontSizeUsedForBestFit;
        lock (_lock){
            if(minFontSize > size){
                minFontSize = size;
                OnChanged?.Invoke();
            }
        }
        ChangeText();
     }
     void ChangeText(){
         text.resizeTextMaxSize = minFontSize;
     }
}