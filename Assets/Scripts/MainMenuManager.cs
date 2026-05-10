using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CargarJuego()
    {
        Vida.VidaJugador = 1f;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

}
