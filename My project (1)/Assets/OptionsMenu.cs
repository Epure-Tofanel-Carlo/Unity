using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;


    public void Start() //vreau sa aflu rezolutiile corecte pentru orice computer
    {

        resolutions = Screen.resolutions; // stochez in vector rezolutia 


        if (resolutionDropdown == null)
        {
            Debug.LogError("Nu a fost gasit TMP_Dropdown pe GameObject.");
            return;
        }

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)  // fac lista cu rezolutiile monitorului 
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options); // adaug lista in dropdown
        resolutionDropdown.value = currentResolutionIndex; // imi pune rezolutia by default a ecranului meu 
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex) //setez rezolutia pe ecran
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume); // pentru audio mixer de la volume, ca sa pot ajuta volumul din slider
    }

    public void SetQuality(int qualityIndex) // pentru dropdownul de graphics
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen) // pentru toggle ul de fullscreen
    {
        Screen.fullScreen = isFullscreen;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0); 
    }

}
