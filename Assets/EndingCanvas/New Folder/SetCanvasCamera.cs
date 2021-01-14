using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SetCanvasCamera : MonoBehaviour
{
    Canvas m_canvas;
    

    private void Awake()
    {
        SetCam();
        //this.gameObject.SetActive(false);
    }

    public void SetCam()
    {
        m_canvas = GetComponent<Canvas>();
        m_canvas.worldCamera = Camera.main;
    }


}
