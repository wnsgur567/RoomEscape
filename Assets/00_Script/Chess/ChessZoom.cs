using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessZoom : MonoBehaviour
{
    public Transform ZoomPos;       //���� ������
    public CharactorInteraction Player;           //�ܵǴ� �÷��̾�

    //�ܵǴ� �ӵ�
    public float PositionSpeed;
    public float RotationSpeed;

    public Button ExitButton;        //������ ��ư


    public ZOOMSTATE chessZoomState;   //���� �� ����

    Vector3 OriginPlayerPos;                        //���� ��ġ
    Vector3 OriginPlayerRotate;                     //���� �����̼�

    BoxCollider boxCollider;

    bool isChessSet;                                    //ó�� �� �ϴ°���

    Transform camera;
    [SerializeField]
    private bool m_isChess;
    void Start()
    {

        boxCollider = GetComponent<BoxCollider>();
        ExitButton.transform.gameObject.SetActive(false);
    }

    void Update()
    {
        switch (chessZoomState)
        {
            case ZOOMSTATE.ZOOMIN:
                ZoomIn();
                break;
            case ZOOMSTATE.ZOOMOUT:
                ZoomOut();
                break;
        }

    }

    private void OnDestroy()
    {
        ExitButton.onClick.RemoveAllListeners();
    }

    //�� �� ����
    public void ZoomInSet()
    {
        if (!isChessSet)
        {
            ZoomInit();
        }

        camera = Player.m_cam.transform;
        OriginPlayerPos = new Vector3(camera.position.x, camera.position.y, camera.position.z);                                                       //���� ������ ��
        OriginPlayerRotate = new Vector3(camera.rotation.eulerAngles.x, camera.rotation.eulerAngles.y, camera.rotation.eulerAngles.z);      //���� �����̼� ��
        chessZoomState = ZOOMSTATE.ZOOMIN;                                                                                                      //���� ����
        boxCollider.enabled = false;                                                                                                                                //�ݶ��̴� ����
        ExitButton.transform.gameObject.SetActive(true);
        Player.MouseSetTrue();
    }

    //�� �ƿ� ����
    public void ZoomOutSet()
    {
        ExitButton.transform.gameObject.SetActive(false);
        chessZoomState = ZOOMSTATE.ZOOMOUT;


    }

    //ó������ ����, �� �ʱ�ȭ
    void ZoomInit()
    {
        //2 line only chess
        if (m_isChess)
        {
            isChessSet = true;
            ChessMissionManager.Instance.__Init();
        }
        ExitButton.onClick.AddListener(() => ZoomOutSet());
    }

    //����
    void ZoomIn()
    {
        camera.position = Vector3.MoveTowards(camera.position, ZoomPos.position, PositionSpeed * Time.deltaTime);
        camera.rotation = Quaternion.Euler(Vector3.MoveTowards(camera.rotation.eulerAngles, ZoomPos.rotation.eulerAngles, RotationSpeed * Time.deltaTime));
    }

    //�ܾƿ�
    void ZoomOut()
    {
        camera.position = Vector3.MoveTowards(camera.position, OriginPlayerPos, PositionSpeed * Time.deltaTime);
        camera.rotation = Quaternion.Euler(Vector3.MoveTowards(camera.rotation.eulerAngles, OriginPlayerRotate, RotationSpeed * Time.deltaTime));

        //���� ��ġ�� ���� ��� ���� ����
        if (camera.position == OriginPlayerPos
            && camera.rotation.eulerAngles == OriginPlayerRotate)
        {
            chessZoomState = ZOOMSTATE.NONE;
            Player.MouseSetFalse();
            if (!ChessMissionManager.Instance.isClear)
            {
                boxCollider.enabled = true;
            }
        }
    }
}
