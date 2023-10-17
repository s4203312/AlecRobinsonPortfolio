using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;


public class Options : MonoBehaviour {
    float masterVolume;
    float musicVolume;
    float sfxVolume;
    public AudioSource mainMusic;
    [SerializeField] Slider[] sliders;
    float[] volumes;
    public EventSystem eventSystem;

    //Lighting
    public Light mainLight;
    float lightIntensity;

    enum sliderType {
        Master, Music, SFX, Light
    }
    private void Awake() {
        mainMusic = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        masterVolume = PlayerPrefs.GetFloat("masterVolume");
        musicVolume = PlayerPrefs.GetFloat("musicVolume");
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        //Lighting
        mainLight = GameObject.FindGameObjectWithTag("Light").GetComponent<Light>();
        lightIntensity = PlayerPrefs.GetFloat("lighting");

        volumes = new float[] { masterVolume, musicVolume, sfxVolume, lightIntensity };
        sliders[0].value = masterVolume;
        sliders[1].value = musicVolume;
        sliders[2].value = sfxVolume;
        sliders[3].value = lightIntensity;

        if (Handler.currentScene == "MainMenu") {
            transform.Find("MmButton").gameObject.SetActive(false);
        }
        eventSystem = EventSystem.current;
    }

    /// <summary>
    /// Will update volumes and audio sources
    /// </summary>
    /// <param name="slider"></param>
    public void sliderChanged(Slider slider) {
        int nType = Array.IndexOf(sliders, slider); //what slider has been changed
        volumes[nType] = slider.value;
        if (nType is (int)sliderType.Master or (int)sliderType.SFX) {
            masterVolume = volumes[(int)sliderType.Master]; sfxVolume = volumes[(int)sliderType.SFX];
            if (Handler.sfxSources != null) {
                foreach (AudioSource source in Handler.sfxSources) {
                    source.volume = (sfxVolume * (masterVolume / 100) / 100);
                }
            }
        }
        if (nType is (int)sliderType.Master or (int)sliderType.Music) {
            masterVolume = volumes[(int)sliderType.Master]; musicVolume = volumes[(int)sliderType.Music];
            mainMusic.volume = (musicVolume * (masterVolume / 100) / 100);
        }
        //Lighting
        if(nType is (int)sliderType.Light) {
            lightIntensity = volumes[(int)sliderType.Light];
            mainLight.intensity = lightIntensity;
        }
    }

    public void OnDestroy() {
        PlayerPrefs.SetFloat("masterVolume", masterVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        //Lighting
        PlayerPrefs.SetFloat("lighting", lightIntensity);
    }

    public void backButtonClicked() {
        Destroy(gameObject);
        eventSystem.SetSelectedGameObject(GameObject.Find("Start"));
    }

    public void MainMenuButtonClicked() {
        Handler.ChangeScene("Menu UI");
        eventSystem.SetSelectedGameObject(GameObject.Find("Start"));
    }
}