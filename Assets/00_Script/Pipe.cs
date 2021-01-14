using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Pipe : MonoBehaviourPunCallbacks
{
    [Header("파이프 활성화")]
    public bool M_PipeActivate;

    public PhotonView M_PV;
    private MeshRenderer m_MeshRenderer;
    void Start()
    {
        M_PV = GetComponent<PhotonView>();
        m_MeshRenderer = GetComponent<MeshRenderer>();
        if (M_PipeActivate == true)
        {
            //지금은 모델이없어 SetActive로 하지만 파이프 받으면 애니메이션으로 변경예정
            m_MeshRenderer.material.color = Color.blue;
            //파이프 실행
        }
        else
        {
            m_MeshRenderer.material.color = Color.gray;
            //파이프 끄기
        }
    }
    [PunRPC]
    public void OncollisionPipe()
    {
        M_PipeActivate = !M_PipeActivate;
        if (M_PipeActivate == true)
        {
            //지금은 모델이없어 SetActive로 하지만 파이프 받으면 애니메이션으로 변경예정
            //this.gameObject.SetActive(true);
            m_MeshRenderer.material.color = Color.blue;
            //파이프 실행
        }
        else
        {
            //this.gameObject.SetActive(false);
            m_MeshRenderer.material.color = Color.gray;
            //파이프 끄기
        }

    }


}
