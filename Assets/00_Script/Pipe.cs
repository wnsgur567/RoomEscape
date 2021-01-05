using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Pipe : MonoBehaviourPunCallbacks
{
    [Header("������ Ȱ��ȭ")]
    public bool M_Activate;

    public PhotonView M_PV;

    void Start()
    {
        M_PV = GetComponent<PhotonView>();    
    }
    [PunRPC]
    public void OncollisionPipe()
    {
        M_Activate = !M_Activate;
        if (M_Activate == true)
        {
            //������ ���̾��� SetActive�� ������ ������ ������ �ִϸ��̼����� ���濹��
            this.gameObject.SetActive(true);
            //������ ����
        }
        else
        {
            this.gameObject.SetActive(false);
            //������ ����
        }

    }


}
