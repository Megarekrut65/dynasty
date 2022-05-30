using UnityEngine;

public class OpenUrl : MonoBehaviour {
    public string url = "https://";
    
    public void Open() {
        Application.OpenURL(url);
    }
}
