using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionDropDownFunctions : MonoBehaviour
{
    _OptionInfoManager _infoManager = null;
    Dropdown m_dropdown = null;

    private void Awake()
    {
        _infoManager = _OptionInfoManager.Instance;
        m_dropdown = GetComponent<Dropdown>();
    }

    public void __OnChange_Resolution()
    {
        // 선택한 해상도 ( ex / 1920 x 1080)
        string _resolution = m_dropdown.options[m_dropdown.value].text;

        // parsing
        string[] _retStr = _resolution.Split('x');
        int _width = int.Parse(_retStr[0]);
        int _height = int.Parse(_retStr[1]);

        Debug.Log(_height);
        _infoManager.ChangeResolution(_width, _height);
    }

}
