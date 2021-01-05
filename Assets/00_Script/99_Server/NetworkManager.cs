using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public GameObject LobbyImage;
    public GameObject CM;
    private string SpawnName;

    public GameManager GM;
    void Awake()
    {
        Screen.SetResolution(1920, 1080, false);//�ػ󵵼���

        //�Ʒ��� ���� ����? �ӵ�������
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }
    public void Connect() => PhotonNetwork.ConnectUsingSettings();//��ư������ ����Ǵ��Լ�

    public override void OnConnectedToMaster()//������ ����
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 5 }, null);
    }

    public override void OnCreatedRoom()//���̻����Ǹ�
    {

    }
    public override void OnJoinedRoom()//�濡���ӵǸ�
    {
        Debug.Log("���ӵ�");
        CM.SetActive(false);
        LobbyImage.SetActive(false);

        GM.Spawn(SpawnName);
    }
    public void SetButtonName(string name)
    {
        SpawnName = name;
    }
}