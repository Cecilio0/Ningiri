using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    GameObject [] panels;
    private bool on;
    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
        panels = new GameObject [pauseMenu.transform.childCount];
        for (int i = 0; i < pauseMenu.transform.childCount; i++)
        {
            panels[i] = pauseMenu.transform.GetChild(i).gameObject;
        }
    }

        private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        bool buttonDown = playerControls.Land.Pause.WasPressedThisFrame();
        if (buttonDown)//cuando se presione una tecla vinculada a la accion pause 
        {
            on = !on;
        }

        if (on)
        {
            pauseMenu.SetActive(true);//abre el menu
            panels[0].transform.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;//reactiva el cursor
            Cursor.visible = true;
            Time.timeScale = 0;//escencialmente detiene el tiempo
        } 
        else
        {
            closeMenu();
        }
        
    }

    public void ButtonContinue() 
    {
        closeMenu();
        on = false;
    }

    private void closeMenu()
    {
        pauseMenu.SetActive(false);//cierra el menu
        Cursor.lockState = CursorLockMode.Locked;//desactiva el cursor
        Cursor.visible = false;
        Time.timeScale = 1;//reanuda el tiempo
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

}
