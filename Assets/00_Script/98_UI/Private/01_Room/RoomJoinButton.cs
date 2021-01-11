using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomJoinButton : MonoBehaviour
{
    private NetworkRoomManager m_roomManager;    
    [SerializeField, ShowOnly] TextMeshProUGUI m_text;

    private void Awake()
    {
        m_roomManager = NetworkRoomManager.Instance;        
    }
   
    public void __JoinRoom(string p_sceneName)
    {
        if (m_roomManager._JoinRoom(m_roomManager.m_selectedRoomName))
        {
            _OnButtonProcess.__OnLoadScene(p_sceneName);
        }
    }    
}
