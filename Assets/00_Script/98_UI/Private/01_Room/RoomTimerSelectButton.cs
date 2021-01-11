using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTimerSelectButton : MonoBehaviour
{
    _NetworkInfoManager m_infoManager = null;
    private void Awake()
    {
        m_infoManager = _NetworkInfoManager.Instance;
    }
    public void __SetRoomMaxTimer(int p_time)
    {
        m_infoManager.m_playerInfo.deadLine_time = p_time;
    }
}
