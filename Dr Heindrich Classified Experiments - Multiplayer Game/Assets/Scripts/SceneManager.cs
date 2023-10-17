using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    Transform checkpoint;
    // Start is called before the first frame update
    void Start()
    {
        if (Handler.checkpointName != null) {
            Vector3 offset = Vector3.zero;
            Debug.Log(Handler.checkpointName);
            if (Handler.checkpointName != "CheckpointFloorRotated") {
                offset = new Vector3(1, 0, 0);
            } else {
                offset = new Vector3(0, 0, 1);
            }
            foreach (GameObject PlayerG in GameObject.FindGameObjectsWithTag("Player")) {
                PlayerG.transform.position = Handler.checkpointPos + offset + (Vector3.up);
                offset = -offset;
            }
        }
        AudioSource mainMusic = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        float masterVolume = PlayerPrefs.GetFloat("masterVolume");
        float musicVolume = PlayerPrefs.GetFloat("musicVolume");
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        float lightIntensity = PlayerPrefs.GetFloat("lighting");
        mainMusic.volume = (musicVolume * (masterVolume / 100) / 100);
        foreach (AudioSource source in Handler.sfxSources) {
            source.volume = (sfxVolume * (masterVolume / 100) / 100);
        }
        GameObject.FindGameObjectWithTag("Light").GetComponent<Light>().intensity = lightIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetCheckPoint(Transform point) {
        Handler.checkpointPos = point.position;
        Handler.checkpointName = point.name;
    }

    public void Reset() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name); 
    }
}