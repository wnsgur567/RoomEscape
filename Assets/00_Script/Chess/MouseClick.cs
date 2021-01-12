using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MouseClick : MonoBehaviourPun
{
    public float m_range;
    public Camera m_cam;

    public GameObject ClickObj;             //Ŭ���� ü����
    public List<BoxCollider> ChessGame;   //��ȣ�ۿ��ϴ� ü������
    //public BoardManager boardManager;

    //ü������ ����
    void ChessGameClick(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log($"{hit.transform.tag}");

            //ü������ Ŭ��
            if (hit.transform.CompareTag("ChessBoard"))
            {
                // ü������(Ŭ���� ���� ��ġ)�� ������
                if (ClickObj != null)
                {
                    //�÷��̾ ������ ��
                    Piece playerpiece = ClickObj.GetComponent<Piece>();
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
                        ClickObj = null;

                    }
                }
            }
            else if (hit.transform.CompareTag("ChessPiece"))    //ü���� Ŭ��
            {
                //���� ������ ü���� null�� �ƴ� ���
                if (ClickObj != null)
                {
                    //�÷��̾ ������ ü����
                    Piece playerpiece = ClickObj.GetComponent<Piece>();
                    //���� ������ ü����
                    Piece hitpiece = hit.transform.GetComponent<Piece>();
                    //���� ������ ü������ ����Ŵ���
                    BoardManager boardManager = hitpiece.boardManager;
                    //Ŭ���� ���� ��ġ�� ����
                    Board hitBoard = boardManager.M_BoardArr[hitpiece.pieceInfo.Index.y, hitpiece.pieceInfo.Index.x];

                    //�ൿ���(Ŭ���� ���� ������ ������ ���� ���� ���)
                    if (hit.transform.gameObject == ClickObj)
                    {
                        playerpiece.MoveTileFalse();
                        ClickObj = null;

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
                            ClickObj = null;
                            //hitpiece.gameObject.SetActive(false);
                            //�̼Ǽ���?
                            if(ChessMissionManager.Instance.isMission(hitpiece.pieceInfo))
                            {
                                hitpiece.gameObject.SetActive(true);
                            }
                        }
                    }
                    else //�÷��̾� �� �ٲٱ�(������ ���� ���� ������ ������ ���� ���� ���� ���)
                    {
                       
                        playerpiece.MoveTileFalse();
                        ClickObj = hit.transform.gameObject;
                        playerpiece = ClickObj.GetComponent<Piece>();
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
                //ü������ Ȱ��ȭ
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
