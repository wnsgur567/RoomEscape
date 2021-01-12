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

        // owner�� �� ���� ��ư�� ���� �� 
        // ������ ����� �⺻owner �� ��������
        //Debug.LogFormat("1111111111111111____________ {0}", PhotonNetwork.IsMasterClient);
        if (PhotonNetwork.IsMasterClient)
        {
            // �� Ÿ�� ����
            m_infoManager.m_playerInfo.type = E_RoomType.A;

            // �� ��� Owner ǥ�� Text 
            m_OwnerText.text = "�� ��";

            // ��ư Ȱ��ȭ, ��Ȱ��ȭ (���常 ���� ����)
            m_startButtonText.text = "�����ϱ�";
            m_startButton.enabled = true;
            m_Aroom_selectButton.enabled = true;
            m_Broom_selectButton.enabled = true;
        }
        else
        {
            // �� Ÿ�� ����
            m_infoManager.m_playerInfo.type = E_RoomType.B;

            // �� ��� Owner ǥ�� Text panel �¿���
            m_OwnerText.text = "������";

            // ���� ��ư Ȱ��ȭ, ��Ȱ��ȭ
            m_startButtonText.text = "�غ�Ϸ�";
            m_startButton.enabled = true;
            m_Aroom_selectButton.enabled = false;
            m_Broom_selectButton.enabled = false;
        }
    }

    private string Make_NickText(int _playerNum, string _nick)
    {
        return string.Format($"������ {_playerNum} �г��� : {_nick}");
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
