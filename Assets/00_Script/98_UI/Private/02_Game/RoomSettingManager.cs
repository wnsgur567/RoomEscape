using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class RoomSettingManager : MonoBehaviour
{
    _NetworkInfoManager m_infoManager;
    PhotonView m_pv = null;

    [SerializeField] TextMeshProUGUI m_OwnerText;

    [SerializeField] TextMeshProUGUI m_1nickText;
    [SerializeField] TextMeshProUGUI m_2nickText;

    [SerializeField] Button m_Aroom_selectButton;
    [SerializeField] Button m_Broom_selectButton;
    [SerializeField] Button m_startButton;
    [SerializeField] TextMeshProUGUI m_startButtonText;


    private void Awake()
    {
        m_infoManager = _NetworkInfoManager.Instance;
        m_pv = GetComponent<PhotonView>();
    }

    public void __Initialize()
    {
        m_pv.RequestOwnership();

        // owner는 방 생성 버튼을 누를 때 
        // 생성한 사람을 기본owner 로 설정했음
        //Debug.LogFormat("1111111111111111____________ {0}", PhotonNetwork.IsMasterClient);
        if (PhotonNetwork.IsMasterClient)
        {
            // 방 타입 설정
            m_infoManager.m_playerInfo.type = E_RoomType.A;

            // 최 상단 Owner 표시 Text 
            m_OwnerText.text = "방 장";

            // 버튼 활성화, 비활성화 (방장만 권한 있음)
            m_startButtonText.text = "시작하기";
            m_startButton.enabled = true;
            m_Aroom_selectButton.enabled = true;
            m_Broom_selectButton.enabled = true;
        }
        else
        {
            // 방 타입 설정
            m_infoManager.m_playerInfo.type = E_RoomType.B;

            // 최 상단 Owner 표시 Text panel 온오프
            m_OwnerText.text = "참가자";

            // 시작 버튼 활성화, 비활성화
            m_startButtonText.text = "준비완료";
            m_startButton.enabled = true;
            m_Aroom_selectButton.enabled = false;
            m_Broom_selectButton.enabled = false;
        }
    }

    private string Make_NickText(int _playerNum, string _nick)
    {
        return string.Format($"참가자 {_playerNum} 닉네임 : {_nick}");
    }

    private void Update()
    {
        __Initialize();

        var check = PhotonNetwork.MasterClient;
        if (PhotonNetwork.IsMasterClient)
        {
            m_1nickText.text = Make_NickText(1, m_infoManager.m_playerInfo.nickname);
            m_2nickText.text = Make_NickText(2, m_infoManager.m_playerInfo.other_nickname);
        }
        else
        {
            m_1nickText.text = Make_NickText(1, m_infoManager.m_playerInfo.other_nickname);
            m_2nickText.text = Make_NickText(2, m_infoManager.m_playerInfo.nickname);
        }
    }
}
