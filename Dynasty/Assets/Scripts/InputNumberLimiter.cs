using UnityEngine;
using UnityEngine.UI;
using System;

public class InputNumberLimiter : MonoBehaviour {
	[SerializeField]
	private InputField field;
	[Header("Limit")]
	[SerializeField]
	private int min;
	[SerializeField]
	private int max;

	public void Change(String value) {
		field.text = Mathf.Clamp(Convert.ToInt32(value), min, max).ToString();
	}
}