using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class CharactorInteraction : MonoBehaviourPunCallbacks
{
    [Header("캐릭터와 오브젝트의 상호작용 거리")]
    [SerializeField]
    private float m_range;//상호작용 거리

    [SerializeField]
    private Transform m_cam;//카메라

    [SerializeField]
    private Image m_Crosshair;//크로스헤어

    public PipeButton M_pipeButton;//파이프버튼 스크립트

    private PhotonView m_PV;

    void Start()
    {
        m_PV = GetComponent<PhotonView>();
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;

    }
    void Update()
    {
        //마우스커서 확인할려고 임시로 넣은거
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if(m_PV.IsMine)
            Interaction();

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

                M_pipeButton = hit.transform.GetComponent<PipeButton>();
                //안내메세지같은거 추가해야함

                //파이프 버튼에서 마우스누르면 파이프 실행
                if (Input.GetMouseButtonDown(0))
                {
                    M_pipeButton.ActiveButton();
                }
            }
            //오브젝트가 그림퍼즐인 경우
            if (hit.transform.CompareTag("Paint")) // || 정답일 때)
            {
                //상호작용 크로스헤어 활성화
                m_Crosshair.gameObject.SetActive(true);

                //안내메세지같은거 추가해야함

                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("그림임");
                    if(hit.transform.CompareTag("Paint"))
                    {
                        //그림이면
                        //전구깜빡이는 패널티
                    }
                    else
                    {
                        //정답이면

                    }
                    //M_pipeButton.ActiveButton();
                }
            }
        }
        else
        {
            //상호작용할게 없으면 크로스헤어 끄기
            m_Crosshair.gameObject.SetActive(false);
        }
    }
    
}
