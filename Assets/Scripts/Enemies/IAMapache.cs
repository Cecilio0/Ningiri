using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMapache : MonoBehaviour
{
    //Elementos a utilizar
    public int rutina;
    public float cronometro;
    public Animator animacion;
    public int direccion;
    public float walkSpeed;
    public float runSpeed;
    public GameObject target;
    public bool atacando;
    // Start is called before the first frame update
    void Start()
    {
        animacion = GetComponent<Animator>();
        target = GameObject.Find("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
