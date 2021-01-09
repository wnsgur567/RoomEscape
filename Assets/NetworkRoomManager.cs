using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class NetworkRoomManager : SingletonPunCallback<NetworkRoomManager>
{
    _NetworkManager m_netManager = null;
    public RoomPanel m_RoomOriginPrefab;
    public ContentSizeFitter m_RoomListContent;
    private GameObject m_RoomListContentObj = null;

    public TMP_InputField m_roomNameInputfield;
    public TMP_InputField m_roomPasswordInputfield;

    private List<RoomInfo> m_roomList = null;


    override protected void Awake()
    {
        m_netManager = _NetworkManager.Instance;

        m_roomList = new List<RoomInfo>();
        m_RoomListContentObj = m_RoomListContent.gameObject;
    }

    private void NewRoomObj(RoomInfo p_info)
    {
        RoomPanel _roomPanel = GameObject.Instantiate<RoomPanel>(m_RoomOriginPrefab);        
        Room _room = PhotonNetwork.CurrentRoom;
        _roomPanel.__Initialize(p_info.Name, p_info.PlayerCount);
        _roomPanel.transform.SetParent(m_RoomListContentObj.transform, false);
    }

    #region ��(���ӹ�)

    // ���� �� ����
    // �� �ִ��ο� 2��
    public void _CreateRoom()
    {
        PhotonNetwork.CreateRoom(m_roomNameInputfield.text, new RoomOptions { MaxPlayers = 2 });
    }

    // �� ���� ���� �� callback
    public override void OnCreatedRoom()
    {
        Debug.Log("�� ���� ����");
    }
    // �� ���� ���� �� callback
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("�� ���� ����");
    }

    // �� �̸����� ����
    public void _JoinRoom(string p_roomName)
        => PhotonNetwork.JoinRoom(p_roomName);

    // ���� �� ���� �õ�
    public void _JoinRandomRoom()
        => PhotonNetwork.JoinRandomRoom();

    // �� ���� ���� �� callback
    public override void OnJoinedRoom()
    {
        Debug.Log("�� ���� �Ϸ�");
    }

    // �� ���� ���� ��(�ش� ���� ���� ��� ��) callback
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogFormat($"{message}");
    }
    // ���� �� ���� ���� �� callback
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogFormat($"{message}");
    }

    // �� ���� �õ�
    public void _LeaveRoom()
        => PhotonNetwork.LeaveRoom();

    // �� ���� �� callback
    public override void OnLeftRoom()
    {
        Debug.Log("�� ���� �Ϸ�");
    }

    // ���� ������Ʈ �� ��� (�ٸ� ������ ���� or ���� ��)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            // Room�� Ȱ��ȭ�� ���¶��
            if (!roomList[i].RemovedFromList)
            {
                // ���� member ����Ʈ�� ������� �߰�
                if (!m_roomList.Contains(roomList[i]))
                {
                    m_roomList.Add(roomList[i]);
                }
                else
                {   // �ִ� ��� ����
                    m_roomList[m_roomList.IndexOf(roomList[i])] = roomList[i];
                }
            }
            else if (m_roomList.IndexOf(roomList[i]) != -1)
            {   // ��Ȱ�� ���¶�� member ����Ʈ���� ����
                m_roomList.RemoveAt(m_roomList.IndexOf(roomList[i]));
            }
        }

        UpdateMyRoomList();
    }
    #endregion

    private void UpdateMyRoomList()
    {
        int _count = m_RoomListContentObj.transform.childCount;
        for (int i = 0; i < _count; i++)
        {
            Destroy(m_RoomListContentObj.transform.GetChild(i).gameObject);
        }
        foreach (var item in m_roomList)
        {
            NewRoomObj(item);
        }
    }
}
