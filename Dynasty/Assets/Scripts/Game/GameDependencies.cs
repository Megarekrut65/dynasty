using System;

public class GameDependencies {
	public BigCardManager bigCardManager { get; set; }
	public ScrollManager scrollManager { get; set; }
	public PlayerManager playerManager { get; set; }
	public RoundManager roundManager { get; set; }
	public CameraMove cameraMove { get; set; }
	public GameLogger logger { get; set; }
}