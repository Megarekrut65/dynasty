
using System;

public class HideRoomInfo : SavedHideToggle {
      protected override void Start() {
            if (PrefabsKeys.GetValue(PrefabsKeys.GAME_MODE, "offline") == "offline") {
                  obj.SetActive(false);
                  gameObject.SetActive(false);
                  return;
            }
            key = PrefabsKeys.HIDE_ROOM_INFO;
            base.Start();
      }
}