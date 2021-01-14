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
    private MeshRenderer m_MeshRenderer;
    void Start()
    {
        M_PV = GetComponent<PhotonView>();
        m_MeshRenderer = GetComponent<MeshRenderer>();
        if (M_PipeActivate == true)
        {
            //������ ���̾��� SetActive�� ������ ������ ������ �ִϸ��̼����� ���濹��
            m_MeshRenderer.material.color = Color.blue;
            //������ ����
        }
        else
        {
            m_MeshRenderer.material.color = Color.gray;
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
            //this.gameObject.SetActive(true);
            m_MeshRenderer.material.color = Color.blue;
            //������ ����
        }
        else
        {
            //this.gameObject.SetActive(false);
            m_MeshRenderer.material.color = Color.gray;
            //������ ����
        }

    }


}
