using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NickName_InputField : MonoBehaviour
{
    TMPro.TMP_InputField m_inputfield = null;
    [SerializeField] List<Button> m_button = null;

    private void Awake()
    {
        m_inputfield = GetComponent<TMPro.TMP_InputField>();        
    }

    // text 길이가 0이면 false
    public bool CheckTextLength()
    {
        if(m_inputfield.text.Length == 0)
            return false;
        return true;
    }

    public void __OnActivateButtons()
    {
        if(CheckTextLength())
        {
            foreach (var item in m_button)
            {
                item.interactable = true;
            }
        }
    }
}
