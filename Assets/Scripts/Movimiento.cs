using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Movimiento : MonoBehaviour
{
    public float velocidad = 6.7f;
    public float salto = 6.7f;
    public float gravedad = -9.81f;
    public float dash = 30f;
    public float dashDuration = 0.15f;
    public float cooldowndash = 0f;

    public CharacterController controller;
    private Vector3 playerVelocity;
    private Vector3 dashVelocity = Vector3.zero;
    private bool groundedPlayer;
    private bool DashSi = true;
    // Start is called before the first frame update
    private int ultimadireccion = 1;
    void Start()
    {
        controller = GetComponent<CharacterController>();

    }
    // Update is called once per frame
    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            // Slight downward velocity to keep grounded stable
            if (playerVelocity.y < 0f)
                playerVelocity.y = -2f;
        }

        // Read input
        float valorX = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(valorX, 0, 0);
        move = Vector3.ClampMagnitude(move, 1f);

        if (valorX > 0)
            ultimadireccion = 1;
        else if (valorX < 0)
            ultimadireccion = -1;

        if (move != Vector3.zero)
            transform.forward = move;

        // Jump using WasPressedThisFrame()
        if (groundedPlayer && Input.GetKeyDown(KeyCode.Space))
        {
            playerVelocity.y = salto * -gravedad;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && DashSi)
        {
            int direccion;
            if (valorX > 0)
                direccion = 1;
            else if (valorX < 0)
                direccion = -1;
            else
                direccion = ultimadireccion;

            StartCoroutine(HazDash(direccion));
        }

        if (cooldowndash > 0f)
        {
            cooldowndash -= Time.deltaTime;
            if (cooldowndash <= 0f)
            {
                dashVelocity = Vector3.zero;
                cooldowndash = 0f;
            }
        }

        // Apply gravity
        playerVelocity.y += gravedad * Time.deltaTime;

        // Move
        Vector3 finalMove = move * velocidad + dashVelocity + Vector3.up * playerVelocity.y;
        controller.Move(finalMove * Time.deltaTime);
    }
    private IEnumerator HazDash(int direccion)
    {   DashSi = false;
        dashVelocity = Vector3.right * direccion * dash;

        yield return new WaitForSeconds(dashDuration);
        dashVelocity = Vector3.zero;

        cooldowndash = 1f; // Cooldown of 1 second
        yield return new WaitForSeconds(cooldowndash);

        DashSi = true;
    }
}
