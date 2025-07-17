using UnityEngine;
using UnityEngine.UI;

public class SetBGM : MonoBehaviour
{
    [SerializeField] private Slider _bgm;
    [SerializeField] private Menu_BGM _menuBGM;

    void Start()
    {
        if (_bgm != null && _menuBGM != null)
        {
            _bgm.onValueChanged.AddListener(_menuBGM.SetVolume);
            _menuBGM.SetVolume(_bgm.value); // 볼륨 설정
        }
    }
}
