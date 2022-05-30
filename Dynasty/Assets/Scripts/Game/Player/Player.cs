using UnityEngine;

/// <summary>
/// Class with all player data
/// </summary>
public class Player {
    private static int _id = 0;
    private readonly int id;
    public string Nickname { get; set; }
    private Desk desk;
    public int Coins { get; private set; } = 0;
    public int Order => desk.Order;
    public GameObject Label => desk.PlayerLabel;
    public string Key { get; private set; }
    public bool LeftGame { get; set; }
    public Player(string nickname, Desk desk, string key) {
        id = _id++;
        this.Nickname = nickname;
        this.desk = desk;
        Key = key;
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

        Player p = (Player) obj;
        return (p.id == this.id);
    }
    public override int GetHashCode() {
        return id;
    }
}