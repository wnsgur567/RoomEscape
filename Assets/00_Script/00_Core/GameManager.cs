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
        //���������� �Ϸ��
        //��ź�Ѳ� �������
        Debug.Log("������ Ŭ����!");
    }


    public void Spawn(Vector3 SpawnPosition)//��ư�� �Ҵ��� Position���� prefab�ν��Ͻ� ����
    {
        PhotonNetwork.Instantiate("Charactor", SpawnPosition, Quaternion.identity);
    }
}
