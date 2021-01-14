using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomJoinButton : MonoBehaviour
{
    private _NetworkInfoManager m_infoManager = null;
    private NetworkRoomManager m_roomManager = null;    
    [SerializeField, ShowOnly] TextMeshProUGUI m_text;

    private void Awake()
    {
        m_infoManager = _NetworkInfoManager.Instance;
        m_roomManager = NetworkRoomManager.Instance;        
    }

    public void JoinRoom(string p_sceneName)
    {
        if (m_roomManager.m_selectedRoomName == "")
            //return;

        if (m_roomManager._JoinRoom(m_roomManager.m_selectedRoomName))
        {
            _OnButtonProcess.__OnLoadScene(p_sceneName);
        }
    }
}
