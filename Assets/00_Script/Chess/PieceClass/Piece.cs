using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Piece : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public BoardManager boardManager;       

    public PieceInfo pieceInfo;             //이 말의 정보
    public List<Vector3> MoveIndex;     //이동 가능한 위치
    public Material OriginMaterial;        //원래 색
    public Material PenaltyMatrial;        //패널티 색

    protected List<DIRECTIONTYPE> m_directiontypeList = new List<DIRECTIONTYPE>();      //이동하는 방향
    protected List<Board> m_moveBoard = new List<Board>();                                        //이동가능한 보드 표시

    MeshRenderer m_meshRenderer;                                                                          

    void Start()
    {
       

        __Init();
        
    }


    //초기화
    protected virtual void __Init()
    {
        PV = GetComponent<PhotonView>();

        foreach (Vector3 item in MoveIndex)
        {
            m_directiontypeList.Add(Direction(item));
        }

        m_meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
    }

    //움직일 수 있는지
    public virtual bool IsMove(Vector3 _index, Board _hitBoard, bool _attck)
    {
        //보드 밖으로 벗어나지는 않는지
        if(_index.x + this.pieceInfo.Index.x > boardManager.BoardSize.x
            || _index.x + this.pieceInfo.Index.x < 0
            || _index.z + this.pieceInfo.Index.y > boardManager.BoardSize.z
            || _index.z + this.pieceInfo.Index.y < 0)
        {
            return false;
        }

        //움직이려는 방향
        DIRECTIONTYPE dir = Direction(_index);

        //대각선 움직임
        if (dir > DIRECTIONTYPE.LEFT)
        {
            if (Mathf.Abs(_index.x) != Mathf.Abs(_index.z))
            {
                return false;
            }
        }

        //방향 찾음
        int listIndex = m_directiontypeList.IndexOf(dir);

        //가려는 방향이 없음
        if(listIndex < 0)
        {
            return false;
        }

        //이동하려는 위치가 갈수있는 위치보다 크면 false
        if(Mathf.Abs(_index.x) > Mathf.Abs(MoveIndex[listIndex].x)
            || Mathf.Abs(_index.z) > Mathf.Abs(MoveIndex[listIndex].z))
        {
            return false;
        }
        
        //위동 위치에 말이 있는지 확인하기 위해 더하는 값
        Index tempindex = GetDirection(dir);
        //현재 말 위치
        int i = pieceInfo.Index.y; int j = pieceInfo.Index.x;
        
        int count = 0;
        
        //이동하려는 위치 중간에 말이 있는지
        while (true)
        {
            i += tempindex.y; j += tempindex.x;
        
            //말이 있다
            if(boardManager.M_BoardArr[i, j].M_isPiece
                && boardManager.M_BoardArr[i, j] != _hitBoard)
            {
                return false;
            }
            //이동위치와 같다면 반복문 종료
            if (i == _hitBoard.pieceInfo.Index.y && j == _hitBoard.pieceInfo.Index.x)
            {
                break;
            }
        
            //무한 루프 방지
            if(++count > 1000)
            {
                Debug.Log("Err");
            }
        }
        
        return true;
    }

    //움직임
    [PunRPC]
    public virtual void MoveTo(int _hitboardPType, int _hitboardPiece, int indexX, int indexY/*PieceInfo _hitboard*/)
    {
        //변환
        PieceInfo info = new PieceInfo(_hitboardPType, _hitboardPiece, indexX, indexY);

        Board hitBoard = null;

        //같은 보드 찾음
        foreach (Board board in boardManager.M_BoardArr)
        {
            if(info == board.pieceInfo)
            {
                hitBoard = board;
                break;
            }
        }

        //클릭한 말 null
        boardManager.ClickManager.ClickChessPieceNull();
        //이동 가능한 타일표시X
        MoveTileFalse();

        //이동 하는 보드에 말 정보 등록
        hitBoard.M_isPiece = true;
        hitBoard.pieceInfo.SetType(this.pieceInfo);

        //현재 위치의 보드 말 정보 초기화
        boardManager.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].M_isPiece = false;
        boardManager.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].pieceInfo.InitInfo();

        //이동
        this.transform.position = hitBoard.transform.position;
        //인덱스 변경
        this.pieceInfo.Index.x = hitBoard.pieceInfo.Index.x;
        this.pieceInfo.Index.y = hitBoard.pieceInfo.Index.y;
    }

    //움직임, 말잡기
    [PunRPC]
    public virtual void MoveTo(int _hitboardPType, int _hitboardPiece, int _hitboardindexX, int _hitboardindexY,
        int _hitpiecePType, int _hitpiecePiece, int _hitpieceindexX, int _hitpieceindexY
        /*PieceInfo _hitboard, PieceInfo _hitpiece*/)
    {
        Board hitBoard = null;
        Piece hitPiece = null;
        //변환
        PieceInfo boardinfo = new PieceInfo(_hitboardPType, _hitboardPiece, _hitboardindexX, _hitboardindexY);
        PieceInfo pieceinfo = new PieceInfo(_hitpiecePType, _hitpiecePiece, _hitpieceindexX, _hitpieceindexY);

        //같은 보드 찾기
        foreach (Board board in boardManager.M_BoardArr)
        {
            if (boardinfo == board.pieceInfo)
            {
                hitBoard = board;
                break;
            }
        }

        //같은 말 찾기
        foreach (Piece piece in boardManager.PieceList)
        {
            if (pieceinfo == piece.pieceInfo)
            {
                hitPiece = piece;
                break;
            }
        }

        //선택한 말 null
        boardManager.ClickManager.ClickChessPieceNull();

        //이동가능 보드 표시X
        MoveTileFalse();

        //이동하는 보드 
        hitBoard.M_isPiece = true;
        hitBoard.pieceInfo.SetType(this.pieceInfo);
        //이전에 있던 보드 초기화
        boardManager.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].M_isPiece = false;
        boardManager.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].pieceInfo.InitInfo();

        //움직임
        this.transform.position = hitBoard.transform.position;
        this.pieceInfo.Index.x = hitBoard.pieceInfo.Index.x;
        this.pieceInfo.Index.y = hitBoard.pieceInfo.Index.y;

        //잡은 말 비활성화
        if (hitPiece != null)
        {
            hitPiece.gameObject.SetActive(false);
        }
    }

    //이동가능한 말 보드 색 표시
    public virtual void MoveTileTrue()
    {
        //이동하는 위치
        foreach (Vector3 vec in MoveIndex)
        {
            //더하는 방향 값
            Index dirvec = GetDirection(Direction(vec));
            //for문 반복 수
            int count = 8;
            //해당 위치
            Vector3 tempvec = new Vector3(dirvec.x, 0f, dirvec.y);
            for (int j = 0; j < count; j++)
            {
                //현재 위치+이동하는 위치 범위 밖이 아닐 경우
                if (pieceInfo.Index.x + (int)tempvec.x < boardManager.BoardSize.x
                        && pieceInfo.Index.x + (int)tempvec.x > -1
                        && pieceInfo.Index.y + (int)tempvec.z < boardManager.BoardSize.z
                        && pieceInfo.Index.y + (int)tempvec.z > -1)
                {
                    //해당 위치의 보드 
                    Board board = boardManager.M_BoardArr[pieceInfo.Index.y + (int)tempvec.z, pieceInfo.Index.x + (int)tempvec.x];

                    //움직일 수 있는지
                    if (IsMove(tempvec, board, false))
                    {
                        //같은 색이 아닐 경우
                        if (board.pieceInfo.playerType
                       != pieceInfo.playerType)
                        {
                            //이동 가능
                            board.MaterialMove();
                            m_moveBoard.Add(board);
                        }
                    }


                }
                else  //범위 밖일 경우 반복문 종료
                { 
                    break;
                }

                //다음 보드로
                tempvec.x += dirvec.x;
                tempvec.z += dirvec.y;
            }
        }

        //선택한 말 위치 색 활성화
        boardManager.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].MaterialSelect();
    }

    //움직이려는 보드 초기화
    public void MoveTileFalse()
    {
        //색 표시 끔
        foreach (Board board in m_moveBoard)
        {
            board.MaterialOff();
        }

        //리스트 초기화
        m_moveBoard.Clear();

        //선택한 말 위치 색 초기화
        boardManager.M_BoardArr[pieceInfo.Index.y, pieceInfo.Index.x].MaterialOff();
    }

    //마테리얼 세팅
    public void SetMaterial(Material material)
    {
        m_meshRenderer.material = material;
    }

    //방향얻기
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

    //방향값얻기
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
