using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("All UIPanels")]
    [SerializeField] private List<MonoBehaviour> panelScripts = new List<MonoBehaviour>();

    private List<IUIPanel> panels = new List<IUIPanel>();

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
}