using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPausaManager : MonoBehaviour
{

    bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused == false)
            {
                isPaused = true;
                Time.timeScale = 0.0f;

            }
            else
            {
                isPaused = false;
                Time.timeScale = 1.0f;

            }
            Menu_Manager.SetActive(isPaused);
        }

    }
}
