using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Pipe : MonoBehaviourPunCallbacks
{
    [Header("파이프 활성화")]
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
            //지금은 모델이없어 SetActive로 하지만 파이프 받으면 애니메이션으로 변경예정
            this.gameObject.SetActive(true);
            //파이프 실행
        }
        else
        {
            this.gameObject.SetActive(false);
            //파이프 끄기
        }

    }


}
