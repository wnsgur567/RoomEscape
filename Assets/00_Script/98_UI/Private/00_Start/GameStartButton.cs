using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStartButton : MonoBehaviour
{
    public TMP_InputField m_inputField = null;
    _NetworkInfoManager infoManager = null;
    private void Awake()
    {
        infoManager = _NetworkInfoManager.Instance;
    }
    public void SetNickName()
    {
        infoManager.SetNickName(m_inputField.text);
    }
}
