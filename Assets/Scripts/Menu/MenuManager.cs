using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button loadGameButton;
    private void Start()
    {
        if(!DataPersistenceManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
            loadGameButton.interactable = false;
        }
    }
    public void OnNewGameButtonPressed()
    {
        saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }

    public void OnLoadGameButtonPressed()
    {
        saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }
    public void OnContinueButtonPressed()
    {
        DisableMenuButtons();
        Debug.Log("Continue Button Pressed");
        //Load the next scene - which will in turn Load the same because of OnSceneLoaded() in the DataPersistenceManager
        SceneManager.LoadSceneAsync(DataPersistenceManager.instance.gameData.currentLevel);
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

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
    }
    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
