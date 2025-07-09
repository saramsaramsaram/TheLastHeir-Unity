using UnityEngine;

public class Menu_BGM : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _BGM;

    void Start()
    {
        _audioSource.clip = _BGM;
        _audioSource.loop = true;
        _audioSource.Play();
    }
}
