using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PipeButton : MonoBehaviourPunCallbacks
{
    //�ϳ��� ��������ư�� 3���� ������ ����
    public Pipe[] pipe;
    public void ActiveButton()
    {
        //���߿� �̺κ� ������ ��ư ������ �ִϸ��̼� �߰��ؾ���

        //�������� ������ ����
        for (int i = 0; i < 3; i++)
        {
            //RpcTarget.All(�� �濡�ִ� ��� �������) �� �Լ��� �����ض�
            //RpcTarget.All�� ����� ȣ��Ǿ� ��������� AllBuffered�� �����ӵǵ� �����̴�.
            pipe[i].M_PV.RPC("OncollisionPipe", RpcTarget.AllBuffered);
        }
    }
}
