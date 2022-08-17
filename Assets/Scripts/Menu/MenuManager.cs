using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //Cuando se presiona el boton Quit
    public void ButtonQuit() {
        Debug.Log("sale del juego");
        Application.Quit();
    }

    //Cuando se presiona un boton que lleva a una nueva escena
    public void ButtonEscena(string escena) {
        SceneManager.LoadScene(escena);
    }
}
