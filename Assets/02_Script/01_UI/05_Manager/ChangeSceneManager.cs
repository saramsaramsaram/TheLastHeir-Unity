using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public string nextSceneName;

    // 이 함수는 버튼에 연결하세요.
    public void OnLoginButtonClicked()
    {
        // 로그인 검증 로직 (임시로 항상 성공)
        bool loginSuccess = true; // 서버통신으로 변경 예정
        
        if (loginSuccess)
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.Log("로그인 실패!");
        }
    }
}