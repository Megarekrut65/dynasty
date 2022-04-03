using UnityEngine;

public class SettingsBoard : MonoBehaviour {
    [SerializeField]
    private Animation anim;

    public void SetActive(bool active) {
        anim.Play(active ? "SettingsShowAnimation" : "SettingsHideAnimation");
    }
}
