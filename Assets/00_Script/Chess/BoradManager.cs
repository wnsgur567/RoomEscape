using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public Vector3 StartPos;
    public Vector3 PieceSize;
    public Vector3 BoardSize;

    public List<Piece> PieceList;

    public Board BoardObj;
    public Board[,] M_BoardArr = new Board[8, 8];

    void Start()
    {
        CreateCollider();
        __Init();
    }

    void Update()
    {

    }

    void __Init()
    {
        foreach(Board board in M_BoardArr)
        {
            foreach(Piece piece in PieceList)
            {
                if(board.transform.position == piece.transform.position)
                {
                    board.M_isPiece = true;
                }
            }
        }
    }

    [ContextMenu("CreateBoardCollider")]
    public void CreateCollider()
    {
        float tempy = StartPos.y;
        float tempx = StartPos.x;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject gameObject = Instantiate(BoardObj.gameObject, new Vector3(tempx, 0f, tempy), Quaternion.identity, this.transform);
                M_BoardArr[i,j] = gameObject.GetComponent<Board>();
                tempx += PieceSize.x;
            }
            tempx = StartPos.x;
            tempy += PieceSize.y;

        }
    }

    [ContextMenu("ResetBoard")]
    public void ResetBoard()
    {
        foreach (Board item in M_BoardArr)
        {
            GameObject.DestroyImmediate(item.gameObject);
        }
    }
}
