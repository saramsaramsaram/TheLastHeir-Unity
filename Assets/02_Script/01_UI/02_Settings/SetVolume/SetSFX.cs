using UnityEngine;
using UnityEngine.UI;
public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private AudioSource _sfxSources;

    void Update()
    {
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        SetSFXVolume(_sfxSlider.value);
    }

    void SetSFXVolume(float volume)
    {
        _sfxSources.volume = volume;
    }
}
