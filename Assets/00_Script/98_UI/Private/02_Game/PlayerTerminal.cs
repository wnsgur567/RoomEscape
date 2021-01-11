using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTerminal : MonoBehaviour
{
    [SerializeField, ShowOnly] bool isAvailable;    // 단말기 이용 가능한 상태인지(활성화 상태인지)

    // Update is called once per frame
    void Update()
    {
        KeyInput();
    }

    private void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isAvailable)
                isAvailable = false;
            else
                isAvailable = true;
        }
    }

    private void MoveTerminal()
    {
        // dotween
    }

}
