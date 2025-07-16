using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.Prepare();
        videoPlayer.loopPointReached += OnVideoEnd;
        StartCoroutine(PlayWhenReady());
    }

    private System.Collections.IEnumerator PlayWhenReady()
    {
        while (!videoPlayer.isPrepared)
        {
            yield return null; // 영상 준비될 때까지 기다림
        }

        videoPlayer.Play();
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("StartMenu");
    }
}