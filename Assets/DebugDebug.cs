using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDebug : MonoBehaviour
{
    private void Update()
    {
        Debug.LogFormat($"nickname : {_NetworkInfoManager.Instance.m_playerInfo.nickname}");
        Debug.LogFormat($"other nickname : {_NetworkInfoManager.Instance.m_playerInfo.other_nickname}");
    }
}
