using UnityEngine;

public class LoginPanel : MonoBehaviour, IUIPanel
{
    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}
