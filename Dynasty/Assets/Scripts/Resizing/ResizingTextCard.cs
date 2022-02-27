using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ResizingTextCard : MonoBehaviour {
    [SerializeField]
    private Text text;
    [SerializeField]
    private Text title;
    private ResizingManager manager;
    void Start(){
        manager = GameObject.Find("ResizingManager").GetComponent<ResizingManager>();
        manager.data.OnChanged += ChangeText;
        StartCoroutine(MakeTextSameSize());
    }
     IEnumerator MakeTextSameSize(){
        yield return null;

        int size = text.cachedTextGenerator.fontSizeUsedForBestFit;
        manager.data.MinFontSize = size;
        ChangeText();
     }
     void ChangeText(){
         text.resizeTextMaxSize = Math.Min(manager.data.MinFontSize,
            title.cachedTextGenerator.fontSizeUsedForBestFit);
     }
}