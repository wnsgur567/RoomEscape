using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager M_gameManager = null;

    public Transform A_SpawnPosition;
    public Transform B_SpawnPosition;
    public PhotonView M_PV;
    [SerializeField]
    private GameObject m_ACover;
    [SerializeField]
    private GameObject m_BCover;
    [SerializeField]
    private GameObject m_CCover;
    [SerializeField]
    private GameObject m_DCover;


    
    _NetworkInfoManager m_infoManager = null;
    // Start is called before the first frame update
    private void Awake()
    {
        m_infoManager = _NetworkInfoManager.Instance;
        M_gameManager = this;
    }
    private void Start()
    {
        DigitalClock.M_clock.m_minute = m_infoManager.m_playerInfo.deadLine_time;
        DigitalClock.M_clock.Clock_Initialize();

        Spawn(m_infoManager);
    }
    public void Complete_PipePuzzle()
    {
        //파이프퍼즐 완료시
        m_ACover.tag = "Cover";
        Debug.Log("파이프 클리어!");
    }
    [PunRPC]
    public void Complete_PaintPuzzle()
    {
        //페인트퍼즐 완료시
        m_DCover.tag = "Cover";
        Debug.Log("페인트 클리어!");
    }
    [PunRPC]
    public void StartRadio()
    {
        m_CCover.tag = "Cover";
        Debug.Log("라디오퍼즐 시작");
    }
    [PunRPC]
    public void Complete_ChessPuzzle()
    {
        m_BCover.tag = "Cover";
        Debug.Log("체스퍼즐 클리어");
    }


    public void Spawn(_NetworkInfoManager Infomanager)//버튼에 할당한 Position으로 prefab인스턴스 생성
    {
        if(Infomanager.m_playerInfo.type == E_RoomType.A)
        {
            PhotonNetwork.Instantiate("Charactor", A_SpawnPosition.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("Charactor", B_SpawnPosition.position, Quaternion.identity);
        }
    }
    public void DefuseSuccess()
    {

    }
    public void DefuseFailed()
    {

    }
}
