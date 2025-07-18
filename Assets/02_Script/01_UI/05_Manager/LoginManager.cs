using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;
using System.Text;
using System.Collections;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public string nextSceneName;

    private const string loginUrl = "https://831e612033c4.ngrok-free.app/api/accounts/login";

    public void OnLoginButtonClicked()
    {
        TMP_InputField usernameInput = GameObject.Find("InputID").GetComponent<TMP_InputField>();
        TMP_InputField passwordInput = GameObject.Find("InputPW").GetComponent<TMP_InputField>();

        if (usernameInput == null || passwordInput == null)
        {
            Debug.LogError("InputField를 찾을 수 없음");
            return;
        }

        string username = usernameInput.text;
        string password = passwordInput.text;
        
        Debug.Log($"로그인 정보: {username} / {password}");

        StartCoroutine(TryLogin(username, password));
    }

    private IEnumerator TryLogin(string username, string password)
    {
        Debug.Log("API 요청 시작");
        Debug.Log("로그인 시도" + username + " / " + password);
        LoginRequest requestData = new LoginRequest
        {
            username = username,
            password = password
        };

        string json = JsonUtility.ToJson(requestData);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(loginUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        yield return request.SendWebRequest();
        Debug.Log("API 요청 끝, 응답: " + request.downloadHandler.text);

        if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
        {
            string responseJson = request.downloadHandler.text;
            Debug.Log("API 응답: " + responseJson);
            TokenResponse token = JsonUtility.FromJson<TokenResponse>(responseJson);

            PlayerPrefs.SetString("jwt_token", token.token);
            PlayerPrefs.Save();

            Debug.Log("로그인 성공! 토큰: " + token.token);
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning($"로그인 실패: {request.responseCode} - {request.downloadHandler.text} / {request.error}");
        }
    }
}