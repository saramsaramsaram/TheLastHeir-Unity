using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

/// <summary>
/// 실제 서버와 HTTP 통신을 담당하는 구현체
/// </summary>
public class HttpServerClient : MonoBehaviour, IServerClient
{
    // 서버의 기본 URL (실제 서버 주소로 변경할 것!)
    private const string BASE_URL = "https://example.com/api";

    /// <summary>
    /// 새 게임 생성 요청을 보냄 (외부에서 호출되는 함수)
    /// </summary>
    public void SendNewGameRequest(string nickname)
    {
        // HTTP 요청은 코루틴으로 비동기 처리
        StartCoroutine(SendNewGameCoroutine(nickname));
    }

    /// <summary>
    /// 게임 불러오기 요청을 보냄 (외부에서 호출되는 함수)
    /// </summary>
    public void SendLoadGameRequest(string nickname)
    {
        StartCoroutine(SendLoadGameCoroutine(nickname));
    }

    /// <summary>
    /// [내부용] 새 게임 생성 요청을 서버에 보냄
    /// </summary>
    private IEnumerator SendNewGameCoroutine(string nickname)
    {
        // 요청 보낼 URL (POST로 전송)
        string url = $"{BASE_URL}/newgame";

        // 요청에 보낼 데이터를 form 형태로 구성
        WWWForm form = new WWWForm();
        form.AddField("nickname", nickname); // 키: nickname, 값: 입력된 닉네임

        // POST 요청 생성
        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            // 서버 응답 기다림 (비동기)
            yield return request.SendWebRequest();

            // Unity 버전에 따라 에러 체크 방식 다름
#if UNITY_2020_1_OR_NEWER
            if (request.result == UnityWebRequest.Result.Success)
#else
            if (!request.isNetworkError && !request.isHttpError)
#endif
            {
                // 요청 성공
                Debug.Log("[ Success ] 새 게임 생성 성공");
            }
            else
            {
                // 요청 실패
                Debug.LogError($"[ Fail ] 새 게임 생성 실패: {request.error}");
            }
        }
    }

    /// <summary>
    /// [내부용] 저장된 게임 불러오기 요청
    /// </summary>
    private IEnumerator SendLoadGameCoroutine(string nickname)
    {
        // GET 요청은 URL에 직접 파라미터 포함
        string url = $"{BASE_URL}/loadgame?nickname={UnityWebRequest.EscapeURL(nickname)}";

        // GET 요청 생성
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (request.result == UnityWebRequest.Result.Success)
#else
            if (!request.isNetworkError && !request.isHttpError)
#endif
            {
                // 요청 성공 - 결과 출력
                Debug.Log($"[ Success ] 게임 불러오기 성공: {request.downloadHandler.text}");

                // 여기서 JSON 파싱해서 데이터를 게임에 반영 가능
            }
            else
            {
                // 요청 실패
                Debug.LogError($"[ Fail ] 게임 불러오기 실패: {request.error}");
            }
        }
    }
}
