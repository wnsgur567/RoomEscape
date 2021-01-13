using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChessMissionManager : SingletonPunCallback<ChessMissionManager>
{
    public PLAYERTYPE TurnPlayer;                 //���� �÷��̾� ��
    public List<ChessMissionInfo> MissionList;    //�̼� ����Ʈ
    public int SuccessMissionCount;                //���� ���� ī��Ʈ 
    public int MaxMissionCount;                     //�ִ� ���� ī��Ʈ

    public float PenaltyTime;

    public List<BoardManager> boardManagerList;     //����Ŵ��� ����Ʈ

    [SerializeField]
    ChessMissionInfo CurMission;                   //���� �̼�
    int CurTurn;                                        //���� ü������ ��

    int CurCount;                                      //���� ���� ī��Ʈ

    PhotonView PV;


    List<CHESSPIECE> m_PenaltyPieceList = new List<CHESSPIECE>(); //�ٲ� ü��
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }


    //�ʱ�ȭ
    public void __Init()
    {
        TurnPlayer = PLAYERTYPE.WHITE;
        if (PhotonNetwork.IsMasterClient)
        {
            SetMission();
        }

    }

    //�� �߰�
    [PunRPC]
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
            //�г�Ƽ �߰�
            if (PhotonNetwork.IsMasterClient)
            {
                PenaltySet();
            }
            //���� �̼� 6�� ���� ��
            CheckGameCount(false);
            Debug.Log($"ü���̼� ����:CurCount {CurCount}");
        }
    }

    //�̼��� �����ߴ���(�������� ���)
    public bool isMission(PieceInfo _piece)
    {
        //�̼Ǽ���
        if(_piece.playerType == CurMission.Color
            && _piece.chessPiece == CurMission.Piece)
        {
            Debug.Log("ChessGameComplete");
            CurTurn = 0;
            //�̼� 3�� ����
            CheckGameCount(true);
            return true;
        }
        //�̼ǽ���
       

        return false;
    }

    //����� ���Ӻ��� ������Ʈ
    public void UpdateChess(BoardManager _UpdateboardManager, Piece _movepiece, Board _hitboard, Piece _hitpiece)
    {
        Piece tempmovepiece = null;
        Piece temphitpiece = null;
        Board tempboard = null;
        BoardManager UpdateBoardManager = null;

        //����� ����Ŵ��� ã��
        if(boardManagerList.IndexOf(_UpdateboardManager) == 0)
        {
            UpdateBoardManager = boardManagerList[1];
        }
        else
        {
            UpdateBoardManager = boardManagerList[0];
        }

        //����� ����Ŵ����� ���� �� ã��
        foreach(Piece piece in UpdateBoardManager.PieceList)
        {
            if(piece.pieceInfo == _movepiece.pieceInfo)
            {
                tempmovepiece = piece;
                break;
            }
        }
        //����� ����Ŵ����� ���� ���� ã��
        foreach (Board board in UpdateBoardManager.M_BoardArr)
        {
            if (board.pieceInfo == _hitboard.pieceInfo)
            {
                tempboard = board;
                break;
            }
        }
        //��� ���� ���� ���
        if (_hitpiece != null)
        {
            //���� �� ã��
            foreach (Piece piece in UpdateBoardManager.PieceList)
            {
                if (piece.pieceInfo == _hitpiece.pieceInfo)
                {
                    temphitpiece = piece;
                    break;
                }
            }
        }
     
        //��� ���� ���� ���
        if (_hitpiece != null)
        {
            tempmovepiece.PV.RPC("MoveTo", RpcTarget.AllBuffered,
                (int)tempboard.pieceInfo.playerType, (int)tempboard.pieceInfo.chessPiece, tempboard.pieceInfo.Index.x, tempboard.pieceInfo.Index.y,
                (int)temphitpiece.pieceInfo.playerType, (int)temphitpiece.pieceInfo.chessPiece, temphitpiece.pieceInfo.Index.x, temphitpiece.pieceInfo.Index.y);
        }
        else //���� ���
        {
            tempmovepiece.PV.RPC("MoveTo", RpcTarget.AllBuffered, (int)tempboard.pieceInfo.playerType, (int)tempboard.pieceInfo.chessPiece
                , tempboard.pieceInfo.Index.x, tempboard.pieceInfo.Index.y);
        }
        //�÷��̾� �� �ٲ�
        PV.RPC("SwitchPlayerType", RpcTarget.AllBuffered);// SwitchPlayerType();
        //�� �߰�
        PV.RPC("CurTurnAdd", RpcTarget.AllBuffered);// CurTurnAdd();
    }

    //�ܸ��� �޼���
    public string ChessMissionMessage()
    {
        string text = "";

        text += CurMission.Turn.ToString() + "���ҽ��ϴ�.\n";
        text += "��:\t" + CurMission.Turn + "\n";
        text += "����:\t" + CurMission.Color + "\n";
        text += "��ǥ:\t" + CurMission.Piece + "\n";

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

        PV.RPC("ChangePieceColor", RpcTarget.AllBuffered, (int)changePiece);// ChangePieceColor(changePiece);

    }

    //ü�� �� �ٲ�
    [PunRPC]
    void ChangePieceColor(int _changeP)
    {
        CHESSPIECE changePiece = (CHESSPIECE)_changeP;

        foreach (BoardManager boardManager in boardManagerList)
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


    //�̼� ����
    void SetMission()
    {
        Debug.Log("MissionSet");
        CurMission = MissionList[Random.Range(0, MissionList.Count)];
        CurMission.Turn *= 2;

        

        PV.RPC("SendMission", RpcTarget.AllBuffered, CurMission.Turn, (int)CurMission.Color, (int)CurMission.Piece);

    }

    //�̼� ����
    [PunRPC]
    void SendMission(int _Turn, int _Color, int _Piece)
    {
        this.CurMission.Turn = _Turn;
        this.CurMission.Color = (PLAYERTYPE)_Color;
        this.CurMission.Piece = (CHESSPIECE)_Piece;
        //�ܸ��� �޼��� ����
        _NetworkChatManager.Instance.AddLine(ChessMissionMessage());
    }

    //���� ��������
    bool CheckGameCount(bool _isMission)
    {
        //��ǥ �� ���?
        if(_isMission)
        {        
            //�ִ� ���� ī��Ʈ �ȿ� ���� ����
            if (CurCount >= SuccessMissionCount
                && CurCount < MaxMissionCount)
            {
                //å�󿭸��°� ���� �߰�

                Debug.Log($"ü������ ����, ����ī��Ʈ :CurCount {CurCount}");
                return true;
            }

        }
        else
        {
            //�ִ� ���� ī��Ʈ���� ������
            if (CurCount >= MaxMissionCount)
            {
                //��ź �ð� �پ��
                DigitalClock.M_clock.M_currentSeconds -= PenaltyTime;

                foreach (BoardManager boardManager in boardManagerList)
                {
                    boardManager.BoardInit();
                }
                Debug.Log("ü�����ӽ���");
                return true;
            }
        }

        if(PhotonNetwork.IsMasterClient)
        {
            SetMission();
        }
        return false;
    }

    //�� �� �ٲ�
    [PunRPC]
    void SwitchPlayerType()
    {
        switch (TurnPlayer)
        {
            case PLAYERTYPE.WHITE:
                TurnPlayer = PLAYERTYPE.BLACK;
                break;
            case PLAYERTYPE.BLACK:
                TurnPlayer = PLAYERTYPE.WHITE;
                break;
        }
    }

}
