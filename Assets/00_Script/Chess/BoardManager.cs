using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    public Vector3 PieceSize;
    public Vector3 BoardSize;
    public GameObject BoardParent;

    public List<Piece> PieceList;

    public Board BoardObj;
    public Board[,] M_BoardArr = new Board[8, 8];

    public Material MoveMaterial;
    public Material SelectMaterial;

    List<Vector3> m_PiecePositionList = new List<Vector3>();
    public List<CHESSPIECE> m_PenaltyPieceList = new List<CHESSPIECE>();

    void Start()
    {
        CreateBoardCollider();
        ChessInit();
        PiecePositionInit();
    }

    public void PenaltySet()
    {
        CHESSPIECE changePiece;

        if (m_PenaltyPieceList.Count >= (int)CHESSPIECE.MAX-1)
        {
            return;
        }

        int count = 0;

        while (true)
        {
            changePiece = (CHESSPIECE)Random.Range((int)CHESSPIECE.NONE + 1, (int)CHESSPIECE.MAX);
            if(m_PenaltyPieceList.IndexOf(changePiece) == -1)
            {
                break;
            }

            if(++count > 1000)
            {

                Debug.LogError("Loof");
                return;
            }
            
        }

        foreach(Piece piece in PieceList)
        {
            if(piece.pieceInfo.chessPiece == changePiece)
            {
                piece.SetMaterial(piece.PenaltyMatrial);
            }
        }
        m_PenaltyPieceList.Add(changePiece);

    }

    void PiecePositionInit()
    {
        foreach (Piece piece in PieceList)
        {
            m_PiecePositionList.Add(piece.gameObject.transform.position);
            if(piece.pieceInfo.chessPiece == CHESSPIECE.PAWN)
            {
                Pawn pawn = piece.gameObject.transform.GetComponent<Pawn>();
                pawn.M_PawnMove = false;
            }
        }
    }

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
                        M_BoardArr[i, j].M_isPiece = true;
                        M_BoardArr[i, j].pieceInfo.SetType(piece.pieceInfo);                        
                        piece.pieceInfo.Index.y = i;
                        piece.pieceInfo.Index.x = j;
                        break;
                    }
                }
            }
        }

        
    }

    public void BoardInit()
    {
        foreach(Board board in M_BoardArr)
        {
            board.M_isPiece = false;
            board.pieceInfo.InitInfo();
        }

        foreach (Piece piece in PieceList)
        {
            int index = PieceList.IndexOf(piece);
            piece.gameObject.transform.position = m_PiecePositionList[index];
            m_PiecePositionList.Add(piece.transform.position);
            piece.gameObject.SetActive(true);
            piece.SetMaterial(piece.OriginMaterial);


        }

        ChessInit();
    }
    

    [ContextMenu("CreateBoardCollider")]
    void CreateBoardCollider()
    {
        float tempy = 0;
        float tempx = 0;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject gameObject = Instantiate(BoardObj.gameObject, new Vector3(tempx, 0f, tempy) + BoardParent.gameObject.transform.position
                    , Quaternion.identity, BoardParent.transform);
                M_BoardArr[i,j] = gameObject.GetComponent<Board>();
                M_BoardArr[i, j].pieceInfo.InitInfo();
                M_BoardArr[i, j].pieceInfo.Index.y = i;
                M_BoardArr[i, j].pieceInfo.Index.x = j;

                tempx += PieceSize.x;

            }
            tempx = 0;
            tempy += PieceSize.y;

        }


        BoardParent.transform.localRotation = BoardParent.transform.rotation;
    }

    [ContextMenu("ResetBoard")]
    void ResetBoard()
    {
        foreach (Board item in M_BoardArr)
        {
            GameObject.DestroyImmediate(item.gameObject);
        }
    }
}
