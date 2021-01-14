using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CharactorMove : MonoBehaviourPunCallbacks
{

    public static CharactorMove Instance = null;
    private PhotonView m_PV;
    public float moveSpeed = 5.0f;
    public float rotSpeed = 0.2f;
    public Transform cameraArm;

    private float m_h = 0.0f;
    private float m_v = 0.0f;
    private Transform m_tr;

    public bool M_Input;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        m_tr = GetComponent<Transform>();
        m_PV = GetComponent<PhotonView>();
        if (!m_PV.IsMine)//IsMine ũ��Ƽ�� ���ǰ�����
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);//�ڱ� �ڽ��� �ƴҰ�� �ٸ� ī�޶�� �ı�
        }
        M_Input = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (M_Input)
        {
            CharactorRotation();
        }
    }
    private void FixedUpdate()
    {
        if (M_Input)
        {
            Move();
        }
    }
    private void Move()
    {
        m_h = Input.GetAxis("Horizontal");
        m_v = Input.GetAxis("Vertical");
        Vector3 moveDir = (Vector3.forward * m_v) + (Vector3.right * m_h);

        _SoundManager.Instance.PlayObjInterationSound(E_ObjectInterationSound.user_walk);
        m_tr.Translate(moveDir * Time.deltaTime * moveSpeed, Space.Self);

    }
    void CharactorRotation()
    {
        float rotX = Input.GetAxis("Mouse X") * rotSpeed;
        float rotY = Input.GetAxis("Mouse Y") * rotSpeed;

        float tempcamY = Mathf.LerpAngle(0, -rotY, 10f);

        // -180~180��
        float angle = cameraArm.transform.eulerAngles.x > 180 ? cameraArm.transform.eulerAngles.x - 360 : cameraArm.transform.eulerAngles.x;

        if (angle + tempcamY >= 50)//�ϴ�, �������� �Ʒ��� �ѹ���
            angle = 50;
        else if (angle + tempcamY <= -60)  //��� �������� ����
            angle = -60;
        else
            angle += tempcamY;

        cameraArm.transform.eulerAngles = new Vector3(angle, cameraArm.transform.eulerAngles.y, 0);
        this.transform.localRotation *= Quaternion.Euler(0, rotX, 0);
    }
}
