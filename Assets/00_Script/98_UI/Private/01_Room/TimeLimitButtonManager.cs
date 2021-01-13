using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLimitButtonManager : MonoBehaviour
{
    _NetworkInfoManager m_infoManager = null;
    [SerializeField] Button[] DeadLineButtons;
    [SerializeField] Color _participant_selectedColor;
    ColorBlock m_defaultColor;
    ColorBlock m_defaultChangeColor;

    private void Awake()
    {
        m_infoManager = _NetworkInfoManager.Instance;

        m_defaultColor = DeadLineButtons[0].colors;
        m_defaultColor = DeadLineButtons[0].colors;
        ColorBlock _block = m_defaultColor;
        _block.normalColor = _participant_selectedColor;
        m_defaultChangeColor = _block;

        ClearButtonsColor();
    }

    public void ClearButtonsColor()
    {
        foreach (var item in DeadLineButtons)
        {
            item.colors = m_defaultColor;
        }
    }
    public void _OnChangeDeadLine()
    {
        switch (m_infoManager.m_playerInfo.deadLine_time)
        {
            case 20:
                ClearButtonsColor();
                DeadLineButtons[0].colors = m_defaultChangeColor;
                break;
            case 25:
                ClearButtonsColor();
                DeadLineButtons[1].colors = m_defaultChangeColor;
                break;
            case 30:
                ClearButtonsColor();
                DeadLineButtons[2].colors = m_defaultChangeColor;
                break;
        }
    }
}
