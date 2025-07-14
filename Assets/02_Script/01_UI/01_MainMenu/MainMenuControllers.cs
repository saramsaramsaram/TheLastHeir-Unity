using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour, IUIPanel
{
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    [Header("Manager Reference")]
    [SerializeField] private UIManager uiManager;

    private void Awake()
    {
        // 버튼 클릭 이벤트
        startButton.onClick.AddListener(OnStartButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnDestroy()
    {
        // 버튼 리스너 제거 -> 메모리 누수 방지 
        startButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
    }

    // 인터페이스
    public void Show() => gameObject.SetActive(true); // 표현 단축
    public void Hide() => gameObject.SetActive(false);

    // Button: Start Game
    private void OnStartButtonClicked()
    {
        Debug.Log("Start Button Clicked!");
        uiManager.ShowPanel<StartSettingsController>(true);
    }

    // Button: Open Settings
    private void OnSettingsButtonClicked()
    {
        uiManager.ShowPanel<GameSettingsController>();
    }

    // Button: Exit Game
    private void OnExitButtonClicked()
    {
        Application.Quit(); // 실제 -> 앱 중단

#if UNITY_EDITOR // 유니티 에디터 일시 플레이 중단
    UnityEditor.EditorApplication.isPlaying = false;    
#endif
    }
}
