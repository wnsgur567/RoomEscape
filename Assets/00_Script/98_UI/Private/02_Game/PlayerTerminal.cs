using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTerminal : MonoBehaviour
{
    private _NetworkChatManager m_chatManager = null;
    [SerializeField, ShowOnly] public bool isAvailable;    // 단말기 이용 가능한 상태인지(활성화 상태인지)    
    [Tooltip("터미널이 활성화 됬을때 위치"), SerializeField] Vector3 m_terminal_pos;
    [Tooltip("터미널이 비활성화 됬을때 위치"), SerializeField] Vector3 m_terminal_hidePos;
    public TMP_InputField m_focusObject;

    private void Awake()
    {
        m_chatManager = _NetworkChatManager.Instance;
    }

    void Update()
    {
        KeyInput();
        
        //MoveTerminal();
    }

    private void KeyInput()
    {        
        if (m_chatManager.isGameStart && Input.GetKeyDown(KeyCode.Tab))
        {
            if (isAvailable)
            {
                SetAvailable(false);
            }                
            else
            {
                SetAvailable(true);
            }                
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            // setfocus
            m_focusObject.ActivateInputField();
            m_focusObject.Select();

            m_chatManager.OnInputText();
        }
    }
    
    // 활성화 비활성화시 관련 함수 호출
    public void SetAvailable(bool _b)
    {
        isAvailable = _b;
        MoveTerminal();
        SetFocusObj();
    }

    // 터미널 이동 애니메이션
    private void MoveTerminal()
    {
        if (isAvailable)
        {
            transform.DOLocalMove(m_terminal_pos, 1f);
        }
        else
        {
            transform.DOLocalMove(m_terminal_hidePos, 1f);
        }
    }

    private void SetFocusObj()
    {
        if(isAvailable)
        {
            m_focusObject.ActivateInputField();
            m_focusObject.Select();
        }
        else
        {
            m_focusObject.DeactivateInputField();
        }
        
    }
}
