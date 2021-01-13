using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChessMissionManager : SingletonPunCallback<ChessMissionManager>
{
    public PLAYERTYPE TurnPlayer;                 //현재 플레이어 색
    public List<ChessMissionInfo> MissionList;    //미션 리스트
    public int SuccessMissionCount;                //성공 게임 카운트 
    public int MaxMissionCount;                     //최대 게임 카운트

    public float PenaltyTime;

    public List<BoardManager> boardManagerList;     //보드매니져 리스트

    [SerializeField]
    ChessMissionInfo CurMission;                   //현재 미션
    int CurTurn;                                        //현재 체스게임 턴

    int CurCount;                                      //현재 게임 카운트

    PhotonView PV;


    List<CHESSPIECE> m_PenaltyPieceList = new List<CHESSPIECE>(); //바뀐 체스
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }


    //초기화
    public void __Init()
    {
        TurnPlayer = PLAYERTYPE.WHITE;
        if (PhotonNetwork.IsMasterClient)
        {
            SetMission();
        }

    }

    //턴 추가
    [PunRPC]
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
            //패널티 추가
            if (PhotonNetwork.IsMasterClient)
            {
                PenaltySet();
            }
            //게임 미션 6번 실패 시
            CheckGameCount(false);
            Debug.Log($"체스미션 실패:CurCount {CurCount}");
        }
    }

    //미션이 성공했는지(말잡을때 사용)
    public bool isMission(PieceInfo _piece)
    {
        //미션성공
        if(_piece.playerType == CurMission.Color
            && _piece.chessPiece == CurMission.Piece)
        {
            Debug.Log("ChessGameComplete");
            CurTurn = 0;
            //미션 3번 성공
            CheckGameCount(true);
            return true;
        }
        //미션실패
       

        return false;
    }

    //상대편 게임보드 업데이트
    public void UpdateChess(BoardManager _UpdateboardManager, Piece _movepiece, Board _hitboard, Piece _hitpiece)
    {
        Piece tempmovepiece = null;
        Piece temphitpiece = null;
        Board tempboard = null;
        BoardManager UpdateBoardManager = null;

        //상대편 보드매니져 찾음
        if(boardManagerList.IndexOf(_UpdateboardManager) == 0)
        {
            UpdateBoardManager = boardManagerList[1];
        }
        else
        {
            UpdateBoardManager = boardManagerList[0];
        }

        //상대편 보드매니져의 같은 말 찾음
        foreach(Piece piece in UpdateBoardManager.PieceList)
        {
            if(piece.pieceInfo == _movepiece.pieceInfo)
            {
                tempmovepiece = piece;
                break;
            }
        }
        //상대편 보드매니져의 같은 보드 찾음
        foreach (Board board in UpdateBoardManager.M_BoardArr)
        {
            if (board.pieceInfo == _hitboard.pieceInfo)
            {
                tempboard = board;
                break;
            }
        }
        //잡는 말이 있을 경우
        if (_hitpiece != null)
        {
            //같은 말 찾음
            foreach (Piece piece in UpdateBoardManager.PieceList)
            {
                if (piece.pieceInfo == _hitpiece.pieceInfo)
                {
                    temphitpiece = piece;
                    break;
                }
            }
        }
     
        //잡는 말이 있을 경우
        if (_hitpiece != null)
        {
            tempmovepiece.PV.RPC("MoveTo", RpcTarget.AllBuffered,
                (int)tempboard.pieceInfo.playerType, (int)tempboard.pieceInfo.chessPiece, tempboard.pieceInfo.Index.x, tempboard.pieceInfo.Index.y,
                (int)temphitpiece.pieceInfo.playerType, (int)temphitpiece.pieceInfo.chessPiece, temphitpiece.pieceInfo.Index.x, temphitpiece.pieceInfo.Index.y);
        }
        else //없을 경우
        {
            tempmovepiece.PV.RPC("MoveTo", RpcTarget.AllBuffered, (int)tempboard.pieceInfo.playerType, (int)tempboard.pieceInfo.chessPiece
                , tempboard.pieceInfo.Index.x, tempboard.pieceInfo.Index.y);
        }
        //플레이어 색 바꿈
        PV.RPC("SwitchPlayerType", RpcTarget.AllBuffered);// SwitchPlayerType();
        //턴 추가
        PV.RPC("CurTurnAdd", RpcTarget.AllBuffered);// CurTurnAdd();
    }

    //단말기 메세지
    public string ChessMissionMessage()
    {
        string text = "";

        text += CurMission.Turn.ToString() + "남았습니다.\n";
        text += "턴:\t" + CurMission.Turn + "\n";
        text += "색상:\t" + CurMission.Color + "\n";
        text += "목표:\t" + CurMission.Piece + "\n";

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

        PV.RPC("ChangePieceColor", RpcTarget.AllBuffered, (int)changePiece);// ChangePieceColor(changePiece);

    }

    //체스 말 바꿈
    [PunRPC]
    void ChangePieceColor(int _changeP)
    {
        CHESSPIECE changePiece = (CHESSPIECE)_changeP;

        foreach (BoardManager boardManager in boardManagerList)
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


    //미션 세팅
    void SetMission()
    {
        Debug.Log("MissionSet");
        CurMission = MissionList[Random.Range(0, MissionList.Count)];
        CurMission.Turn *= 2;

        

        PV.RPC("SendMission", RpcTarget.AllBuffered, CurMission.Turn, (int)CurMission.Color, (int)CurMission.Piece);

    }

    //미션 보냄
    [PunRPC]
    void SendMission(int _Turn, int _Color, int _Piece)
    {
        this.CurMission.Turn = _Turn;
        this.CurMission.Color = (PLAYERTYPE)_Color;
        this.CurMission.Piece = (CHESSPIECE)_Piece;
        //단말기 메세지 보냄
        _NetworkChatManager.Instance.AddLine(ChessMissionMessage());
    }

    //게임 끝났는지
    bool CheckGameCount(bool _isMission)
    {
        //목표 말 잡기?
        if(_isMission)
        {        
            //최대 게임 카운트 안에 게임 끝냄
            if (CurCount >= SuccessMissionCount
                && CurCount < MaxMissionCount)
            {
                //책상열리는거 여기 추가

                Debug.Log($"체스게임 성공, 현재카운트 :CurCount {CurCount}");
                return true;
            }

        }
        else
        {
            //최대 게임 카운트보다 많아짐
            if (CurCount >= MaxMissionCount)
            {
                //폭탄 시간 줄어듬
                DigitalClock.M_clock.M_currentSeconds -= PenaltyTime;

                foreach (BoardManager boardManager in boardManagerList)
                {
                    boardManager.BoardInit();
                }
                Debug.Log("체스게임실패");
                return true;
            }
        }

        if(PhotonNetwork.IsMasterClient)
        {
            SetMission();
        }
        return false;
    }

    //턴 색 바꿈
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
