using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTerminal : MonoBehaviour
{
    private _NetworkChatManager m_chatManager = null;
    [SerializeField, ShowOnly] public bool isAvailable;    // �ܸ��� �̿� ������ ��������(Ȱ��ȭ ��������)    
    [Tooltip("�͹̳��� Ȱ��ȭ ������ ��ġ"), SerializeField] Vector3 m_terminal_pos;
    [Tooltip("�͹̳��� ��Ȱ��ȭ ������ ��ġ"), SerializeField] Vector3 m_terminal_hidePos;
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
    
    // Ȱ��ȭ ��Ȱ��ȭ�� ���� �Լ� ȣ��
    public void SetAvailable(bool _b)
    {
        isAvailable = _b;
        MoveTerminal();
        SetFocusObj();
    }

    // �͹̳� �̵� �ִϸ��̼�
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
