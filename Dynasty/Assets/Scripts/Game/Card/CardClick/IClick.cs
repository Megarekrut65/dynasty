using UnityEngine.EventSystems;

public interface IClick {
    public bool Down(PointerEventData eventData);
    public bool Up(PointerEventData eventData);
}