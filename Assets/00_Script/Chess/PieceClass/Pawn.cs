using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public bool M_PawnMove = false;
    public List<Vector3> AttckMoveList;
    List<DIRECTIONTYPE> m_AttackDirList = new List<DIRECTIONTYPE>();

    void Start()
    {
        __Init();
    }

    void Update()
    {
        
    }

    protected override void __Init()
    {
        base.__Init();

        foreach (Vector3 item in AttckMoveList)
        {
            m_AttackDirList.Add(Direction(item));
        }
    }


    public override bool IsMove(Vector3 _index, Board _hitBoard, bool _attck)
    {
        DIRECTIONTYPE dir = Direction(_index);

        List<DIRECTIONTYPE> tempDirList = m_directiontypeList;
        List<Vector3> tempVec = MoveIndex;
        if (_attck)
        {
            tempDirList = m_AttackDirList;
            tempVec = AttckMoveList;
        }

        //대각선
        if (dir > DIRECTIONTYPE.LEFT)
        {
            if (Mathf.Abs(_index.x) != Mathf.Abs(_index.z))
            {
                return false;
            }
        }

        //폰 처음 움직임
        if(!M_PawnMove)
        {
            MoveIndex[0] = new Vector3(MoveIndex[0].x, MoveIndex[0].y, MoveIndex[0].z * 2);
        }


        foreach (DIRECTIONTYPE item in tempDirList)
        {
            int listIndex = tempDirList.IndexOf(item);

           //방향같음
            if (item == dir)
            {
               //움직이려는 위치가 갈수있는 위치보다 큰지
                if (Mathf.Abs(_index.x) > Mathf.Abs(tempVec[listIndex].x)
                    || Mathf.Abs(_index.z) > Mathf.Abs(tempVec[listIndex].z))
                {
                    return false;
                }


                Index tempindex = GetDirection(dir);
                int i = pieceInfo.Index.y; int j = pieceInfo.Index.x;

                while (true)
                {
                    i += tempindex.y; j += tempindex.x;

                    if (BoardManager.M_BoardArr[i, j].M_isPiece
                        && BoardManager.M_BoardArr[i, j] != _hitBoard)
                    {
                        return false;
                    }

                    if (i == _hitBoard.M_BoardIndex.y && j == _hitBoard.M_BoardIndex.x)
                    {
                        break;
                    }
                }

                //폰 처음움직임
                if (!M_PawnMove)
                {
                    MoveIndex[0] = new Vector3(MoveIndex[0].x, MoveIndex[0].y, MoveIndex[0].z * 0.5f);
                    M_PawnMove = true;
                }

                return true;
            }
        }

        return false;
    }

}
