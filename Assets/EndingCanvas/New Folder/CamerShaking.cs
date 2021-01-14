using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// main 카메라에 넣기
public class CamerShaking : MonoBehaviour
{    
    Vector3 m_StdPos;

    [SerializeField] float m_shakeAmount;
    [SerializeField,Range(1f,10f)] float m_shakeTime;
    [SerializeField, Range(1f, 10f)] float m_delay;

    private void Start()
    {
        m_StdPos = this.transform.position;
    }

    void Update()
    {
        if(m_delay > 0)
        {
            m_delay -= Time.deltaTime;
            return;
        }

        if (m_shakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * m_shakeAmount + m_StdPos;
            m_shakeTime -= Time.deltaTime;
        }
        else
        {
            transform.position = m_StdPos;
        }
    }
}
