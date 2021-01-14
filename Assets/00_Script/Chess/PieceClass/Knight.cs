using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    void Start()
    {
        base.__Init();
    }

    public override bool IsMove(Vector3 _index, Board _hitBoard, bool _attck)
    {
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
            if (pieceInfo.Index.x + (int)vec.x < boardManager.BoardSize.x
            && pieceInfo.Index.x + (int)vec.x > -1
            && pieceInfo.Index.y + (int)vec.z < boardManager.BoardSize.z
            && pieceInfo.Index.y + (int)vec.z > -1)
            {
                Board board = boardManager.M_BoardArr[pieceInfo.Index.y + (int)vec.z, pieceInfo.Index.x + (int)vec.x];
                if (board.pieceInfo.playerType
                    != pieceInfo.playerType)
                {
                    board.MaterialMove();
                    m_moveBoard.Add(board);
                }
            }
        }

        boardManager.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].MaterialSelect();
    }

}
