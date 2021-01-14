using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject M_cutRed;

    public GameObject M_cutBlue;

    public GameObject M_cutBlack;

    public GameObject M_cutWhite;
    public GameObject M_cutYellow;

    public GameObject M_cutGreen;

    public static BombScript M_instance;

    [SerializeField]
    private GameObject m_SuccessMsg;

    [SerializeField]
    private GameObject m_FailedMsg;

    public bool A_Clear = false;
    public bool B_Clear = false;
    public bool C_Clear = false;
    public bool D_Clear = false;
    public bool M_totalClear = false;
    public string M_PassWord;
    private void Awake()
    {
        M_instance = this;
        M_PassWord = "REMEMBER1017";
    }
    public void SuccessMsg()
    {
        m_SuccessMsg.SetActive(true);
        Invoke("InvkeSuccessMsg", 1f);
    }
    void InvkeSuccessMsg()
    {
        m_SuccessMsg.SetActive(false);
    }

    public void FailedMsg()
    {
        m_FailedMsg.SetActive(true);
        Invoke("InvkeFaildMsg", 1f);
    }
    void InvkeFaildMsg()
    {
        m_FailedMsg.SetActive(false);
    }
}
