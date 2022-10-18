using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brightness : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;
    public GameObject canvas;
    private Image[] images;

    // Start is called before the first frame update
    void Start()
    {
        images = new Image[canvas.transform.childCount];
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            images[i] = canvas.transform.GetChild(i).GetComponent<Image>();
        }
        slider.value = PlayerPrefs.GetFloat("Brightness", 0.5f);
        foreach(Image image in images)
            image.color = new Color(image.color.r, image.color.g, image.color.b, slider.value);
    }

    public void ChangeBrightness(float valor){
        slider.value = valor;
        PlayerPrefs.SetFloat("Brightness", slider.value);
        foreach(Image image in images)
            image.color = new Color(image.color.r, image.color.g, image.color.b, slider.value);
    }
}
