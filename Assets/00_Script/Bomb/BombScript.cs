using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject M_cutRed;

    public GameObject M_cutBlue;

    public GameObject M_cutBlack;

    public GameObject M_cutWhite;
    public GameObject M_cutYellow;

    public GameObject M_cutGreen;

    public static BombScript M_instance;
    private void Awake()
    {
        M_instance = this;
    }
}
