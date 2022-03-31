using UnityEngine.UI;

public class SelectObjectData<TObjectType> {
	public Outline outline;
	public SelectClick selectClick;
	public TObjectType obj;
	public Player owner;

	public SelectObjectData(Outline outline, SelectClick selectClick, TObjectType obj, Player owner) {
		this.outline = outline;
		this.selectClick = selectClick;
		this.obj = obj;
		this.owner = owner;
	}
}