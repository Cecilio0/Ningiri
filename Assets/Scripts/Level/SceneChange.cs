using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] public int escena;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision is BoxCollider2D)
        {
            SceneManager.LoadScene(escena);
        }
    }

}
