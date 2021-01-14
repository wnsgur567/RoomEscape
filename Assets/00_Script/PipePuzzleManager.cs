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

    public bool M_IsClearPuzzle;
    private bool m_endflag;

    [SerializeField]
    private GameObject m_OldPipe;

    [SerializeField]
    private GameObject m_BrokenPipe;
    [SerializeField]
    private MeshRenderer m_MeshRenderer;

    private void Awake()
    {
        M_pipeManager = this;
    }
    private void Start()
    {
        M_IsClearPuzzle = false;
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
                M_IsClearPuzzle = true;
            }
            else
            {
                M_IsClearPuzzle = false;
                break;
            }
        }

        if (M_IsClearPuzzle)
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
        m_OldPipe.SetActive(false);
        m_BrokenPipe.SetActive(true);
        m_MeshRenderer.material.color = Color.black;

    }
}
