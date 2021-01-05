using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeButton : MonoBehaviour
{
    private bool onPipe = true;
    public GameObject[] pipe;
    public void ActiveButton()
    {
        if(onPipe)
        {
            for (int i = 0; i < 3; i++)
            {
                pipe[i].SetActive(false);
                onPipe = false;
            }
        }
        else
        {
            for (int i=0;i<3;i++)
            {
                pipe[i].SetActive(true);
                onPipe = true;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
