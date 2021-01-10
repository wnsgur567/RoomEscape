using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    public float m_range;
    public Transform m_cam;

    public GameObject ClickObj;
    //public BoardManager boardManager;

    void ChessGameClick(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log($"{hit.transform.tag}");

            if (hit.transform.CompareTag("ChessBoard"))
            {
                // 체스보드(클릭한 보드 위치)로 움직임
                if (ClickObj != null)
                {
                    Piece playerpiece = ClickObj.GetComponent<Piece>();
                    Board hitBoard = hit.transform.GetComponent<Board>();
                    BoardManager boardManager = hitBoard.boardManager;
                    Vector3 tempvec = new Vector3();

                    tempvec.x = hitBoard.pieceInfo.Index.x - playerpiece.pieceInfo.Index.x;
                    tempvec.z = hitBoard.pieceInfo.Index.y - playerpiece.pieceInfo.Index.y;


                    if (playerpiece.IsMove(tempvec, hitBoard, false))
                    {
                        ChessMissionManager.Instance.UpdateChess(boardManager, playerpiece, hitBoard, null);
                        playerpiece.MoveTileFalse();
                        playerpiece.MoveTo(hitBoard, null);
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
                    BoardManager boardManager = hitpiece.boardManager;
                    Board hitBoard = boardManager.M_BoardArr[hitpiece.pieceInfo.Index.y, hitpiece.pieceInfo.Index.x];

                    //행동취소
                    if (hit.transform.gameObject == ClickObj)
                    {
                        playerpiece = ClickObj.GetComponent<Piece>();
                        playerpiece.MoveTileFalse();
                        ClickObj = null;

                        return;
                    }
                    //상대편 말 잡기
                    if (playerpiece.pieceInfo.playerType != hitpiece.pieceInfo.playerType)
                    {
                        Vector3 tempvec = new Vector3();

                        tempvec.x = hitBoard.pieceInfo.Index.x - playerpiece.pieceInfo.Index.x;
                        tempvec.z = hitBoard.pieceInfo.Index.y - playerpiece.pieceInfo.Index.y;

                        //움직임 여부 체크 후 움직임
                        if (playerpiece.IsMove(tempvec, hitBoard, true))
                        {
                            ChessMissionManager.Instance.UpdateChess(boardManager, playerpiece, hitBoard, hitpiece);
                            playerpiece.MoveTo(hitBoard, hitpiece);
                            ClickObj = null;
                            //hitpiece.gameObject.SetActive(false);
                            //미션성공?
                            if(ChessMissionManager.Instance.isMission(hitpiece.pieceInfo))
                            {
                                hitpiece.gameObject.SetActive(true);
                            }
                        }
                    }
                    else
                    {
                        //플레이어 말 바꾸기
                        playerpiece.MoveTileFalse();
                        ClickObj = hit.transform.gameObject;
                        playerpiece = ClickObj.GetComponent<Piece>();
                        playerpiece.MoveTileTrue();
                    }
                }
                else //플레이어 본인 말 선택
                {
                    Piece playerpiece = hit.transform.GetComponent<Piece>();
                    BoardManager boardManager = playerpiece.boardManager;

                    if (playerpiece.pieceInfo.playerType == ChessMissionManager.Instance.TurnPlayer
                        && boardManager.playerType == ChessMissionManager.Instance.TurnPlayer)
                    {
                        if(ChessMissionManager.Instance.boardManagerList[0].playerType == boardManager.playerType)
                        {

                        }
                        else
                        {

                        }


                        ClickObj = hit.transform.gameObject;
                        playerpiece.MoveTileTrue();
                    }

                    
                }
            }

        }
    }

    void Interaction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(m_cam.transform.position, m_cam.transform.forward, out hit, m_range))
        {
/*
            if (Physics.Raycast(ray, out hit, m_range))
        {*/
            ChessGameClick(hit);
        }
    }

    
    void Update()
    {
        Interaction();
    }
}
