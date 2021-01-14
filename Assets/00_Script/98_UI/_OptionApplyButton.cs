using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _OptionApplyButton : MonoBehaviour
{
    _SoundManager m_soundManager = null;
    private void Awake()
    {
        m_soundManager = _SoundManager.Instance;
    }

    public void __Apply()
    {
        m_soundManager._ApplyOptions();
    }
}
