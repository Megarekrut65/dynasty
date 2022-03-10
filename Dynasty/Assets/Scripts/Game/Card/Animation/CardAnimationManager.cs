
using System;
using UnityEngine;

public class CardAnimationManager : MonoBehaviour
{
    [SerializeField]
    private AnimationClip[] clips;

    public void AddAnimation(GameObject obj, string name)
    {
        var clip = Array.Find(clips, (cl) => cl.name == name);
        if (clip != null)
        {
            var anim = obj.GetComponent<Animation>();
            if (anim[name] == null) anim.AddClip(clip, name);
        }
    }
    public void PlayCardHideShowAnimation(GameObject obj, Func<bool> afterHide, Func<bool> afterShow)
    {
        AddAnimation(obj, "CardHideAnimation");
        AddAnimation(obj, "CardShowAnimation");
        CardAnimation cardAnimation = obj.AddComponent<CardAnimation>();
        cardAnimation.Play("CardHideAnimation", () =>
         {
             afterHide();
             cardAnimation.Play("CardShowAnimation", DestroyContainer(afterShow, cardAnimation));
             return true;
         });
    }
    public void PlayCardHideAnimation(GameObject obj, Func<bool> afterHide)
    {
        AddAnimation(obj, "CardHideAnimation");
        CardAnimation cardAnimation = obj.AddComponent<CardAnimation>();
        cardAnimation.Play("CardHideAnimation", DestroyContainer(afterHide, cardAnimation));
    }
    public void PlayCardFromDesk(GameObject obj, Func<bool> after)
    {
        AddAnimation(obj, "CardFromDeskAnimation");
        CardAnimation cardAnimation = obj.AddComponent<CardAnimation>();
        cardAnimation.Play("CardFromDeskAnimation", DestroyContainer(after, cardAnimation));
    }
    public void PlayCardToDesk(GameObject obj, Func<bool> after)
    {
        AddAnimation(obj, "CardToDeskAnimation");
        CardAnimation cardAnimation = obj.AddComponent<CardAnimation>();
        cardAnimation.Play("CardToDeskAnimation", DestroyContainer(after, cardAnimation));
    }
    public void PlayDropCard(GameObject obj, Func<bool> after)
    {
        AddAnimation(obj, "CardToDropAnimation");
        CardAnimation cardAnimation = obj.AddComponent<CardAnimation>();
        cardAnimation.Play("CardToDropAnimation", DestroyContainer(after, cardAnimation));
    }
    private Func<bool> DestroyContainer(Func<bool> func, CardAnimation cardAnimation)
    {
        return () =>
        {
            func();
            Destroy(cardAnimation);
            return true;
        };
    }
}