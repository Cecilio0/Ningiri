using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ImageQuality : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public int quality;

    // Start is called before the first frame update
    void Start()
    {
        quality = PlayerPrefs.GetInt("Quality", 3);
        dropdown.value = quality;
        AdjustQuality();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustQuality()
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("Quality", dropdown.value);
        quality = dropdown.value;
    }
}
