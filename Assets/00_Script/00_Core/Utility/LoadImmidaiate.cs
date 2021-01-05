using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadImmidaiate : MonoBehaviour
{
    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
