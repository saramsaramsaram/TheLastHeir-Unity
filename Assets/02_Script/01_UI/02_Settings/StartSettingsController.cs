using UnityEngine;
using UnityEngine.UI;

public class StartSettingsController : MonoBehaviour, IUIPanel
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private UIManager uiManager;

    private void Awake()
    {
        newGameButton.onClick.AddListener(OnNewGameClicked);
    }

    private void OnDestroy()
    {
        newGameButton.onClick.RemoveListener(OnNewGameClicked);
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    private void OnNewGameClicked()
    {
        uiManager.ShowPanel<LoginPanel>(); // 원하는 패널로 전환
    }
}