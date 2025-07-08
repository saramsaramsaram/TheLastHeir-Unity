using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("All UIPanels")]
    [SerializeField] private List<MonoBehaviour> panelScripts = new List<MonoBehaviour>();

    private bool isTransitioning = false;
    
    private List<IUIPanel> panels = new List<IUIPanel>();
    private Stack<IUIPanel> panelStack = new Stack<IUIPanel>(); // Stack 기능 추가 -- 뒤로가기

    private void Awake()
    {
        Debug.Log("[UIManager] Awake called");

        foreach (var panel in panelScripts)
        {
            Debug.Log($"[UIManager] panelScripts item: {panel.name}");

            if (panel is IUIPanel uiPanel)
            {
                Debug.Log($"[UIManager] -> IUIPanel detected: {panel.GetType().Name}");
                panels.Add(uiPanel);
            }
            else
            {
                Debug.LogWarning($"{panel.name} does not implement IUIPanel.");
            }
        }
    }

    
    /// 지정된 패털 표시, 다른 패널 비활성화
    public void ShowPanel<T>() where T : IUIPanel
{
    if (isTransitioning)
    {
        Debug.Log("[UIManager] Transition in progress. ShowPanel ignored.");
        return; // 전환 중이면 무시
    }


    // 현재 보여야 할 패널 찾기
    IUIPanel targetPanel = null;
    foreach(var panel in panels)
    {
        if (panel is T)
        {
            targetPanel = panel;
            break;
        }
    }

    if (targetPanel == null)
    {
        Debug.LogWarning($"[UIManager] 패널 {typeof(T).Name}을 찾을 수 없음");
        return;
    }

    if (panelStack.Count > 0 && panelStack.Peek() == targetPanel)
    {
        Debug.Log($"[UIManager] 이미 최상단 패널 {typeof(T).Name} 입니다. FadeIn 시도.");
        var fade = ((MonoBehaviour)targetPanel).GetComponent<UIFadeController>();
        if (fade != null) fade.FadeIn(() => Debug.Log("[UIManager] FadeIn 완료"));
        else
        {
            targetPanel.Show();
            Debug.Log("[UIManager" + "] Show() 호출 완료");
        }
        return;
    }

    isTransitioning = true;
    Debug.Log($"[UIManager] ShowPanel 시작 - 이전 패널 FadeOut 시도");

    // 이전 패널 FadeOut
    if (panelStack.Count > 0)
    {
        var currentPanel = panelStack.Peek();
        var currentFade = ((MonoBehaviour)currentPanel).GetComponent<UIFadeController>();

        Action afterFadeOut = () =>
        {
            Debug.Log("[UIManager] 이전 패널 FadeOut 완료");
            panelStack.Pop();
            panelStack.Push(targetPanel);

            var targetGO = ((MonoBehaviour)targetPanel).gameObject;
            targetGO.SetActive(true);
            Debug.Log("[UIManager] 새 패널 활성화");

            var targetFade = targetGO.GetComponent<UIFadeController>();
            if (targetFade != null)
            {
                targetFade.FadeIn();
                Debug.Log("[UIManager] 새 패널 FadeIn 완료");
                isTransitioning = false;
            }
            else
            {
                targetPanel.Show();
                isTransitioning = false;
            }
        };

        if (currentFade != null)
            currentFade.FadeOut(afterFadeOut);
        else
        {
            Debug.Log("[UIManager] 이전 패널 Hide() 호출 완료");
            currentPanel.Hide();
            afterFadeOut();
        }
    }
    else
    {
        panelStack.Push(targetPanel);
        var targetGO = ((MonoBehaviour)targetPanel).gameObject;
        targetGO.SetActive(true);
        Debug.Log("[UIManager] 첫 패널 활성화");

        var targetFade = targetGO.GetComponent<UIFadeController>();
        if (targetFade != null)
        {
            targetFade.FadeIn(() =>
            {
                Debug.Log("[UIManager] 첫 패널 FadeIn 완료");
                isTransitioning = false;
            });
        }
        else
        {
            targetPanel.Show();
            Debug.Log("[UIManager] 첫 패널 Show() 호출 완료");
            isTransitioning = false;
        }
    }
}


    
    /// 모든 패널 비활성화
    public void HideAllPanels()
    {
        foreach (var panel in panels)
        {
            var fade = ((MonoBehaviour)panel).GetComponent<UIFadeController>();
            if (fade != null) fade.FadeOut();
            else panel.Hide();
        }
    }

    
    public void Back()
    {
        if (isTransitioning)
            return;

        if (panelStack.Count == 0)
            return;

        isTransitioning = true;

        var current = panelStack.Pop();
        var currentFade = ((MonoBehaviour)current).GetComponent<UIFadeController>();

        Action showPrevious = () =>
        {
            if (panelStack.Count == 0)
            {
                isTransitioning = false;
                return;
            }

            var previous = panelStack.Peek();
            var previousFade = ((MonoBehaviour)previous).GetComponent<UIFadeController>();

            if (previousFade != null)
                previousFade.FadeIn(() => { isTransitioning = false; });
            else
            {
                previous.Show();
                isTransitioning = false;
            }
        };

        if (currentFade != null)
            currentFade.FadeOut(showPrevious);
        else
        {
            current.Hide();
            showPrevious();
        }
    }



}