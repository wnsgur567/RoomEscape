using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Cover_State
    {
        Open = 0,
        Close,
        
    }
    public Cover_State M_State;
    void Start()
    {
        M_State = Cover_State.Close;
        Debug.Log(transform.localEulerAngles);
    }
    //���� 210 ���� 90

    //������ 30 ���� -90

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
                    transform.Rotate(Vector3.down, Time.deltaTime * 250f, Space.Self);
                }
                break;
            case Cover_State.Close:
                transform.localEulerAngles = new Vector3(90f, 0f, 0f);
                //transform.Rotate(Vector3.down, Time.deltaTime * 150f, Space.World);
                break;
            default:
                break;
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("Ű���崩��");
            Debug.Log(transform.localEulerAngles.x);
        }
    }

    
}
