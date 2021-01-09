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
            Debug.Log($"{hit.transform.tag}");

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

            if(hit.transform.CompareTag("ChessPiece") || hit.transform.CompareTag("ChessBoard"))
            {
                //��ȣ�ۿ� ũ�ν���� Ȱ��ȭ
                m_Crosshair.gameObject.SetActive(true);
                ChessGameClick(hit);
            }


        }
        else
        {
            //��ȣ�ۿ��Ұ� ������ ũ�ν���� ����
            m_Crosshair.gameObject.SetActive(false);
        }
    }

    public GameObject ClickObj;
    void ChessGameClick(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log($"{hit.transform.tag}");

            if (hit.transform.CompareTag("ChessBoard"))
            {
                if (ClickObj != null)
                {
                    Piece playerpiece = ClickObj.GetComponent<Piece>();
                    Board hitBoard = hit.transform.GetComponent<Board>();
                    Vector3 tempvec = new Vector3();

                    tempvec.x = hitBoard.pieceInfo.Index.x - playerpiece.pieceInfo.Index.x;
                    tempvec.z = hitBoard.pieceInfo.Index.y - playerpiece.pieceInfo.Index.y;


                    if (playerpiece.IsMove(tempvec, hitBoard, false))
                    {
                        playerpiece.MoveTileFalse();
                        playerpiece.MoveTo(hitBoard);
                        ClickObj = null;

                    }
                }
            }
            else if (hit.transform.CompareTag("ChessPiece"))
            {
                if (ClickObj != null)
                {
                    Piece playerpiece = ClickObj.GetComponent<Piece>();


                    Piece hitpiece = hit.transform.GetComponent<Piece>();
                    Board hitBoard = BoardManager.Instance.M_BoardArr[hitpiece.pieceInfo.Index.y, hitpiece.pieceInfo.Index.x];

                    if (hit.transform.gameObject == ClickObj)
                    {
                        playerpiece = ClickObj.GetComponent<Piece>();
                        playerpiece.MoveTileFalse();
                        ClickObj = null;

                        return;
                    }

                    if (playerpiece.pieceInfo.playerType != hitpiece.pieceInfo.playerType)
                    {
                        Vector3 tempvec = new Vector3();

                        tempvec.x = hitBoard.pieceInfo.Index.x - playerpiece.pieceInfo.Index.x;
                        tempvec.z = hitBoard.pieceInfo.Index.y - playerpiece.pieceInfo.Index.y;


                        if (playerpiece.IsMove(tempvec, hitBoard, true))
                        {

                            playerpiece.MoveTo(hitBoard);
                            //ClickObj = null;
                            hitpiece.gameObject.SetActive(false);
                            //�̼Ǽ���?
                            if (!ChessMissionManager.Instance.isMission(hitpiece.pieceInfo))
                            {
                                hitpiece.gameObject.SetActive(true);
                            }
                        }
                    }
                    else
                    {
                        playerpiece.MoveTileFalse();
                        ClickObj = hit.transform.gameObject;
                        playerpiece = ClickObj.GetComponent<Piece>();
                        playerpiece.MoveTileTrue();
                    }
                }
                else
                {
                    Piece playerpiece = hit.transform.GetComponent<Piece>();
                    if (playerpiece.pieceInfo.playerType != ChessMissionManager.Instance.TurnPlayer)
                    {
                        return;
                    }

                    ClickObj = hit.transform.gameObject;
                    playerpiece.MoveTileTrue();
                }
            }

        }
    }

}
