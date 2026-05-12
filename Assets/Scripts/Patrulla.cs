using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrulla : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 2f;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3 (velocidad * Time.deltaTime, 0, 0);
        if (transform.position.x >= puntoB.position.x)
        {
            velocidad = velocidad * -1;
        }
        else if (transform.position.x <= puntoA.position.x)
        {
            velocidad = velocidad * -1;
        }
    }
}
