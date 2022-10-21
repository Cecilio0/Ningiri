using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Felicitaciones : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string texto;
    [SerializeField] private float textSpeed;
    [SerializeField] private TextMeshProUGUI campoTexto;
    void Start()
    {
        StartCoroutine(Felicitar());
    }

    // Update is called once per frame


    private IEnumerator Felicitar()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject.FindGameObjectWithTag("Player").transform.position = Vector2.zero;
        campoTexto.text = "";
        yield return new WaitForSeconds(1.9f);
        foreach (char letter in texto)
        {
            campoTexto.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        yield return new WaitForSeconds(1);
        campoTexto.text = "";
    }

}
