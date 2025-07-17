using UnityEngine;
using UnityEngine.UI;

public class SetBGM : MonoBehaviour
{
    [SerializeField] private Slider _bgm;
    [SerializeField] private Menu_BGM _menuBGM;

    void Update()
    {
        if (_bgm != null && _menuBGM != null)
        {
            _menuBGM.SetVolume(_bgm.value);
        }
    }
}
