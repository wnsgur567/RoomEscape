using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundCS : MonoBehaviour
{
    _SoundManager m_soundManager = null;

    [SerializeField] E_SoundType m_soundType;
    [SerializeField] E_BGMSound m_bgm;
    [SerializeField] E_UISound m_ui;
    [SerializeField] E_ObjectSound m_object;
    [SerializeField] E_ObjectInterationSound m_interation;

    virtual protected void Awake()
    {
        m_soundManager = _SoundManager.Instance;        
    }

    public void _Play()
    {
        switch (m_soundType)
        {            
            case E_SoundType.BGM:
                m_soundManager.PlayBGMSound(m_bgm);
                break;
            case E_SoundType.UI:
                m_soundManager.PlayUISound(m_ui);
                break;
            case E_SoundType.Object:
                // ...
                break;
            case E_SoundType.ObjectInteraction:
                m_soundManager.PlayObjInterationSound(m_interation);
                break;          
        }
    }
}
