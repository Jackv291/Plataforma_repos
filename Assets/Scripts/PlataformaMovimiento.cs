using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlataformaMovimiento : MonoBehaviour
{
    public float velocidad = 0.5f;
    public GameObject InicioPlataforma;
    public GameObject FinPlataforma;
    public GameObject cuboprota;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       transform.position += Vector3.right * velocidad;
       if (transform.position.x >= FinPlataforma.transform.position.x)
        {
            velocidad = velocidad * -1f;
        }
       else if (transform.position.x <= InicioPlataforma.transform.position.x )
        {
            velocidad = velocidad * -1;
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
     if (collision.gameObject == cuboprota)
        {
            collision.gameObject.transform.parent = transform;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == cuboprota)
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
