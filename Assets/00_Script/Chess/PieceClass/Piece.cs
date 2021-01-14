using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Piece : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public BoardManager boardManager;

    public PieceInfo pieceInfo;             //�� ���� ����
    public List<Vector3> MoveIndex;     //�̵� ������ ��ġ
    public Material OriginMaterial;        //���� ��
    public Material PenaltyMatrial;        //�г�Ƽ ��

    protected List<DIRECTIONTYPE> m_directiontypeList = new List<DIRECTIONTYPE>();      //�̵��ϴ� ����
    protected List<Board> m_moveBoard = new List<Board>();                                        //�̵������� ���� ǥ��

    MeshRenderer m_meshRenderer;

    void Start()
    {


        __Init();

    }


    //�ʱ�ȭ
    protected virtual void __Init()
    {
        PV = GetComponent<PhotonView>();

        foreach (Vector3 item in MoveIndex)
        {
            m_directiontypeList.Add(Direction(item));
        }

        m_meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
    }

    //������ �� �ִ���
    public virtual bool IsMove(Vector3 _index, Board _hitBoard, bool _attck)
    {
        //�����̷��� ����
        DIRECTIONTYPE dir = Direction(_index);

        //�밢�� ������
        if (dir > DIRECTIONTYPE.LEFT)
        {
            if (Mathf.Abs(_index.x) != Mathf.Abs(_index.z))
            {
                return false;
            }
        }

        //���� ã��
        int listIndex = m_directiontypeList.IndexOf(dir);

        //������ ������ ����
        if (listIndex < 0)
        {
            return false;
        }

        //�̵��Ϸ��� ��ġ�� �����ִ� ��ġ���� ũ�� false
        if (Mathf.Abs(_index.x) > Mathf.Abs(MoveIndex[listIndex].x)
            || Mathf.Abs(_index.z) > Mathf.Abs(MoveIndex[listIndex].z))
        {
            return false;
        }

        //���� ��ġ�� ���� �ִ��� Ȯ���ϱ� ���� ���ϴ� ��
        Index tempindex = GetDirection(dir);
        //���� �� ��ġ
        int i = pieceInfo.Index.y; int j = pieceInfo.Index.x;

        int count = 0;

        //�̵��Ϸ��� ��ġ �߰��� ���� �ִ���
        while (true)
        {
            i += tempindex.y; j += tempindex.x;

            //���� �ִ�
            if (boardManager.M_BoardArr[i, j].M_isPiece
                && boardManager.M_BoardArr[i, j] != _hitBoard)
            {
                return false;
            }
            //�̵���ġ�� ���ٸ� �ݺ��� ����
            if (i == _hitBoard.pieceInfo.Index.y && j == _hitBoard.pieceInfo.Index.x)
            {
                break;
            }

            //���� ���� ����
            if (++count > 1000)
            {
                Debug.Log("Err");
            }
        }

        return true;
    }

    //������
    [PunRPC]
    public virtual void MoveTo(int _hitboardPType, int _hitboardPiece, int indexX, int indexY, bool isSound)
    {
        //��ȯ
        PieceInfo info = new PieceInfo(_hitboardPType, _hitboardPiece, indexX, indexY);

        Board hitBoard = null;

        //���� ���� ã��
        foreach (Board board in boardManager.M_BoardArr)
        {
            if (info == board.pieceInfo)
            {
                hitBoard = board;
                break;
            }
        }

        //Ŭ���� �� null
        boardManager.ClickManager.ClickChessPieceNull();
        //�̵� ������ Ÿ��ǥ��X
        MoveTileFalse();

        //�̵� �ϴ� ���忡 �� ���� ���
        hitBoard.M_isPiece = true;
        hitBoard.pieceInfo.SetType(this.pieceInfo);

        //���� ��ġ�� ���� �� ���� �ʱ�ȭ
        boardManager.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].M_isPiece = false;
        boardManager.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].pieceInfo.InitInfo();

        //�̵�
        this.transform.position = hitBoard.transform.position;
        //�ε��� ����
        this.pieceInfo.Index.x = hitBoard.pieceInfo.Index.x;
        this.pieceInfo.Index.y = hitBoard.pieceInfo.Index.y;
        if (isSound)
        {
            _SoundManager.Instance.PlayObjInterationSound(E_ObjectInterationSound.chess_chess);
        }
    }

    //������, �����
    [PunRPC]
    public virtual void MoveTo(int _hitboardPType, int _hitboardPiece, int _hitboardindexX, int _hitboardindexY,
        int _hitpiecePType, int _hitpiecePiece, int _hitpieceindexX, int _hitpieceindexY, bool isSound
        /*PieceInfo _hitboard, PieceInfo _hitpiece*/)
    {
        Board hitBoard = null;
        Piece hitPiece = null;
        //��ȯ
        PieceInfo boardinfo = new PieceInfo(_hitboardPType, _hitboardPiece, _hitboardindexX, _hitboardindexY);
        PieceInfo pieceinfo = new PieceInfo(_hitpiecePType, _hitpiecePiece, _hitpieceindexX, _hitpieceindexY);

        //���� ���� ã��
        foreach (Board board in boardManager.M_BoardArr)
        {
            if (boardinfo == board.pieceInfo)
            {
                hitBoard = board;
                break;
            }
        }

        //���� �� ã��
        foreach (Piece piece in boardManager.PieceList)
        {
            if (pieceinfo == piece.pieceInfo)
            {
                hitPiece = piece;
                break;
            }
        }

        //������ �� null
        boardManager.ClickManager.ClickChessPieceNull();

        //�̵����� ���� ǥ��X
        MoveTileFalse();

        //�̵��ϴ� ���� 
        hitBoard.M_isPiece = true;
        hitBoard.pieceInfo.SetType(this.pieceInfo);
        //������ �ִ� ���� �ʱ�ȭ
        boardManager.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].M_isPiece = false;
        boardManager.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].pieceInfo.InitInfo();

        //������
        this.transform.position = hitBoard.transform.position;
        this.pieceInfo.Index.x = hitBoard.pieceInfo.Index.x;
        this.pieceInfo.Index.y = hitBoard.pieceInfo.Index.y;

        //���� �� ��Ȱ��ȭ
        if (hitPiece != null)
        {
            hitPiece.gameObject.SetActive(false);
        }
        if (isSound)
        {
            _SoundManager.Instance.PlayObjInterationSound(E_ObjectInterationSound.chess_chess);
        }
    }

    //�̵������� �� ���� �� ǥ��
    public virtual void MoveTileTrue()
    {
        //�̵��ϴ� ��ġ
        foreach (Vector3 vec in MoveIndex)
        {
            //���ϴ� ���� ��
            Index dirvec = GetDirection(Direction(vec));
            //for�� �ݺ� ��
            int count = 8;
            //�ش� ��ġ
            Vector3 tempvec = new Vector3(dirvec.x, 0f, dirvec.y);
            for (int j = 0; j < count; j++)
            {
                //���� ��ġ+�̵��ϴ� ��ġ ���� ���� �ƴ� ���
                if (pieceInfo.Index.x + (int)tempvec.x < boardManager.BoardSize.x
                        && pieceInfo.Index.x + (int)tempvec.x > -1
                        && pieceInfo.Index.y + (int)tempvec.z < boardManager.BoardSize.z
                        && pieceInfo.Index.y + (int)tempvec.z > -1)
                {
                    //�ش� ��ġ�� ���� 
                    Board board = boardManager.M_BoardArr[pieceInfo.Index.y + (int)tempvec.z, pieceInfo.Index.x + (int)tempvec.x];

                    //������ �� �ִ���
                    if (IsMove(tempvec, board, false))
                    {
                        //���� ���� �ƴ� ���
                        if (board.pieceInfo.playerType
                       != pieceInfo.playerType)
                        {
                            //�̵� ����
                            board.MaterialMove();
                            m_moveBoard.Add(board);
                        }
                    }


                }
                else  //���� ���� ��� �ݺ��� ����
                {
                    break;
                }

                //���� �����
                tempvec.x += dirvec.x;
                tempvec.z += dirvec.y;
            }
        }

        //������ �� ��ġ �� Ȱ��ȭ
        boardManager.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].MaterialSelect();
    }

    //�����̷��� ���� �ʱ�ȭ
    public void MoveTileFalse()
    {
        //�� ǥ�� ��
        foreach (Board board in m_moveBoard)
        {
            board.MaterialOff();
        }

        //����Ʈ �ʱ�ȭ
        m_moveBoard.Clear();

        //������ �� ��ġ �� �ʱ�ȭ
        boardManager.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].MaterialOff();
    }

    //���׸��� ����
    public void SetMaterial(Material material)
    {
        m_meshRenderer.material = material;
    }

    //������
    protected DIRECTIONTYPE Direction(Vector3 _dir)
    {
        if (_dir.z > 0 && _dir.x == 0)
        {
            return DIRECTIONTYPE.UP;
        }
        else if (_dir.z < 0 && _dir.x == 0)
        {
            return DIRECTIONTYPE.DOWN;
        }
        else if (_dir.z == 0 && _dir.x > 0)
        {
            return DIRECTIONTYPE.RIGHT;
        }
        else if (_dir.z == 0 && _dir.x < 0)
        {
            return DIRECTIONTYPE.LEFT;
        }
        else if (_dir.z > 0 && _dir.x > 0)
        {
            return DIRECTIONTYPE.UP_RIGHT;
        }
        else if (_dir.z < 0 && _dir.x > 0)
        {
            return DIRECTIONTYPE.DOWN_RIGHT;
        }
        else if (_dir.z > 0 && _dir.x < 0)
        {
            return DIRECTIONTYPE.UP_LEFT;
        }
        else if (_dir.z < 0 && _dir.x < 0)
        {
            return DIRECTIONTYPE.DOWN_LEFT;
        }

        return DIRECTIONTYPE.NONE;
    }

    //���Ⱚ���
    protected Index GetDirection(DIRECTIONTYPE dir)
    {
        Index tempindex = new Index();

        switch (dir)
        {
            case DIRECTIONTYPE.UP:
                tempindex.x = 0; tempindex.y = 1;
                break;
            case DIRECTIONTYPE.DOWN:
                tempindex.x = 0; tempindex.y = -1;
                break;
            case DIRECTIONTYPE.RIGHT:
                tempindex.x = 1; tempindex.y = 0;
                break;
            case DIRECTIONTYPE.LEFT:
                tempindex.x = -1; tempindex.y = 0;
                break;
            case DIRECTIONTYPE.UP_RIGHT:
                tempindex.x = 1; tempindex.y = 1;
                break;
            case DIRECTIONTYPE.DOWN_RIGHT:
                tempindex.x = 1; tempindex.y = -1;
                break;
            case DIRECTIONTYPE.UP_LEFT:
                tempindex.x = -1; tempindex.y = 1;
                break;
            case DIRECTIONTYPE.DOWN_LEFT:
                tempindex.x = -1; tempindex.y = -1;
                break;
        }

        return tempindex;
    }


}
