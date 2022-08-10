using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //Cuando se presiona el boton Quit
    public void  QuitButton() {
        Debug.Log("sale del juego");
        Application.Quit();
    }
}
