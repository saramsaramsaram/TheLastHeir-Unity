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

    // UI를 페이드 인 (보이게)
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


    
    // UI를 페이드 아웃 (안보이게)
    public void FadeOut(Action onComplete = null)
    {
        if (canvasGroup == null)
        {
            Debug.LogWarning($"{gameObject.name}의 canvasGroup이 null이어서 FadeOut 못함");
            onComplete?.Invoke();
            return;
        }

        // 기존 트윈이 있으면 중지
        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();

        // DOTween으로 alpha값 0까지 페이드
        currentTween = canvasGroup.DOFade(0f, duration).OnComplete(() =>
        {
            Debug.Log($"UIFadeController: FadeOut 완료 콜백 호출 - {gameObject.name}");
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }


}