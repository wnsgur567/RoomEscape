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
    _NetworkInfoManager m_netInfoManager = null;

    public RoomPanel m_RoomOriginPrefab;
    public ContentSizeFitter m_RoomListContent;
    private GameObject m_RoomListContentObj = null;

    public TMP_InputField m_roomNameInputfield;
    public TMP_InputField m_roomPasswordInputfield;

    private List<RoomInfo> m_roomList = null;

    public string m_selectedRoomName;

    override protected void Awake()
    {
        _JoinLobby();

        m_netManager = _NetworkManager.Instance;
        m_netInfoManager = _NetworkInfoManager.Instance;

        m_roomList = new List<RoomInfo>();
        m_RoomListContentObj = m_RoomListContent.gameObject;

        m_selectedRoomName = "";
        
    }

    private void NewRoomObj(RoomInfo p_info)
    {
        RoomPanel _roomPanel = GameObject.Instantiate<RoomPanel>(m_RoomOriginPrefab);
        Photon.Realtime.Room currentRoom = PhotonNetwork.CurrentRoom;        
        _roomPanel.__Initialize(p_info.Name, p_info.PlayerCount);
        _roomPanel.transform.SetParent(m_RoomListContentObj.transform, false);
    }

    #region 룸(게임방)

    // 게임 방 생성
    // 방 최대인원 2명
    public bool _CreateRoom()
    {
        m_netInfoManager.m_playerInfo.currRoomName = m_roomNameInputfield.text;
        return PhotonNetwork.CreateRoom(m_roomNameInputfield.text, new RoomOptions { MaxPlayers = 2 });
    }

    // 방 생성 성공 시 callback
    public override void OnCreatedRoom()
    {
        Debug.Log("방 생성 성공");
    }
    // 방 생성 실패 시 callback
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("방 생성 실패");
    }

    // 방 이름으로 참가
    public bool _JoinRoom(string p_roomName)
    {
        m_netInfoManager.m_playerInfo.currRoomName = m_selectedRoomName;
        return PhotonNetwork.JoinRoom(p_roomName);
    }
        

    // 랜덤 방 참가 시도
    public void _JoinRandomRoom()
        => PhotonNetwork.JoinRandomRoom();

    // 방 참가 성공 시 callback
    public override void OnJoinedRoom()
    {
        Debug.Log("방 참가 완료");
    }

    // 방 참가 실패 시(해당 방이 없는 경우 등) callback
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogFormat($"{message}");
    }
    // 랜덤 방 참가 실패 시 callback
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogFormat($"{message}");
    }

    // 방 퇴장 시도
    public void _LeaveRoom()
        => PhotonNetwork.LeaveRoom();

    // 방 퇴장 시 callback
    public override void OnLeftRoom()
    {
        Debug.Log("방 퇴장 완료");
    }

    // 룸이 업데이트 될 경우 (다른 유저가 생성 or 삭제 등)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            // Room이 활성화된 상태라면
            if (!roomList[i].RemovedFromList)
            {
                // 현재 member 리스트에 없을경우 추가
                if (!m_roomList.Contains(roomList[i]))
                {
                    m_roomList.Add(roomList[i]);
                }
                else
                {   // 있는 경우 갱신
                    m_roomList[m_roomList.IndexOf(roomList[i])] = roomList[i];
                }
            }
            else if (m_roomList.IndexOf(roomList[i]) != -1)
            {   // 비활성 상태라면 member 리스트에서 삭제
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


    #region 로비 
    // 대형게임의 경우 로비를 여러개 사용
    // 1개만 사용 예정

    public void _JoinLobby() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 입장");
    }
    public void _LeaveLobby() => PhotonNetwork.LeaveLobby();
    public override void OnLeftLobby()
    {
        Debug.Log("로비 퇴장");
    }
    #endregion
}
