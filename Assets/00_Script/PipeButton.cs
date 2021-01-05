using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PipeButton : MonoBehaviourPunCallbacks
{
    //하나의 파이프버튼에 3개의 파이프 연결
    public Pipe[] pipe;
    public void ActiveButton()
    {
        //나중에 이부분 파이프 버튼 돌리는 애니메이션 추가해야함

        //돌렸으면 파이프 실행
        for (int i = 0; i < 3; i++)
        {
            //RpcTarget.All(그 방에있는 모든 사람한테) 이 함수를 실행해라
            //RpcTarget.All은 그즉시 호출되어 사라지지만 AllBuffered는 재접속되도 지속이다.
            pipe[i].M_PV.RPC("OncollisionPipe", RpcTarget.AllBuffered);
        }
    }
}
