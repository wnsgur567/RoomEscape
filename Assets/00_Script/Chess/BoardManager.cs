using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public Vector3 PieceSize;
    public Vector3 BoardSize;

    public List<Piece> PieceList;

    public Board BoardObj;
    public Board[,] M_BoardArr = new Board[8, 8];

    void Start()
    {
        CreateBoardCollider();
        __Init();
    }


    void __Init()
    {
        for (int i=0; i<8; i++)
        {
            for(int j=0; j<8; j++)
            {
                foreach (Piece piece in PieceList)
                {
                    if (M_BoardArr[i,j].transform.position == piece.transform.position)
                    {
                        M_BoardArr[i, j].M_isPiece = true;
                        piece.pieceInfo.Index.y = i;
                        piece.pieceInfo.Index.x = j;
                        break;
                    }
                }
            }
        }
     
         
     
    }

    [ContextMenu("CreateBoardCollider")]
    public void CreateBoardCollider()
    {
        float tempy = 0;
        float tempx = 0;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject gameObject = Instantiate(BoardObj.gameObject, new Vector3(tempx, 0f, tempy) + this.gameObject.transform.position
                    , Quaternion.identity, this.transform);
                M_BoardArr[i,j] = gameObject.GetComponent<Board>();
                M_BoardArr[i, j].M_BoardIndex.y = i;
                M_BoardArr[i, j].M_BoardIndex.x = j;
                tempx += PieceSize.x;

            }
            tempx = 0;
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
