using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverBackground : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject hoverBackground;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverBackground?.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    { 
        hoverBackground?.SetActive(false);
    }
}