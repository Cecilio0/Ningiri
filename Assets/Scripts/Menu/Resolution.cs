using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Resolution : MonoBehaviour
{
    public Toggle toggle;
    
    public TMP_Dropdown resolutionsdropdown;
    UnityEngine.Resolution[] resolutions;
    // Start is called before the first frame update
    void Start()
    {
        if(Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
        CheckResolutions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateFullScreen(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
    }

    public void CheckResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionsdropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if(Screen.fullScreen && resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionsdropdown.AddOptions(options);
        resolutionsdropdown.value = currentResolutionIndex;
        resolutionsdropdown.RefreshShownValue();

        resolutionsdropdown.value = PlayerPrefs.GetInt("Resolution", 0);
    }

    public void SetResolution(int resolutionIndex)
    {

        PlayerPrefs.SetInt("Resolution", resolutionsdropdown.value);
        UnityEngine.Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
