using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager M_gameManager = null;
    public GameObject m_Pipe_puzzle;
    public GameObject m_Paint_puzzle;
    public Transform SpawnPosition;

    [SerializeField]
    private GameObject m_ACover;
    [SerializeField]
    private GameObject m_BCover;
    [SerializeField]
    private GameObject m_CCover;
    [SerializeField]
    private GameObject m_DCover;
    // Start is called before the first frame update
    private void Awake()
    {
        M_gameManager = this;
    }
    private void Start()
    {
        DigitalClock.M_clock.Clock_Initialize();

        Spawn();
    }
    public void Complete_PipePuzzle()
    {
        //파이프퍼즐 완료시
        //폭탄뚜껑 열어야함
        m_ACover.tag = "Cover";
        m_Pipe_puzzle.SetActive(false);
        Debug.Log("파이프 클리어!");
    }


    public void Spawn()//버튼에 할당한 Position으로 prefab인스턴스 생성
    {
        PhotonNetwork.Instantiate("Charactor", SpawnPosition.position, Quaternion.identity);
    }
}
