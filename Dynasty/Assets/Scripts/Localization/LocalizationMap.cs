using System.Collections;
using System.Collections.Generic;
using System.IO;
public class LocalizationMap<ValueType>
{
    private SortedDictionary<string, ValueType> map{get; set;}
    private static LocalizationMap<ValueType> _instance;
    private static readonly object _lock = new object();
    private bool isChanged = false;
    public bool IsChanged{
        get{return isChanged;}
    }
    public delegate void ChangeLanguageText();
    public event ChangeLanguageText OnLanguageChanged;
    private LocalizationMap(){
        map = new SortedDictionary<string, ValueType>();
    }
    public static LocalizationMap<ValueType> GetInstance(){
        if(_instance == null){
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new LocalizationMap<ValueType>();
                }
            }
        }
        return _instance;
    }
    public void Change(SortedDictionary<string, ValueType> map){
        this.map = map;
        isChanged = true;
        OnLanguageChanged?.Invoke();
    }
    public ValueType GetValue(string key){
        if(map.ContainsKey(key)){
            return map[key];
        }
        return default(ValueType);
    }
}
