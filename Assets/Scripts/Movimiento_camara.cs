using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento_camara : MonoBehaviour
{
    public GameObject jugador;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float valorY = Input.GetAxis("Vertical");
        if (valorY > 0)
        {
            transform.position = new (jugador.transform.position.x, jugador.transform.position.y + 2, transform.position.z);
        }
        else if (valorY < 0)
        {
            transform.position = new (jugador.transform.position.x, jugador.transform.position.y - 2, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(jugador.transform.position.x, jugador.transform.position.y, transform.position.z);
        }
            
            
    }
}
