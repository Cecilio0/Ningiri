using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightEnter : MonoBehaviour
{
    [SerializeField] private GameObject[] toDisable;
    [SerializeField] private GameObject[] toEnable;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (GameObject thing in toDisable)
        {
            if (thing != null)
                thing.SetActive(false);
        }
            
        foreach (GameObject thing in toEnable)
            if (thing != null)
                thing.SetActive(true);
    }
}
