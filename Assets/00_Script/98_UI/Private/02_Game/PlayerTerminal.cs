using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTerminal : MonoBehaviour
{
    [SerializeField, ShowOnly] bool isAvailable;    // �ܸ��� �̿� ������ ��������(Ȱ��ȭ ��������)

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
