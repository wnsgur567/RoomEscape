using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Board : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public BoardManager boardManager;       //���� �Ŵ���

    public bool M_isPiece;                          //�� ���� ��ġ�� ���� �ִ���
    public PieceInfo pieceInfo;                    //�� ���� ��ġ�� �ִ� �� ����
    public SpriteRenderer ColorQuad;           //������ �� ������Ʈ(����, �̵����� �� �� ǥ���� �� ��)

    private void Start()
    {
        PV = GetComponent<PhotonView>();

        MaterialOff();
    }

    //�� ���带 ����������
    public void MaterialSelect()
    {
        ColorQuad.color = boardManager.SelectRenderer.color;
        ColorQuad.gameObject.SetActive(true);
    }

    //�̵������� ��ġ ǥ���Ҷ�
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
