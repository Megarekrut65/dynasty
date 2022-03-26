using UnityEngine;
using System.Collections.Generic;

public class RoundManager {
	public delegate void NextRound();
	public event NextRound Next;

	private int index = 0;
	private int rounds = 0;
	private bool pause = false;
	public bool Pause {
		get {
			return pause;
		}
		set {
			pause = value;
		}
	}
	private List<Player> players;

	public RoundManager(List<Player> players) {
		this.players = players;
	}
	public void AddMoreRounds(int moreRounds) {
		rounds += moreRounds;
	}
	public Player GetTheNextPlayer() {
		int i;
		if (rounds > 0) {
			i = index - 1;
			if (i < 0) i = players.Count - 1;
			rounds--;
		} else {
			i = index++;
			rounds = 0;
		}
		Player player = players[i];
		if (index >= players.Count) index = 0;

		return player;
	}
	public Player WhoIsNextPlayer() {
		int i;
		if (rounds > 0) {
			i = index - 1;
			if (i < 0) i = players.Count - 1;
		} else i = index;

		return players[i];
	}
	public void CallNextPlayer() {
		pause = false;
		Next?.Invoke();
	}
}