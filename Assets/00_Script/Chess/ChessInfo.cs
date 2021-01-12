using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//체스말
[System.Serializable]
public enum CHESSPIECE
{
    NONE,

    PAWN,
    ROOK,
    KNIGHT,
    BISHOP,
    QUEEN,
    KING,

    MAX
}

//플레이어 색
[System.Serializable]
public enum PLAYERTYPE
{
    NONE,

    BLACK,
    WHITE,

    MAX
}

//방향
[System.Serializable]
public enum DIRECTIONTYPE
{
    NONE,

    UP,
    DOWN,
    RIGHT,
    LEFT,
    UP_RIGHT,
    DOWN_RIGHT,
    UP_LEFT,
    DOWN_LEFT,

    MAX
}

//보드 배열 인덱스
[System.Serializable]
public struct Index
{
    public int x;
    public int y;

    public Index(int indexX, int indexY) : this()
    {
        x = indexX;
        y = indexY;
    }

    public static bool operator ==(Index op1, Index op2)
    {
        if (op1.x == op2.x
            && op1.y == op2.y)
        {
            return true;
        }

        return false;
    }

    public static bool operator !=(Index op1, Index op2)
    {
        if (op1.x != op2.x
            && op1.y != op2.y)
        {
            return true;
        }

        return false;
    }
}


//체스 정보
[System.Serializable]
public struct PieceInfo
{
    //색
    public PLAYERTYPE playerType;
    //말
    public CHESSPIECE chessPiece;
    //보드 인덱스
    public Index Index;

    public PieceInfo(int _PType, int _Piece, int indexX, int indexY)
    {
        playerType = (PLAYERTYPE)_PType;
        chessPiece = (CHESSPIECE)_Piece;
        Index = new Index(indexX, indexY);
    }

    public void SetType(PieceInfo _info)
    {
        playerType = _info.playerType;
        chessPiece = _info.chessPiece;
    }

    public void InitInfo()
    {
        playerType = PLAYERTYPE.NONE;
        chessPiece = CHESSPIECE.NONE;
    }

    public static bool operator ==(PieceInfo op1, PieceInfo op2)
    {
        if(op1.playerType == op2.playerType
            && op1.chessPiece == op2.chessPiece
            && op1.Index == op2.Index)
        {
            return true;
        }

        return false;
    }

    public static bool operator !=(PieceInfo op1, PieceInfo op2)
    {
        if (op1.playerType != op2.playerType
           && op1.chessPiece != op2.chessPiece
           && op1.Index != op2.Index)
        {
            return true;
        }

        return false;
    }

}

//미션 정보
[System.Serializable]
public struct ChessMissionInfo
{
    public int Turn;
    public PLAYERTYPE Color;
    public CHESSPIECE Piece;
}

