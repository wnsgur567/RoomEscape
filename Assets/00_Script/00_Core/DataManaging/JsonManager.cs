using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// Json 입출력용 struct class guide
/*
 
    1. ISerializationCallbackReceiver  인터페이스는 클래스에서만 사용 (구조체 x, API 가 그러라고 함.... )
        ps. 실제 구조체에 선언해보면 암 (Editor 가 멈추는 마술)
    2. [Serializable] 로 직렬화된 객체가 유니티에서 직렬화 된 객체로 사용하는 경우
        Editor에서 지속적으로 호출 될 수 있음 (Inspector 에서 표기된다는 것은 메모리가 할당 된다는 것)
    3. 2번의 이유로 ISerializationCallbackReceiver 인터페이스 내부에서 무거운 객체를 할당하는 것은 바람직 하지 않음
 
 */

[System.Serializable]
public class Serialization<T>
{
    public List<T> target;
    public Serialization(List<T> _target) => target = _target;
}

[System.Serializable]
public class Serialization_1DArray<T>
{
    public T[] array;
    public Serialization_1DArray(T[] _array) => array = _array;
}

// 콜백 인터페이스는 클래스에서만 작동합니다. 구조체에서는 작동하지 않습니다.
[System.Serializable]
public class Serialization_2DArray<T> : ISerializationCallbackReceiver
{
    [SerializeField]
    Serialization_1DArray<T>[] arrayList;

    public T[][] array;

    public Serialization_2DArray(T[][] _array)
    {
        array = _array;
    }

    public void OnBeforeSerialize()
    {
        int length = array.Length;
        arrayList = new Serialization_1DArray<T>[length];
        for (int i = 0; i < length; i++)
        {
            arrayList[i] = new Serialization_1DArray<T>(array[i]);
        }
    }

    public void OnAfterDeserialize()
    {
        int length = arrayList.Length;
        array = new T[length][];
        for (int i = 0; i < length; i++)
        {
            array[i] = arrayList[i].array;
        }
    }
}

[System.Serializable]
public class Serialization<TKey, TValue> : ISerializationCallbackReceiver where TKey: System.Enum
{
    [SerializeField]
    List<TKey> keys;
    [SerializeField]
    List<TValue> values;

    public Dictionary<TKey, TValue> target;
    public Serialization(Dictionary<TKey, TValue> _target) => target = _target;

    public void OnBeforeSerialize()
    {   // 직렬화 전
        keys = new List<TKey>();
        values = new List<TValue>();
        foreach (var item in target)
        {
            keys.Add(item.Key);
            values.Add(item.Value);
        }
    }
    public void OnAfterDeserialize()
    {   // 역직렬화 후
        var count = Mathf.Min(keys.Count, values.Count);
        target = new Dictionary<TKey, TValue>(count);
        for (var i = 0; i < count; i++)
        {
            target.Add(keys[i], values[i]);
        }
    }
}

public static class JsonManager 
{    
    // Persitent 기준으로 Path 생성 및 반환
    public static string GetPersistentPath(string p_directoryPath)
    {
        string persistentPath;
        string resultPath;        // persistent + directory + filename         

        persistentPath = Application.persistentDataPath;
        resultPath = Methods.CombinePath(persistentPath, p_directoryPath);        

        return resultPath;
    }
    public static string GetFilePath(string p_directoryPath, string p_filename)
    {
        string m_persistentPath;
        string m_resultPath;        // persistent + directory + filename         

        m_persistentPath = Application.persistentDataPath;
        m_resultPath = Methods.CombinePath(m_persistentPath, p_directoryPath, p_filename);

        return m_resultPath;
    }

    // 해당 경로의 디렉토리 준비
    // 없는 경우 디렉토리 생성
    public static void PrepareDirectory(string p_directoryPath)
    {
        // 해당 경로에 디렉토리가 없는 경우
        if (Directory.Exists(p_directoryPath) == false)
        {   // 디렉토리 생성
            Directory.CreateDirectory(p_directoryPath);
        }        
    }

    // 해당 경로의 파일 준비
    // 없는 경우 파일 생성
    public static void PrepareFile(string p_filePath)
    {
        if(File.Exists(p_filePath) == false)
        {
            File.Create(p_filePath);
        }        
    }


    /// Save

    // 기본 데이터 형식 or Serializable class 를 Json으로 저장
    public static void Save<T>(string p_filepath, T p_data)
    {
        string jsonData = JsonUtility.ToJson(p_data,true);
        File.WriteAllText(p_filepath, jsonData);
    }

    // List를 직렬화 시켜 Json으로 저장
    public static void Save<T>(string p_filepath, List<T> p_listData)
    {        
        string jsonData = JsonUtility.ToJson(new Serialization<T>(p_listData), true);
        File.WriteAllText(p_filepath, jsonData);
    }

    public static void Save<TKey, TValue>(string p_filepath, Dictionary<TKey, TValue> p_DicData) where TKey : System.Enum
    {
        string jsonData = JsonUtility.ToJson(new Serialization<TKey, TValue>(p_DicData), true);
        File.WriteAllText(p_filepath, jsonData);
    }
    public static void Save<T>(string p_filepath, T[] p_array1D)
    {
        string jsonData = JsonUtility.ToJson(new Serialization_1DArray<T>(p_array1D), true);
        Debug.Log(jsonData);
        File.WriteAllText(p_filepath, jsonData);
    }
    public static void Save<T>(string p_filepath, T[][] p_array2D)
    {
        string jsonData = JsonUtility.ToJson(new Serialization_2DArray<T>(p_array2D), true);
        Debug.Log(jsonData);
        File.WriteAllText(p_filepath, jsonData);
    }


    /// Load   


    public static void Load<T>(string p_filepath, out T __outData)
    {
        string jsonData = File.ReadAllText(p_filepath);
        __outData = JsonUtility.FromJson<T>(jsonData);
    }    
    public static void Load<T>(string p_filepath,out List<T> __outListData)
    {
        string jsonData = File.ReadAllText(p_filepath);
        __outListData = JsonUtility.FromJson<Serialization<T>>(jsonData).target;
    }
    public static void Load<TKey, TValue>(string p_filepath, out Dictionary<TKey, TValue> __outDicData) where TKey : System.Enum
    {
        string jsonData = File.ReadAllText(p_filepath);
        __outDicData = JsonUtility.FromJson<Serialization<TKey, TValue>>(jsonData).target;
    }
    public static void Load<T>(string p_filepath, out T[] __outArrayData)
    {
        string jsonData = File.ReadAllText(p_filepath);
        __outArrayData = JsonUtility.FromJson<Serialization_1DArray<T>>(jsonData).array;
    }
    public static void Load<T>(string p_filepath, out T[][] __outArrayData)
    {
        string jsonData = File.ReadAllText(p_filepath);
        __outArrayData = JsonUtility.FromJson<Serialization_2DArray<T>>(jsonData).array;
    }
}
