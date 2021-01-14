using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public enum E_EndingType
{
    None = 0,

    Clear = 1,
    Fail = 2,

    Max
}
public class FadeInOut : MonoBehaviour
{
    [SerializeField] E_EndingType m_type;
    [SerializeField] TextMeshProUGUI m_text;
    Image m_panel_image;

    private void Awake()
    {
        m_panel_image = GetComponent<Image>();
    }
    private void Start()
    {
        switch (m_type)
        {
            case E_EndingType.Clear:
                {
                    Color _color = Color.white;
                    _color.a = 0f;
                    m_panel_image.color = _color;

                    m_text.text = "GAME CLEAR";
                    m_text.color = Color.black;
                    StartFadeIn();
                }                
                break;
            case E_EndingType.Fail:
                {
                    Color _color = Color.black;
                    _color.a = 0f;
                    m_panel_image.color = _color;                    

                    m_text.text = "GAME OVER";
                    m_text.color = Color.red;
                    StartFadeIn();
                }
                
                break;
        }
    }

    [ContextMenu("FadeIn")]
    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    [ContextMenu("FadeOut")]
    public void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    [SerializeField, Range(1f, 10f)] float m_fadeInOut_time;  // ms    
    IEnumerator FadeIn()
    {
        while (true)
        {
            var _color = m_panel_image.color;
            _color.a += (Time.deltaTime * m_fadeInOut_time * 0.1f);
            Debug.Log(_color.a);
            if (_color.a > 1f)
            {
                _color.a = 1f;
                m_panel_image.color = _color;
                break;
            }

            m_panel_image.color = _color;

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator FadeOut()
    {
        while (true)
        {
            var _color = m_panel_image.color;
            _color.a -= m_fadeInOut_time * 0.01f;
            
            if (_color.a < 0f)
            {
                _color.a = 0f;
                m_panel_image.color = _color;
                break;
            }
            m_panel_image.color = _color;

            yield return new WaitForSeconds(0.1f);
        }
    }


}
