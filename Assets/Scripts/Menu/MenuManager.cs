using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    private void Start()
    {
        if(!DataPersistenceManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
        }
    }
    public void OnNewGameButtonPressed()
    {
        DisableMenuButtons();
        //Create a new game - which will initialize the game data
        DataPersistenceManager.instance.NewGame();
        //Load the gameplay scene - which will in turn save the game because of OnSceneUnloaded() in the DataPersistenceManager
        SceneManager.LoadScene("ZonaPruebas");
    }
    public void OnContinueButtonPressed()
    {
        DisableMenuButtons();
        Debug.Log("Continue Button Pressed");
        //Load the next scene - which will in turn Load the same because of OnSceneLoaded() in the DataPersistenceManager
        SceneManager.LoadScene("ZonaPruebas");
    }
    
    //Cuando se presiona el boton Quit
    public void ButtonQuit() {
        Debug.Log("sale del juego");
        Application.Quit();
    }

    //Cuando se presiona un boton que lleva a una nueva escena
    public void ButtonEscena(string escena) {
        SceneManager.LoadScene(escena);
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }
}
