using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _OptionSyncronizer : MonoBehaviour
{
    [SerializeField, ShowOnly] CurrentOptionInfo m_loadedData;

    // �����͸� ����
    public void __OnSaveData()
    {

    }

    public void __OnLoadData()
    {
        // m_loadedData �� �ε�
    }

    // �ҷ��� �����ͷ� ����ȭ
    public void __OnDataSync()
    {
        // m_loadedData -> p_curInfo �� ����
        _OptionInfoManager.Instance.m_currentOptionInfo = m_loadedData;
    }

}
