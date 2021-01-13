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
    [Header("캐릭터와 오브젝트의 상호작용 거리")]
    [SerializeField]
    private float m_range;//상호작용 거리

    [SerializeField]
    public Camera m_cam;//카메라

    [SerializeField]
    private Image m_Crosshair;//크로스헤어

    public PipeButton M_pipeButton;//파이프버튼 스크립트
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

    ZOOMSTATE zoomState;

    [SerializeField]
    GameObject ClickPiece;             //클릭한 체스말


    void Start()
    {
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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MouseSetTrue()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Interaction()
    {
        //if (!IsPointerOverUIObject())
        //{
        //    return;
        //}

            RaycastHit hit;
        //카메라 기준으로 가운데에다 m_range만큼 레이를 쏨
        if (Physics.Raycast(m_cam.transform.position, m_cam.transform.forward, out hit, m_range))
        {


            //오브젝트가 파이프퍼즐인 경우
            if (hit.transform.CompareTag("PipeButton"))
            {
                //상호작용 크로스헤어 활성화
                m_Crosshair.gameObject.SetActive(true);

                //안내메세지같은거 추가해야함

                //파이프 버튼에서 마우스누르면 파이프 실행
                if (Input.GetMouseButtonDown(0))
                {
                    if (PipePuzzleManager.M_pipeManager.M_IsStarted == false)
                    {
                        PipePuzzleManager.M_pipeManager.M_IsStarted = true;
                        DigitalClock.M_clock.M_puzzleTimer = DigitalClock.M_clock.M_currentSeconds - 120f;
                        Debug.Log("파이프퍼즐 시작");
                        return;
                    }

                    M_pipeButton = hit.transform.GetComponent<PipeButton>();
                    M_pipeButton.ActiveButton();

                    hit.transform.Rotate(Vector3.back, Time.deltaTime * 250f, Space.Self);
                }
            }
            else if (hit.transform.CompareTag("Paint") || hit.transform.CompareTag("Paint_correct")) //오브젝트가 그림퍼즐인 경우
            {
                //상호작용 크로스헤어 활성화
                m_Crosshair.gameObject.SetActive(true);

                //안내메세지같은거 추가해야함

                if (Input.GetMouseButtonDown(0))
                {
                    //그림 상호작용이 첫번째(시작) 이라면
                    if (m_PaintStart == false)
                    {
                        m_PaintStart = true;
                        Debug.Log("그림퍼즐시작");
                        return;
                    }
                    else
                    {
                        m_PaintCount++;
                    }

                    //그림 상호작용 카운트를 증가시키고

                    Debug.Log(m_PaintCount);
                    if (hit.transform.CompareTag("Paint"))
                    {
                        Debug.Log("그림임");
                        //그림을 클릭하면
                        //전구깜빡이는 패널티
                        LightPenalty.instance.M_IsPenalty = true;
                        LightPenalty.instance.StartPenalty();
                        if (m_PaintCount == 10)
                        {

                        }
                        //그림이면
                    }
                    else if (hit.transform.CompareTag("Paint_correct"))
                    {
                        //정답이면
                        //비활성화시킴

                        LightPenalty.instance.M_IsPenalty = false;
                        hit.transform.gameObject.SetActive(false);
                        Debug.Log("정답임");
                    }
                    //M_pipeButton.ActiveButton();
                }
            }
            else if (hit.transform.CompareTag("Bomb")) //오브젝트가 폭탄인경우
            {
                //상호작용 크로스헤어 활성화
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
                    zoomState = ZOOMSTATE.ZOOMIN;
                }
            }
            else if (hit.transform.CompareTag("ChessGame"))
            {
                //상호작용 크로스헤어 활성화
                m_Crosshair.gameObject.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    m_Crosshair.gameObject.SetActive(false);

                    zoomState = ZOOMSTATE.ZOOMIN;

                    chess = hit.transform.GetComponent<ChessZoom>();
                    chess.ZoomInSet();
                }
            }
            else
            {
                m_Crosshair.gameObject.SetActive(false);
            }
        }
        //else if(hit.transform.CompareTag("Wall") || hit.transform.CompareTag("Untagged"))
        //{
        //    //상호작용할게 없으면 크로스헤어 끄기
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
        //if (!IsPointerOverUIObject())
        //{
        //    return;
        //}

        m_moveScript.M_Input = false;
        MouseSetTrue();

        RaycastHit _hit;
        if (Physics.Raycast(m_cam.ScreenPointToRay(Input.mousePosition), out _hit))
        {
            //줌된상태라면
            if (Input.GetMouseButtonDown(0))
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
                    Debug.Log("폭탄커버클릭");
                }
                Debug.Log(_hit.transform.tag);

            }

            ChessGameClick(_hit);
            //hit.transform.parent.transform.position = temp_pos.position;
            //hit.transform.parent.transform.rotation = temp_pos.rotation;


            //m_moveScript.M_Input = true;

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("키눌림");
            m_tempHit.collider.enabled = true;
            m_tempHit.transform.position = m_tempTransform.position;


            m_moveScript.M_Input = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            temp_obj.transform.parent = temp_pos;

            zoomState = ZOOMSTATE.NONE;
        }
    }

    ChessZoom chess;

    //체스게임 조작
    void ChessGameClick(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0)
            && chess != null
            && chess.chessZoomState == ZOOMSTATE.ZOOMIN)
        {
            Debug.Log($"{hit.transform.tag}");

            //체스보드 클릭
            if (hit.transform.CompareTag("ChessBoard"))
            {
                // 체스보드(클릭한 보드 위치)로 움직임
                if (ClickPiece != null)
                {
                    //플레이어가 선택한 말
                    Piece playerpiece = ClickPiece.GetComponent<Piece>();
                    //클릭한 보드
                    Board hitBoard = hit.transform.GetComponent<Board>();
                    BoardManager boardManager = hitBoard.boardManager;
                    Vector3 tempvec = new Vector3();

                    //움직이려는 위치 계산
                    tempvec.x = hitBoard.pieceInfo.Index.x - playerpiece.pieceInfo.Index.x;
                    tempvec.z = hitBoard.pieceInfo.Index.y - playerpiece.pieceInfo.Index.y;

                    //움직임 확인
                    if (playerpiece.IsMove(tempvec, hitBoard, false))
                    {
                        //반대편 체스판도 업데이트
                        ChessMissionManager.Instance.UpdateChess(boardManager, playerpiece, hitBoard, null);
                        //이동가능한 타일 비활성화
                        playerpiece.MoveTileFalse();
                        //움직임
                        playerpiece.PV.RPC("MoveTo", RpcTarget.AllBuffered,
                            (int)hitBoard.pieceInfo.playerType, (int)hitBoard.pieceInfo.chessPiece, hitBoard.pieceInfo.Index.x, hitBoard.pieceInfo.Index.y);/*.MoveTo(hitBoard, null);*/
                        //플레이어가 선택한 말 null
                        ClickPiece = null;

                    }
                }
            }
            else if (hit.transform.CompareTag("ChessPiece"))    //체스말 클릭
            {
                //현재 선택한 체스말 null이 아닐 경우
                if (ClickPiece != null)
                {
                    //플레이어가 선택한 체스말
                    Piece playerpiece = ClickPiece.GetComponent<Piece>();
                    //새로 선택한 체스말
                    Piece hitpiece = hit.transform.GetComponent<Piece>();
                    //새로 선택한 체스말의 보드매니져
                    BoardManager boardManager = hitpiece.boardManager;
                    //클릭한 보드 위치의 보드
                    Board hitBoard = boardManager.M_BoardArr[hitpiece.pieceInfo.Index.y, hitpiece.pieceInfo.Index.x];

                    //행동취소(클릭한 말이 이전에 선택한 말과 같을 경우)
                    if (hit.transform.gameObject == ClickPiece)
                    {
                        playerpiece.MoveTileFalse();
                        ClickPiece = null;

                        return;
                    }
                    //상대편 말 잡기(클릭한 말이 이전에 선택한 말과 색이 다를 경우)
                    if (playerpiece.pieceInfo.playerType != hitpiece.pieceInfo.playerType)
                    {
                        Vector3 tempvec = new Vector3();

                        //움직이려는 위치 계산
                        tempvec.x = hitBoard.pieceInfo.Index.x - playerpiece.pieceInfo.Index.x;
                        tempvec.z = hitBoard.pieceInfo.Index.y - playerpiece.pieceInfo.Index.y;

                        //움직임 여부 체크 후 움직임
                        if (playerpiece.IsMove(tempvec, hitBoard, true))
                        {
                            //반대편 체스판 업데이트
                            ChessMissionManager.Instance.UpdateChess(boardManager, playerpiece, hitBoard, hitpiece);
                            //움직임
                            playerpiece.PV.RPC("MoveTo", RpcTarget.AllBuffered,
                                (int)hitBoard.pieceInfo.playerType, (int)hitBoard.pieceInfo.chessPiece, hitBoard.pieceInfo.Index.x, hitBoard.pieceInfo.Index.y,
                                (int)hitpiece.pieceInfo.playerType, (int)hitpiece.pieceInfo.chessPiece, hitpiece.pieceInfo.Index.x, hitpiece.pieceInfo.Index.y);/*.MoveTo(hitBoard, hitpiece);*/
                            //체스말 선택 해제
                            ClickPiece = null;
                            //hitpiece.gameObject.SetActive(false);
                            //미션성공?
                            if (ChessMissionManager.Instance.isMission(hitpiece.pieceInfo))
                            {
                                hitpiece.gameObject.SetActive(true);
                            }
                        }
                    }
                    else //플레이어 말 바꾸기(선택한 말의 색이 이전에 선택한 말의 색과 같을 경우)
                    {

                        playerpiece.MoveTileFalse();
                        ClickPiece = hit.transform.gameObject;
                        playerpiece = ClickPiece.GetComponent<Piece>();
                        playerpiece.MoveTileTrue();
                    }
                }
                else //플레이어 본인 말 선택(이전에 선택한 말이 null일 경우)
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
