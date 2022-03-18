using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour {
	[Header("Camera controller")]
	[SerializeField]
	private CameraMove cameraMove;
	[Header("Scroll View")]
	[SerializeField]
	private GameObject contentObject;
	[SerializeField]
	private GameObject scrollRect;
	[SerializeField]
	private GameObject view;
	public delegate void NextRound();
	public event NextRound Next;
	private bool gameOver;
	public bool GameOver {
		get {
			return gameOver;
		}
		set {
			gameOver = value;
		}
	}
	private int playerCount = 1;
	public int PlayerCount {
		get {
			return playerCount;
		}
	}
	private int botCount = 5;
	public int BotCount {
		get {
			return botCount;
		}
	}
	private List<Player> players;
	public List<Player> Players {
		get {
			return players;
		}
	}
	private string[] nicknames = new string[6];
	private int index = 0;
	private int cardCount = 0;
	private bool pause = false;
	public bool Pause {
		get {
			return pause;
		}
		set {
			pause = value;
		}
	}
	[Header("Big card")]
	[SerializeField]
	private bool makeBig;
	public bool MakeBig {
		get {
			return makeBig;
		}
	}
	private float waitTime;
	public float WaitTime {
		get {
			return waitTime;
		}
	}
	private ScrollManager scrollManager;
	public ScrollManager Scroll {
		get {
			return scrollManager;
		}
	}
	public GameObject CardPlace {
		get {
			return scrollRect;
		}
	}
	public void CameraMoveActive(bool active) {
		cameraMove.Stop = !active;
		view.SetActive(!active);
	}
	public void ChangeBigCard(bool change) {
		makeBig = change;
		if (makeBig) waitTime = 1.6f;
		else waitTime = 0.8f;
	}
	void Start() {
		makeBig = false;
		waitTime = 0.8f;
		scrollManager = new ScrollManager(scrollRect.GetComponent<ScrollRect>(), contentObject);
		view.SetActive(false);
	}
	public void CreatePlayers() {
		players = new List<Player>();
		for (int i = 0; i < nicknames.Length; i++) {
			nicknames[i] = "Player" + UnityEngine.Random.Range(0, 1000);
		}
		nicknames[0] = "You";
		for (int i = 0; i < playerCount + botCount; i++) {
			players.Add(new Player(nicknames[i]));
		}
	}
	public Player NextPlayer() {
		int i;
		if (cardCount > 0) {
			i = index - 1;
			if (i < 0) i = playerCount + botCount - 1;
			cardCount--;
		} else {
			i = index++;
			cardCount = 0;
		}
		Player player = players[i];
		if (index >= playerCount + botCount) index = 0;

		return player;
	}
	public Player GetNextPlayer() {
		int i;
		if (cardCount > 0) {
			i = index - 1;
			if (i < 0) i = playerCount + botCount - 1;
		} else i = index;

		return players[i];
	}
	public void AddCount(int add) {
		cardCount += add;
	}
	public void CallNext() {
		pause = false;
		Next?.Invoke();
	}
	public bool IsPlayer(Player player) {
		int index = players.FindIndex((pl) => pl.Equals(player));
		return (index < playerCount);
	}
	public List<Player> GetEnemies(Player player) {
		return players.Where(p => !p.Equals(player)).ToList();
	}
}