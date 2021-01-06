using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Piece : MonoBehaviour
{
    public PieceInfo pieceInfo;
    public List<Vector3> MoveIndex;
    public BoardManager BoardManager;

    protected List<DIRECTIONTYPE> m_directiontypeList = new List<DIRECTIONTYPE>();

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

    public DIRECTIONTYPE Direction(Vector3 _dir)
    {
        if(_dir.z > 0 && _dir.x == 0)
        {
            return DIRECTIONTYPE.UP;
        }
        else if(_dir.z < 0 && _dir.x == 0)
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

    public Index GetDirection(DIRECTIONTYPE dir)
    {
        Index tempindex = new Index();

        switch(dir)
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

    public virtual bool IsMove(Vector3 _index, Board _hitBoard, bool _attck)
    {
        if(_index.x + this.transform.position.x >= BoardManager.BoardSize.x
            || _index.x + this.transform.position.x < 0
            || _index.z + this.transform.position.z >= BoardManager.BoardSize.z
            || _index.z + this.transform.position.z < 0)
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

                    if(BoardManager.M_BoardArr[i, j].M_isPiece
                        && BoardManager.M_BoardArr[i, j] != _hitBoard)
                    {
                        return false;
                    }

                    if(i == _hitBoard.M_BoardIndex.y && j == _hitBoard.M_BoardIndex.x)
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

    public void MoveTo(Board _hitboard)
    {
        _hitboard.M_isPiece = true;

        BoardManager.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].M_isPiece = false;

        this.transform.position = _hitboard.transform.position;
        this.pieceInfo.Index.x = _hitboard.M_BoardIndex.x;
        this.pieceInfo.Index.y = _hitboard.M_BoardIndex.y;
    }

}
