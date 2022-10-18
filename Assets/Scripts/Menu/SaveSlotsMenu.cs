using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private MenuManager menuManager;
    [Header("Menu Buttons")]
    [SerializeField] private Button backButton; 
    private SaveSlot[] saveSlots;
    private bool isLoadingGame = false;

    private void Awake()
    {
        saveSlots = GetComponentsInChildren<SaveSlot>();

    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        //Desactivar los botones
        DisableMenuButtons();
        //Actualizar el profile id seleccionado que va a ser utilizado para persistir los datos
        DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

        if(!isLoadingGame)
        {
            //Crear un nuevo juego que va a inicializar los datos en un slot vacio
            DataPersistenceManager.instance.NewGame();
        }

        //Cargar la escena. Que va a activar el guardado (SaveGame) debido a OnSceneUnloaded en el DataPersistenceManager
        SceneManager.LoadSceneAsync("ZonaPruebas");
    }
    public void OnBackClicked()
    {
        menuManager.ActivateMenu();
        this.DeactivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        //Set este menu para ser activado
        this.gameObject.SetActive(true);
        //set mode
        this.isLoadingGame = isLoadingGame;
        //Cargar todos los profiles que existen
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();
        
        //Loop por cada save slot en el UI y organizar el contenido apropiadamente
        GameObject firstSelected = backButton.gameObject;
        foreach(SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if(profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
                if(firstSelected.Equals(backButton.gameObject))
                {
                    firstSelected = saveSlot.gameObject;
                }
            }
        }

        //Setear el primer boton seleccionado
        StartCoroutine(this.SetFirstSelected(firstSelected));
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    private void DisableMenuButtons()
    {
        foreach(SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }
}
