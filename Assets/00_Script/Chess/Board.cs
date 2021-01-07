using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board : MonoBehaviour
{
    public bool M_isPiece;
    public PieceInfo pieceInfo;
    //public Index M_BoardIndex;
    public MeshRenderer ColorQuad;

    private void Start()
    {
        MaterialOff();
    }

    public void MaterialSelect()
    {
        ColorQuad.material = BoardManager.Instance.SelectMaterial;
        ColorQuad.gameObject.SetActive(true);
    }

    public void MaterialMove()
    {
        ColorQuad.material = BoardManager.Instance.MoveMaterial;
        ColorQuad.gameObject.SetActive(true);
    }

    public void MaterialOff()
    {
        ColorQuad.gameObject.SetActive(false);
    }

}
