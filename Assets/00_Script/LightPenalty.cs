using UnityEngine;
using System.Collections;

public class LightPenalty : MonoBehaviour
{
    public GameObject obj;
    public static LightPenalty instance;
    public bool M_IsPenalty;

    void Awake()
    {
        if (LightPenalty.instance == null)
            LightPenalty.instance = this;
    }
    // Use this for initialization
    void Start()
    {
        M_IsPenalty = false;
    }

    IEnumerator ShowReady()
    {
        while (M_IsPenalty)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            obj.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void StartPenalty()
    {
        if (M_IsPenalty)
            StartCoroutine(ShowReady());
    }
}

