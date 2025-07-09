using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IButtonSound, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip enterSound;
    [SerializeField] private AudioClip exitSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.PlayOneShot(enterSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        audioSource.PlayOneShot(exitSound);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        audioSource.PlayOneShot(clickSound);
    }
}
