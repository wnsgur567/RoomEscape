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
    private Transform m_cam;//ī�޶�

    [SerializeField]
    private Image m_Crosshair;//ũ�ν����

    public PipeButton M_pipeButton;//��������ư ��ũ��Ʈ

    private PhotonView m_PV;
    private int m_PaintCount;
    private bool m_PaintStart;
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (m_PV.IsMine)
        {
            Interaction();
         
            if (Input.GetMouseButton(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
            }
        }
    }
    public float rotate_spd = 100.0f;

    private void OnMouseDrag()
    {
        float temp_x_axis = Input.GetAxis("Mouse X") * rotate_spd * Time.deltaTime;
        float temp_y_axis = Input.GetAxis("Mouse Y") * rotate_spd * Time.deltaTime;
        transform.Rotate(temp_y_axis, -temp_x_axis, 0, Space.World);

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
        }
        else
        {
            //��ȣ�ۿ��Ұ� ������ ũ�ν���� ����
            m_Crosshair.gameObject.SetActive(false);
        }
    }
    
}
