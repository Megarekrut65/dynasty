using UnityEngine.UI;

public class SelectObjectData<TObjectType> {
    public Outline outline;
    public CardClick cardClick;
    public TObjectType obj;
    public Player owner;
    public int Id { get; }

    public SelectObjectData(Outline outline, CardClick cardClick, int id, TObjectType obj, Player owner) {
        this.outline = outline;
        this.cardClick = cardClick;
        this.Id = id;
        this.obj = obj;
        this.owner = owner;
    }
}