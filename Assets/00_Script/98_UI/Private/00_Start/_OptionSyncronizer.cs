using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _OptionSyncronizer : MonoBehaviour
{
    [SerializeField, ShowOnly] CurrentOptionInfo m_loadedData;

    // 데이터를 저장
    public void __OnSaveData()
    {

    }

    public void __OnLoadData()
    {
        // m_loadedData 를 로드
    }

    // 불러온 데이터로 동기화
    public void __OnDataSync()
    {
        // m_loadedData -> p_curInfo 에 적용
        _OptionInfoManager.Instance.m_currentOptionInfo = m_loadedData;
    }

}
