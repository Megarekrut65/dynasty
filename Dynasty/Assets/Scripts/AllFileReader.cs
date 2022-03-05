using UnityEngine.Networking;
using UnityEngine;
using System.IO;
public class AllFileReader{
    public static string Read(string path){
        string res;
        if (Application.platform == RuntimePlatform.Android)
        {
            UnityWebRequest www = UnityWebRequest.Get(path);
            www.SendWebRequest();
            while (!www.isDone){}
            res = www.downloadHandler.text;
        }
        else
        {
            res = File.ReadAllText(path);
        }
        return res;
    }
}