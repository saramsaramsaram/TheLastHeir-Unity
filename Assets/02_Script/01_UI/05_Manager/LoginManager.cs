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

    private const string loginUrl = "https://831e612033c4.ngrok-free.app/api/accounts";

    public void OnLoginButtonClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        StartCoroutine(TryLogin(username, password));
    }

    private IEnumerator TryLogin(string username, string password)
    {
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

        if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
        {
            string responseJson = request.downloadHandler.text;
            TokenResponse token = JsonUtility.FromJson<TokenResponse>(responseJson);

            PlayerPrefs.SetString("jwt_token", token.token);
            PlayerPrefs.Save();

            Debug.Log("로그인 성공! 토큰: " + token.token);
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning($"로그인 실패: {request.responseCode} - {request.downloadHandler.text}");
        }
    }
}