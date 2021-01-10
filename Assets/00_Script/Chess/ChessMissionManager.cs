using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessMissionManager : Singleton<ChessMissionManager>
{
    //public BoardManager boardManager;
    public PLAYERTYPE TurnPlayer;                 //���� �÷��̾� ��
    public List<ChessMissionInfo> MissionList;    //�̼� ����Ʈ
    public int SuccessMissionCount;                //���� ���� ī��Ʈ 
    public int MaxMissionCount;                     //�ִ� ���� ī��Ʈ

    public List<BoardManager> boardManagerList;


    ChessMissionInfo CurMission;                   //���� �̼�
    int CurTurn;                                        //���� ü������ ��

    int CurCount;                                      //���� ���� ī��Ʈ


    List<CHESSPIECE> m_PenaltyPieceList = new List<CHESSPIECE>(); //�ٲ� ü��
    void Start()
    {
        TurnPlayer = PLAYERTYPE.WHITE;
        SetMission();
    }

    public void CurTurnAdd()
    {
        CurTurn++;
        Debug.Log($"CurTurn: {CurTurn}");

        //���� ���� �־��� �̼��� �Ϻ��� ������ ������
        if(CurTurn > CurMission.Turn)
        {
            //���� �� �ʱ�ȭ
            CurTurn = 0;
            //����ī��Ʈ �ø�
            CurCount++;
            PenaltySet();
            if (!CheckGameCount(false))
            {
                
            }
            Debug.Log($"ChessGameFail:CurCount {CurCount}");
        }
    }

    //�̼��� �����ߴ���(�������� ���)
    public bool isMission(PieceInfo _piece)
    {
        //�̼Ǽ���
        if(_piece.playerType == CurMission.Color
            && _piece.chessPiece == CurMission.piece)
        {
            Debug.Log("ChessGameComplete");
            CurTurn = 0;
            CheckGameCount(true);
            /*
            //���� ī��Ʈ �ø�
            CurTurnAdd();*/
            return true;
        }
        //�̼ǽ���
       

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

    //�ܸ��� �޼���
    public string ChessMissionMessage()
    {
        string text = "";

        text += CurMission.Turn.ToString() + "���ҽ��ϴ�.\n";
        text += "��:\t" + CurMission.Turn + "\n";
        text += "����:\t" + CurMission.Color + "\n";
        text += "��ǥ:\t" + CurMission.piece + "\n";

        return text;
    }

    //�г�Ƽ(���ٲٱ�)
    public void PenaltySet()
    {
        CHESSPIECE changePiece;

        //�̹� ��� ü������ �� �ٲ���ٸ� ����
        if (m_PenaltyPieceList.Count >= (int)CHESSPIECE.MAX - 1)
        {
            return;
        }

        int count = 0;

        while (true)
        {
            //���� �� ����
            changePiece = (CHESSPIECE)Random.Range((int)CHESSPIECE.NONE + 1, (int)CHESSPIECE.MAX);

            //���� ���� ���ٸ� �ݺ��� ����
            if (m_PenaltyPieceList.IndexOf(changePiece) == -1)
            {
                break;
            }

            //���ѷ�������
            if (++count > 10000)
            {

                Debug.LogError("���ѷ���");
                return;
            }

        }

        foreach(BoardManager boardManager in boardManagerList)
        {
            //���ٲ�
            foreach (Piece piece in boardManager.PieceList)
            {
                if (piece.pieceInfo.chessPiece == changePiece)
                {
                    piece.SetMaterial(piece.PenaltyMatrial);
                }
            }
        }
        
        //���� �ٲ� �� �߰�
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
        {        //�ִ� ���� ī��Ʈ �ȿ� ���� ����
            if (CurCount <= SuccessMissionCount)
            {
                Debug.Log($"Complete :CurCount {CurCount}");
                return true;
            }

        }
        else
        {
            //�ִ� ���� ī��Ʈ���� ������
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
