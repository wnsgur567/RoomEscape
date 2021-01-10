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

        _Connect();
    }


    #region 서버 연결

    // 서버와 연결하기
    public void _Connect() => PhotonNetwork.ConnectUsingSettings();
    public override void OnConnectedToMaster()
    {
        //_JoinLobby();
    }

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

    //#region 로비 
    //// 대형게임의 경우 로비를 여러개 사용
    //// 1개만 사용 예정

    //public void _JoinLobby() => PhotonNetwork.JoinLobby();
    //public override void OnJoinedLobby()
    //{
    //    Debug.Log("로비 입장");
    //}
    //public void _LeaveLobby() => PhotonNetwork.LeaveLobby();
    //public override void OnLeftLobby()
    //{
    //    Debug.Log("로비 퇴장");
    //}
    //#endregion



}
