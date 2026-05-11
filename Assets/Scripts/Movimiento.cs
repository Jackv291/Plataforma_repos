using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

// Script de control del jugador: movimiento horizontal, salto, dash y recogida de coleccionables.
public class Movimiento : MonoBehaviour
{
    public float velocidad = 7f;         // velocidad de movimiento horizontal (unidades/segundo)
    public float salto = 6.7f;           // fuerza de salto (se usa junto a la gravedad)
    public float gravedad = -9.81f;      // aceleración gravitatoria (valor negativo)
    public float dash = 30f;             // magnitud del dash (unidades/segundo)
    public float dashDuration = 0.15f;   // duración en segundos del empuje del dash
    public float cooldowndash = 0f;      // variable usada actualmente para restar en Update 
    public Animator animator;            // referencia al Animator para controlar animaciones
    public CharacterController controller; // referencia al CharacterController del personaje
    public TextMeshProUGUI puntuacionText; // UI para mostrar la puntuación
    public BarraVida barraVida;              // referencia a la barra de vida para actualizarla 
    public GameObject PantallaVictoria;          // referencia a la pantalla de victoria para activarla


    private Rigidbody rb;                  // referencia al Rigidbody para aplicar física (si se usa)
    private Vector3 playerVelocity;      // velocidad vertical acumulada (componentes X,Z no se usan aquí)
    private Vector3 dashVelocity = Vector3.zero; // velocidad horizontal temporal aplicada durante el dash
    private bool groundedPlayer;         // true si el CharacterController está apoyado en el suelo
    private bool DashSi = true;          // bandera para permitir/disallow dash (previene dashes simultáneos)
    private int ultimadireccion = 1;     // última dirección horizontal conocida: 1 = derecha, -1 = izquierda
    private int puntuacion = 0;          // contador de coleccionables recogidos
     // bandera para mostrar la pantalla de victoria (puedes usar esta variable o activar el objeto directamente)

    // Start se ejecuta una vez al inicio
    void Start()
    {
        // Obtener componentes necesarios en el mismo GameObject
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        rb=GetComponent<Rigidbody>();
        // Inicializar UI (asegúrate de asignar 'puntuacionText' en el Inspector)
        puntuacionText.text = "Score:" + puntuacion.ToString();
    }

    // Update se ejecuta una vez por frame
    void Update()
    {
        // --- Detección de suelo usando CharacterController ---
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            // Mantener una pequeña velocidad hacia abajo cuando estamos en el suelo para
            // estabilizar isGrounded y evitar "flotar" por pequeñas variaciones.
            if (playerVelocity.y < 0f)
                playerVelocity.y = -2f;

            // Si estamos en el suelo, desactivar la animación de salto
            animator.SetBool("Saltar", false);
        }

        // --- Lectura de entrada horizontal ---
        float valorX = Input.GetAxis("Horizontal");         // -1 .. 1 según A/D
        float valorY = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(valorX, 0, 0);          // vector de movimiento horizontal
        move = Vector3.ClampMagnitude(move, 1f);           // normalizar para evitar velocidades > 1 diagonal

        // --- Voltear sprite/modelo según dirección del input ---
        if (valorX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);  // mirar a la derecha 
        }
        else if (valorX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // mirar a la izquierda
        }

        // --- Control de animaciones de movimiento ---
        if (valorX != 0)
        {
            animator.SetBool("Mover", true);   // activar animación de caminar/correr
        }
        else
        {
            animator.SetBool("Mover", false);  // desactivar animación de movimiento
        }

        if (valorY > 0)
        {

        }

        // --- Guardar la última dirección horizontal conocida ---
        if (valorX > 0)
            ultimadireccion = 1;
        else if (valorX < 0)
            ultimadireccion = -1;

        // --- Salto ---
        // Solo permitido si estamos apoyados y se pulsa Space en este frame
        if (groundedPlayer && Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Saltar", true);                 // activar animación de salto
            // Calcular la velocidad vertical inicial necesaria para el salto.
            // Se multiplica por -gravedad porque gravedad es negativo.
            playerVelocity.y = salto * -gravedad;
        }

        // --- Dash (inicio con LeftShift) ---
        // Comprueba la tecla y si el dash está disponible (DashSi)
        if (Input.GetKeyDown(KeyCode.LeftShift) && DashSi)
        {
            // Calcular la dirección del dash: si hay input horizontal se usa,
            // si no, se usa la última dirección conocida (ultimadireccion).
            int direccion;
            if (valorX > 0)
                direccion = 1;
            else if (valorX < 0)
                direccion = -1;
            else
                direccion = ultimadireccion;

            // Inicia la coroutine que aplica la velocidad del dash y gestiona cooldown
            StartCoroutine(HazDash(direccion));
        }

        // --- Gestión de "cooldowndash" (si se usa) ---
        // El script mantiene esta variable y la decrementa aquí; la coroutine también
        // establece cooldowndash al terminar la fase de dash.
        if (cooldowndash > 0f)
        {
            cooldowndash -= Time.deltaTime;
            if (cooldowndash <= 0f)
            {
                dashVelocity = Vector3.zero; // asegurar que no quede velocidad residual
                cooldowndash = 0f;
            }
        }
        // --- Aplicar gravedad cada frame ---
        playerVelocity.y += gravedad * Time.deltaTime;
        // --- Construir movimiento final y aplicarlo ---
        // finalMove suma: movimiento de input * velocidad, + dashVelocity temporal, + componente vertical
        Vector3 finalMove = move * velocidad + dashVelocity + Vector3.up * playerVelocity.y;
        controller.Move(finalMove * Time.deltaTime); // Move espera desplazamiento por frame
    }

    // Coroutine que aplica el empuje del dash durante dashDuration y espera cooldown
    private IEnumerator HazDash(int direccion)
    {
        DashSi = false; // bloquear nuevos dash hasta finalizar
        // Aplicar velocidad horizontal pura en el eje X: derecha (1) o izquierda (-1)
        dashVelocity = Vector3.right * direccion * dash;

        // Mantener el empuje durante dashDuration segundos
        yield return new WaitForSeconds(dashDuration);

        // Termina el empuje
        dashVelocity = Vector3.zero;

        // Poner un cooldown antes de permitir otro dash (aquí 1 segundo)
        cooldowndash = 1f;
        yield return new WaitForSeconds(cooldowndash);

        DashSi = true; // permitir de nuevo el dash
    }

    // Colisiones por trigger: recogida de coleccionables
    private void OnTriggerEnter(Collider colision)
    {
        if (colision.tag == "Coleccionable")
        {
            // Incrementa la puntuación y actualiza la UI
            puntuacion++;
            puntuacionText.text = "Score:" + puntuacion.ToString();

            // Desactiva el objeto coleccionable (simula recogida)
            colision.gameObject.SetActive(false);
        }

        if (colision.tag == "Victoria")
        {
           if (puntuacion >= 20)
            {
                VictoriaActiva();
            }
        }

        if (colision.tag == "Senal")
        {
            rb.AddForce(Vector3.right * 30f, ForceMode.Impulse);
        }

    }

    public void veljug(float valor)
    {         
        velocidad = valor;
    }

    private void OnTriggerStay (Collider colision)
    {
        if (colision.tag == "Pinchos")
        {
            barraVida.Damage(0.005f);
        }
    }
    private void VictoriaActiva()
    { 
        PantallaVictoria.SetActive(true); // Activa la pantalla de victoria
        Time.timeScale = 0f; // Pausar el juego
    }


}
