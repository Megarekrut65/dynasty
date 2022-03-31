using System;
using UnityEngine;

public class CardAnimationManager : MonoBehaviour {
	[SerializeField]
	private AnimationClip[] clips;

	private void AddAnimation(GameObject obj, string name) {
		if (obj == null) return;
		var clip = Array.Find(clips, cl => cl.name == name);
		if (clip != null) {
			var anim = obj.GetComponent<Animation>();
			if (anim == null) anim = obj.AddComponent<Animation>();
			if (anim[name] == null) anim.AddClip(clip, name);
		}
	}
	private void PlayAnimation(GameObject obj, string name, Action afterAnimation) {
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
	public void PlayPulsationCardAnimation(GameObject obj, Action after) {
		PlayAnimation(obj, "CardPulsationAnimation", after);
	}
	public void PlayCountAmountAnimation(GameObject obj, Action after) {
		PlayAnimation(obj, "CardAmountAnimation", after);
	}
	public void PlayCardHideAnimation(GameObject obj, Action after) {
		PlayAnimation(obj, "CardDisappearanceAnimation", after);
	}
	public void PlayCardFromDeskAnimation(GameObject obj, Action after) {
		PlayAnimation(obj, "CardFromDeskAnimation", after);
	}
	public void PlayCardToDeskAnimation(GameObject obj, Action after) {
		PlayAnimation(obj, "CardToDeskAnimation", after);
	}
	public void PlayDropCardAnimation(GameObject obj, Action after) {
		PlayAnimation(obj, "CardToDropAnimation", after);
	}
	private Action DestroyContainer(Action func, CardAnimation cardAnimation) {
		return () => {
			func();
			Destroy(cardAnimation);
		};
	}
	public void PlayCoverCardAnimation(GameObject obj, Action cover, Action after) {
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