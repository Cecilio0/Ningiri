using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool on;
    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
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
        bool buttonDown = playerControls.Land.Pause.IsPressed();
        if (buttonDown)//cuando se presione una tecla vinculada a la accion pause 
        {
            on = !on;
        }

        if (on)
        {
            pauseMenu.SetActive(true);//abre el menu
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
    }

}
