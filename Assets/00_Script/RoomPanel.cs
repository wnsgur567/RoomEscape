using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomPanel : MonoBehaviour
{
    public TextMeshProUGUI m_text;
    private void Awake()
    {
        m_text.text = "";
    }

    public void __Initialize(string p_roomName, int p_participantCount)
    {   // 현재 방 최대인원은 2명
        string _label = string.Format("{0} ({1}/{2})", p_roomName, p_participantCount, 2);
        m_text.text = _label;
    }
}
