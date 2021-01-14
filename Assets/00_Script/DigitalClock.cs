using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class DigitalClock :  MonoBehaviour
{
    public static DigitalClock M_clock;
    public bool M_IsStop;

    [SerializeField]
    private TextMeshPro m_CountdownText;    //텍스트 출력

    public float M_puzzleTimer = 0;
    

    public int m_minute;
    public int m_second;
    public float M_currentSeconds;          //현재 시간

    private bool m_penalty;
    private int m_timerdefault;

    [SerializeField]
    private Image m_PenaltyImage;

    private float m_PenaltyTime = 3.5f;
    private float m_time = 0f;
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
            if (PipePuzzleManager.M_pipeManager.M_IsClearPuzzle == true)
                return;
            if (!m_penalty)
            {
                m_penalty = true;
                //시야를 가린다

                Debug.Log("Activate PipePenalty");
                return;
            }
            if (m_penalty)
            {
                FadeIn();
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

    void FadeIn()
    {
        Color color = m_PenaltyImage.color;
        if (color.a < 1)
        {
            color.a += Time.deltaTime * 0.2f;
        }

        m_PenaltyImage.color = color;
    }
}
