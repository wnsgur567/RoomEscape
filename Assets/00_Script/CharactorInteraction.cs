using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class CharactorInteraction : MonoBehaviourPunCallbacks
{
    public float m_range;
    public Transform m_cam;

    public PipeButton pipeButton;
    void Interaction()
    {
        RaycastHit hit;
        if(Physics.Raycast(m_cam.transform.position, m_cam.transform.forward, out hit, m_range))
        {
            if(hit.transform.CompareTag("PipeButton"))
            {
                pipeButton = hit.transform.GetComponent<PipeButton>();
                Debug.Log("��������ư");

                //�ȳ��޼��������� �߰��ؾ���
                if (Input.GetMouseButtonDown(0))
                {
                    pipeButton.ActiveButton();
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        Interaction();
    }
}
