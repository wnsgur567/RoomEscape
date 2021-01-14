using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;

[System.Serializable]
public enum ZOOMSTATE
{
    NONE,
    ZOOMIN,
    ZOOMOUT,

    MAX

}


public class CharactorInteraction : MonoBehaviourPunCallbacks
{
    [Header("ĳ���Ϳ� ������Ʈ�� ��ȣ�ۿ� �Ÿ�")]
    [SerializeField]
    private float m_range;//��ȣ�ۿ� �Ÿ�

    [SerializeField]
    public Camera m_cam;//ī�޶�

    [SerializeField]
    private Image m_Crosshair;//ũ�ν����

    public PipeButton M_pipeButton;//��������ư ��ũ��Ʈ
    public BombCoverScript M_bombCoverScript;

    private PhotonView m_PV;
    private int m_PaintCount;
    private bool m_PaintStart;

    [SerializeField]
    private CharactorMove m_moveScript;

    [SerializeField]
    private Transform m_Zoom_pos;

    private Transform temp_pos;
    private GameObject temp_obj;

    ZOOMSTATE zoomState;

    [SerializeField]
    GameObject ClickPiece;             //Ŭ���� ü����


    private bool Iskey; 
    private bool IsUsb; 
    
    void Start()
    {
        Iskey = false;
        IsUsb = false;
        zoomState = ZOOMSTATE.NONE;
        m_PV = GetComponent<PhotonView>();
        MouseSetFalse();
        m_PaintCount = 0;
        m_PaintStart = false;

        if (m_PV.IsMine)
        {
            FindInit();
            
        }
    }



    void Update()
    {
        if (m_PV.IsMine)
        {

            if (zoomState == ZOOMSTATE.NONE)
                Interaction();
            else
                ZoomIn();
            
        }
    }

    public void MouseSetFalse()
    {
        zoomState = ZOOMSTATE.NONE;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_moveScript.M_Input = true;
    }

    public void MouseSetTrue()
    {
        zoomState = ZOOMSTATE.ZOOMIN;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        m_moveScript.M_Input = false;
    }

    void Interaction()
    {
        //if (!IsPointerOverUIObject())
        //{
        //    return;
        //}

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
                        Debug.Log("Activate PipePenalty");
                        return;
                    }

                    M_pipeButton = hit.transform.GetComponent<PipeButton>();
                    M_pipeButton.ActiveButton();


                    //hit.transform.Rotate(Vector3.forward, Time.deltaTime * 250f, Space.Self);

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
                        LightPenalty.instance.obj.SetActive(true);
                        hit.transform.gameObject.SetActive(false);
                        Debug.Log("������");
                    }
                }
            }
            else if (hit.transform.CompareTag("Radio"))
            {
                m_Crosshair.gameObject.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    GameManager.M_gameManager.StartRadio();
                }
            }
            else if (hit.transform.CompareTag("Bomb")) //������Ʈ�� ��ź�ΰ��
            {
                //��ȣ�ۿ� ũ�ν���� Ȱ��ȭ
                m_Crosshair.gameObject.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    m_Crosshair.gameObject.SetActive(false);
                    ////��ź ������ġ

                    m_tempObj = hit.transform.parent.gameObject;
                    m_tempCol = hit.collider;
                    m_tempTransform = hit.transform.parent.position;
                    m_tempTransformRotate = hit.transform.parent.eulerAngles;

                    m_moveScript.M_Input = false;
                    Debug.Log(hit.transform.tag);

                    temp_pos = hit.transform.parent;
                    temp_obj = hit.transform.parent.gameObject;

                    hit.collider.enabled = false;
                    hit.transform.parent.position = m_Zoom_pos.position;
                    hit.transform.parent.rotation = m_Zoom_pos.rotation;


                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;

                    zoomState = ZOOMSTATE.ZOOMIN;
                    //bomb = hit.transform.parent.GetComponent<ChessZoom>();
                    //bomb.ZoomInSet();
                }
            }
            else if (hit.transform.CompareTag("ChessGame"))
            {
                //��ȣ�ۿ� ũ�ν���� Ȱ��ȭ
                m_Crosshair.gameObject.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    m_Crosshair.gameObject.SetActive(false);

                    zoomState = ZOOMSTATE.ZOOMIN;

                    chess = hit.transform.GetComponent<ChessZoom>();
                    chess.ZoomInSet();
                }
            }
            else if (hit.transform.CompareTag("USB"))
            {
                m_Crosshair.gameObject.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    IsUsb = true;
                    hit.transform.gameObject.SetActive(false);
                }
            }
            else if (hit.transform.CompareTag("Key"))
            {
                m_Crosshair.gameObject.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    Iskey = true;
                    hit.transform.gameObject.SetActive(false);
                }
            }
            else
            {
                m_Crosshair.gameObject.SetActive(false);
            }
        }
        else
        {
            m_Crosshair.gameObject.SetActive(false);
        }
    }
    [SerializeField]
    private GameObject m_tempObj;
    private Collider m_tempCol;

    [SerializeField]
    Vector3 m_tempTransform;
    Vector3 m_tempTransformRotate;
    void ZoomIn()
    {
        //if (!IsPointerOverUIObject())
        //{
        //    return;
        //}

        //m_moveScript.M_Input = false;

        RaycastHit _hit;
        if (Physics.Raycast(m_cam.ScreenPointToRay(Input.mousePosition), out _hit))
        {
            //�ܵȻ��¶��
            if (Input.GetMouseButtonDown(0))
            {

                if (_hit.transform.CompareTag("Cover"))
                {
                    M_bombCoverScript = _hit.transform.GetComponent<BombCoverScript>();
                    if (M_bombCoverScript.M_State == BombCoverScript.Cover_State.Close)
                        M_bombCoverScript.M_State = BombCoverScript.Cover_State.Open;
                    else
                    {
                        M_bombCoverScript.M_State = BombCoverScript.Cover_State.Close;
                    }
                    Debug.Log("��źĿ��Ŭ��");
                }
                if (_hit.transform.CompareTag("Button"))
                {
                    DigitalClock.M_clock.M_currentSeconds -= 30f;
                }
                if(_hit.transform.CompareTag("CorrectButton"))
                {
                    //d���� ����
                }
                if(_hit.transform.CompareTag("KeyHolder"))
                {
                    if(Iskey)
                    {
                        _hit.collider.enabled = false;
                        _hit.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
                if(_hit.transform.CompareTag("USBHolder"))
                {
                    if (IsUsb)
                    {
                        _hit.collider.enabled = false;
                        _hit.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
                CutWire(_hit.transform.tag, _hit.transform.parent.gameObject);
            }

            ChessGameClick(_hit);
            //hit.transform.parent.transform.position = temp_pos.position;
            //hit.transform.parent.transform.rotation = temp_pos.rotation;


            //m_moveScript.M_Input = true;

        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Ű����");
            //m_tempObj.collider.enabled = true;
            m_tempCol.enabled = true;
            m_tempObj.transform.position = m_tempTransform;
            m_tempObj.transform.eulerAngles = m_tempTransformRotate;


            m_moveScript.M_Input = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            temp_obj.transform.parent = temp_pos;

            zoomState = ZOOMSTATE.NONE;
        }
    }

    ChessZoom chess;
    ChessZoom bomb;

    //ü������ ����
    void ChessGameClick(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0)
            && chess != null
            && chess.chessZoomState == ZOOMSTATE.ZOOMIN)
        {
            Debug.Log($"{hit.transform.tag}");

            //ü������ Ŭ��
            if (hit.transform.CompareTag("ChessBoard"))
            {
                // ü������(Ŭ���� ���� ��ġ)�� ������
                if (ClickPiece != null)
                {
                    //�÷��̾ ������ ��
                    Piece playerpiece = ClickPiece.GetComponent<Piece>();
                    //Ŭ���� ����
                    Board hitBoard = hit.transform.GetComponent<Board>();
                    BoardManager boardManager = hitBoard.boardManager;
                    Vector3 tempvec = new Vector3();

                    //�����̷��� ��ġ ���
                    tempvec.x = hitBoard.pieceInfo.Index.x - playerpiece.pieceInfo.Index.x;
                    tempvec.z = hitBoard.pieceInfo.Index.y - playerpiece.pieceInfo.Index.y;

                    //������ Ȯ��
                    if (playerpiece.IsMove(tempvec, hitBoard, false))
                    {
                        //�ݴ��� ü���ǵ� ������Ʈ
                        ChessMissionManager.Instance.UpdateChess(boardManager, playerpiece, hitBoard, null);
                        //�̵������� Ÿ�� ��Ȱ��ȭ
                        playerpiece.MoveTileFalse();
                        //������
                        playerpiece.PV.RPC("MoveTo", RpcTarget.AllBuffered,
                            (int)hitBoard.pieceInfo.playerType, (int)hitBoard.pieceInfo.chessPiece, hitBoard.pieceInfo.Index.x, hitBoard.pieceInfo.Index.y);/*.MoveTo(hitBoard, null);*/
                        //�÷��̾ ������ �� null
                        ClickPiece = null;

                    }
                }
            }
            else if (hit.transform.CompareTag("ChessPiece"))    //ü���� Ŭ��
            {
                //���� ������ ü���� null�� �ƴ� ���
                if (ClickPiece != null)
                {
                    //�÷��̾ ������ ü����
                    Piece playerpiece = ClickPiece.GetComponent<Piece>();
                    //���� ������ ü����
                    Piece hitpiece = hit.transform.GetComponent<Piece>();
                    //���� ������ ü������ ����Ŵ���
                    BoardManager boardManager = hitpiece.boardManager;
                    //Ŭ���� ���� ��ġ�� ����
                    Board hitBoard = boardManager.M_BoardArr[hitpiece.pieceInfo.Index.y, hitpiece.pieceInfo.Index.x];

                    //�ൿ���(Ŭ���� ���� ������ ������ ���� ���� ���)
                    if (hit.transform.gameObject == ClickPiece)
                    {
                        playerpiece.MoveTileFalse();
                        ClickPiece = null;

                        return;
                    }
                    //����� �� ���(Ŭ���� ���� ������ ������ ���� ���� �ٸ� ���)
                    if (playerpiece.pieceInfo.playerType != hitpiece.pieceInfo.playerType)
                    {
                        Vector3 tempvec = new Vector3();

                        //�����̷��� ��ġ ���
                        tempvec.x = hitBoard.pieceInfo.Index.x - playerpiece.pieceInfo.Index.x;
                        tempvec.z = hitBoard.pieceInfo.Index.y - playerpiece.pieceInfo.Index.y;

                        //������ ���� üũ �� ������
                        if (playerpiece.IsMove(tempvec, hitBoard, true))
                        {
                            //�ݴ��� ü���� ������Ʈ
                            ChessMissionManager.Instance.UpdateChess(boardManager, playerpiece, hitBoard, hitpiece);
                            //������
                            playerpiece.PV.RPC("MoveTo", RpcTarget.AllBuffered,
                                (int)hitBoard.pieceInfo.playerType, (int)hitBoard.pieceInfo.chessPiece, hitBoard.pieceInfo.Index.x, hitBoard.pieceInfo.Index.y,
                                (int)hitpiece.pieceInfo.playerType, (int)hitpiece.pieceInfo.chessPiece, hitpiece.pieceInfo.Index.x, hitpiece.pieceInfo.Index.y);/*.MoveTo(hitBoard, hitpiece);*/
                            //ü���� ���� ����
                            ClickPiece = null;
                            //hitpiece.gameObject.SetActive(false);
                            //�̼Ǽ���?
                            if (ChessMissionManager.Instance.isMission(hitpiece.pieceInfo))
                            {
                                hitpiece.gameObject.SetActive(true);
                            }
                        }
                    }
                    else //�÷��̾� �� �ٲٱ�(������ ���� ���� ������ ������ ���� ���� ���� ���)
                    {

                        playerpiece.MoveTileFalse();
                        ClickPiece = hit.transform.gameObject;
                        playerpiece = ClickPiece.GetComponent<Piece>();
                        playerpiece.MoveTileTrue();
                    }
                }
                else //�÷��̾� ���� �� ����(������ ������ ���� null�� ���)
                {
                    Piece playerpiece = hit.transform.GetComponent<Piece>();
                    BoardManager boardManager = playerpiece.boardManager;

                    if (playerpiece.pieceInfo.playerType == ChessMissionManager.Instance.TurnPlayer
                        && boardManager.playerType == ChessMissionManager.Instance.TurnPlayer)
                    {
                        ClickPiece = hit.transform.gameObject;
                        playerpiece.MoveTileTrue();
                    }


                }
            }

        }
    }

    void FindInit()
    {
        BoardManager[] tempObj;
        tempObj = GameObject.FindObjectsOfType<BoardManager>();

        foreach(BoardManager boardManager in tempObj)
        {
            boardManager.ClickManager = this;
        }

        ChessZoom[] chesszomm;
        chesszomm = GameObject.FindObjectsOfType<ChessZoom>();

        foreach (ChessZoom chess in chesszomm)
        {
            chess.Player = this;
        }
    }

    public void ClickChessPieceNull()
    {
        ClickPiece = null;
    }

    void CutWire(string Wire_name, GameObject Hide_Wire)
    {
        switch (Wire_name)
        {
            case "RedWire":
                m_PV.RPC("BombTimePenalty", RpcTarget.AllBuffered);
                BombScript.M_instance.M_cutRed.SetActive(true);
                Hide_Wire.SetActive(false);
                break;
            case "BlueWire":
                m_PV.RPC("BombTimePenalty", RpcTarget.AllBuffered);
                BombScript.M_instance.M_cutBlue.SetActive(true);
                Hide_Wire.SetActive(false);
                break;
            case "BlackWire":
                BombScript.M_instance.M_cutBlack.SetActive(true);
                Hide_Wire.SetActive(false);
                break;
            case "WhiteWire":
                m_PV.RPC("BombTimePenalty", RpcTarget.AllBuffered);
                BombScript.M_instance.M_cutWhite.SetActive(true);
                Hide_Wire.SetActive(false);
                break;
            case "YellowWire":
                m_PV.RPC("BombTimePenalty", RpcTarget.AllBuffered);
                BombScript.M_instance.M_cutYellow.SetActive(true);
                Hide_Wire.SetActive(false);
                break;
            case "GreenWire":
                m_PV.RPC("BombTimePenalty", RpcTarget.AllBuffered);
                BombScript.M_instance.M_cutGreen.SetActive(true);
                Hide_Wire.SetActive(false);
                break;
        }
    }

    [PunRPC]
    void BombTimePenalty()
    {
        DigitalClock.M_clock.M_currentSeconds -= 30f;
    }


    
    private bool IsPointerOverUIObject()
    {
        List<RaycastResult> results = GetUIObjectsUnderPointer();
        return results.Count > 0;
    }

    private List<RaycastResult> GetUIObjectsUnderPointer()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results;
    }
}
