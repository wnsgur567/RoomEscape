using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class DigitalClock : MonoBehaviour
{
    public static DigitalClock M_clock;
    public bool M_IsStop;
    [SerializeField]
    private TextMeshPro m_CountdownText;

    public int m_minute;
    public int m_second;
    private int m_timerdefault;
    public float M_currentSeconds;
    private void Awake()
    {
        M_clock = this;
    }
    public void Clock_Initialize()
    {
        M_IsStop = false;

        m_timerdefault = 0;
        m_timerdefault += (m_second + (m_minute * 60));
        M_currentSeconds = m_timerdefault;
    }
    void Update()
    {

        if (M_IsStop)
            return;
        if (M_currentSeconds > 0)
        {
            M_currentSeconds -= Time.deltaTime;
            m_CountdownText.text = TimeSpan.FromSeconds(M_currentSeconds).ToString(@"mm\:ss");
        }
        else if (M_currentSeconds <= 0)
        {
            m_CountdownText.text = "00:00";
        }
    }
}
