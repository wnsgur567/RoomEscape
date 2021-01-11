using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class _NetworkManager : SingletonPunCallback<_NetworkManager>
{
    protected override void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
    }


    #region 서버 연결

    // 서버와 연결하기
    public void _Connect() => PhotonNetwork.ConnectUsingSettings();

    // 서버 연결 시 callback
    public override void OnConnected()
    {
        Debug.Log("서버 접속 완료");
    }
    // 서버와 연결 끊기
    public void _DisConnect() => PhotonNetwork.Disconnect();

    // 서버 연결 끊겼을 시 callback
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("서버와 연결 끊김");
    }
    #endregion

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

    #region 룸(게임방)

    // 게임 방 생성
    // 방 최대인원 2명
    public void _CreateRoom(string p_roomName) 
        => PhotonNetwork.CreateRoom(p_roomName, new RoomOptions { MaxPlayers = 2 });

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
    public void _JoinRoom(string p_roomName)
        => PhotonNetwork.JoinRoom(p_roomName);

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
    #endregion
}
