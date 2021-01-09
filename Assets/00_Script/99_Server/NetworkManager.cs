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
    public GameManager GM;

    private Transform SpawnPosition;
    void Awake()
    {
        Screen.SetResolution(1920, 1080, false);//해상도설정

        //아래는 서버 반응? 속도같은거
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }
    public void Connect() => PhotonNetwork.ConnectUsingSettings();//버튼누르면 실행되는함수

    public override void OnConnectedToMaster()//서버에 접속
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 3 }, null);
    }

    public override void OnCreatedRoom()//방이생성되면
    {

    }
    public override void OnJoinedRoom()//방에접속되면
    {
        Debug.Log("접속됨");
        CM.SetActive(false);
        LobbyImage.SetActive(false);

        GM.Spawn(SpawnPosition);
    }
    public void SetSpawnPosition(Transform Position)
    {
        SpawnPosition = Position;
    }
}
