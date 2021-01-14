using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionActivate : MonoBehaviour
{
    [SerializeField] List<GameObject> m_effects_1;
    [SerializeField] List<GameObject> m_effects_2;

    [SerializeField] float m_interval_1;
    [SerializeField] float m_interval_2;
    [SerializeField] float m_variable_1;
    [SerializeField] float m_variable_2;

    private void Start()
    {
        StartCoroutine(explosion1());
        StartCoroutine(explosion2());
    }

    IEnumerator explosion1()
    {
        foreach (var item in m_effects_1)
        {
            item.SetActive(true);
            yield return new WaitForSeconds(m_interval_1 + Random.Range(0f, m_variable_1));
        }        
    }
    IEnumerator explosion2()
    {
        foreach (var item in m_effects_2)
        {
            item.SetActive(true);
            yield return new WaitForSeconds(m_interval_2 + Random.Range(0f, m_variable_2));
        }
    }

}
