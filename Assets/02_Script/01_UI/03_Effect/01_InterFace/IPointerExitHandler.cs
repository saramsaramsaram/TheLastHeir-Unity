using UnityEngine;
using UnityEngine.EventSystems;

public interface IPointerExitHandler
{
    void OnPointerExit(PointerEventData eventData);
}