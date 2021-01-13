using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessZoom : MonoBehaviour
{
    public Transform ZoomPos;       //줌할 포지션
    public CharactorInteraction Player;           //줌되는 플레이어

    //줌되는 속도
    public float PositionSpeed;        
    public float RotationSpeed;

    public Button ExitButton;        //나가기 버튼


    public ZOOMSTATE chessZoomState;   //현재 줌 상태

    Vector3 OriginPlayerPos;                        //원래 위치
    Vector3 OriginPlayerRotate;                     //원래 로테이션

    BoxCollider boxCollider;

    bool isChessSet;                                    //처음 줌 하는건지

    Transform camera;

    void Start()
    {

        boxCollider = GetComponent<BoxCollider>();
        ExitButton.transform.gameObject.SetActive(false);
    }

    void Update()
    {
        switch(chessZoomState)
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

    //줌 인 세팅
    public void ZoomInSet()
    {
        if (!isChessSet)
        {
            ZoomInit();
        }

        camera = Player.m_cam.transform;
        OriginPlayerPos = new Vector3(camera.position.x, camera.position.y, camera.position.z);                                                       //원래 포지션 값
        OriginPlayerRotate = new Vector3(camera.rotation.eulerAngles.x, camera.rotation.eulerAngles.y, camera.rotation.eulerAngles.z);      //원래 로테이션 값
        chessZoomState = ZOOMSTATE.ZOOMIN;                                                                                                      //상태 변경
        boxCollider.enabled = false;                                                                                                                                //콜라이더 끄기
        ExitButton.transform.gameObject.SetActive(true);
    }

    //줌 아웃 세팅
    public void ZoomOutSet()
    {
        ExitButton.transform.gameObject.SetActive(false);
        chessZoomState = ZOOMSTATE.ZOOMOUT;
        Player.MouseSetFalse();
        
    }

    //처음에만 실행, 줌 초기화
    void ZoomInit()
    {
        isChessSet = true;
        ChessMissionManager.Instance.__Init();
        ExitButton.onClick.AddListener(() => ZoomOutSet());
    }

    //줌인
    void ZoomIn()
    {
        camera.position = Vector3.MoveTowards(camera.position, ZoomPos.position, PositionSpeed * Time.deltaTime);
        camera.rotation = Quaternion.Euler(Vector3.MoveTowards(camera.rotation.eulerAngles, ZoomPos.rotation.eulerAngles, RotationSpeed * Time.deltaTime));
    }

    //줌아웃
    void ZoomOut()
    {
        camera.position = Vector3.MoveTowards(camera.position, OriginPlayerPos, PositionSpeed * Time.deltaTime);
        camera.rotation = Quaternion.Euler(Vector3.MoveTowards(camera.rotation.eulerAngles, OriginPlayerRotate, RotationSpeed * Time.deltaTime));

        //원래 위치로 갔을 경우 상태 변경
        if(camera.position == OriginPlayerPos
            && camera.rotation.eulerAngles == OriginPlayerRotate)
        {
            chessZoomState = ZOOMSTATE.NONE;
            boxCollider.enabled = true;
        }
    }
}
