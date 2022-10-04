using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Play Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button SaveFileButton1;
    [SerializeField] private Button SaveFileButton2;
    [SerializeField] private Button SaveFileButton3;

    //Cuando se presiona el boton Quit
    public void ButtonQuit() {
        Debug.Log("sale del juego");
        Application.Quit();
    }

    //Cuando se presiona un boton que lleva a una nueva escena
    public void ButtonEscena(string escena) {
        SceneManager.LoadScene(escena);
    }

    public void OnNewGameClicked(){
        DisablePlayButtons();
        //create a new game - which will initialize our game 
        
        //Load the gameplay scene - which will in turn save the game because of the OnSceneUnloaded() in DataPersistenceManager
        DataPersistenceManager.instance.isNewGame = true;
        SceneManager.LoadSceneAsync("ZonaPruebas");
        DataPersistenceManager.instance.LoadGame();
    }

    public void OnSaveFileClicked(){
        DisablePlayButtons();
        //Load the next scene - which will in turn save the game because of the OnSceneLoaded() in DataPersistenceManager
        DataPersistenceManager.instance.isNewGame = false;
        SceneManager.LoadSceneAsync("ZonaPruebas");
        DataPersistenceManager.instance.LoadGame();
    }

    private void DisablePlayButtons(){
        newGameButton.interactable = false;
        SaveFileButton1.interactable = false;
        SaveFileButton2.interactable = false;
        SaveFileButton3.interactable = false;
    }
}
