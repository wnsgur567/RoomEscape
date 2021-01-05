using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonLoader : Singleton<JsonLoader>
{
    [SerializeField]
    List<MonoBehaviour> storage;

    private List<IJsonLoad> m_load_list;

    override protected void Awake()
    {
        base.Awake();

        m_load_list = new List<IJsonLoad>();
        foreach (var item in storage)
        {
            var _sc = item as IJsonLoad;
            if (_sc != null)
                m_load_list.Add(_sc);
            else
                Debug.LogFormat("IJsonLoad ¹ÌÆ÷ÇÔ : {0}", item.gameObject.name);
        }

        __Initialize();
        __LoadData();
        __Finalize();
    }

    private void __Initialize()
    {
        foreach (var item in m_load_list)
        {
            item.__Initialize();
        }
    }

    private void __LoadData()
    {
        foreach (var item in m_load_list)
        {
            item.__LoadData();
        }
    }

    private void __Finalize()
    {
        foreach (var item in m_load_list)
        {
            item.__Finalize();

        }
    }

}
