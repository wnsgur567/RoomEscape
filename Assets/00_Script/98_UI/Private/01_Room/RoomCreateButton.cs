using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoomCreateButton : MonoBehaviour
{
    private _NetworkInfoManager m_infoManager = null;
    private NetworkRoomManager m_roomManager;
    [SerializeField, ShowOnly] TextMeshProUGUI m_roomName_Text;
    [SerializeField, ShowOnly] TextMeshProUGUI m_roomPW_Text;

    private void Awake()
    {
        m_infoManager = _NetworkInfoManager.Instance;
        m_roomManager = NetworkRoomManager.Instance;
    }

    public void __CreateRoom(string p_sceneName)
    {
        if(m_roomManager._CreateRoom())
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            _OnButtonProcess.__OnLoadScene(p_sceneName);
            Debug.Log("ABC");
        }
    }

    public void __SetOwner()
    {
        m_infoManager.m_playerInfo.isRoomOwner = true;
    }
}
