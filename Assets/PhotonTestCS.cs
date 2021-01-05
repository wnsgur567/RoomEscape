using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonTestCS : Photon.Pun.MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate("Test", new Vector3(0, 0, 0), Quaternion.identity, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
