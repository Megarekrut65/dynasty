using System;
using UnityEngine;

public class SettingsButtonClick : MonoBehaviour {
    [SerializeField]
    private Animation anim;
    [SerializeField]
    private SettingsBoard settingsBoard;
    private bool isShowed = false;

    public void Click() {
        isShowed = !isShowed;
        anim.Play("SettingsButtonClickAnimation");
        settingsBoard.SetActive(isShowed);
    }
}
