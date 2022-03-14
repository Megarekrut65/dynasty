using UnityEngine;
using UnityEngine.UI;

public class SelectData {
	public Outline outline { get; set; }
	public SelectClick selectClick { get; set; }
	public Card card { get; set; }
	public SelectData(Outline outline, SelectClick selectClick, Card card) {
		this.outline = outline;
		this.selectClick = selectClick;
		this.card = card;
	}
}