using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HitObjectInformation
{
    public string name;
    public float distance;
    public Collider hitCollider;

    public HitObjectInformation(string _name, float _distance, Collider _hitCollider)
    {
        name = _name;
        distance = _distance;
        hitCollider = _hitCollider;
    }
}


public class MyRaycaster : MonoBehaviour, IAwake 
{
    _InputManager _inputManager = null;

    public new Camera camera = null;
    [Tooltip("hitObject가 2개 이상인 경우 활성화")]
    public bool isMulti;
    [Tooltip("ray에 충돌하는 레이어 이름")]
    public List<string> layerNames;
    [Tooltip("ray 최대 거리")]
    public float maxDistance;
    
    [Header("Hit 오브젝트 (단일)"),ShowOnly]
    public GameObject hitObject;    
    [Header("Hit 오브젝트 (다중)"),ShowOnly]
    public List<HitObjectInformation> hitObjects;

    // Ray를 적용시킬 레이어마스크 집합 변수
    private int cullingLayer;    

    public void __Awake()
    {
        _inputManager = _InputManager.Instance;

        if (camera == null)
            camera = Camera.main;

        // event link
        _inputManager.mouseMove_Event.AddHandler("MyRaycaster", __OnMouseMove);
        _inputManager.mouseLButtonDown_Event.AddHandler("MyRaycaster", __OnLButtonDown);

        // ray 적용시킬 레이어 값 가져오기
        cullingLayer = CullingLayer();
    }
   
    private void Update()
    {        
        // 디버깅용
        ShowRay(Input.mousePosition);
    }

    private RaycastHit hit;
    private void UpdateHitObject(Vector3 p_mousePos)
    {
        UnshowReceiver(hitObject);
        hitObject = null;
        Ray _ray = MakeRay(p_mousePos);

        if (cullingLayer == 0)
        {
            if (Physics.Raycast(_ray, out hit, maxDistance))
            {
                hitObject = hit.transform.gameObject;
                ShowReceiver(hitObject);
            }
        }
        else
        {
            if (Physics.Raycast(_ray, out hit, maxDistance, cullingLayer))
            {
                hitObject = hit.transform.gameObject;
                ShowReceiver(hitObject);
            }
        }
    }

    private void UpdateHitObjects(Vector3 p_mousePos)
    {
        Ray _ray = MakeRay(p_mousePos);

        RaycastHit[] hits;

        if (cullingLayer == 0)
        {
            hits = Physics.RaycastAll(_ray, maxDistance);
        }
        else
        {
            hits = Physics.RaycastAll(_ray, maxDistance, cullingLayer);
        }

        foreach (var item in hitObjects)
        {
            UnshowReceiver(item.hitCollider.gameObject);
        }
        hitObjects.Clear();
        foreach (var item in hits)
        {
            // 거리의 순서와 상관없이 저장됨
            hitObjects.Add(new HitObjectInformation(item.transform.name, item.distance, item.collider));
            ShowReceiver(item.collider.gameObject);            
            //Debug.LogFormat("{0} : {1}", item.transform.name, item.distance);
        }
    }

    private Ray MakeRay(Vector3 p_mousePos, float rayLength = -1f)
    {
        float near = camera.nearClipPlane;
        float far = camera.farClipPlane;

        if (rayLength < 0f || rayLength > far)
            rayLength = far;

        Ray _ray = camera.ScreenPointToRay(p_mousePos);
        //Debug.DrawRay(_ray.origin, _ray.GetPoint(rayLenth),Color.red);
        return _ray;
    }

    private void ShowRay(Vector3 p_mousePos,float rayLength = -1f)
    {
        float near = camera.nearClipPlane;
        float far = camera.farClipPlane;

        if (rayLength < 0f || rayLength > far)
            rayLength = far;

        Ray _ray = camera.ScreenPointToRay(p_mousePos);
        Debug.DrawRay(_ray.origin, _ray.GetPoint(rayLength), Color.red);
    }

    // layerNames에서 검사할 레이어 추출
    private int CullingLayer()
    {
        int ret = 0;

        foreach (var item in layerNames)
        {
            ret = ret | LayerMask.GetMask(item);
        }

        Debug.LogFormat($"LayerCulling : {ret}");
        return ret;
    }
    
    private void ShowReceiver(GameObject p_Receiver_obj)
    {
        if (p_Receiver_obj == null)
            return;

        var _receiver = p_Receiver_obj.GetComponent<MyRayReciever>();
        if (_receiver != null)
        {
            _receiver.Show();
        }
    }

    private void UnshowReceiver(GameObject p_Receiver_obj)
    {
        if (p_Receiver_obj == null)
            return;

        var _receiver = p_Receiver_obj.GetComponent<MyRayReciever>();
        if (_receiver != null)
        {
            _receiver.UnShow();
        }
    }    

    private void __OnMouseMove(GameObject p_obj, MouseEventArgs p_args)
    {
        Debug.Log("RayCaster Move Event");

        if (isMulti)
            UpdateHitObjects(p_args.mouse_position);
        else
            UpdateHitObject(p_args.mouse_position);
    }

    private void __OnLButtonDown(GameObject p_obj, MouseEventArgs p_args)
    {
        Debug.Log("RayCaster LButton Event");

        if (isMulti)
            UpdateHitObjects(p_args.mouse_position);
        else
            UpdateHitObject(p_args.mouse_position);
    }
}
