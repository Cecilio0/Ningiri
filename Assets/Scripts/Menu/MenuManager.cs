using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string escena;

    // Cuando presionen el boton de menu
    public void StartButton() {
        SceneManager.LoadScene(escena);
    }

    // Cuando se presione el boton de opciones
    public void OptionsButton() {

    }

    // Cuando se presione el boton Quit
    public void QuitButton() {
        Debug.Log("funciona");
        Application.Quit();
    }
}
