using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public enum E_RoomType
{
    A = 0,
    B,

    Max
}

public class RoomButton : MonoBehaviour
{
    _NetworkInfoManager m_netInfoManager = null;
    private Button m_button = null;

    private void Awake()
    {
        m_netInfoManager = _NetworkInfoManager.Instance;

        m_button = GetComponent<Button>();
        m_button.interactable = false;
    }

    public void OnSelectRoom_A()
    {
        m_netInfoManager.SetRoomType(E_RoomType.A);
    }
    public void OnSelectRoom_B()
    {
        m_netInfoManager.SetRoomType(E_RoomType.B);
    }

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            // 방 인원 수가 충족 되면
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
            {
                m_button.interactable = true;
            }
        }
    }
}