using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_RoomType
{
    A = 0,
    B,

    Max
}

public class RoomButton : MonoBehaviour
{
    _NetworkInfoManager m_netInfoManager = null;
    
    private void Awake()
    {
        m_netInfoManager = _NetworkInfoManager.Instance;
    }

    public void __OnSelectRoom_A()
    {
        m_netInfoManager.SetRoomType(E_RoomType.A);
    }
    public void __OnSelectRoom_B()
    {
        m_netInfoManager.SetRoomType(E_RoomType.B);
    }

}
