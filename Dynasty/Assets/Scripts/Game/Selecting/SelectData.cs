using System.Collections.Generic;
using UnityEngine;


public class SelectData {
	public List<SelectObjectData<Card>> selectingCards = new List<SelectObjectData<Card>>();
	public string lastType = "";
	public bool toOwner = true;
	public List<SelectObjectData<GameObject>> selectingPlayers = new List<SelectObjectData<GameObject>>();
	public void Clear() {
		selectingCards.Clear();
		selectingPlayers.Clear();
	}
}