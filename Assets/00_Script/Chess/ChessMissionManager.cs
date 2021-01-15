using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChessMissionManager : SingletonPunCallback<ChessMissionManager>
{
    public Animator ChessTableAni;
    public List<ChessMissionInfo> MissionList;    //�̼� ����Ʈ
    public int SuccessMissionCount;                //���� ���� ī��Ʈ 
    public int MaxMissionCount;                     //�ִ� ���� ī��Ʈ
    public float PenaltyTime;
    public List<BoardManager> boardManagerList;     //����Ŵ��� ����Ʈ

    public PLAYERTYPE TurnPlayer;                 //���� �÷��̾� ��

    ChessMissionInfo CurMission;                   //���� �̼�
    int CurTurn;                                        //���� ü������ ��
    int CurCount;                                      //���� ���� ī��Ʈ
    int SuccessCount;

    public bool isClear;
    public PhotonView PV;
    bool isInit;

    List<CHESSPIECE> m_PenaltyPieceList = new List<CHESSPIECE>(); //�ٲ� ü��
    [SerializeField]
    List<ChessMissionInfo> m_UseMission = new List<ChessMissionInfo>(); //�̹� �� �̼�

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
        if (CurTurn >= CurMission.Turn)
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
            if (CheckGameCount(false))
            {

            }
            Debug.Log($"ü���̼� ����:CurCount {CurCount}");

        }
    }

    [PunRPC]
    public void isMission(int _hitboardPType, int _hitboardPiece, int indexX, int indexY)
    {
        PieceInfo piece = new PieceInfo(_hitboardPType, _hitboardPiece, indexX, indexY);


        //�̼Ǽ���
        if (piece.playerType == CurMission.Color
            && piece.chessPiece == CurMission.Piece)
        {
            Debug.Log("ChessGameComplete");
            CurTurn = 0;
            CurCount++;
            SuccessCount++;
            //�̼� 3�� ����
            CheckGameCount(true);
            return;
        }
    }

    //����� ���Ӻ��� ������Ʈ
    public void UpdateChess(BoardManager _UpdateboardManager, Piece _movepiece, Board _hitboard, Piece _hitpiece)
    {
        Piece tempmovepiece = null;
        Piece temphitpiece = null;
        Board tempboard = null;
        BoardManager UpdateBoardManager = null;

        //����� ����Ŵ��� ã��
        if (boardManagerList.IndexOf(_UpdateboardManager) == 0)
        {
            UpdateBoardManager = boardManagerList[1];
        }
        else
        {
            UpdateBoardManager = boardManagerList[0];
        }

        //����� ����Ŵ����� ���� �� ã��
        foreach (Piece piece in UpdateBoardManager.PieceList)
        {
            if (piece.pieceInfo == _movepiece.pieceInfo)
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
                (int)temphitpiece.pieceInfo.playerType, (int)temphitpiece.pieceInfo.chessPiece, temphitpiece.pieceInfo.Index.x, temphitpiece.pieceInfo.Index.y, false);
        }
        else //���� ���
        {
            tempmovepiece.PV.RPC("MoveTo", RpcTarget.AllBuffered, (int)tempboard.pieceInfo.playerType, (int)tempboard.pieceInfo.chessPiece
                , tempboard.pieceInfo.Index.x, tempboard.pieceInfo.Index.y, false);
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
        string turntext = (CurMission.Turn * 0.5).ToString();

        text += SuccessMissionCount + "번 남았습니다.\n";
        text += "턴:\t" + turntext + "\n";
        text += "색상:\t" + CurMission.Color + "\n";
        text += "목표:\t" + CurMission.Piece + "\n";

        Debug.Log(text);

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

        int count = 0;
        while (true)
        {

            //���ٲ�
            foreach (ChessMissionInfo usemission in m_UseMission)
            {
                if (CurMission != usemission)
                {
                    break;
                }
            }
            CurMission = MissionList[Random.Range(0, MissionList.Count)];

            if (count++ > 10000)
            {
                break;
            }
        }


        CurMission.Turn *= 2;



        PV.RPC("SendMission", RpcTarget.AllBuffered, CurMission.Turn, (int)CurMission.Color, (int)CurMission.Piece);

    }

    //�̼� ����
    [PunRPC]
    void SendMission(int _Turn, int _Color, int _Piece)
    {
        ChessMissionInfo mission = new ChessMissionInfo(_Turn, _Color, _Piece);

        mission.Turn = (int)(mission.Turn * 0.5);
        foreach (ChessMissionInfo chessMission in MissionList)
        {
            if (chessMission == mission)
            {
                m_UseMission.Add(chessMission);
                break;
            }
        }

        this.CurMission.Turn = _Turn;
        this.CurMission.Color = (PLAYERTYPE)_Color;
        this.CurMission.Piece = (CHESSPIECE)_Piece;
        Debug.Log("newMission");
        //�ܸ��� �޼��� ����
        _NetworkChatManager.Instance.AddLineTermianl(ChessMissionMessage());
    }

    //���� ��������
    bool CheckGameCount(bool _isMission)
    {
        //��ǥ �� ���?
        if (_isMission)
        {
            //�ִ� ���� ī��Ʈ �ȿ� ���� ����
            if (SuccessCount >= SuccessMissionCount)
            {
                //å�󿭸�
                GameManager.M_gameManager.Complete_ChessPuzzle();
                TableAniTrigger();
                isClear = true;


                Debug.Log($"ü������ ����, ����ī��Ʈ :CurCount {CurCount}");
                return true;
            }

        }
        else
        {
            //�ִ� ���� ī��Ʈ���� ������
            if (CurCount >= MaxMissionCount)
            {
                PV.RPC("IsInitTrue", RpcTarget.AllBuffered);

                Debug.Log("ü�����ӽ���");
                return true;
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            SetMission();
        }
        return false;
    }

    [PunRPC]
    void IsInitTrue()
    {
        isInit = true;
    }

    [PunRPC]
    public void ChessFail()
    {
        if (isInit)
        {
            TurnPlayer = PLAYERTYPE.WHITE;
            DigitalClock.M_clock.M_currentSeconds -= PenaltyTime;
            CurTurn = 0;
            CurCount = 0;
            SuccessCount = 0;
            m_UseMission.Clear();
            m_PenaltyPieceList.Clear();
            if (PhotonNetwork.IsMasterClient)
            {
                SetMission();
            }
            foreach (BoardManager boardManager in boardManagerList)
            {
                boardManager.BoardInit();
            }
            isInit = false;
        }


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

    //å�󼭶� ������ �ִϸ��̼�
    void TableAniTrigger()
    {
        ChessTableAni.SetTrigger("isMove");
        _SoundManager.Instance.PlayUISound(E_UISound.drawer_open);
    }
}
