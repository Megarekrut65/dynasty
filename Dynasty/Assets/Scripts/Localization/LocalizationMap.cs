using System.Collections;
using System.Collections.Generic;

public class LocalizationMap
{
    private SortedDictionary<string, string> map{get; set;}
    private static LocalizationMap _instance;
    private static readonly object _lock = new object();
    private bool isChanged = false;
    public bool IsChanged{
        get{return isChanged;}
    }
    public delegate void ChangeLanguageText();
    public event ChangeLanguageText OnLanguageChanged;
    private LocalizationMap(){
        map = new SortedDictionary<string, string>();
    }
    public static LocalizationMap GetInstance(){
        if(_instance == null){
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new LocalizationMap();
                }
            }
        }
        return _instance;
    }
    public void Change(SortedDictionary<string, string> map){
        this.map = map;
        isChanged = true;
        OnLanguageChanged?.Invoke();
    }
    public string GetValue(string key){
        if(map.ContainsKey(key)){
            return map[key];
        }
        return "";
    }
}
