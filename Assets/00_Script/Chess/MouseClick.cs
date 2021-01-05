using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    public float m_range;
    public Transform m_cam;

    public GameObject ClickObj;

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
                        Vector3 tempvec = new Vector3();

                        if(piece.pieceInfo.playerType == PLAYERTYPE.WHITE)
                        {
                            tempvec = hit.transform.position - ClickObj.transform.position;
                        }
                        else
                        {
                            tempvec = ClickObj.transform.position- hit.transform.position;
                        }
                        



                        if (piece.IsMove(tempvec))
                        {
                            ClickObj.transform.position = hit.transform.position;
                        }
                    }
                }
                else if(hit.transform.CompareTag("ChessPiece"))
                {
                    ClickObj = hit.transform.gameObject;
                }

            }

            
        }
    }

    
    void Update()
    {
        Interaction();
    }
}
