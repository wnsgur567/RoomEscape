using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MouseClick : MonoBehaviourPun
{
    public float m_range;
    public Camera m_cam;

    public GameObject ClickObj;             //클릭한 체스말
    public List<BoxCollider> ChessGame;   //상호작용하는 체스게임
    //public BoardManager boardManager;

    //체스게임 조작
    void ChessGameClick(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log($"{hit.transform.tag}");

            //체스보드 클릭
            if (hit.transform.CompareTag("ChessBoard"))
            {
                // 체스보드(클릭한 보드 위치)로 움직임
                if (ClickObj != null)
                {
                    //플레이어가 선택한 말
                    Piece playerpiece = ClickObj.GetComponent<Piece>();
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
                        ClickObj = null;

                    }
                }
            }
            else if (hit.transform.CompareTag("ChessPiece"))    //체스말 클릭
            {
                //현재 선택한 체스말 null이 아닐 경우
                if (ClickObj != null)
                {
                    //플레이어가 선택한 체스말
                    Piece playerpiece = ClickObj.GetComponent<Piece>();
                    //새로 선택한 체스말
                    Piece hitpiece = hit.transform.GetComponent<Piece>();
                    //새로 선택한 체스말의 보드매니져
                    BoardManager boardManager = hitpiece.boardManager;
                    //클릭한 보드 위치의 보드
                    Board hitBoard = boardManager.M_BoardArr[hitpiece.pieceInfo.Index.y, hitpiece.pieceInfo.Index.x];

                    //행동취소(클릭한 말이 이전에 선택한 말과 같을 경우)
                    if (hit.transform.gameObject == ClickObj)
                    {
                        playerpiece.MoveTileFalse();
                        ClickObj = null;

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
                            ClickObj = null;
                            //hitpiece.gameObject.SetActive(false);
                            //미션성공?
                            if(ChessMissionManager.Instance.isMission(hitpiece.pieceInfo))
                            {
                                hitpiece.gameObject.SetActive(true);
                            }
                        }
                    }
                    else //플레이어 말 바꾸기(선택한 말의 색이 이전에 선택한 말의 색과 같을 경우)
                    {
                       
                        playerpiece.MoveTileFalse();
                        ClickObj = hit.transform.gameObject;
                        playerpiece = ClickObj.GetComponent<Piece>();
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
                        ClickObj = hit.transform.gameObject;
                        playerpiece.MoveTileTrue();
                    }

                    
                }
            }

        }
    }

    void Interaction()
    {
        Ray ray = m_cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //if (Physics.Raycast(m_cam.transform.position, m_cam.transform.forward, out hit, m_range))
        //{

        if (Physics.Raycast(ray, out hit, m_range))
        {
            if (Input.GetMouseButtonDown(0))
            {
                //체스게임 활성화
                if (hit.transform.tag == "ChessGame")
                {
                    ChessMissionManager.Instance.__Init();

                    foreach (BoxCollider gameObject in ChessGame)
                    {
                        gameObject.transform.gameObject.SetActive(false);
                    }
                }
            }

            ChessGameClick(hit);
        }
    }

    
    void Update()
    {
        Interaction();
    }
}
