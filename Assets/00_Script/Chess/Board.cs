using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board : MonoBehaviour
{
    public BoardManager boardManager;

    public bool M_isPiece;
    public PieceInfo pieceInfo;
    public SpriteRenderer ColorQuad;

    private void Start()
    {
        MaterialOff();
    }

    public void MaterialSelect()
    {
        ColorQuad.color = boardManager.SelectRenderer.color;
        ColorQuad.gameObject.SetActive(true);
    }

    public void MaterialMove()
    {
        ColorQuad.color = boardManager.MoveRenderer.color;
        ColorQuad.gameObject.SetActive(true);
    }

    public void MaterialOff()
    {
        ColorQuad.gameObject.SetActive(false);
    }

}
