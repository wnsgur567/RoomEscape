using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(_OrderManager))]
public class _OrderManagerCaller : MonoBehaviour
{
    _OrderManager p_manager;

    private void Awake()
    {
        p_manager = GetComponent<_OrderManager>();
        p_manager.__Awake();
    }
    private void OnEnable()
    {
        p_manager.__OnEnable();
    }
    private void Start()
    {
        p_manager.__Start();
    }
    private void OnDisable()
    {
        p_manager.__OnDisable();
    }

}
