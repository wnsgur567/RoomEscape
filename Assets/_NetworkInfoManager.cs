using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NetworkPlayerInfo
{
    public string nickname;
    public string currRoomName;  // 참가한 방 이름
}

public class _NetworkInfoManager : Singleton<_NetworkInfoManager>
{
    [ShowOnly] public NetworkPlayerInfo m_playerInfo;

    public void SetNickName(string p_nick)
    {
        m_playerInfo.nickname = p_nick;
    }
}
