using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Piece : MonoBehaviour
{
    public PieceInfo pieceInfo;
    public List<Vector3> MoveIndex;

    protected List<DIRECTIONTYPE> m_directiontypeList = new List<DIRECTIONTYPE>();
    protected List<Board> m_moveBoard = new List<Board>();
    void Start()
    {
        __Init();

    }

    protected virtual void __Init()
    {
        foreach (Vector3 item in MoveIndex)
        {
            m_directiontypeList.Add(Direction(item));
        }
    }

    public virtual bool IsMove(Vector3 _index, Board _hitBoard, bool _attck)
    {
        if(_index.x + this.pieceInfo.Index.x > BoardManager.Instance.BoardSize.x
            || _index.x + this.pieceInfo.Index.x < 0
            || _index.z + this.pieceInfo.Index.y > BoardManager.Instance.BoardSize.z
            || _index.z + this.pieceInfo.Index.y < 0)
        {
            return false;
        }

        DIRECTIONTYPE dir = Direction(_index);

        if (dir > DIRECTIONTYPE.LEFT)
        {
            if (Mathf.Abs(_index.x) != Mathf.Abs(_index.z))
            {
                return false;
            }
        }

        

        foreach (DIRECTIONTYPE item in m_directiontypeList)
        {
            int listIndex = m_directiontypeList.IndexOf(item);

            if (item == dir)
            {
                if(Mathf.Abs(_index.x) > Mathf.Abs(MoveIndex[listIndex].x)
                    || Mathf.Abs(_index.z) > Mathf.Abs(MoveIndex[listIndex].z))
                {
                    return false;
                }


                Index tempindex = GetDirection(dir);
                int i = pieceInfo.Index.y; int j = pieceInfo.Index.x;

                int count = 0;

                while (true)
                {
                    i += tempindex.y; j += tempindex.x;

                    if(BoardManager.Instance.M_BoardArr[i, j].M_isPiece
                        && BoardManager.Instance.M_BoardArr[i, j] != _hitBoard)
                    {
                        return false;
                    }

                    /*if(i == _hitBoard.M_BoardIndex.y && j == _hitBoard.M_BoardIndex.x)*/
                    if (i == _hitBoard.pieceInfo.Index.y && j == _hitBoard.pieceInfo.Index.x)
                    {
                        break;
                    }

                    if(++count > 1000)
                    {
                        Debug.Log("Err");
                    }
                }

                return true;
            }
        }
     


        return false;
    }

    public virtual void MoveTo(Board _hitboard)
    {
        MouseClick.Instance.ClickObj = null;

        MoveTileFalse();

        _hitboard.M_isPiece = true;
        _hitboard.pieceInfo.SetType(this.pieceInfo);

        BoardManager.Instance.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].M_isPiece = false;
        BoardManager.Instance.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].pieceInfo.InitInfo();

        this.transform.position = _hitboard.transform.position;
        this.pieceInfo.Index.x = _hitboard.pieceInfo.Index.x;
        this.pieceInfo.Index.y = _hitboard.pieceInfo.Index.y;

        ChessMissionManager.Instance.TurnPlayer = SwitchPlayerType(ChessMissionManager.Instance.TurnPlayer);
        ChessMissionManager.Instance.CurTurnAdd();
    }

    public virtual void MoveTileTrue()
    {
        foreach (Vector3 vec in MoveIndex)
        {
            Index dirvec = GetDirection(Direction(vec));
            int count = 8;

            Vector3 tempvec = new Vector3(dirvec.x, 0f, dirvec.y);
            for (int j = 0; j < count; j++)
            {
                if (pieceInfo.Index.x + (int)tempvec.x < BoardManager.Instance.BoardSize.x
                        && pieceInfo.Index.x + (int)tempvec.x > -1
                        && pieceInfo.Index.y + (int)tempvec.z < BoardManager.Instance.BoardSize.z
                        && pieceInfo.Index.y + (int)tempvec.z > -1)
                {
                    Board board = BoardManager.Instance.M_BoardArr[pieceInfo.Index.y + (int)tempvec.z, pieceInfo.Index.x + (int)tempvec.x];

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

        BoardManager.Instance.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].MaterialSelect();
    }

    public void MoveTileFalse()
    {
        foreach (Board board in m_moveBoard)
        {
            board.MaterialOff();
        }

        m_moveBoard.Clear();

        BoardManager.Instance.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].MaterialOff();
    }

    protected DIRECTIONTYPE Direction(Vector3 _dir)
    {
        if (_dir.z > 0 && _dir.x == 0)
        {
            return DIRECTIONTYPE.UP;
        }
        else if (_dir.z < 0 && _dir.x == 0)
        {
            return DIRECTIONTYPE.DOWN;
        }
        else if (_dir.z == 0 && _dir.x > 0)
        {
            return DIRECTIONTYPE.RIGHT;
        }
        else if (_dir.z == 0 && _dir.x < 0)
        {
            return DIRECTIONTYPE.LEFT;
        }
        else if (_dir.z > 0 && _dir.x > 0)
        {
            return DIRECTIONTYPE.UP_RIGHT;
        }
        else if (_dir.z < 0 && _dir.x > 0)
        {
            return DIRECTIONTYPE.DOWN_RIGHT;
        }
        else if (_dir.z > 0 && _dir.x < 0)
        {
            return DIRECTIONTYPE.UP_LEFT;
        }
        else if (_dir.z < 0 && _dir.x < 0)
        {
            return DIRECTIONTYPE.DOWN_LEFT;
        }

        return DIRECTIONTYPE.NONE;
    }

    protected Index GetDirection(DIRECTIONTYPE dir)
    {
        Index tempindex = new Index();

        switch (dir)
        {
            case DIRECTIONTYPE.UP:
                tempindex.x = 0; tempindex.y = 1;
                break;
            case DIRECTIONTYPE.DOWN:
                tempindex.x = 0; tempindex.y = -1;
                break;
            case DIRECTIONTYPE.RIGHT:
                tempindex.x = 1; tempindex.y = 0;
                break;
            case DIRECTIONTYPE.LEFT:
                tempindex.x = -1; tempindex.y = 0;
                break;
            case DIRECTIONTYPE.UP_RIGHT:
                tempindex.x = 1; tempindex.y = 1;
                break;
            case DIRECTIONTYPE.DOWN_RIGHT:
                tempindex.x = 1; tempindex.y = -1;
                break;
            case DIRECTIONTYPE.UP_LEFT:
                tempindex.x = -1; tempindex.y = 1;
                break;
            case DIRECTIONTYPE.DOWN_LEFT:
                tempindex.x = -1; tempindex.y = -1;
                break;
        }

        return tempindex;
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
