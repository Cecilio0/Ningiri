using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] public int escena;
    [SerializeField] private Vector2 respawn;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision is BoxCollider2D)
        {
            DataPersistenceManager.instance.gameData.currentLevel = escena;
            collision.GetComponent<Health>().respawnPoint = Vector2.zero;
            SceneManager.LoadScene(escena);
        }
    }

}
