using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessMissionManager : Singleton<ChessMissionManager>
{
    public PLAYERTYPE TurnPlayer;                 //현재 플레이어 색
    public List<ChessMissionInfo> MissionList;    //미션 리스트
    public int SuccessMissionCount;                //성공 게임 카운트 
    public int MaxMissionCount;                     //최대 게임 카운트


    ChessMissionInfo CurMission;                   //현재 미션
    int CurTurn;                                        //현재 체스게임 턴

    int CurCount;                                      //현재 게임 카운트

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
            if(!CheckGameCount())
            {
                BoardManager.Instance.PenaltySet();
            }
            Debug.Log($"ChessGameFail:CurCount {CurCount}");
        }
    }

    //미션이 성공했는지
    public bool isMission(PieceInfo _piece)
    {
        //미션성공
        if(_piece.playerType == CurMission.Color
            && _piece.chessPiece == CurMission.piece)
        {
            Debug.Log("ChessGameComplete");
            CurTurn = 0;
            //게임 카운트 올림
            CurCount++;
            CheckGameCount();
            return true;
        }
        //미션실패
       

        return false;
    }

    public string ChessMissionMessage()
    {
        string text = "";

        text += CurMission.Turn.ToString() + "남았습니다.\n";
        text += "턴:\t" + CurMission.Turn + "\n";
        text += "색상:\t" + CurMission.Color + "\n";
        text += "목표:\t" + CurMission.piece + "\n";

        return text;
    }

    void SetMission()
    {
        Debug.Log("MissionSet");
        CurMission = MissionList[Random.Range(0, MissionList.Count)];
        CurMission.Turn *= 2;
    }

    bool CheckGameCount()
    {
        //최대 게임 카운트 안에 게임 끝냄
        if (CurCount >= SuccessMissionCount)
        {
            Debug.Log($"Complete :CurCount {CurCount}");
            return true;
        }

        //최대 게임 카운트보다 많아짐
        if (CurCount >= MaxMissionCount)
        {
            BoardManager.Instance.BoardInit();
            Debug.Log("Fail");
            return false;
        }

        SetMission();
        return false;
    }
}
