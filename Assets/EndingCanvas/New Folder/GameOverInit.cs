using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverInit : MonoBehaviour
{
    [SerializeField] ExplosionActivate m_bomb;
    [SerializeField] Canvas m_endUI;
    [SerializeField,Range(0f,10f)] float explosion_delay;
    [SerializeField, Range(0f, 10f)] float UI_delay;

    void Start()
    {
        //_SoundManager.Instance.PlayObjInterationSound(E_ObjectInterationSound.bomb_fail);
        Invoke("ActiveBombs", explosion_delay);
        Invoke("ActiveUI", UI_delay);
    }

    public void ActiveBombs()
    {
        m_bomb.gameObject.SetActive(true);
    }

    public void ActiveUI()
    {
        m_endUI.gameObject.SetActive(true);
    }

    
}
