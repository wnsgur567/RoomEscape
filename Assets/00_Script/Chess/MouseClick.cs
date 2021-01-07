using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClick : Singleton<MouseClick>
{
    public float m_range;
    public Transform m_cam;

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
                            if(ChessMissionManager.Instance.isMission(hitpiece.pieceInfo))
                            {
                                Debug.Log("ChessGameComplete");
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

    void Interaction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, m_range))
        {
            ChessGameClick(hit);
        }
    }

    
    void Update()
    {
        Interaction();
    }
}
