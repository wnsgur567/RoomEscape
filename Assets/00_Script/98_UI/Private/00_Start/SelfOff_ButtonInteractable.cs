using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfOff_ButtonInteractable : MonoBehaviour
{
    Button m_button = null;
    private void Awake()
    {
        m_button = GetComponent<Button>();
        m_button.interactable = false;
    }
}
