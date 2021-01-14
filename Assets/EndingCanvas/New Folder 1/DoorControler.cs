using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControler : MonoBehaviour
{
    [SerializeField] GameObject m_doorPivot;
    [SerializeField] E_RoomType m_roomType;
    [SerializeField] Canvas m_canvas;

    private void Start()
    {
        
        //StartCoroutine(__OpenDoor()); // test code
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;
        StartCoroutine(__OpenDoor());
    }

    IEnumerator __OpenDoor()
    {
        int _loopCount = 70;
        while (true)
        {
            --_loopCount;
            if (_loopCount < 0)
            {
                m_canvas.gameObject.SetActive(true);
                break;
            }            
            if (m_roomType == E_RoomType.A)
            {
                m_doorPivot.transform.Rotate(new Vector3(0, -1, 0));
            }
            else
            {
                m_doorPivot.transform.Rotate(Vector3.up);
            }
            yield return null;
        }             
    }
}
