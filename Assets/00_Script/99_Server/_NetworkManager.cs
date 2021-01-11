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


    #region ���� ����

    // ������ �����ϱ�
    public void _Connect() => PhotonNetwork.ConnectUsingSettings();
    public override void OnConnectedToMaster()
    {
        //_JoinLobby();
    }

    // ���� ���� �� callback
    public override void OnConnected()
    {        
        Debug.Log("���� ���� �Ϸ�");
    }
    // ������ ���� ����
    public void _DisConnect() => PhotonNetwork.Disconnect();

    // ���� ���� ������ �� callback
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("������ ���� ����");
    }
    #endregion

    //#region �κ� 
    //// ���������� ��� �κ� ������ ���
    //// 1���� ��� ����

    //public void _JoinLobby() => PhotonNetwork.JoinLobby();
    //public override void OnJoinedLobby()
    //{
    //    Debug.Log("�κ� ����");
    //}
    //public void _LeaveLobby() => PhotonNetwork.LeaveLobby();
    //public override void OnLeftLobby()
    //{
    //    Debug.Log("�κ� ����");
    //}
    //#endregion



}
