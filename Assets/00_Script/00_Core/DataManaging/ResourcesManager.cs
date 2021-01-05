using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Prefab resource만 로드
public static class ResourcesManager
{
    public static Dictionary<T1, GameObject> LoadPrefabs<T1>(string folderName) where T1 : Enum
    {
        Dictionary<T1, GameObject> ret = new Dictionary<T1, GameObject>();
        GameObject[] prefabs = Resources.LoadAll<GameObject>(folderName);

        foreach (T1 item in Enum.GetValues(typeof(T1)))
        {
            if (item.ToString() != "Max")
            {
                ret[item] = prefabs[(int)(Enum.Parse(typeof(T1), item.ToString()))];
            }
        }

        return ret;
    }

    public static Dictionary<T,AudioClip> LoadAudios<T>(string foldername) where T : Enum
    {
        Dictionary<T, AudioClip> ret = new Dictionary<T, AudioClip>();
        AudioClip[] clips = Resources.LoadAll<AudioClip>(foldername);

        foreach (T item in Enum.GetValues(typeof(T)))
        {
            if (item.ToString() != "Max")
            {
                ret[item] = clips[(int)(Enum.Parse(typeof(T), item.ToString()))];
            }
        }

        return ret;
    }   
}
