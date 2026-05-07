using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlataformaMovimiento : MonoBehaviour
{
    public float velocidad = 0.5f;
    public GameObject cuboprota;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    //Physics.SphereCast(Transform)
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
