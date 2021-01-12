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

        Spawn();
    }
    public void Complete_PipePuzzle()
    {
        //���������� �Ϸ��
        m_ACover.tag = "Cover";
        Debug.Log("������ Ŭ����!");
    }
    public void Complete_PaintPuzzle()
    {
        //����Ʈ���� �Ϸ��
        m_DCover.tag = "Cover";
        Debug.Log("����Ʈ Ŭ����!");
    }


    public void Spawn()//��ư�� �Ҵ��� Position���� prefab�ν��Ͻ� ����
    {
        PhotonNetwork.Instantiate("Charactor", SpawnPosition.position, Quaternion.identity);
    }
}
