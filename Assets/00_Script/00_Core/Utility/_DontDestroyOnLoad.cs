using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _DontDestroyOnLoad : MonoBehaviour
{      
    void Awake() 
    {
        DontDestroyOnLoad(gameObject);
    }
}
