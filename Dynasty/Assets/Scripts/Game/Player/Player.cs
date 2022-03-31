using UnityEngine;

public class Player {
	public readonly string nickname;
	private Desk desk;
	public int Coins { get; private set; } = 0;

	public int Order => desk.Order;

	public Player(string nickname, Desk desk) {
		this.nickname = nickname;
		this.desk = desk;
		desk.SetName(nickname);
	}
	public void AddCard(Card card) {
		if (card == null) return;
		if (card.data.type == "A") {
			Coins += card.data.amount;
			desk.SetCoins(Coins);
		} else desk.AddCard(card);
	}
	public void AddCoins(int coins) {
		this.Coins += coins;
		desk.SetCoins(this.Coins);
	}
	public Color GetColor() {
		return desk.PlayerColor;
	}
	public override bool Equals(object obj) {
		if ((obj == null) || this.GetType() != obj.GetType()) {
			return false;
		}
		Player p = (Player)obj;
		return (p.nickname == this.nickname);
	}
	public override int GetHashCode() {
		return nickname.GetHashCode();
	}
	public GameObject Label => desk.PlayerLabel;
}