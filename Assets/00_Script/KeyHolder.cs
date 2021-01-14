using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHolder : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Holder_State
    {
        None = 0,
        Activated,
    }
    public Holder_State M_State;
    private SphereCollider m_col;
    [SerializeField]
    Transform m_key;
    private bool m_endflag = false;
    void Start()
    {
        m_col = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_endflag == true)
            return;

        switch (M_State)
        {
            case Holder_State.Activated:
                if (m_key.localEulerAngles.z >= 180f)
                {
                    m_col.enabled = false;
                    m_endflag = true;
                    M_State = Holder_State.None;
                    BombScript.M_instance.SuccessMsg();
                    
                    break;
                }
                else
                {
                    m_key.Rotate(Vector3.forward, Time.deltaTime * 250f, Space.Self);
                    Debug.Log(m_key.localEulerAngles);
                }
                break;
            case Holder_State.None:
                //transform.localEulerAngles = new Vector3(m_Angle, 0f, 0f);
                break;
            default:
                break;
        }
    }
}
