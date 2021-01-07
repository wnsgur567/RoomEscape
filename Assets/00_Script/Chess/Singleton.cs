using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//출처: https://hyunity3d.tistory.com/627 [Unity3D]
public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    protected static T m_instance = null;

    protected virtual void Init()
    {
        if (null == m_instance)
        {
            Debug.Log($"singleton:{typeof(T)}");
            m_instance = FindObjectOfType(typeof(T)) as T;
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
                Debug.Log($"singleton:{typeof(T)}");
                m_instance = FindObjectOfType(typeof(T)) as T;
            }

            return m_instance;

        }
    }
}
