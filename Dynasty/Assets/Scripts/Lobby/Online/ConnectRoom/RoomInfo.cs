public class RoomInfo {
    public int playerCount = 0;
    public int currentCount = 0;
    public string date = "";
    public bool keepPrivate = false;
    public int deskSeed = 0;
    public RoomInfo() {
    }
    public RoomInfo(int playerCount, int currentCount, string date, bool keepPrivate, int deskSeed) {
        this.playerCount = playerCount;
        this.currentCount = currentCount;
        this.date = date;
        this.keepPrivate = keepPrivate;
        this.deskSeed = deskSeed;
    }
}