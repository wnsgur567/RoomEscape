using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NetworkPlayerInfo
{
    public string nickname;
}

public class _NetworkInfoManager : Singleton<_NetworkInfoManager>
{
    public NetworkPlayerInfo m_playerInfo;

    public void SetNickName(string p_nick)
    {
        m_playerInfo.nickname = p_nick;
    }
}
