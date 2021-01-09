using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Pipe : MonoBehaviourPunCallbacks
{
    [Header("������ Ȱ��ȭ")]
    public bool M_PipeActivate;

    public PhotonView M_PV;

    void Start()
    {
        M_PV = GetComponent<PhotonView>();
        if (M_PipeActivate == true)
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
    [PunRPC]
    public void OncollisionPipe()
    {
        M_PipeActivate = !M_PipeActivate;
        if (M_PipeActivate == true)
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
