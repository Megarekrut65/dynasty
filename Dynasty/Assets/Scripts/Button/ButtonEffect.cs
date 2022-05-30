using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class that add to object button effects
/// </summary>
public class ButtonEffect {
    /// <summary>
    /// Transform of button
    /// </summary>
    private Transform transform;
    /// <summary>
    /// Normal scale of button
    /// </summary>
    private Vector3 scale;
    /// <summary>
    /// Event that be played after button down
    /// </summary>
    private UnityEvent downEvent;
    /// <summary>
    /// Event that be played after button up
    /// </summary>
    private UnityEvent upEvent;
    /// <summary>
    /// True if need play sound false else
    /// </summary>
    private bool needSound;
    /// <summary>
    /// Index of sound in SoundManager
    /// </summary>
    private int soundIndex;
    
    /// <param name="transform">of button</param>
    /// <param name="downEvent">play when button down</param>
    /// <param name="upEvent">play when button up</param>
    /// <param name="needSound">true if need play sound else fasle</param>
    /// <param name="soundIndex">index of sound in SoundManager</param>
    public ButtonEffect(Transform transform, UnityEvent downEvent = null, UnityEvent upEvent = null,
        bool needSound = false, int soundIndex = 0) {
        this.transform = transform;
        this.downEvent = downEvent;
        this.upEvent = upEvent;
        this.scale = transform.localScale;
        this.needSound = needSound;
        this.soundIndex = soundIndex;
    }
    /// <summary>
    /// Calls down button effects
    /// </summary>
    public void Down() {
        transform.localScale = 1.1f * scale;
        downEvent?.Invoke();
        if (needSound) SoundManager.Instance.Play(soundIndex);
    }
    /// <summary>
    /// Calls up button effects
    /// </summary>
    public void Up() {
        transform.localScale = scale;
        upEvent?.Invoke();
    }
}