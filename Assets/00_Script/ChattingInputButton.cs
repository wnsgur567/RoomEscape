using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ChattingInputButton : MonoBehaviour
{
    public TMP_InputField m_focusObject;  
    private Button m_button = null;

    private void Awake()
    {
        m_button = GetComponent<Button>();        
    }

    private void Update()
    {
        CheckEnterKey();
    }

    public void CheckEnterKey()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            // setfocus
            m_focusObject.ActivateInputField();
            m_focusObject.Select();
            
            // event invoke
            m_button.onClick.Invoke();
        }
    }    
}
