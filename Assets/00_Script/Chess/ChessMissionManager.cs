using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessMissionManager : Singleton<ChessMissionManager>
{
    //public BoardManager boardManager;
    public PLAYERTYPE TurnPlayer;                 //현재 플레이어 색
    public List<ChessMissionInfo> MissionList;    //미션 리스트
    public int SuccessMissionCount;                //성공 게임 카운트 
    public int MaxMissionCount;                     //최대 게임 카운트

    public List<BoardManager> boardManagerList;


    ChessMissionInfo CurMission;                   //현재 미션
    int CurTurn;                                        //현재 체스게임 턴

    int CurCount;                                      //현재 게임 카운트


    List<CHESSPIECE> m_PenaltyPieceList = new List<CHESSPIECE>(); //바뀐 체스
    void Start()
    {
        TurnPlayer = PLAYERTYPE.WHITE;
        SetMission();
    }

    public void CurTurnAdd()
    {
        CurTurn++;
        Debug.Log($"CurTurn: {CurTurn}");

        //현재 턴이 주어진 미션의 턴보다 많으면 실패함
        if(CurTurn > CurMission.Turn)
        {
            //현재 턴 초기화
            CurTurn = 0;
            //게임카운트 올림
            CurCount++;
            PenaltySet();
            if (!CheckGameCount(false))
            {
                
            }
            Debug.Log($"ChessGameFail:CurCount {CurCount}");
        }
    }

    //미션이 성공했는지(말잡을때 사용)
    public bool isMission(PieceInfo _piece)
    {
        //미션성공
        if(_piece.playerType == CurMission.Color
            && _piece.chessPiece == CurMission.piece)
        {
            Debug.Log("ChessGameComplete");
            CurTurn = 0;
            CheckGameCount(true);
            /*
            //게임 카운트 올림
            CurTurnAdd();*/
            return true;
        }
        //미션실패
       

        return false;
    }

    public void UpdateChess(BoardManager _UpdateboardManager, Piece _movepiece, Board _hitboard, Piece _hitpiece)
    {
        Piece tempmovepiece = null;
        Piece temphitpiece = null;
        Board tempboard = null;

        if (boardManagerList[0] == _UpdateboardManager)
        {
            foreach(Piece piece in boardManagerList[1].PieceList)
            {
                if(piece.pieceInfo == _movepiece.pieceInfo)
                {
                    tempmovepiece = piece;
                    break;
                }
            }

            foreach (Board board in boardManagerList[1].M_BoardArr)
            {
                if (board.pieceInfo == _hitboard.pieceInfo)
                {
                    tempboard = board;
                    break;
                }
            }

            if (_hitpiece != null)
            {
                foreach (Piece piece in boardManagerList[1].PieceList)
                {
                    if (piece.pieceInfo == _hitpiece.pieceInfo)
                    {
                        temphitpiece = piece;
                        break;
                    }
                }
            }
        }
        else
        {
            foreach (Piece piece in boardManagerList[0].PieceList)
            {
                if (piece.pieceInfo == _movepiece.pieceInfo)
                {
                    tempmovepiece = piece;
                    break;
                }
            }

            foreach (Board board in boardManagerList[0].M_BoardArr)
            {
                if (board.pieceInfo == _hitboard.pieceInfo)
                {
                    tempboard = board;
                    break;
                }
            }

            if (_hitpiece != null)
            {
                foreach (Piece piece in boardManagerList[1].PieceList)
                {
                    if (piece.pieceInfo == _hitpiece.pieceInfo)
                    {
                        temphitpiece = piece;
                        break;
                    }
                }
            }
        }

        if(_hitpiece != null)
        {

        }

        tempmovepiece.MoveTo(tempboard, temphitpiece);
        TurnPlayer = SwitchPlayerType(TurnPlayer);
        CurTurnAdd();
    }

    //단말기 메세지
    public string ChessMissionMessage()
    {
        string text = "";

        text += CurMission.Turn.ToString() + "남았습니다.\n";
        text += "턴:\t" + CurMission.Turn + "\n";
        text += "색상:\t" + CurMission.Color + "\n";
        text += "목표:\t" + CurMission.piece + "\n";

        return text;
    }

    //패널티(색바꾸기)
    public void PenaltySet()
    {
        CHESSPIECE changePiece;

        //이미 모든 체스말이 다 바뀌었다면 리턴
        if (m_PenaltyPieceList.Count >= (int)CHESSPIECE.MAX - 1)
        {
            return;
        }

        int count = 0;

        while (true)
        {
            //랜덤 말 지정
            changePiece = (CHESSPIECE)Random.Range((int)CHESSPIECE.NONE + 1, (int)CHESSPIECE.MAX);

            //같은 말이 없다면 반복문 종료
            if (m_PenaltyPieceList.IndexOf(changePiece) == -1)
            {
                break;
            }

            //무한루프방지
            if (++count > 10000)
            {

                Debug.LogError("무한루프");
                return;
            }

        }

        foreach(BoardManager boardManager in boardManagerList)
        {
            //색바꿈
            foreach (Piece piece in boardManager.PieceList)
            {
                if (piece.pieceInfo.chessPiece == changePiece)
                {
                    piece.SetMaterial(piece.PenaltyMatrial);
                }
            }
        }
        
        //색이 바뀐 말 추가
        m_PenaltyPieceList.Add(changePiece);

    }

    void SetMission()
    {
        Debug.Log("MissionSet");
        CurMission = MissionList[Random.Range(0, MissionList.Count)];
        CurMission.Turn *= 2;
    }

    bool CheckGameCount(bool _isMission)
    {
        if(_isMission)
        {        //최대 게임 카운트 안에 게임 끝냄
            if (CurCount <= SuccessMissionCount)
            {
                Debug.Log($"Complete :CurCount {CurCount}");
                return true;
            }

        }
        else
        {
            //최대 게임 카운트보다 많아짐
            if (CurCount >= MaxMissionCount)
            {
                foreach (BoardManager boardManager in boardManagerList)
                {
                    boardManager.BoardInit();
                }
                Debug.Log("Fail");
                return true;
            }
        }



        SetMission();
        return false;
    }

    PLAYERTYPE SwitchPlayerType(PLAYERTYPE _type)
    {
        switch (_type)
        {
            case PLAYERTYPE.WHITE:
                return PLAYERTYPE.BLACK;
            case PLAYERTYPE.BLACK:
                return PLAYERTYPE.WHITE;
        }

        return PLAYERTYPE.NONE;
    }

}
