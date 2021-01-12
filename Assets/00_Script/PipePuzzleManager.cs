using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePuzzleManager : MonoBehaviour
{
    [SerializeField]
    Pipe[] m_pipe;
    [SerializeField]
    BoxCollider[] m_pipeButton;

    public static PipePuzzleManager M_pipeManager = null;

    public bool M_IsStarted;

    private bool m_IsClearPuzzle;
    private bool m_endflag;
    private void Awake()
    {
        M_pipeManager = this;
    }
    private void Start()
    {
        m_IsClearPuzzle = false;
        m_endflag = false;
        M_IsStarted = false;
    }
    private void Update()
    {
        if (m_endflag == true)
            return;

        for (int i = 0; i < 6; i++)
        {
            if (m_pipe[i].M_PipeActivate == true)
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
            GameManager.M_gameManager.Complete_PipePuzzle();
            EndPuzzle();
            m_endflag = true;
        }
    }
    void EndPuzzle()
    {
        for (int i = 0; i < 4; i++)
        {
            m_pipeButton[i].enabled = false;
        }
    }
}
