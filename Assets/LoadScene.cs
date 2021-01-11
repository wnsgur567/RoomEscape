using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{    
    public void __OnLoadScene(string _name)
    {
        SceneManager.LoadScene(_name);        
    }   
    
    public void __OnLoadSceneAdditive(string _name)
    {
        SceneManager.LoadScene(_name, LoadSceneMode.Additive);
    }

}
