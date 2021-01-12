using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Board : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public BoardManager boardManager;       //보드 매니져

    public bool M_isPiece;                          //이 보드 위치에 말이 있는지
    public PieceInfo pieceInfo;                    //이 보드 위치에 있는 말 정보
    public SpriteRenderer ColorQuad;           //보드의 색 오브젝트(선택, 이동가능 한 곳 표시할 때 씀)

    private void Start()
    {
        PV = GetComponent<PhotonView>();

        MaterialOff();
    }

    //이 보드를 선택했을때
    public void MaterialSelect()
    {
        ColorQuad.color = boardManager.SelectRenderer.color;
        ColorQuad.gameObject.SetActive(true);
    }

    //이동가능한 위치 표시할때
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
