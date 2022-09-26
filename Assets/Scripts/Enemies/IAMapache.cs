using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMapache : MonoBehaviour
{
    
    [SerializeField] private float pursuitSpeed;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float timerRutinas;
    private float cronometro;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Awake()
    {
        cronometro = 0;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        cronometro += Time.deltaTime;
        //si el cronometro supera al timer se hace una rutina nueva
        if (cronometro > timerRutinas)
            Behaviour();
    }

    //esto unicamente se hace si el personaje no esta dentro del rango de vision
    private void Behaviour()
    {

        cronometro = 0;
        int action = Random.Range(0, 2);
        switch(action)
        {
            case 0://se queda quieto   
                break;
            case 1://se mueve
                int direction = Random.Range(0, 2);
                switch (direction)
                {
                    case 0:
                        Move(patrolSpeed, direction);
                        break;
                    case 1:
                        Move(patrolSpeed, direction);
                        break;
                }
                break;
        }
    }

    private void Move(float speed, int direction)
    {
        rb.velocity = new Vector2(speed*(float)direction, rb.velocity.y);
        //transform.localScale = new Vector2(transform.localScale.x);
    }
    
}
