using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueMusic : MonoBehaviour
{
    [SerializeField] private AudioClip music;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        source.clip = music;
        source.Play();
    }

    // Update is called once per frame
    
}
