using System.Collections.Generic;
using UnityEngine;
using System;

public class Player {
	public string nickname;
	private Desk desk;
	private int coins = 0;
	public Player(string nickname) {
		this.nickname = nickname;
	}
	public void SetDesk(Desk desk) {
		this.desk = desk;
		desk.SetName(nickname);
	}
	public void AddCard(Card card) {
		if (card == null) return;
		if (card.data.type == "A") {
			coins += card.data.amount;
			desk.SetCoins(coins);
		} else desk.AddCard(card);
	}
	public void AddCoins(int coins) {
		this.coins += coins;
		desk.SetCoins(this.coins);
	}
	public Color GetColor() {
		return desk.PlayerColor;
	}
	public override bool Equals(System.Object obj) {
		if ((obj == null) || !this.GetType().Equals(obj.GetType())) {
			return false;
		}
		Player p = (Player)obj;
		return (p.nickname == this.nickname);
	}
	public override int GetHashCode() {
		return nickname.GetHashCode();
	}
}