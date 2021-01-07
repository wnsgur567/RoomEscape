using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    void Start()
    {
        base.__Init();
    }

    void Update()
    {
        
    }

    public override bool IsMove(Vector3 _index, Board _hitBoard, bool _attck)
    {
        if (_index.x + this.transform.position.x >= BoardManager.Instance.BoardSize.x
            || _index.x + this.transform.position.x < 0
            || _index.z + this.transform.position.z >= BoardManager.Instance.BoardSize.z
            || _index.z + this.transform.position.z < 0)
        {
            return false;
        }

        foreach (Vector3 item in MoveIndex)
        {
            if (item == _index)
            {
                return true;
            }
        }

        return false;
    }

    public override void MoveTileTrue()
    {
        foreach (Vector3 vec in MoveIndex)
        {
            if (pieceInfo.Index.x + (int)vec.x  < BoardManager.Instance.BoardSize.x
            && pieceInfo.Index.x + (int)vec.x > -1
            && pieceInfo.Index.y + (int)vec.z < BoardManager.Instance.BoardSize.z
            && pieceInfo.Index.y + (int)vec.z > -1)
            {
                Board board = BoardManager.Instance.M_BoardArr[pieceInfo.Index.y + (int)vec.z, pieceInfo.Index.x + (int)vec.x];
                if (board.pieceInfo.playerType
                    != pieceInfo.playerType)
                {
                    board.MaterialMove();
                    m_moveBoard.Add(board);
                }
            }
        }

        BoardManager.Instance.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].MaterialSelect();
    }

}
