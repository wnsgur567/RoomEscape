using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessMissionManager : Singleton<ChessMissionManager>
{
    public PLAYERTYPE TurnPlayer;                 //���� �÷��̾� ��
    public List<ChessMissionInfo> MissionList;    //�̼� ����Ʈ
    public int SuccessMissionCount;                //���� ���� ī��Ʈ 
    public int MaxMissionCount;                     //�ִ� ���� ī��Ʈ


    ChessMissionInfo CurMission;                   //���� �̼�
    int CurTurn;                                        //���� ü������ ��

    int CurCount;                                      //���� ���� ī��Ʈ

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
            if(!CheckGameCount())
            {
                BoardManager.Instance.PenaltySet();
            }
            Debug.Log($"ChessGameFail:CurCount {CurCount}");
        }
    }

    //�̼��� �����ߴ���
    public bool isMission(PieceInfo _piece)
    {
        //�̼Ǽ���
        if(_piece.playerType == CurMission.Color
            && _piece.chessPiece == CurMission.piece)
        {
            Debug.Log("ChessGameComplete");
            CurTurn = 0;
            //���� ī��Ʈ �ø�
            CurCount++;
            CheckGameCount();
            return true;
        }
        //�̼ǽ���
       

        return false;
    }

    public string ChessMissionMessage()
    {
        string text = "";

        text += CurMission.Turn.ToString() + "���ҽ��ϴ�.\n";
        text += "��:\t" + CurMission.Turn + "\n";
        text += "����:\t" + CurMission.Color + "\n";
        text += "��ǥ:\t" + CurMission.piece + "\n";

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
        //�ִ� ���� ī��Ʈ �ȿ� ���� ����
        if (CurCount >= SuccessMissionCount)
        {
            Debug.Log($"Complete :CurCount {CurCount}");
            return true;
        }

        //�ִ� ���� ī��Ʈ���� ������
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
