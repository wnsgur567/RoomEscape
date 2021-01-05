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
public struct PieceInfo
{
    public PLAYERTYPE playerType;
    public CHESSPIECE chessPiece;
}

