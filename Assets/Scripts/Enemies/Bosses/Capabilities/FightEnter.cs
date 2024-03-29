using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightEnter : MonoBehaviour
{
    [SerializeField] private GameObject[] earlyEnable;
    [SerializeField] private GameObject[] toDisable;
    [SerializeField] private GameObject[] toEnable;
    [SerializeField] private GameObject[] lateDisable;
    [SerializeField] private GameObject bossElements;
    [SerializeField] private int frames;
    [SerializeField] private string bossName;
    [SerializeField] private string bossSubtitle;
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private AudioSource audioEscena;
    private Resolution resolution;
    private Image brillo;
    private RectTransform titulo;
    private RectTransform titulo2;
    private Collider2D coll;
    

    
    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        resolution = GetComponent<Resolution>();
        brillo = bossElements.GetComponent<Image>();
        titulo = bossElements.transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>();
        titulo2 = bossElements.transform.GetChild(0).transform.GetChild(1).GetComponent<RectTransform>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            toDisable = GameObject.FindGameObjectsWithTag("Enemy");
            titulo.GetComponent<TextMeshProUGUI>().text = bossName;
            bossElements.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = bossName;
            titulo2.GetComponent<TextMeshProUGUI>().text = bossSubtitle;
            bossElements.transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = bossSubtitle;
            coll.enabled = false;
            StartCoroutine(Titulo());
        }
                
    }

    private IEnumerator Titulo()
    {
        float ogVol = audioEscena.volume;
        audioEscena.volume = 0;
        audioEscena.clip = bossMusic;
        audioEscena.Play();
        float musicFade = ogVol/((float)frames/2f);
        Vector2 origen = titulo.position;
        Vector2 origen2 = titulo2.position;
        

        bossElements.SetActive(true);
        foreach (GameObject thing in toDisable)
        {
            if (thing != null)
                thing.SetActive(false);
        }
        foreach (GameObject thing in earlyEnable)
        {
            if (thing != null)
                thing.SetActive(true);
        }
        brillo.color = new Color(brillo.color.r, brillo.color.g, brillo.color.b, 1);
        titulo.gameObject.SetActive(true);
        float dist = 2*(Screen.height/1080f)*1600f/(float)frames;
        for(int i = 0; i < frames/2; i++)
        {
            titulo.position = new Vector2(titulo.position.x + dist, titulo.position.y);
            titulo2.position = new Vector2(titulo2.position.x + dist, titulo2.position.y);
            audioEscena.volume = audioEscena.volume + musicFade; 
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        
        
        yield return new WaitForSeconds(2);
            
        for(int i = 0; i < frames/2; i++)
        {
            titulo.position = new Vector2(titulo.position.x + dist, titulo.position.y);
            titulo2.position = new Vector2(titulo2.position.x + dist, titulo2.position.y);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        brillo.color = new Color(brillo.color.r, brillo.color.g, brillo.color.b, 0);
        foreach (GameObject thing in toEnable)
            if (thing != null)
                thing.SetActive(true);

        foreach (GameObject thing in lateDisable)
        {
            if (thing != null)
                thing.SetActive(false);
        }
        titulo.position = origen;
        titulo2.position = origen2;
        
        gameObject.SetActive(false);
    }
}
