using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// Controla la barra de vida del jugador y la pantalla de derrota
public class BarraVida : MonoBehaviour
{
    // RectTransform de la UI que representa la barra de vida (debe estar en el mismo GameObject)
    private RectTransform Barra;

    // Panel que se activará al morir; asignar desde el Inspector obligatoriamente
    public GameObject PantallaDerrota;


    // Start is called before the first frame update
    void Start()
    {
        // Obtener el RectTransform para poder escalar la barra según la vida
        Barra = GetComponent<RectTransform>();
    }

    // Aplica daño al jugador y muestra la pantalla de derrota cuando la vida es menor a 0
    public void Damage(float damage)
    {
        if ((Vida.VidaJugador -= damage) >= 0f)
        {
            // Se resta vida adicional en esta rama
            Vida.VidaJugador -= damage;
        }
        else if (Vida.VidaJugador < 0f)
        {
            // Al llegar por debajo de 0, se activa la pantalla de derrota y se pausa el juego
            PantallaDerrota.SetActive(true);
            Time.timeScale = 0f;
        }

        // Actualizar el tamaño visual de la barra tras aplicar el daño
        TamanoBarra(Vida.VidaJugador);
    }
    // Método para reintentar: restaura la vida, reactiva el tiempo y recarga la escena por índice
    public void Reintentar()
    {
        Vida.VidaJugador = 1f;
        Time.timeScale = 1F;
        SceneManager.LoadScene(1);
    }

    // Ajusta la escala en X del RectTransform para representar la vida (valores esperados 0..1)
    public void TamanoBarra (float tamano )
    {
        Barra.localScale = new Vector3(tamano, 1f, 1f);
    }

}
