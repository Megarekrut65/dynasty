using UnityEngine;

public class EnableBots : SavedHideToggle {
	protected override void Start() {
		key = PrefabsKeys.ENABLE_BOTS;
		base.Start();
	}
}