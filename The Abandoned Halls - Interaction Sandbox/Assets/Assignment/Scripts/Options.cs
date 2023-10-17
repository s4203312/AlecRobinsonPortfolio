using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Options : MonoBehaviour
{
    private float masterAudio, masterBrightness;
    [SerializeField] private AudioSource[] speakers;        //A list of the audio sources in the scene (they need to be added by hand)
    [SerializeField] private GameObject lights;

    private void Awake(){
        masterAudio = GetFloat("volume");                   //Collect the volume setting from player preferances
        UpdateSpeakers(masterAudio);                        //Set the volume of all speakers
        masterBrightness = GetFloat("brightness");          //Collect the brightness setting from player preferances
        UpdateLights(masterBrightness);
        //mouseSensitivity(masterSensitivity);
    }

    //Setting Player Preferances
    public void SetFloat(string KeyName, float Value){      //Setting preferances
        PlayerPrefs.SetFloat(KeyName, Value);
    }
    public float GetFloat(string KeyName){                  //Getting preferances
        return PlayerPrefs.GetFloat(KeyName);
    }

    public void AudioValueUpdate(Slider slider){            //Setting the player preferances for the audio
        masterAudio = slider.value;
        SetFloat("volume", masterAudio);        
        UpdateSpeakers(masterAudio);
    }
    private void UpdateSpeakers(float volume){              //Updates the volume
        foreach (AudioSource speaker in speakers)
        {
            speaker.volume = volume;
        }
    }

    public void BrightnessValueUpdate(Slider slider){       //Setting the player preferances for the brightness
        masterBrightness = slider.value;
        SetFloat("brightness", masterBrightness);
        UpdateLights(masterBrightness);
    }
    private void UpdateLights(float intensity){             //Updates the brightness
        lights = GameObject.Find("Directional Light");
        lights.GetComponent<Light>().intensity = intensity;
    }

    //private void UpdateSensitivity(float volume)           //Updates the sensitivity
    //{}
    //public float mouseSensitivity{ get; set; } ///*****

}
