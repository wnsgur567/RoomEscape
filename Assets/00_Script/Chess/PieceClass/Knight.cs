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
        if (_index.x + this.transform.position.x >= BoardManager.BoardSize.x
            || _index.x + this.transform.position.x < 0
            || _index.z + this.transform.position.z >= BoardManager.BoardSize.z
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

    

}
