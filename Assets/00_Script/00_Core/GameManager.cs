using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject LobbyImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn(string name)//��ư�� �Ҵ��� name���� prefab�ν��Ͻ� ����
    {
        PhotonNetwork.Instantiate(name, Vector3.zero, Quaternion.identity);
    }
}
