using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("All UIPanels")]
    [SerializeField] private List<MonoBehaviour> panelScripts = new List<MonoBehaviour>();

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
        foreach (var panel in panels)
        {
            Debug.Log($"[UIManager] panel type: {panel.GetType().Name}");

            if (panel is T)
            {
                Debug.Log($"[UIManager] --> Matched: {typeof(T)} == {panel.GetType().Name}");
                // 현재 패널이랑 같지 않을 때만 스택에 넣기
                if (panelStack.Count == 0 || panelStack.Peek() == panel)
                {
                    if (panelStack.Count > 0)
                    {
                        panelStack.Peek().Hide();
                    }

                    panelStack.Push(panel); // 스택에 새 패널 추가
                }

                panel.Show();
            }
            else
            {
                Debug.Log($"[UIManager] --> Not matched: {panel.GetType().Name}");
                panel.Hide();
            }
        }

    }
    
    /// 모든 패널 비활성화
    public void HideAllPanels()
    {
        foreach (var panel in panels)
        {
            panel.Hide();
        }
    }
    
    public void Back() // 기능 추가
    {
        if (panelStack.Count > 0)
        {
            // 현재 패널 숨기기
            var current = panelStack.Pop();
            current.Hide();

            // 이전 패널 보여주기
            if (panelStack.Count > 0)
            {
                var previous = panelStack.Peek();
                previous.Show();
            }
        }
    }

}