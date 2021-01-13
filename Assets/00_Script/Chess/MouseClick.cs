using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MouseClick : MonoBehaviourPun
{
    public float m_range;
    public Camera m_cam;







    void Interaction()
    {
        Ray ray = m_cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //if (Physics.Raycast(m_cam.transform.position, m_cam.transform.forward, out hit, m_range))
        //{

        if (Physics.Raycast(ray, out hit, m_range))
        {
            if (Input.GetMouseButtonDown(0))
            {
                //체스게임 활성화
                if (hit.transform.tag == "ChessGame")
                {
                    ChessZoom chess = hit.transform.GetComponent<ChessZoom>();
                    chess.ZoomInSet();
                }
            }

            //ChessGameClick(hit);
        }
    }

    
    void Update()
    {
        Interaction();
    }
}
