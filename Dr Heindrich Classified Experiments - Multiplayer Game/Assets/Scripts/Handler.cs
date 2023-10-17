using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Handler
{
    static public Vector3 checkpointPos;
    static public string checkpointName;
    static public List<AudioSource> sfxSources = new List<AudioSource>();
    static public string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    static public DateTime strt;
    public static void ChangeScene(string scene) {
        sfxSources.Clear();
        currentScene = scene;
        checkpointName = null;
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        Debug.Log("Working");
        if (scene == "End Screen")
        {
            GameObject.Find("Time").GetComponent<TMP_Text>().text = DateTime.Now.Subtract(strt).ToString();
        }



    }
    public static void SfxAddMe(AudioSource source) {
        sfxSources.Add(source);
    }
    public static void SfxRemoveMe(AudioSource source) {
        sfxSources.Remove(source);
    }

    public static void Test(string text) {
        Debug.Log(text);
    }
}
