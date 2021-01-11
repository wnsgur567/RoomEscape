using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public struct MouseEventArgs
{
    public bool isPointer_overUIObj;
    public List<RaycastResult> UIObjs;

    public bool l_button;
    public bool r_button;
    public float l_hold_count;
    public float r_hold_count;
    public Vector3 mouse_position;      // z = 0
    public Vector3 mouse_delta;         // z = 0

    public void Clear()
    {
        isPointer_overUIObj = false;
        l_button = false;
        r_button = false;
        l_hold_count = 0f;
        r_hold_count = 0f;
        mouse_position = Vector3.zero;
        mouse_delta = Vector2.zero;
    }
}

// 이건 할 필요가 없네
public struct KeyEventArgs
{
    public KeyCode current;
    public bool isCurrentPushed;    // current 키가 현제 눌려있는 상태인지

    public void Clear()
    {
        current = KeyCode.None;
    }
}


public class _InputManager : Singleton<_InputManager>, IAwake
{
    public void __Awake()
    {
        base.Awake();

        __Initialize_MouseInput();
        __Initialize_KeyInput();
    }
    

    public void Update()
    {
        __Update_MouseInput();
        __Update_KeyInput();
    }


    #region MouseInput  Variable
    MouseEventArgs m_args;

    public MyEvent<MouseEventArgs> mouseMove_Event;
    public MyEvent<MouseEventArgs> mouseLButtonDown_Event;
    public MyEvent<MouseEventArgs> mouseLButtonUp_Event;
    public MyEvent<MouseEventArgs> mouseRButtonDown_Event;
    public MyEvent<MouseEventArgs> mouseRButtonUp_Event;

    public float m_hold_delay;
    public MyEvent<MouseEventArgs> mouseLHold_Event;
    public MyEvent<MouseEventArgs> mouseRHold_Event;

    // delta 계산용 멤버 변수
    private Vector3 m_beforePosition;
    private Vector3 m_currentPosition;

    private float m_ZeroThreshold = 0.02f;      // 마우스 움직임 감지 정도
    #endregion
    #region MouseInput Functions
    public void __Initialize_MouseInput()
    {
        // 델타값 계산용 mousePosition 초기화
        m_beforePosition = new Vector3();
        m_currentPosition = new Vector3();

        // Mouse Event 데이터 저장 변수
        m_args = new MouseEventArgs();

        // Mouse Event 생성
        mouseMove_Event = new MyEvent<MouseEventArgs>();
        mouseLButtonDown_Event = new MyEvent<MouseEventArgs>();
        mouseLButtonUp_Event = new MyEvent<MouseEventArgs>();
        mouseRButtonDown_Event = new MyEvent<MouseEventArgs>();
        mouseRButtonUp_Event = new MyEvent<MouseEventArgs>();
        mouseLHold_Event = new MyEvent<MouseEventArgs>();
        mouseRHold_Event = new MyEvent<MouseEventArgs>();
    }

    private void __Update_MouseInput()
    {
        // UI 
        m_args.UIObjs = GetUIObjectsUnderPointer();
        m_args.isPointer_overUIObj = (m_args.UIObjs.Count > 0);

        // position
        m_currentPosition = Input.mousePosition;
        m_args.mouse_position = m_currentPosition;

        // delta
        m_args.mouse_delta = GetMouseDelta();

        // Move
        if (IsMouseMove())
            mouseMove_Event.Invoke(null, m_args);

        // Left Button
        if (Input.GetMouseButtonDown(0))
        {
            m_args.l_button = true;
            mouseLButtonDown_Event.Invoke(null, m_args);
        }
        if (Input.GetMouseButton(0))
        {
            m_args.l_hold_count += Time.deltaTime;
            if (m_args.l_hold_count > m_hold_delay)
                mouseLHold_Event.Invoke(null, m_args);
        }
        if (Input.GetMouseButtonUp(0))
        {
            m_args.l_button = false;
            m_args.l_hold_count = 0;
            mouseLButtonUp_Event.Invoke(null, m_args);
        }

        // Right Button
        if (Input.GetMouseButtonDown(1))
        {
            m_args.r_button = true;
            mouseRButtonDown_Event.Invoke(null, m_args);
        }
        if (Input.GetMouseButton(1))
        {
            m_args.r_hold_count += Time.deltaTime;
            if (m_args.r_hold_count > m_hold_delay)
                mouseRHold_Event.Invoke(null, m_args);
        }
        if (Input.GetMouseButtonUp(1))
        {
            m_args.r_button = false;
            m_args.r_hold_count = 0;
            mouseRButtonUp_Event.Invoke(null, m_args);
        }
    }

    private bool IsMouseMove()
    {
        if (Vector3.Distance(m_args.mouse_delta, Vector3.zero) < m_ZeroThreshold)
            //if(m_args.mouse_delta == Vector3.zero)    // 이걸로 해도 됨
            return false;
        return true;
    }

    private Vector3 GetMouseDelta()
    {
        Vector3 ret = new Vector2();
        ret = m_currentPosition - m_beforePosition;
        m_beforePosition = m_currentPosition;
        return ret;
    }

    private bool IsPointerOverUIObject()
    {
        List<RaycastResult> results = GetUIObjectsUnderPointer();
        return results.Count > 0;
    }

    private List<RaycastResult> GetUIObjectsUnderPointer()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results;
    }
    #endregion


    #region KeyInput Variable

    #endregion
    #region KeyInput Fuctions
    private void __Initialize_KeyInput()
    {

    }
    private void __Update_KeyInput()
    {

    }


    #endregion
}
