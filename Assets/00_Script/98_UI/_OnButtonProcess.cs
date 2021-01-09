using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class _OnButtonProcess : MonoBehaviour
{
    public static void __OnLoadScene(string _name)
    {
        SceneManager.LoadScene(_name);
    }
    public static void __OnLoadSceneAdditive(string _name)
    {
        SceneManager.LoadScene(_name, LoadSceneMode.Additive);
    }
    public static void __OnApplicationQuit()
    {
        Application.Quit();
    }
    

    ////////////////////////////////////
    
    public static void __OnActiveGameObject(GameObject p_obj)
    {
        p_obj.SetActive(true);
    }
    public static void __OnDeActiveGameObject(GameObject p_obj)
    {
        p_obj.SetActive(false);
    }
}
