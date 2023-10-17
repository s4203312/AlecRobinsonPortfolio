using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public void ChangeScene(string sceneName)       //Used to navigate between scenes
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()                          //Used to quit the application
    {
        Application.Quit();
    }
}
