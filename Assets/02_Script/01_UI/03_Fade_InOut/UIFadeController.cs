using DG.Tweening;
using UnityEngine;
using System;  

[RequireComponent(typeof(CanvasGroup))]
public class UIFadeController : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Tween currentTween;
    private float duration = 0.5f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            Debug.LogError($"CanvasGroup missing on {gameObject.name}");
    }

    public void FadeIn(Action onComplete = null)
    {
        if (canvasGroup == null)
        {
            Debug.LogWarning("FadeIn 실패: CanvasGroup 없음");
            onComplete?.Invoke();
            return;
        }

        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();

        gameObject.SetActive(true);
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        currentTween = canvasGroup.DOFade(1f, duration).OnComplete(() =>
        {
            Debug.Log("UIFadeController: FadeIn 완료 콜백 호출");
            onComplete?.Invoke();
        });
    }



    public void FadeOut(Action onComplete = null)
    {
        if (canvasGroup == null)
        {
            Debug.LogWarning($"{gameObject.name}의 canvasGroup이 null이어서 FadeOut 못함");
            onComplete?.Invoke();
            return;
        }

        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();

        currentTween = canvasGroup.DOFade(0f, duration).OnComplete(() =>
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }

}