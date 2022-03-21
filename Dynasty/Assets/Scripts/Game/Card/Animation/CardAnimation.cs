using System.Collections;
using System;
using UnityEngine;

public class CardAnimation : MonoBehaviour {
	public void Play(string name, Action end) {
		var animation = gameObject.GetComponent<Animation>();
		StartCoroutine(CallEnd(end, name, animation));
	}
	IEnumerator CallEnd(Action end, string name, Animation animation) {

		animation.Play(name);
		yield return new WaitForSeconds(animation[name].length);
		yield return new WaitForSeconds(0.05f);
		animation.Stop(name);
		end();
	}
}