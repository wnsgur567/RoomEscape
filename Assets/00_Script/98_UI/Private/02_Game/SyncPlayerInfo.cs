using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SyncPlayerInfo : MonoBehaviourPunCallbacks
{
    _NetworkInfoManager m_infoManager = null;
    PhotonView m_pv;

    private void Awake()
    {
        m_infoManager = _NetworkInfoManager.Instance;
        m_pv = GetComponent<PhotonView>();
    }


    private void Update()
    {        
        if( PhotonNetwork.NetworkClientState == ClientState.Joined)
            m_pv.RPC("__RPC_SyncNickName", RpcTarget.Others , m_infoManager.m_playerInfo.nickname);
    }


    [PunRPC]
    private void __RPC_SyncNickName(string p_nick)
    {
        if (m_pv.IsMine)
            return;
        Debug.Log("isMine");
        m_infoManager.m_playerInfo.other_nickname = p_nick;
    }

    public void ChangeRoom()
    {
        m_pv.RPC("__RPC_ChangeRoom", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void __RPC_ChangeRoom()
    {
        if (m_infoManager.m_playerInfo.type == E_RoomType.A)
        {
            m_infoManager.m_playerInfo.type = E_RoomType.B;
        }
        else
        {
            m_infoManager.m_playerInfo.type = E_RoomType.A;
        }
    }

    public void __SetOwner()
    {
        m_infoManager.m_playerInfo.isRoomOwner = true;
    }
    public void __SetParticipant()
    {
        m_infoManager.m_playerInfo.isRoomOwner = false; ;
    }

    public void ChangeOwner()
    {
        m_pv.RPC("__RPC_ChangeOwner", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void __RPC_ChangeOwner()
    {
        if (m_infoManager.m_playerInfo.isRoomOwner)
        {
            m_infoManager.m_playerInfo.isRoomOwner = false;
        }
        else
        {
            m_infoManager.m_playerInfo.isRoomOwner = true;
        }
    }

    public void GameStart()
    {
        // 방장만 시작 권한이 있음
        if (m_infoManager.m_playerInfo.isRoomOwner)
        {
            m_pv.RPC("__RPC_start", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void __RPC_start()
    {
        // 게임 시작 버튼 관련 처리 루틴을 call
        // 현재 버튼 부분에 있음
    }
}
