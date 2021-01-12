using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public MouseClick ClickManager;                                           //Ŭ���Ŵ���, ��ĥ �� ������
    public PLAYERTYPE playerType;                                           //�� ������ �÷��̾� ��

    public Vector3 PieceSize;                                                   //ĭ ������
    public Vector3 BoardSize;                                                  //���� ������(8*8)
    public GameObject BoardParent;                                          //���尡 �� �θ� ������Ʈ

    public List<Piece> PieceList;                                                //���� ü����

    public Board BoardObj;                                                      //���� ���� ������Ʈ
    public Board[,] M_BoardArr = new Board[8, 8];                         //���� ����

    public SpriteRenderer MoveRenderer;                                     //������ �� �ִ� ��ġ�� ��Ÿ���� �� Ÿ��
    public SpriteRenderer SelectRenderer;                                    //���������� ��Ÿ���� �� Ÿ��

    List<Vector3> m_PiecePositionList = new List<Vector3>();            //ü���� �ʱ� ��ġ


    void Start()
    {
        //����(ĭ)����
        CreateBoardCollider();
        //ü�� �ʱ�ȭ
        ChessInit();
        //ü�� �ʱ� ��ġ ����
        PiecePositionInit();
    }


    //ü���� �ʱ� ��ġ ����
    void PiecePositionInit()
    {
        foreach (Piece piece in PieceList)
        {
            m_PiecePositionList.Add(piece.gameObject.transform.position);
            //�� ó�� ������ false
            if(piece.pieceInfo.chessPiece == CHESSPIECE.PAWN)
            {
                Pawn pawn = piece.gameObject.transform.GetComponent<Pawn>();
                pawn.M_PawnMove = false;
            }
        }
    }

    //ü�� ����
    void ChessInit()
    {
        for (int i=0; i<8; i++)
        {
            for(int j=0; j<8; j++)
            {
                foreach (Piece piece in PieceList)
                {
                    if (M_BoardArr[i,j].gameObject.transform.position == piece.gameObject.transform.position)
                    {
                        //ü���� �� �ִ� ������ġ
                        M_BoardArr[i, j].M_isPiece = true;
                        //ü���� �� �ִ� ������ ü������ �Է�
                        M_BoardArr[i, j].pieceInfo.SetType(piece.pieceInfo);                   
                        //ü������ �ε��� ����
                        piece.pieceInfo.Index.y = i;
                        piece.pieceInfo.Index.x = j;
                        break;
                    }
                }
            }
        }
    }

    //ü������,ü�� �� ó�� ���·� �ʱ�ȭ
    public void BoardInit()
    {
        //�ʱ�ȭ
        foreach(Board board in M_BoardArr)
        {
            board.M_isPiece = false;
            board.pieceInfo.InitInfo();
        }

        //����
        foreach (Piece piece in PieceList)
        {
            int index = PieceList.IndexOf(piece);
            //ü����ġ �ʱ�ȭ
            piece.gameObject.transform.position = m_PiecePositionList[index];
            //m_PiecePositionList.Add(piece.transform.position);
            piece.gameObject.SetActive(true);
            //���� ������
            piece.SetMaterial(piece.OriginMaterial);


        }

        ChessInit();
    }
    
    //����(ĭ)����
    void CreateBoardCollider()
    {
        float tempy = 0;
        float tempx = 0;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                //���� ������Ʈ ����
                GameObject gameObject = Instantiate(BoardObj.gameObject, new Vector3(tempx, 0f, tempy) + BoardParent.gameObject.transform.position
                    , Quaternion.identity, BoardParent.transform);
                M_BoardArr[i,j] = gameObject.GetComponent<Board>();
                //���� ü�� ���� �ʱ�ȭ
                M_BoardArr[i, j].pieceInfo.InitInfo();
                //�ε��� ����
                M_BoardArr[i, j].pieceInfo.Index.y = i;
                M_BoardArr[i, j].pieceInfo.Index.x = j;
                //���� �Ŵ��� ����
                M_BoardArr[i, j].boardManager = this;
                gameObject.SetActive(true);

                //���� ��ġ��
                tempx += PieceSize.x;

            }
            tempx = 0;
            tempy += PieceSize.y;

        }


        BoardParent.transform.localRotation = BoardParent.transform.rotation;
    }
}
