using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverBackground : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject hoverBackground;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverBackground != null)
            hoverBackground.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverBackground != null)
            hoverBackground.SetActive(false);
    }
}