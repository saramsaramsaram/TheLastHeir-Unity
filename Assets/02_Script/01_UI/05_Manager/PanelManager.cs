using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [Header("UI 패널 목록")]
    [SerializeField] private List<GameObject> panels = new List<GameObject>();
    
    /// 지정한 패널만 활성화, 나머지 모든 패널은 비활성화
    public void ShowOnly(GameObject target)
    {
        foreach (var panel in panels)
        {
            panel.SetActive(panel == target);
        }
    }
    
    /// 등록된 모든 패널을 비활성화
    public void HideAll()
    {
        foreach (var panel in panels)
        {
            panel.SetActive(false);
        }
    }
    
    /// 특정 패널만 활성화
    public void Show(GameObject panel)
    {
        panel.SetActive(true);
    }
    
    /// 특정 패널만 비활성화
    public void Hide(GameObject panel)
    {
        panel.SetActive(false);
    }
}