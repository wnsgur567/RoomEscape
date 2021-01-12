﻿using System.Collections;
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

    public float M_puzzleTimer = 0;

    [NonSerialized]
    public int m_minute;

    public int m_second = 0;
    public float M_currentSeconds;

    private bool m_penalty;
    private int m_timerdefault;
    private void Awake()
    {
        M_clock = this;
    }
    public void Clock_Initialize()
    {
        M_IsStop = false;
        m_penalty = false;
        m_timerdefault = 0;
        m_timerdefault += (m_second + (m_minute * 60));
        M_currentSeconds = m_timerdefault;
    }
    void Update()
    {

        if (M_IsStop)
            return;

        if (M_puzzleTimer > M_currentSeconds)
        {
            if (!m_penalty)
            {
                m_penalty = true;
                //시야를 가린다

                return;
            }
            if (m_penalty)
            {
                return;
            }
            Debug.Log("2분경과");
        }
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
