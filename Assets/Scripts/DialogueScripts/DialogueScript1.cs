using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class DialogueScript1 : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public string[] lines;
    public TextMeshProUGUI dialogueName;
    public string[] names;
    public float textSpeed = 0.1f;
    public int escena;
    private int index;
    private PlayerInput controles;

    void Start()
    {
        controles = GetComponent<PlayerInput>();
        dialogueText.text = string.Empty;
        dialogueName.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (controles.actions["Attack"].WasPressedThisFrame())
        {
            if (dialogueText.text == lines[index] && dialogueName.text == names[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];
                dialogueName.text = names[index];


            }
        }
    }

    public void StartDialogue()
    {
        index = 0;
        StartCoroutine(WriteLine());
    }

    IEnumerator WriteLine()
    {
        foreach (char letter in names[index].ToCharArray())
        {
            dialogueName.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        foreach (char letter in lines[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        
    }

    public void NextLine()
    {
        if (index < lines.Length - 1 && index < names.Length)
        {
            index++;
            dialogueText.text = string.Empty;
            dialogueName.text = string.Empty;
            StartCoroutine(WriteLine());
        }
        else
        {
            SceneManager.LoadScene(escena);
            gameObject.SetActive(false);
        }
    }
    /*
     * Opcion con colliders:
     
     private void OnCollisionEnter2D(Colission2D collision){

        if(collision.gameObject.CompareTag("Panel")){
            StartDialogue();
        }
    }
     
     
     */
}
