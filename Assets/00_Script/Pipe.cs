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

    [SerializeField]
    private Material m_OnPipe;

    [SerializeField]
    private Material m_OffPipe;
    void Start()
    {
        M_PV = GetComponent<PhotonView>();
        m_MeshRenderer = GetComponent<MeshRenderer>();
        if (M_PipeActivate == true)
        {
            m_MeshRenderer.material = m_OnPipe;
            //������ ����
        }
        else
        {
            m_MeshRenderer.material = m_OffPipe;
            //������ ����
        }
    }
    [PunRPC]
    public void OncollisionPipe()
    {
        M_PipeActivate = !M_PipeActivate;
        if (M_PipeActivate == true)
        {
            //this.gameObject.SetActive(true);
            m_MeshRenderer.material = m_OnPipe;
            //������ ����
        }
        else
        {
            //this.gameObject.SetActive(false);
            m_MeshRenderer.material = m_OffPipe;
            //������ ����
        }

    }


}
