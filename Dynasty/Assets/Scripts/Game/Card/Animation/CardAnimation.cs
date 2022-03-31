using System.Collections;
using System;
using UnityEngine;

public class CardAnimation : MonoBehaviour {
	public void Play(string animationName, Action end) {
		var anim = gameObject.GetComponent<Animation>();
		StartCoroutine(CallEnd(end, animationName, anim));
	}
	private IEnumerator CallEnd(Action end, string animationName, Animation anim) {

		anim.Play(animationName);
		yield return new WaitForSeconds(anim[animationName].length);
		yield return new WaitForSeconds(0.05f);
		anim.Stop(animationName);
		end();
	}
}