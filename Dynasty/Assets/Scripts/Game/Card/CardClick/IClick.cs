using UnityEngine.EventSystems;

public interface IClick {
    public void Down(PointerEventData eventData);
    public void Up(PointerEventData eventData);
}