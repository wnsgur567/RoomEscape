using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public MouseClick ClickManager;                                           //클릭매니저, 합칠 때 봐야함
    public PLAYERTYPE playerType;                                           //이 보드의 플레이어 색

    public Vector3 PieceSize;                                                   //칸 사이즈
    public Vector3 BoardSize;                                                  //보드 사이즈(8*8)
    public GameObject BoardParent;                                          //보드가 들어갈 부모 오브젝트

    public List<Piece> PieceList;                                                //현재 체스들

    public Board BoardObj;                                                      //만들 보드 오브젝트
    public Board[,] M_BoardArr = new Board[8, 8];                         //현재 보드

    public SpriteRenderer MoveRenderer;                                     //움직일 수 있는 위치에 나타나는 색 타일
    public SpriteRenderer SelectRenderer;                                    //선택했을때 나타나는 색 타일

    List<Vector3> m_PiecePositionList = new List<Vector3>();            //체스들 초기 위치


    void Start()
    {
        //보드(칸)생성
        CreateBoardCollider();
        //체스 초기화
        ChessInit();
        //체스 초기 위치 저장
        PiecePositionInit();
    }


    //체스들 초기 위치 저장
    void PiecePositionInit()
    {
        foreach (Piece piece in PieceList)
        {
            m_PiecePositionList.Add(piece.gameObject.transform.position);
            //폰 처음 움직임 false
            if(piece.pieceInfo.chessPiece == CHESSPIECE.PAWN)
            {
                Pawn pawn = piece.gameObject.transform.GetComponent<Pawn>();
                pawn.M_PawnMove = false;
            }
        }
    }

    //체스 설정
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
                        //체스가 서 있는 보드위치
                        M_BoardArr[i, j].M_isPiece = true;
                        //체스가 서 있는 보드의 체스정보 입력
                        M_BoardArr[i, j].pieceInfo.SetType(piece.pieceInfo);                   
                        //체스보드 인덱스 설정
                        piece.pieceInfo.Index.y = i;
                        piece.pieceInfo.Index.x = j;
                        break;
                    }
                }
            }
        }
    }

    //체스보드,체스 말 처음 상태로 초기화
    public void BoardInit()
    {
        //초기화
        foreach(Board board in M_BoardArr)
        {
            board.M_isPiece = false;
            board.pieceInfo.InitInfo();
        }

        //설정
        foreach (Piece piece in PieceList)
        {
            int index = PieceList.IndexOf(piece);
            //체스위치 초기화
            piece.gameObject.transform.position = m_PiecePositionList[index];
            //m_PiecePositionList.Add(piece.transform.position);
            piece.gameObject.SetActive(true);
            //원래 색으로
            piece.SetMaterial(piece.OriginMaterial);


        }

        ChessInit();
    }
    
    //보드(칸)생성
    void CreateBoardCollider()
    {
        float tempy = 0;
        float tempx = 0;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                //보드 오브젝트 생성
                GameObject gameObject = Instantiate(BoardObj.gameObject, new Vector3(tempx, 0f, tempy) + BoardParent.gameObject.transform.position
                    , Quaternion.identity, BoardParent.transform);
                M_BoardArr[i,j] = gameObject.GetComponent<Board>();
                //보드 체스 정보 초기화
                M_BoardArr[i, j].pieceInfo.InitInfo();
                //인덱스 설정
                M_BoardArr[i, j].pieceInfo.Index.y = i;
                M_BoardArr[i, j].pieceInfo.Index.x = j;
                //보드 매니져 설정
                M_BoardArr[i, j].boardManager = this;
                gameObject.SetActive(true);

                //다음 위치로
                tempx += PieceSize.x;

            }
            tempx = 0;
            tempy += PieceSize.y;

        }


        BoardParent.transform.localRotation = BoardParent.transform.rotation;
    }
}
