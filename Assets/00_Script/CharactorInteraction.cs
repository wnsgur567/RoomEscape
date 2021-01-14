using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;

public class CharactorInteraction : MonoBehaviourPunCallbacks
{
    [Header("캐릭터와 오브젝트의 상호작용 거리")]
    [SerializeField]
    private float m_range;//상호작용 거리

    [SerializeField]
    private Camera m_cam;//카메라

    [SerializeField]
    private Image m_Crosshair;//크로스헤어

    public PipeButton M_pipeButton;//파이프버튼 스크립트
    public BombCoverScript M_bombCoverScript;

    private PhotonView m_PV;
    private int m_PaintCount;
    private bool m_PaintStart;

    [SerializeField]
    private CharactorMove m_moveScript;

    [SerializeField]
    private Transform m_Zoom_pos;

    private Transform temp_pos;
    private GameObject temp_obj;

    
    private bool Iskey; 
    private bool IsUsb; 
    public enum State
    {
        Normal = 0,
        ZoomIn
    }
    State state;
    void Start()
    {
        Iskey = false;
        IsUsb = false;
        state = State.Normal;
        m_PV = GetComponent<PhotonView>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_PaintCount = 0;
        m_PaintStart = false;
    }
    void Update()
    {
        if (m_PV.IsMine)
        {
            if (state == State.Normal)
                Interaction();
            else
                ZoomIn();
        }
    }
    void Interaction()
    {
        RaycastHit hit;
        //카메라 기준으로 가운데에다 m_range만큼 레이를 쏨
        if (Physics.Raycast(m_cam.transform.position, m_cam.transform.forward, out hit, m_range))
        {
            //오브젝트가 파이프퍼즐인 경우
            if (hit.transform.CompareTag("PipeButton"))
            {
                //상호작용 크로스헤어 활성화
                m_Crosshair.gameObject.SetActive(true);

                //안내메세지같은거 추가해야함

                //파이프 버튼에서 마우스누르면 파이프 실행
                if (Input.GetMouseButtonDown(0))
                {
                    if (PipePuzzleManager.M_pipeManager.M_IsStarted == false)
                    {
                        PipePuzzleManager.M_pipeManager.M_IsStarted = true;
                        DigitalClock.M_clock.M_puzzleTimer = DigitalClock.M_clock.M_currentSeconds - 120f;
                        Debug.Log("파이프퍼즐 시작");
                        return;
                    }

                    M_pipeButton = hit.transform.GetComponent<PipeButton>();
                    M_pipeButton.ActiveButton();


                    //hit.transform.Rotate(Vector3.forward, Time.deltaTime * 250f, Space.Self);

                }
            }
            else if (hit.transform.CompareTag("Paint") || hit.transform.CompareTag("Paint_correct")) //오브젝트가 그림퍼즐인 경우
            {
                //상호작용 크로스헤어 활성화
                m_Crosshair.gameObject.SetActive(true);

                //안내메세지같은거 추가해야함

                if (Input.GetMouseButtonDown(0))
                {
                    //그림 상호작용이 첫번째(시작) 이라면
                    if (m_PaintStart == false)
                    {
                        m_PaintStart = true;
                        Debug.Log("그림퍼즐시작");
                        return;
                    }
                    else
                    {
                        m_PaintCount++;
                    }

                    //그림 상호작용 카운트를 증가시키고

                    Debug.Log(m_PaintCount);
                    if (hit.transform.CompareTag("Paint"))
                    {
                        Debug.Log("그림임");
                        //그림을 클릭하면
                        //전구깜빡이는 패널티
                        LightPenalty.instance.M_IsPenalty = true;
                        LightPenalty.instance.StartPenalty();
                        if (m_PaintCount == 10)
                        {

                        }
                        //그림이면
                    }
                    else if (hit.transform.CompareTag("Paint_correct"))
                    {
                        //정답이면
                        //비활성화시킴

                        LightPenalty.instance.M_IsPenalty = false;
                        LightPenalty.instance.obj.SetActive(true);
                        hit.transform.gameObject.SetActive(false);
                        Debug.Log("정답임");
                    }
                }
            }
            else if (hit.transform.CompareTag("Radio"))
            {
                m_Crosshair.gameObject.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    GameManager.M_gameManager.StartRadio();
                }
            }
            else if (hit.transform.CompareTag("Bomb")) //오브젝트가 폭탄인경우
            {
                //상호작용 크로스헤어 활성화
                m_Crosshair.gameObject.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    m_Crosshair.gameObject.SetActive(false);
                    //폭탄 원래위치

                    m_tempObj = hit.transform.parent.gameObject;
                    m_tempCol = hit.collider;
                    m_tempTransform = hit.transform.parent.position;
                    m_tempTransformRotate = hit.transform.parent.eulerAngles;

                    m_moveScript.M_Input = false;
                    Debug.Log(hit.transform.tag);

                    temp_pos = hit.transform.parent;
                    temp_obj = hit.transform.parent.gameObject;

                    hit.collider.enabled = false;
                    hit.transform.parent.position = m_Zoom_pos.position;
                    hit.transform.parent.rotation = m_Zoom_pos.rotation;


                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    state = State.ZoomIn;
                }
            }
            else if (hit.transform.CompareTag("USB"))
            {
                m_Crosshair.gameObject.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    IsUsb = true;
                    hit.transform.gameObject.SetActive(false);
                }
            }
            else if (hit.transform.CompareTag("Key"))
            {
                m_Crosshair.gameObject.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    Iskey = true;
                    hit.transform.gameObject.SetActive(false);
                }
            }
            else
            {
                m_Crosshair.gameObject.SetActive(false);
            }
        }
        else
        {
            m_Crosshair.gameObject.SetActive(false);
        }
    }
    [SerializeField]
    private GameObject m_tempObj;
    private Collider m_tempCol;

    [SerializeField]
    Vector3 m_tempTransform;
    Vector3 m_tempTransformRotate;
    void ZoomIn()
    {
        //줌된상태라면
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit _hit;
            if (Physics.Raycast(m_cam.ScreenPointToRay(Input.mousePosition), out _hit))
            {
                if (_hit.transform.CompareTag("Cover"))
                {
                    M_bombCoverScript = _hit.transform.GetComponent<BombCoverScript>();
                    if (M_bombCoverScript.M_State == BombCoverScript.Cover_State.Close)
                        M_bombCoverScript.M_State = BombCoverScript.Cover_State.Open;
                    else
                    {
                        M_bombCoverScript.M_State = BombCoverScript.Cover_State.Close;
                    }
                    Debug.Log("폭탄커버클릭");
                }
                if (_hit.transform.CompareTag("Button"))
                {
                    DigitalClock.M_clock.M_currentSeconds -= 30f;
                }
                if(_hit.transform.CompareTag("CorrectButton"))
                {
                    //d퍼즐 성공
                }
                if(_hit.transform.CompareTag("KeyHolder"))
                {
                    if(Iskey)
                    {
                        _hit.collider.enabled = false;
                        _hit.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
                if(_hit.transform.CompareTag("USBHolder"))
                {
                    if (IsUsb)
                    {
                        _hit.collider.enabled = false;
                        _hit.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
                CutWire(_hit.transform.tag, _hit.transform.parent.gameObject);
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("키눌림");
            //m_tempObj.collider.enabled = true;
            m_tempCol.enabled = true;
            m_tempObj.transform.position = m_tempTransform;
            m_tempObj.transform.eulerAngles = m_tempTransformRotate;


            m_moveScript.M_Input = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            temp_obj.transform.parent = temp_pos;

            state = State.Normal;
        }
    }
    void CutWire(string Wire_name, GameObject Hide_Wire)
    {
        switch (Wire_name)
        {
            case "RedWire":
                DigitalClock.M_clock.M_currentSeconds -= 30f;
                BombScript.M_instance.M_cutRed.SetActive(true);
                Hide_Wire.SetActive(false);
                break;
            case "BlueWire":
                DigitalClock.M_clock.M_currentSeconds -= 30f;
                BombScript.M_instance.M_cutBlue.SetActive(true);
                Hide_Wire.SetActive(false);
                break;
            case "BlackWire":
                BombScript.M_instance.M_cutBlack.SetActive(true);
                Hide_Wire.SetActive(false);
                break;
            case "WhiteWire":
                DigitalClock.M_clock.M_currentSeconds -= 30f;
                BombScript.M_instance.M_cutWhite.SetActive(true);
                Hide_Wire.SetActive(false);
                break;
            case "YellowWire":
                DigitalClock.M_clock.M_currentSeconds -= 30f;
                BombScript.M_instance.M_cutYellow.SetActive(true);
                Hide_Wire.SetActive(false);
                break;
            case "GreenWire":
                DigitalClock.M_clock.M_currentSeconds -= 30f;
                BombScript.M_instance.M_cutGreen.SetActive(true);
                Hide_Wire.SetActive(false);
                break;
        }
    }
    
    private bool IsPointerOverUIObject()
    {
        List<RaycastResult> results = GetUIObjectsUnderPointer();
        return results.Count > 0;
    }

    private List<RaycastResult> GetUIObjectsUnderPointer()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results;
    }
}
