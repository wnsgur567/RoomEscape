using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class CharactorInteraction : MonoBehaviourPunCallbacks
{
    [Header("캐릭터와 오브젝트의 상호작용 거리")]
    [SerializeField]
    private float m_range;//상호작용 거리

    [SerializeField]
    private Transform m_cam;//카메라

    [SerializeField]
    private Image m_Crosshair;//크로스헤어

    public PipeButton M_pipeButton;//파이프버튼 스크립트

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
        //마우스커서 확인할려고 임시로 넣은거
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
        //카메라 기준으로 가운데에다 m_range만큼 레이를 쏨
        if (Physics.Raycast(m_cam.transform.position, m_cam.transform.forward, out hit, m_range))
        {
            Debug.Log($"{hit.transform.tag}");

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
                        Debug.Log("파이프퍼즐 시작");
                        return;
                    }
                    else
                    {
                        //타이머 시작
                    }

                    M_pipeButton = hit.transform.GetComponent<PipeButton>();
                    M_pipeButton.ActiveButton();
                }
            }
            //오브젝트가 그림퍼즐인 경우
            if (hit.transform.CompareTag("Paint")  || hit.transform.CompareTag("Paint_correct"))
            {
                //상호작용 크로스헤어 활성화
                m_Crosshair.gameObject.SetActive(true);

                //안내메세지같은거 추가해야함

                if (Input.GetMouseButtonDown(0))
                {
                    //그림 상호작용이 첫번째(시작) 이라면
                    if(m_PaintStart == false)
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
                        if (m_PaintCount == 10)
                        {

                        }
                        //그림이면
                    }
                    else if(hit.transform.CompareTag("Paint_correct"))
                    {
                        //정답이면
                        //비활성화시킴
                        hit.transform.gameObject.SetActive(false);
                        Debug.Log("정답임");
                    }
                    //M_pipeButton.ActiveButton();
                }
            }

            if(hit.transform.CompareTag("ChessPiece") || hit.transform.CompareTag("ChessBoard"))
            {
                //상호작용 크로스헤어 활성화
                m_Crosshair.gameObject.SetActive(true);
                ChessGameClick(hit);
            }


        }
        else
        {
            //상호작용할게 없으면 크로스헤어 끄기
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
                            //미션성공?
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
