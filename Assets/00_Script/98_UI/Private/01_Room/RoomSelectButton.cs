using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomSelectButton : MonoBehaviour
{
    private NetworkRoomManager m_roomManager;
    [SerializeField] TextMeshProUGUI m_text;

    private void Awake()
    {
        m_roomManager = NetworkRoomManager.Instance;
    }

    public void __SelectRoom()
    {
        m_roomManager.m_selectedRoomName = m_text.text.Split(' ')[0];
    }
}
