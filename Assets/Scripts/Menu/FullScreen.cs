using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreen : MonoBehaviour
{
    public Toggle fullScreenToggle;

    // Start is called before the first frame update
    void Start()
    {
        if(Screen.fullScreen) {
            fullScreenToggle.isOn = true;
        } else {
            fullScreenToggle.isOn = false;
        }
    }
    
    void update()
    {
    }
    public void ActivarPantallaCompleta (bool pantallaCompleta) 
    {
        Screen.fullScreen = pantallaCompleta;
    }
}
