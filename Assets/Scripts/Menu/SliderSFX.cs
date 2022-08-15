using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSFX : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;
    public Image muteImage;

    // Start is called before the first frame update
    void Start()
    {
        slider.value =  PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        AudioListener.volume = slider.value;
        revisarMute();
    }

    public void ChangeVolume(float valor)
    {
        sliderValue = valor;
        PlayerPrefs.SetFloat("SFXVolume", sliderValue);
        AudioListener.volume = sliderValue;
        revisarMute();
    }
    
    public void revisarMute(){
        if(slider.value == 0){
            muteImage.enabled = true;
        }else{
            muteImage.enabled = false;
        }
    }

}
