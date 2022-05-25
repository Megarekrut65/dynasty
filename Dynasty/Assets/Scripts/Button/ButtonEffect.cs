using UnityEngine;
using UnityEngine.Events;

public class ButtonEffect {
    private Transform transform;
    private Vector3 scale;
    private UnityEvent downEvent;
    private UnityEvent upEvent;
    private bool needSound;
    private int soundIndex;

    public ButtonEffect(Transform transform, UnityEvent downEvent = null, UnityEvent upEvent = null,
        bool needSound = false, int soundIndex = 0) {
        this.transform = transform;
        this.downEvent = downEvent;
        this.upEvent = upEvent;
        this.scale = transform.localScale;
        this.needSound = needSound;
        this.soundIndex = soundIndex;
    }
    public void Down() {
        transform.localScale = 1.1f * scale;
        downEvent?.Invoke();
        if (needSound) SoundManager.Instance.Play(soundIndex);
    }
    public void Up() {
        transform.localScale = scale;
        upEvent?.Invoke();
    }
}