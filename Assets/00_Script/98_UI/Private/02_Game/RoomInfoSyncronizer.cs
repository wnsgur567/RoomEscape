using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomInfoSyncronizer : MonoBehaviourPunCallbacks
{
    _NetworkInfoManager m_infoManager = null;
    _NetworkChatManager m_chatManager = null;
    [SerializeField] Canvas m_canvas;
    PhotonView m_pv = null;

    [SerializeField] Button m_StartOrReadyButton;
    [SerializeField] Image m_AroomImage;
    [SerializeField] Image m_BroomImage;
    [SerializeField] Color m_unSelectedColor;
    [SerializeField] Color m_selectColor;

    private void Awake()
    {
        m_chatManager = _NetworkChatManager.Instance;
        m_infoManager = _NetworkInfoManager.Instance;
        m_pv = GetComponent<PhotonView>();        
    }

    // 방 참가자가 레디버튼을 누를 경우 호출
    public void _OnReady()
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            bool _b;
            if (m_infoManager.m_playerInfo.isReady)
            {
                _b = false;
            }
            else
            {
                _b = true;
            }
            m_pv.RPC("__RPC_SyncReady", RpcTarget.Others, _b);
        }
    }

    [PunRPC]
    private void __RPC_SyncReady(bool _b)
    {
        m_infoManager.m_playerInfo.isReady = _b;
        if (m_infoManager.m_playerInfo.isReady)
            m_StartOrReadyButton.interactable = true;
        else
            m_StartOrReadyButton.interactable = false;
    }

    public void _OnStart(string _sceneName)
    {
        if(PhotonNetwork.IsMasterClient && m_infoManager.m_playerInfo.isReady)
        {
            m_pv.RPC("__RPC_SyncStart", RpcTarget.All, _sceneName);
        }
    }
    [PunRPC]
    private void __RPC_SyncStart(string _name)
    {
        SceneManager.LoadScene(_name,LoadSceneMode.Additive);
        m_chatManager._GameStart();
        m_canvas.gameObject.SetActive(false);
    }
    

    // a방 혹은 b방 선택 될 시
    public void _OnRoomSelected()
    {
        // 방장만 선택 권한 있음
        if(PhotonNetwork.IsMasterClient)
        {
            m_pv.RPC("__RPC_SyncRoomSelect", RpcTarget.Others, (int)m_infoManager.m_playerInfo.type);
        }
    }

    
    [PunRPC]
    private void __RPC_SyncRoomSelect(int p_type)
    {
        E_RoomType _type = (E_RoomType)p_type;
        
        switch (_type)
        {
            case E_RoomType.A:
                m_infoManager.m_playerInfo.type = E_RoomType.B;
                SetBroomImageColor();                
                break;
            case E_RoomType.B:
                m_infoManager.m_playerInfo.type = E_RoomType.A;
                SetAroomImageColor();
                break;           
        }
    }

    public void SetAroomImageColor()
    {
        m_BroomImage.color = m_unSelectedColor;
        m_AroomImage.color = m_unSelectedColor;
        m_AroomImage.color = m_selectColor;
    }
    public void SetBroomImageColor()
    {
        m_BroomImage.color = m_unSelectedColor;
        m_AroomImage.color = m_unSelectedColor;
        m_BroomImage.color = m_selectColor;
    }

}
