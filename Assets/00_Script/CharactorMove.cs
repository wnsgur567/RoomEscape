using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CharactorMove : MonoBehaviourPunCallbacks
{
    private PhotonView m_PV;
    public float moveSpeed = 5.0f;
    public float rotSpeed = 0.2f;
    public Transform cameraArm;

    private float m_h = 0.0f;
    private float m_v = 0.0f;
    private Transform m_tr;
    // Start is called before the first frame update
    void Start()
    {
        m_tr = GetComponent<Transform>();
        m_PV = GetComponent<PhotonView>();
        if (!m_PV.IsMine)//IsMine 크리티컬 섹션같은거
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);//자기 자신이 아닐경우 다른 카메라는 파괴
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CharactorRotation();
    }
    private void Move()
    {
        m_h = Input.GetAxis("Horizontal");
        m_v = Input.GetAxis("Vertical");
        Vector3 moveDir = (Vector3.forward * m_v) + (Vector3.right * m_h);

        m_tr.Translate(moveDir * Time.deltaTime * moveSpeed, Space.Self);
        //tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("Mouse X"));
    }
    void CharactorRotation()
    {
        float rotX = Input.GetAxis("Mouse Y") * rotSpeed;
        float rotY = Input.GetAxis("Mouse X") * rotSpeed;

        this.transform.localRotation *= Quaternion.Euler(0, rotY, 0);
        cameraArm.localRotation *= Quaternion.Euler(-rotX, 0, 0);
    }
}
