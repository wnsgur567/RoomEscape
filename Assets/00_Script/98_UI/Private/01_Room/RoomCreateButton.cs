using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomCreateButton : MonoBehaviour
{
    private NetworkRoomManager m_roomManager;
    [SerializeField, ShowOnly] TextMeshProUGUI m_roomName_Text;
    [SerializeField, ShowOnly] TextMeshProUGUI m_roomPW_Text;

    private void Awake()
    {
        m_roomManager = NetworkRoomManager.Instance;
    }

    public void __CreateRoom(string p_sceneName)
    {
        if(m_roomManager._CreateRoom())
        {
            _OnButtonProcess.__OnLoadScene(p_sceneName);
            Debug.Log("ABC");
        }
    }
}
