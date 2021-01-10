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

        //�� ó�� ������
        if (!M_PawnMove)
        {
            MoveIndex[0] = new Vector3(MoveIndex[0].x, MoveIndex[0].y, MoveIndex[0].z * 2);
        }
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

        //�밢��
        if (dir > DIRECTIONTYPE.LEFT)
        {
            if (Mathf.Abs(_index.x) != Mathf.Abs(_index.z))
            {
                return false;
            }
        }

        if (!_attck)
        {
            if (_hitBoard.M_isPiece)
            {
                return false;
            }
        }

        if (_attck)
        {
            if(!_hitBoard.M_isPiece)
            {
                return false;
            }

            if (_hitBoard.M_isPiece 
                && _hitBoard.pieceInfo.playerType == this.pieceInfo.playerType)
            {
                return false;
            }
        }




        foreach (DIRECTIONTYPE item in tempDirList)
        {
            int listIndex = tempDirList.IndexOf(item);

           //���ⰰ��
            if (item == dir)
            {
               //�����̷��� ��ġ�� �����ִ� ��ġ���� ū��
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

                    if (boardManager.M_BoardArr[i, j].M_isPiece
                        && boardManager.M_BoardArr[i, j] != _hitBoard)
                    {
                        return false;
                    }

                    /*if (i == _hitBoard.M_BoardIndex.y && j == _hitBoard.M_BoardIndex.x)*/
                    if (i == _hitBoard.pieceInfo.Index.y && j == _hitBoard.pieceInfo.Index.x)
                    {
                        break;
                    }
                }
                

                return true;
            }
        }

        return false;
    }

    public override void MoveTo(Board _hitboard, Piece _hitpiece)
    {
        base.MoveTo(_hitboard, _hitpiece);

        if (!M_PawnMove)
        {
            M_PawnMove = true;
            MoveIndex[0] = new Vector3(MoveIndex[0].x, MoveIndex[0].y, MoveIndex[0].z * 0.5f);
        }
    }

    public override void MoveTileTrue()
    {
        int count = (int)MoveIndex[0].z;
        count = Mathf.Abs(count);

        foreach (Vector3 vec in MoveIndex)
        {
            Index dirvec = GetDirection(Direction(vec));

            Vector3 tempvec = new Vector3(dirvec.x, 0f, dirvec.y);
            for (int j = 0; j < count; j++)
            {
                if (pieceInfo.Index.x + (int)tempvec.x < boardManager.BoardSize.x
                        && pieceInfo.Index.x + (int)tempvec.x > -1
                        && pieceInfo.Index.y + (int)tempvec.z < boardManager.BoardSize.z
                        && pieceInfo.Index.y + (int)tempvec.z > -1)
                {
                    Board board = boardManager.M_BoardArr[pieceInfo.Index.y + (int)tempvec.z, pieceInfo.Index.x + (int)tempvec.x];

                    if (IsMove(tempvec, board, false))
                    {
                        if (board.pieceInfo.playerType
                       != pieceInfo.playerType)
                        {
                            board.MaterialMove();
                            m_moveBoard.Add(board);
                        }
                    }


                }
                else
                {
                    break;
                }

                tempvec.x += dirvec.x;
                tempvec.z += dirvec.y;
            }
        }


        foreach (Vector3 vec in AttckMoveList)
        {
            Index dirvec = GetDirection(Direction(vec));
            

            Vector3 tempvec = new Vector3(dirvec.x, 0f, dirvec.y);
            for (int j = 0; j < count; j++)
            {
                if (pieceInfo.Index.x + (int)tempvec.x < boardManager.BoardSize.x
                        && pieceInfo.Index.x + (int)tempvec.x > -1
                        && pieceInfo.Index.y + (int)tempvec.z < boardManager.BoardSize.z
                        && pieceInfo.Index.y + (int)tempvec.z > -1)
                {
                    Board board = boardManager.M_BoardArr[pieceInfo.Index.y + (int)tempvec.z, pieceInfo.Index.x + (int)tempvec.x];

                    if (IsMove(tempvec, board, true))
                    {
                        if (board.pieceInfo.playerType
                       != pieceInfo.playerType)
                        {
                            board.MaterialMove();
                            m_moveBoard.Add(board);
                        }
                    }


                }
                else
                {
                    break;
                }

                tempvec.x += dirvec.x;
                tempvec.z += dirvec.y;
            }
        }

        boardManager.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].MaterialSelect();
    }


}
