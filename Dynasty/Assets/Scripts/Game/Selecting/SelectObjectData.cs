using UnityEngine;
using UnityEngine.UI;

public class SelectObjectData<ObjectType> {
	public Outline outline;
	public SelectClick selectClick;
	public ObjectType obj;
	public Player owner;

	public SelectObjectData(Outline outline, SelectClick selectClick, ObjectType obj, Player owner) {
		this.outline = outline;
		this.selectClick = selectClick;
		this.obj = obj;
		this.owner = owner;
	}
}