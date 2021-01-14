using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintPuzzleManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_Paint;
    [SerializeField]
    Collider m_PaintCol;
    public static PaintPuzzleManager M_paintManager = null;

    public bool M_IsStarted;
    private bool m_IsClearPuzzle;
    private bool m_endflag;
    // Start is called before the first frame update
    private void Awake()
    {
        M_paintManager = this;
    }
    private void Start()
    {
        m_IsClearPuzzle = false;
        m_endflag = false;
        M_IsStarted = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (m_endflag == true)
            return;

        for (int i = 0; i < 5; i++)
        {
            if (m_Paint[i].activeSelf == false)
            {
                m_IsClearPuzzle = true;
            }
            else
            {
                m_IsClearPuzzle = false;
                break;
            }
        }
        if (m_IsClearPuzzle)
        {
            GameManager.M_gameManager.Complete_PaintPuzzle();
            EndPuzzle();
            m_endflag = true;
        }
    }
    void EndPuzzle()
    {
        m_PaintCol.enabled = false;
    }
}
