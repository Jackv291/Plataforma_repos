using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Movimiento : MonoBehaviour
{
    public float velocidad = 6.7f;
    public float salto = 6.7f;
    private float direccion = 0;
    private Rigidbody jugador;
    // Start is called before the first frame update
    void Start()
    {
        jugador = GetComponent<Rigidbody>();
        transform.position = new Vector3(-14.38f, 2.39f, -16.89f);
    }

    // Update is called once per frame
    void Update()
    {
        direccion = Input.GetAxis("Horizontal");

        if (direccion > 0)
        {
            jugador.velocity = new Vector3(velocidad, jugador.velocity.y);
        }
        else if (direccion < 0)
        {
            jugador.velocity = new Vector3(-velocidad, jugador.velocity.y);
        }
        {
        if (Input.GetKeyDown(KeyCode.Space))
        {
                jugador.velocity = new Vector3(0, salto , 0 );
        }

        }
    }
}
