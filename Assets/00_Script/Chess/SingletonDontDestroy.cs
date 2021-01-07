using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonDontDestroy<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T m_instance = null;

    protected virtual void Init()
    {
        if (null == m_instance)
        {
            Debug.Log($"singletonsDontDestroy:{typeof(T)}");
            m_instance = FindObjectOfType(typeof(T)) as T;
            DontDestroyOnLoad(m_instance);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                Debug.Log($"singletonsDontDestroy:{typeof(T)}");
                m_instance = FindObjectOfType(typeof(T)) as T;
                DontDestroyOnLoad(m_instance);
            }

            return m_instance;

        }
    }
}
