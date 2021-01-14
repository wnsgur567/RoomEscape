using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    _NetworkInfoManager Infomanager;
    [SerializeField]
    private Transform A_SpawnPosition;

    [SerializeField]
    private Transform B_SpawnPosition;
    void Start()
    {
        if (Infomanager.m_playerInfo.type == E_RoomType.A)
        {
            PhotonNetwork.Instantiate("Charactor", A_SpawnPosition.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("Charactor", B_SpawnPosition.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
