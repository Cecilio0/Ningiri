using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brightness : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;
    public Image brightnessImage;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("Brightness", 0.5f);
        brightnessImage.color = new Color(brightnessImage.color.r, brightnessImage.color.g, brightnessImage.color.b, slider.value);

    }

    public void ChangeBrightness(float valor){
        slider.value = valor;
        PlayerPrefs.SetFloat("Brightness", slider.value);
        brightnessImage.color = new Color(brightnessImage.color.r, brightnessImage.color.g, brightnessImage.color.b, slider.value);
    }
}
