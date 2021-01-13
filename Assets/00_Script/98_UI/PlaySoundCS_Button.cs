using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySoundCS_Button : PlaySoundCS
{
    private Button m_button;
    private void Awake()
    {
        base.Awake();
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(_Play);
    }
}
