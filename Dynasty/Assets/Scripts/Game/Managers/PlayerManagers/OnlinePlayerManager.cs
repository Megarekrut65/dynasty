public class OnlinePlayerManager: PlayerManager{
    public Player Current { private get; set; } = null;

    public OnlinePlayerManager(Desk[] desks, int playerCount)
        :base(desks,playerCount, playerCount, ControllerFactory.ONLINE) {
    }
    public override bool IsPlayer(Player player) {
        return player.Equals(Current);
    }
    public override int GetEntityCount() {
        return playersCount;
    }
}