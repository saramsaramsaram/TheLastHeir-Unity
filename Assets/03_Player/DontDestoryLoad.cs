using Unity.VisualScripting;
using UnityEngine;

public class DontDestory : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
