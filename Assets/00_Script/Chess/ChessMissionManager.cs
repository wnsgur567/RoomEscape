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
        if(CurTurn> CurMission.Turn)
        {
            //����ī��Ʈ �ø�
            CurCount++;
            //���� �ʱ�ȭ �ؾ���
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
            //���� ī��Ʈ �ø�
            CurCount++;

            if(CurCount >= SuccessMissionCount)
            {
                Debug.Log($"Complete :CurCount {CurCount}");
            }

            //�ִ� ���� ī��Ʈ���� ������
            if (CurCount >= MaxMissionCount)
            {
                Debug.Log("Fail");
            }



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
        CurMission = MissionList[Random.Range(0, MissionList.Count)];
        CurMission.Turn *= 2;
    }
}
