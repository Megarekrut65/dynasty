
using System;

public class HideRoomInfo : SavedHideToggle {
      protected override void Start() {
            if (GameModeFunctions.IsMode(GameMode.OFFLINE)) {
                  obj.SetActive(false);
                  gameObject.SetActive(false);
                  return;
            }
            key = LocalStorage.HIDE_ROOM_INFO;
            base.Start();
      }
}