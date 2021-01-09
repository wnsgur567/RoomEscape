using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject LobbyImage;
    public static GameManager M_gameManager = null;
    // Start is called before the first frame update
    private void Awake()
    {
        M_gameManager = this;
        Spawn(Vector3.zero);
    }
    public void Complete_PipePuzzle()
    {
        //파이프퍼즐 완료시
        //폭탄뚜껑 열어야함
        Debug.Log("파이프 클리어!");
    }


    public void Spawn(Vector3 SpawnPosition)//버튼에 할당한 Position으로 prefab인스턴스 생성
    {
        PhotonNetwork.Instantiate("Charactor", SpawnPosition, Quaternion.identity);
    }
}
