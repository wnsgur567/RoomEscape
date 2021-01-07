using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSliderFunctions : MonoBehaviour
{
    _OptionInfoManager _infoManager = null;
    Slider m_slider = null;    

    private void Awake()
    {
        _infoManager = _OptionInfoManager.Instance;
        m_slider = GetComponent<Slider>();    
    }    

    public void __OnChange_MouseSensitive()
    {
        _infoManager.ChangeMouseSensitive(m_slider.value);
    }

    public void __OnChange_Volume()
    {
        _infoManager.ChangeVolunme(m_slider.value);
    }
    public void __OnChange_Volume_BGM()
    {
        _infoManager.ChangeBgmVolume(m_slider.value);
    }
    public void __OnChange_Volume_Effect()
    {
        _infoManager.ChangeEffectVolume(m_slider.value);
    }
}
