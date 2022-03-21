
using System;
using UnityEngine;

public class CardAnimationManager : MonoBehaviour {
	[SerializeField]
	private AnimationClip[] clips;

	public void AddAnimation(GameObject obj, string name) {
		if (obj == null) return;
		var clip = Array.Find(clips, (cl) => cl.name == name);
		if (clip != null) {
			var anim = obj.GetComponent<Animation>();
			if (anim == null) anim = obj.AddComponent<Animation>();
			if (anim[name] == null) anim.AddClip(clip, name);
		}
	}
	public void PlayAnimation(GameObject obj, string name, Action afterAnimation) {
		if (obj == null) return;
		AddAnimation(obj, name);
		CardAnimation cardAnimation = obj.AddComponent<CardAnimation>();
		cardAnimation.Play(name, DestroyContainer(afterAnimation, cardAnimation));
	}
	public void PlayCardHideShowAnimation(GameObject obj, Action afterHide, Action afterShow) {
		if (obj == null) return;
		AddAnimation(obj, "CardHideAnimation");
		AddAnimation(obj, "CardShowAnimation");
		CardAnimation cardAnimation = obj.AddComponent<CardAnimation>();
		cardAnimation.Play("CardHideAnimation", () => {
			afterHide();
			cardAnimation.Play("CardShowAnimation", DestroyContainer(afterShow, cardAnimation));
		});
	}
	public void PlayCardHideAnimation(GameObject obj, Action afterHide) {
		if (obj == null) return;
		AddAnimation(obj, "CardDisappearanceAnimation");
		CardAnimation cardAnimation = obj.AddComponent<CardAnimation>();
		cardAnimation.Play("CardDisappearanceAnimation", DestroyContainer(afterHide, cardAnimation));
	}
	public void PlayCardFromDesk(GameObject obj, Action after) {
		if (obj == null) return;
		AddAnimation(obj, "CardFromDeskAnimation");
		CardAnimation cardAnimation = obj.AddComponent<CardAnimation>();
		cardAnimation.Play("CardFromDeskAnimation", DestroyContainer(after, cardAnimation));
	}
	public void PlayCardToDesk(GameObject obj, Action after) {
		if (obj == null) return;
		AddAnimation(obj, "CardToDeskAnimation");
		CardAnimation cardAnimation = obj.AddComponent<CardAnimation>();
		cardAnimation.Play("CardToDeskAnimation", DestroyContainer(after, cardAnimation));
	}
	public void PlayDropCard(GameObject obj, Action after) {
		if (obj == null) return;
		AddAnimation(obj, "CardToDropAnimation");
		CardAnimation cardAnimation = obj.AddComponent<CardAnimation>();
		cardAnimation.Play("CardToDropAnimation", DestroyContainer(after, cardAnimation));
	}
	private Action DestroyContainer(Action func, CardAnimation cardAnimation) {
		return () => {
			func();
			Destroy(cardAnimation);
		};
	}
	public void PlayCoverCard(GameObject obj, Action cover, Action after) {
		if (obj == null) return;
		AddAnimation(obj, "CardRotationHideAnimation");
		AddAnimation(obj, "CardRotationShowAnimation");
		CardAnimation cardAnimation = obj.AddComponent<CardAnimation>();
		cardAnimation.Play("CardRotationHideAnimation",
		() => {
			cover();
			cardAnimation.Play("CardRotationShowAnimation",
				DestroyContainer(after, cardAnimation));
		});
	}
}