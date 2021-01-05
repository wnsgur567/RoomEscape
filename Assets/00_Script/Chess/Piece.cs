using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Piece : MonoBehaviour
{
    public PieceInfo pieceInfo;
    public List<Vector3> MoveIndex;
    public List<DIRECTIONTYPE> directiontypeList;
    public BoardManager BoardManager;

    void Start()
    {
        foreach(Vector3 item in MoveIndex)
        {
            directiontypeList.Add(Direction(item));
        }
    }

    void Update()
    {

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

    public bool IsMove(Vector3 _index)
    {
        if(_index.x + this.transform.position.x >= BoardManager.BoardSize.x
            || _index.x + this.transform.position.x < 0
            || _index.z + this.transform.position.z >= BoardManager.BoardSize.z
            || _index.z + this.transform.position.z < 0)
        {
            return false;
        }

        if(pieceInfo.chessPiece == CHESSPIECE.KNIGHT)
        {
            foreach(Vector3 item in MoveIndex)
            {
                if(item == _index)
                {
                    return true;
                }
            }
        }
        else
        {
            DIRECTIONTYPE dir = Direction(_index);

            if (dir > DIRECTIONTYPE.LEFT)
            {
                if (Mathf.Abs(_index.x) != Mathf.Abs(_index.z))
                {
                    return false;
                }
            }

            foreach (DIRECTIONTYPE item in directiontypeList)
            {

                if(item == dir)
                {
                    return true;
                }
            }
        }


        return false;
    }



}
