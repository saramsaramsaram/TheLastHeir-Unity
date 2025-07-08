using UnityEngine;

public class GameSettingsController : MonoBehaviour, IUIPanel
{
    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}
