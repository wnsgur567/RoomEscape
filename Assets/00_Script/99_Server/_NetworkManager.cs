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


    #region ���� ����

    // ������ �����ϱ�
    public void _Connect() => PhotonNetwork.ConnectUsingSettings();

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

    #region �κ� 
    // ���������� ��� �κ� ������ ���
    // 1���� ��� ����

    public void _JoinLobby() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� ����");
    }
    public void _LeaveLobby() => PhotonNetwork.LeaveLobby();
    public override void OnLeftLobby()
    {
        Debug.Log("�κ� ����");
    }
    #endregion

    #region ��(���ӹ�)

    // ���� �� ����
    // �� �ִ��ο� 2��
    public void _CreateRoom(string p_roomName) 
        => PhotonNetwork.CreateRoom(p_roomName, new RoomOptions { MaxPlayers = 2 });

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
    #endregion
}
