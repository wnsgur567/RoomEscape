using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private bool m_Left;

    private Vector3 m_Axis;
    private float m_Angle;
    public enum Cover_State
    {
        Open = 0,
        Close,
    }
    public Cover_State M_State;
    void Start()
    {
        this.transform.tag = "None";
        M_State = Cover_State.Close;
        if (m_Left)
        {
            m_Axis = Vector3.down;
            m_Angle = 90f;
        }
        else
        {
            m_Axis = Vector3.down;
            m_Angle = -90f;
        }
    }
    public bool active = false;
    // Update is called once per frame
    void Update()
    {
        switch(M_State)
        {
            case Cover_State.Open:
                if (transform.localEulerAngles.x >= 350f)
                {
                    break;
                }
                else
                {
                    transform.Rotate(m_Axis, Time.deltaTime * 250f, Space.Self);
                }
                break;
            case Cover_State.Close:
                transform.localEulerAngles = new Vector3(m_Angle, 0f, 0f);
                break;
            default:
                break;
        }
    }

    
}
