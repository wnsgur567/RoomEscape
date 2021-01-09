using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _OrderManager : MonoBehaviour, IAwake, IOnEnable, IStart, IOnDisable
{
    [SerializeField]
    private List<MonoBehaviour> m_awake_list = null;
    private List<IAwake> m_awakes = null;

    [SerializeField]
    private List<MonoBehaviour> m_onEnable_list = null;
    private List<IOnEnable> m_onEnables = null;

    [SerializeField]
    private List<MonoBehaviour> m_start_list = null;
    private List<IStart> m_starts = null;

    [SerializeField]
    private List<MonoBehaviour> m_onDisable_list = null;
    private List<IOnDisable> m_onDisables = null;

    public void __Awake()
    {
        m_awakes = new List<IAwake>();
        m_onEnables = new List<IOnEnable>();
        m_starts = new List<IStart>();
        m_onDisables = new List<IOnDisable>();

        foreach (var item in m_awake_list)
        {
            if (item == null)
                continue;

            var awake = item as IAwake;
            if (awake != null)
                m_awakes.Add(awake);
            else
                Debug.LogErrorFormat("_OrderManager : IAake미포함 {0}", item.name);
        }
        foreach (var item in m_onEnable_list)
        {
            if (item == null)
                continue;

            var onEnable = item as IOnEnable;
            if (onEnable != null)
                m_onEnables.Add(onEnable);
            else
                Debug.LogErrorFormat("_OrderManager : IOnEnable미포함 {0}", item.name);
        }
        foreach (var item in m_start_list)
        {
            if (item == null)
                continue;

            var start = item as IStart;
            if (start != null)
                m_starts.Add(start);
            else
                Debug.LogErrorFormat("_OrderManager : IStart 미포함 {0}", item.name);
        }
        foreach (var item in m_onDisable_list)
        {
            if (item == null)
                continue;

            var onDisable = item as IOnDisable;
            if (onDisable != null)
                m_onDisables.Add(onDisable);
            else
                Debug.LogErrorFormat("_OrderManager : IOnDisable 미포함 {0}", item.name);
        }

        Call_Awakes();
    }

    public void __OnEnable()
    {
        Call_OnEnables();
    }
    public void __Start()
    {
        Call_Start();
    }
    public void __OnDisable()
    {
        Call_OnDisables();
    }
    private void Call_Awakes()
    {
        foreach (var item in m_awakes)
        {
            item.__Awake();
        }
    }
    private void Call_OnEnables()
    {
        foreach (var item in m_onEnables)
        {
            item.__OnEnable();
        }
    }
    private void Call_Start()
    {
        foreach (var item in m_starts)
        {
            item.__Start();
        }
    }
    private void Call_OnDisables()
    {
        foreach (var item in m_onDisables)
        {
            item.__OnDisable();
        }
    }

}






