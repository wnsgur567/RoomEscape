using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    public float m_range;
    public Transform m_cam;

    public GameObject ClickObj;
    public BoardManager BoardManager;

    void Interaction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, m_range))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log($"{hit.transform.tag}");

                if (hit.transform.CompareTag("ChessBoard"))
                {
                    if(ClickObj != null)
                    {
                        Piece piece = ClickObj.GetComponent<Piece>();
                        Board hitBoard = hit.transform.GetComponent<Board>();
                        Vector3 tempvec;

                        tempvec = ClickObj.transform.localPosition + hit.transform.localPosition;

                        if (piece.IsMove(tempvec, hitBoard, false))
                        {
                            piece.MoveTo(hitBoard);
                            ClickObj = null;
                        }
                    }
                }
                else if(hit.transform.CompareTag("ChessPiece"))
                {
                    if(ClickObj!=null)
                    {
                        Piece playerpiece = ClickObj.GetComponent<Piece>();
                        Piece hitpiece = hit.transform.GetComponent<Piece>();
                        Board hitBoard = BoardManager.M_BoardArr[hitpiece.pieceInfo.Index.y, hitpiece.pieceInfo.Index.x];

                        if (playerpiece.pieceInfo.playerType != hitpiece.pieceInfo.playerType)
                        {
                            Vector3 tempvec = ClickObj.transform.localPosition + hitBoard.transform.localPosition;
                            if (playerpiece.IsMove(tempvec, hitBoard, true))
                            {
                                playerpiece.MoveTo(hitBoard);
                                ClickObj = null;
                                hitpiece.gameObject.SetActive(false);
                            }
                        }
                        else
                        {
                            ClickObj = hit.transform.gameObject;
                        }
                    }
                    else
                    {
                        ClickObj = hit.transform.gameObject;
                    }
                }

            }

            
        }
    }

    
    void Update()
    {
        Interaction();
    }
}
