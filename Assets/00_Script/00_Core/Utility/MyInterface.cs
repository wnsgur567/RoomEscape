using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Awake(only) -> OnEnable -> Start(only) -> 
// Fixed_Update -> Update -> Late_Update
// Rendering
// OnDisable 
// OnDestroy


public interface IAwake
{
    void __Awake();
}
public interface IStart
{
    void __Start();
}
public interface IOnEnable
{
    void __OnEnable();
}

public interface IUpdate
{
    void __Update();
}
public interface ILateUpdate
{
    void __LateUpdate();
}
public interface IOnDisable
{
    void __OnDisable();
}

public interface IJsonLoad
{
    void __Initialize();        // 자료 메모리 할당
    void __LoadData();          // JsonManager를 통해 Load
    void __Finalize();          // 후처리
}

public interface IResourceLoad
{
    void __Initialize();
    void __LoadData();
    void __Finalize();
}
