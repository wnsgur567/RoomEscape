using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class CharactorInteraction : MonoBehaviourPunCallbacks
{
    [Header("ĳ���Ϳ� ������Ʈ�� ��ȣ�ۿ� �Ÿ�")]
    [SerializeField]
    private float m_range;//��ȣ�ۿ� �Ÿ�

    [SerializeField]
    private Camera m_cam;//ī�޶�

    [SerializeField]
    private Image m_Crosshair;//ũ�ν����

    public PipeButton M_pipeButton;//��������ư ��ũ��Ʈ

    private PhotonView m_PV;
    private int m_PaintCount;
    private bool m_PaintStart;

    [SerializeField]
    private CharactorMove m_moveScript;

    [SerializeField]
    private Transform m_Zoom_pos;
    public enum State
    {
        Normal = 0,
        ZoomIn
    }
    void Start()
    {
        m_PV = GetComponent<PhotonView>();
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
        m_PaintCount = 0;
        m_PaintStart = false;
    }
    void Update()
    {
        //���콺Ŀ�� Ȯ���ҷ��� �ӽ÷� ������
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if(m_PV.IsMine)
            Interaction();

    }
    void Interaction()
    {
        RaycastHit hit;
        //ī�޶� �������� ������� m_range��ŭ ���̸� ��
        if (Physics.Raycast(m_cam.transform.position, m_cam.transform.forward, out hit, m_range))
        {
            //������Ʈ�� ������������ ���
            if (hit.transform.CompareTag("PipeButton"))
            {
                //��ȣ�ۿ� ũ�ν���� Ȱ��ȭ
                m_Crosshair.gameObject.SetActive(true);

                //�ȳ��޼��������� �߰��ؾ���

                //������ ��ư���� ���콺������ ������ ����
                if (Input.GetMouseButtonDown(0))
                {
                    if (PipePuzzleManager.M_pipeManager.M_IsStarted == false)
                    {
                        PipePuzzleManager.M_pipeManager.M_IsStarted = true;
                        Debug.Log("���������� ����");
                        return;
                    }
                    else
                    {
                        //Ÿ�̸� ����
                    }

                    M_pipeButton = hit.transform.GetComponent<PipeButton>();
                    M_pipeButton.ActiveButton();
                }
            }
            //������Ʈ�� �׸������� ���
            if (hit.transform.CompareTag("Paint")  || hit.transform.CompareTag("Paint_correct"))
            {
                //��ȣ�ۿ� ũ�ν���� Ȱ��ȭ
                m_Crosshair.gameObject.SetActive(true);

                //�ȳ��޼��������� �߰��ؾ���

                if (Input.GetMouseButtonDown(0))
                {
                    //�׸� ��ȣ�ۿ��� ù��°(����) �̶��
                    if(m_PaintStart == false)
                    {
                        m_PaintStart = true;
                        Debug.Log("�׸��������");
                        return;
                    }
                    else
                    {
                        m_PaintCount++;
                    }

                    //�׸� ��ȣ�ۿ� ī��Ʈ�� ������Ű��

                    Debug.Log(m_PaintCount);
                    if (hit.transform.CompareTag("Paint"))
                    {
                        Debug.Log("�׸���");
                        //�׸��� Ŭ���ϸ�
                        //���������̴� �г�Ƽ
                        if (m_PaintCount == 10)
                        {

                        }
                        //�׸��̸�
                    }
                    else if(hit.transform.CompareTag("Paint_correct"))
                    {
                        //�����̸�
                        //��Ȱ��ȭ��Ŵ
                        hit.transform.gameObject.SetActive(false);
                        Debug.Log("������");
                    }
                    //M_pipeButton.ActiveButton();
                }
            }

            //������Ʈ�� ��ź�ΰ��
            if(hit.transform.CompareTag("Bomb"))
            {
                //��ȣ�ۿ� ũ�ν���� Ȱ��ȭ
                m_Crosshair.gameObject.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    m_Crosshair.gameObject.SetActive(false);
                    Debug.Log(hit.transform.tag);
                    Transform temp_pos = hit.transform.parent;

                    hit.collider.enabled = false;
                    hit.transform.parent.position = m_Zoom_pos.position;
                    hit.transform.parent.rotation = m_Zoom_pos.rotation;
                    
                    m_moveScript.M_Input = false;
                    
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    
                    if (Input.GetMouseButtonDown(0))
                    {
                        RaycastHit _hit;
                        if (Physics.Raycast(m_cam.ScreenPointToRay(Input.mousePosition),out _hit))
                        {
                            Debug.Log(_hit.transform.tag);
                        }
                        //hit.transform.parent.transform.position = temp_pos.position;
                        //hit.transform.parent.transform.rotation = temp_pos.rotation;

                        //Cursor.visible = false;
                        //Cursor.lockState = CursorLockMode.Locked;
                        //m_moveScript.M_Input = true;

                    }
                }
            }
        }
        else
        {
            //��ȣ�ۿ��Ұ� ������ ũ�ν���� ����
            m_Crosshair.gameObject.SetActive(false);
        }
    }
    
}
