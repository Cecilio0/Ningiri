using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAudio : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;
    public Image muteImage;

    public GameObject origen;

    // Start is called before the first frame update
    void Start()
    {
        slider.value =  PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        origen.GetComponent<AudioSource>().volume = slider.value;
        revisarMute();
    }

    public void ChangeVolume(float valor)
    {
        sliderValue = valor;
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        origen.GetComponent<AudioSource>().volume = slider.value;
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
