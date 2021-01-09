using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


// object field  vs  property filed


[CustomEditor(typeof(_OrderManager))]
public class Editor_OrderManager : Editor
{
    private SerializedProperty _property_myScript;

    private SerializedProperty _property_awake_list;
    private SerializedProperty _property_onEnable_list;
    private SerializedProperty _property_start_list;
    private SerializedProperty _property_onDisable_list;

    private ReorderableList _awake_list;
    private ReorderableList _onEnable_list;
    private ReorderableList _start_list;
    private ReorderableList _onDisable_list;

    private void OnEnable()
    {
        _property_myScript = serializedObject.FindProperty("_OrderManager");

        // load
        _property_awake_list = serializedObject.FindProperty("m_awake_list");
        _property_onEnable_list = serializedObject.FindProperty("m_onEnable_list");
        _property_start_list = serializedObject.FindProperty("m_start_list");
        _property_onDisable_list = serializedObject.FindProperty("m_onDisable_list");

        

        // make list
        _awake_list = new ReorderableList(serializedObject, _property_awake_list, true, true, true, true)
        {
            drawHeaderCallback = DrawAwakeListHeader,
            drawElementCallback = DrawAwakeListElement
        };
        _onEnable_list = new ReorderableList(serializedObject, _property_onEnable_list, true, true, true, true)
        {
            drawHeaderCallback = DrawOnEnableListHeader,
            drawElementCallback = DrawOnEnableListElement,
        };
        _start_list = new ReorderableList(serializedObject, _property_start_list, true, true, true, true)
        {
            drawHeaderCallback = DrawStartListHeader,
            drawElementCallback = DrawStartListElement
        };
        _onDisable_list = new ReorderableList(serializedObject, _property_onDisable_list, true, true, true, true)
        {
            drawHeaderCallback = DrawOnDisableListHeader,
            drawElementCallback = DrawOnDisableListElement
        };
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();       

        serializedObject.Update();
        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((_OrderManager)target), typeof(_OrderManager), false);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();
        _awake_list.DoLayoutList();
        EditorGUILayout.Space();
        _onEnable_list.DoLayoutList();
        EditorGUILayout.Space();
        _start_list.DoLayoutList();
        EditorGUILayout.Space();
        _onDisable_list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    #region Header
    private void DrawAwakeListHeader(Rect rect)
    {
        GUI.Label(rect, "Awake");
    }
    private void DrawOnEnableListHeader(Rect rect)
    {
        GUI.Label(rect, "OnEnable");
    }
    private void DrawStartListHeader(Rect rect)
    {
        GUI.Label(rect, "Start");
    }
    private void DrawOnDisableListHeader(Rect rect)
    {
        GUI.Label(rect, "OnDisable");
    }
    #endregion

    #region Element
    private void DrawAwakeListElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        var item = _property_awake_list.GetArrayElementAtIndex(index);
        EditorGUI.PropertyField(rect, item);
    }
    private void DrawOnEnableListElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        var item = _property_onEnable_list.GetArrayElementAtIndex(index);
        EditorGUI.PropertyField(rect, item);
    }
    private void DrawStartListElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        var item = _property_start_list.GetArrayElementAtIndex(index);
        EditorGUI.PropertyField(rect, item);
    }
    private void DrawOnDisableListElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        var item = _property_onDisable_list.GetArrayElementAtIndex(index);
        EditorGUI.PropertyField(rect, item);
    }
    #endregion

    //#region Add
    //private void AddAwakeList(ReorderableList list)
    //{

    //}
    //private void AddOnEnableList(ReorderableList list)
    //{        
    //    ++list.serializedProperty.arraySize;
    //}
    //private void AddStartList(ReorderableList list)
    //{

    //}
    //private void AddOnDisableList(ReorderableList list)
    //{

    //}
    //#endregion

    //#region Remove
    //private void RemoveAwakeList(ReorderableList list)
    //{
       
    //}
    //private void RemoveOnEnableList(ReorderableList list)
    //{
    //    --_property_onEnable_list.arraySize;        
    //}
    //private void RemoveStartList(ReorderableList list)
    //{

    //}
    //private void RemoveOnDisalbeList(ReorderableList list)
    //{

    //}
    //#endregion

    //#region Reorder
    //private void ReorderAwakeList(ReorderableList list)
    //{

    //}
    //private void ReorderOnEnableList(ReorderableList list)
    //{

    //}
    //private void ReorderStartList(ReorderableList list)
    //{

    //}
    //private void ReorderOnDisableList(ReorderableList list)
    //{

    //}
    //#endregion
}
