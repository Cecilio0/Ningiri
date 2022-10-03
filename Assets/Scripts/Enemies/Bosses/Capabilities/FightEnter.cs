using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightEnter : MonoBehaviour
{
    [SerializeField] private GameObject[] toDisable;
    [SerializeField] private GameObject[] toEnable;
    [SerializeField] private GameObject lateDisable;
    [SerializeField] private GameObject bossElements;
    [SerializeField] private int frames;
    [SerializeField] private string bossName;
    private Resolution resolution;
    private Image brillo;
    private RectTransform titulo;
    
    private void Awake()
    {
        resolution = GetComponent<Resolution>();
        brillo = bossElements.GetComponent<Image>();
        titulo = bossElements.transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>();
        bossElements.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = bossName;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(Titulo());        
    }

    private IEnumerator Titulo()
    {
        bossElements.SetActive(true);
        foreach (GameObject thing in toDisable)
        {
            if (thing != null)
                thing.SetActive(false);
        }
        brillo.color = new Color(brillo.color.r, brillo.color.g, brillo.color.b, 1);
        titulo.gameObject.SetActive(true);
        float dist = (Screen.height/1080f)*1500f/(float)frames;
        for(int i = 0; i < frames/2; i++)
        {
            titulo.position = new Vector2(titulo.position.x + dist, titulo.position.y);
            yield return new WaitForSecondsRealtime(Time.fixedUnscaledDeltaTime);
        }

        yield return new WaitForSeconds(2);
            
        for(int i = 0; i < frames/2; i++)
        {
            titulo.position = new Vector2(titulo.position.x + dist, titulo.position.y);
            yield return new WaitForSecondsRealtime(Time.fixedUnscaledDeltaTime);
        }
        titulo.gameObject.GetComponentInParent<Transform>().gameObject.SetActive(false);
        brillo.color = new Color(brillo.color.r, brillo.color.g, brillo.color.b, 0);
        foreach (GameObject thing in toEnable)
            if (thing != null)
                thing.SetActive(true);
        lateDisable.SetActive(false);
    }
}
