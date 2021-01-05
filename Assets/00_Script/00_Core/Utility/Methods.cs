using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Methods
{
    private static System.DateTime timer;
    public static void CheckTimeIN()
    {
        timer = System.DateTime.Now;
    }
    public static System.TimeSpan CheckTimeStop()
    {
        System.TimeSpan time = System.DateTime.Now - timer;
        return time;
    }

    public static Transform[] GetChilderen(this Transform tf, string name)
    {
        int count = tf.childCount;

        List<Transform> ret_list = new List<Transform>();
        if (tf.name == name)
        {
            ret_list.Add(tf);
        }
        else if (count == 0)
            return null;

        for (int i = 0; i < count; i++)
        {
            Transform[] arr = tf.GetChild(i).GetChilderen(name);
            if (arr != null)
                ret_list.AddRange(arr);
        }

        return ret_list.ToArray();
    }

    public static Transform GetChildObjFind(this Transform _transform, string p_name)
    {
        int count = _transform.childCount;

        Transform temp_child_transforms;

        for (int i = 0; i < count; i++)
        {

            temp_child_transforms = _transform.GetChild(i);
            if (temp_child_transforms.name == p_name)
            {
                return temp_child_transforms;
            }
            else if(temp_child_transforms.childCount > 0)
            {
                temp_child_transforms = GetChildObjFind(temp_child_transforms, p_name);
                if (temp_child_transforms != null)
                {
                    return temp_child_transforms;
                }
            }
        }
        return null;
    }

    //enum 개수 만큼
    public static Dictionary<T, MemoryPool> MakeMemoryPool<T>(Dictionary<T, GameObject> origin, GameObject _poolmanager, int makeSize) where T : Enum
    {        
        Dictionary<T, MemoryPool> ret = new Dictionary<T, MemoryPool>();

        // 종류별로 makeSize만큼 생성
        foreach (var item in origin)
        {
            // 부모 오브젝트 생성
            GameObject parent_obj = new GameObject();
            // 부모 오브젝트 기본 setting
            parent_obj.transform.parent = _poolmanager.transform;
            parent_obj.name = item.Value.name + "_parent";

            // 풀 생성 및 딕셔너리에 추가
            MemoryPool itemPool = new MemoryPool(item.Value, makeSize, parent_obj.transform);
            ret[item.Key] = itemPool;
        }

        return ret;
    }

    public static string CombinePath(params string[] path)
    {
        string ret = "";
        int Length = path.Length;

        for (int i = 0; i < Length - 1; i++)
        {
            ret = ret + path[i] + '/';
        }
        ret = ret + path[Length - 1];

        return ret;
    }
}

public static class String_Extension
{
    public static string[] mySplit(this string str, char seperator)
    {
        int length = str.Length;
        int seperator_pos = -1;
        for (int i = 0; i < length; i++)
        {
            if (str[i] == seperator)
            {
                seperator_pos = i;
                break;
            }
        }

        if (seperator_pos == -1)
            return null;

        string[] ret = new string[2];
        ret[0] = str.Substring(0, seperator_pos);
        ret[1] = str.Substring(seperator_pos + 1, length - seperator_pos - 1);

        return ret;
    }
}