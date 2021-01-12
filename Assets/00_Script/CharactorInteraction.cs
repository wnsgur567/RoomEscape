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
    public BombScript M_bombScript;

    private PhotonView m_PV;
    private int m_PaintCount;
    private bool m_PaintStart;

    [SerializeField]
    private CharactorMove m_moveScript;

    [SerializeField]
    private Transform m_Zoom_pos;

    private Transform temp_pos;
    private GameObject temp_obj;
    public enum State
    {
        Normal = 0,
        ZoomIn
    }
    State state;
    void Start()
    {
        state = State.Normal;
        m_PV = GetComponent<PhotonView>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_PaintCount = 0;
        m_PaintStart = false;
    }
    void Update()
    {
        if (m_PV.IsMine)
        {
            if (state == State.Normal)
                Interaction();
            else
                ZoomIn();
        }
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
                        DigitalClock.M_clock.M_puzzleTimer = DigitalClock.M_clock.M_currentSeconds - 120f;
                        Debug.Log("���������� ����");
                        return;
                    }

                    M_pipeButton = hit.transform.GetComponent<PipeButton>();
                    M_pipeButton.ActiveButton();

                    hit.transform.Rotate(Vector3.back, Time.deltaTime * 250f, Space.Self);
                }
            }
            else if (hit.transform.CompareTag("Paint") || hit.transform.CompareTag("Paint_correct")) //������Ʈ�� �׸������� ���
            {
                //��ȣ�ۿ� ũ�ν���� Ȱ��ȭ
                m_Crosshair.gameObject.SetActive(true);

                //�ȳ��޼��������� �߰��ؾ���

                if (Input.GetMouseButtonDown(0))
                {
                    //�׸� ��ȣ�ۿ��� ù��°(����) �̶��
                    if (m_PaintStart == false)
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
                        LightPenalty.instance.M_IsPenalty = true;
                        LightPenalty.instance.StartPenalty();
                        if (m_PaintCount == 10)
                        {

                        }
                        //�׸��̸�
                    }
                    else if (hit.transform.CompareTag("Paint_correct"))
                    {
                        //�����̸�
                        //��Ȱ��ȭ��Ŵ

                        LightPenalty.instance.M_IsPenalty = false;
                        hit.transform.gameObject.SetActive(false);
                        Debug.Log("������");
                    }
                    //M_pipeButton.ActiveButton();
                }
            }
            else if (hit.transform.CompareTag("Bomb")) //������Ʈ�� ��ź�ΰ��
            {
                //��ȣ�ۿ� ũ�ν���� Ȱ��ȭ
                m_Crosshair.gameObject.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    m_Crosshair.gameObject.SetActive(false);
                    m_tempHit = hit;
                    m_moveScript.M_Input = false;
                    m_tempTransform = hit.transform.parent;
                    Debug.Log(hit.transform.tag);

                    temp_pos = hit.transform.parent;
                    temp_obj = hit.transform.parent.gameObject;

                    hit.collider.enabled = false;
                    hit.transform.parent.position = m_Zoom_pos.position;
                    hit.transform.parent.rotation = m_Zoom_pos.rotation;


                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    state = State.ZoomIn;
                }
            }
            else
            {
                m_Crosshair.gameObject.SetActive(false);
            }
        }
        //else if(hit.transform.CompareTag("Wall") || hit.transform.CompareTag("Untagged"))
        //{
        //    //��ȣ�ۿ��Ұ� ������ ũ�ν���� ����
        //    m_Crosshair.gameObject.SetActive(false);
        //}
        else
        {
            m_Crosshair.gameObject.SetActive(false);
        }
    }
    private RaycastHit m_tempHit;
    private Transform m_tempTransform;
    void ZoomIn()
    {
        //�ܵȻ��¶��
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit _hit;
            if (Physics.Raycast(m_cam.ScreenPointToRay(Input.mousePosition), out _hit))
            {
                if (_hit.transform.CompareTag("Cover"))
                {
                    M_bombScript = _hit.transform.GetComponent<BombScript>();
                    if (M_bombScript.M_State == BombScript.Cover_State.Close)
                        M_bombScript.M_State = BombScript.Cover_State.Open;
                    else
                    {
                        M_bombScript.M_State = BombScript.Cover_State.Close;
                    }
                    Debug.Log("��źĿ��Ŭ��");
                }
                Debug.Log(_hit.transform.tag);

            }
            //hit.transform.parent.transform.position = temp_pos.position;
            //hit.transform.parent.transform.rotation = temp_pos.rotation;


            //m_moveScript.M_Input = true;

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Ű����");
            m_tempHit.collider.enabled = true;
            m_tempHit.transform.position = m_tempTransform.position;


            m_moveScript.M_Input = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            temp_obj.transform.parent = temp_pos;

            state = State.Normal;
        }
    }
}
