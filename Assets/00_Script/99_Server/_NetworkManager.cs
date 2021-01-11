using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks 
{    
    void Awake()
    {
        //Screen.SetResolution(1920, 1080, false);//�ػ󵵼���

        //�Ʒ��� ���� ����? �ӵ�������
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;


        // ��Ʈ��ũ �ʱ�ȭ
        Connect();
    }
    public void Connect() => PhotonNetwork.ConnectUsingSettings();//��ư������ ����Ǵ��Լ�

    public override void OnConnectedToMaster()//������ ����
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, null);
    }

    public override void OnCreatedRoom()//���̻����Ǹ�
    {

    }
    public override void OnJoinedRoom()//�濡���ӵǸ�
    {
        //Debug.Log("���ӵ�");
        //CM.SetActive(false);
        //LobbyImage.SetActive(false);

        //GM.Spawn(SpawnPosition);
    }    
}
