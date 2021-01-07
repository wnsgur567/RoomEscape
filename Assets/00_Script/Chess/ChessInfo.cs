using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
[System.Serializable]
public enum PLAYERTYPE
{
    NONE,

    BLACK,
    WHITE,

    MAX
}

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
[System.Serializable]
public struct Index
{
    public int x;
    public int y;
}

[System.Serializable]
public struct PieceInfo
{
    public PLAYERTYPE playerType;
    public CHESSPIECE chessPiece;
    public Index Index;

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
}

[System.Serializable]
public struct ChessMissionInfo
{
    public int Turn;
    public PLAYERTYPE Color;
    public CHESSPIECE piece;
}

