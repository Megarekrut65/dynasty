using UnityEngine;

public class Player {
	public string Nickname { get; set; }
	private Desk desk;
	public int Coins { get; private set; } = 0;

	public int Order => desk.Order;

	public Player(string nickname, Desk desk) {
		this.Nickname = nickname;
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
		return (p.Nickname == this.Nickname);
	}
	public override int GetHashCode() {
		return Nickname.GetHashCode();
	}
	public GameObject Label => desk.PlayerLabel;
}