using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SingletonPunCallback<T> : MonoBehaviourPunCallbacks where T : MonoBehaviourPunCallbacks
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj;
                obj = GameObject.Find(typeof(T).Name);
                if (obj == null)
                {
                    obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                }
                else
                {
                    instance = obj.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    virtual protected void Awake()
    {
        this.transform.position = new Vector3(-1000, -1000, -1000);
    }
}
