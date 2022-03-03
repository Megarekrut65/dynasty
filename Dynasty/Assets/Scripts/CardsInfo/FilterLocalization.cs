using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterLocalization : MonoBehaviour
{
    [SerializeField]
    private Text label;
    [SerializeField]
    private Dropdown filter;
    void Start(){
        var options = filter.options;
        foreach(var data in options){
            data.text = LocalizationManager.instance.GetWord(data.text.ToLower());
        }
        ChangeText(0);
    }
    public void ChangeText(int value){
        string key = "all";
       switch (value) {
            case 1:
               key = "red";
               break;
            case 2:
               key = "yellow";
               break;
            case 3:
               key = "green";
               break;
           default :
               
               break;
       }
       label.text = LocalizationManager.instance.GetWord(key);
    }
}
