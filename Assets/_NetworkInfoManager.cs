using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NetworkPlayerInfo
{
    public string nickname;
    public string other_nickname;
    public string currRoomName;       // ������ �� �̸�
    public int deadLine_time;         // �� ���ѽð�   
    public E_RoomType type;
    public bool isRoomOwner;          // �����ΰ�?
}

public class _NetworkInfoManager : Singleton<_NetworkInfoManager>
{
    [ShowOnly] public NetworkPlayerInfo m_playerInfo;

    private void Start()
    {
        
    }

    public void SetNickName(string p_nick)
    {
        m_playerInfo.nickname = p_nick;
    }

    public void SetRoomType(E_RoomType _type)
    {
        m_playerInfo.type = _type;
    }
    
}
