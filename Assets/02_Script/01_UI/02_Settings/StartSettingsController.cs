using UnityEngine;

public class StartSettingsController : MonoBehaviour, IUIPanel
{
    public void Show() => gameObject.SetActive(true);

    public void Hide() => gameObject.SetActive(false);
    
}
