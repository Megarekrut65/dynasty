using UnityEngine;
using UnityEngine.UI;

public class SelectObjectData<ObjectType> {
	public Outline outline { get; set; }
	public SelectClick selectClick { get; set; }
	public ObjectType obj { get; set; }
	public SelectObjectData(Outline outline, SelectClick selectClick, ObjectType obj) {
		this.outline = outline;
		this.selectClick = selectClick;
		this.obj = obj;
	}
}