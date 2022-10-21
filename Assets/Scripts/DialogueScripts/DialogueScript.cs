using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;


public class DialogueScript : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public string[] lines;
    public float textSpeed = 0.1f;
    private int index;
    private PlayerInput controles;

    void Start()
    {
        controles = GetComponent<PlayerInput>();
        dialogueText.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (controles.actions["Attack"].WasPressedThisFrame())
        {
            if (dialogueText.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];

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
        foreach (char letter in lines[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(WriteLine());
        }
        else
        {
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
