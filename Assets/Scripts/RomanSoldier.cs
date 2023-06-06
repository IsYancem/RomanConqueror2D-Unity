using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RomanSoldier : MonoBehaviour
{
    public float fuerzaSalto;
    public static int vidas = 5;
    public float velocidadMovimiento; // Velocidad de movimiento del jugador

    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private bool puedeSaltar = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!GameManager.isGameRunning)
        {
            return;
        }

        // Movimiento horizontal
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        rigidbody2D.velocity = new Vector2(movimientoHorizontal * velocidadMovimiento, rigidbody2D.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && puedeSaltar)
        {
            animator.SetBool("isJump", true);
            rigidbody2D.AddForce(new Vector2(0, fuerzaSalto));
            puedeSaltar = false;
        }

        // Dirección del personaje
        if (movimientoHorizontal < 0) // Movimiento hacia la izquierda
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (movimientoHorizontal > 0) // Movimiento hacia la derecha
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Suelo" || collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "Enemy")
        {
            animator.SetBool("isJump", false);
            puedeSaltar = true;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            vidas--;

            if (vidas <= 0)
            {
                GameManager gameManager = FindObjectOfType<GameManager>();
                gameManager.DecreaseLives();
                GameManager.isGameRunning = false;
            }
        }

        if (collision.gameObject.tag == "Obstacle") // Si colisiona con un obstáculo, incrementa la puntuación.
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.IncreaseScore(10);
        }
    }

    public void ResetPosition()
    {
        Vector2 initialPosition = new Vector2(0, 0); // Cambia las coordenadas a las que desees que regrese el jugador
        rigidbody2D.velocity = Vector2.zero;
        transform.position = initialPosition;
        puedeSaltar = true; // Reinicia la capacidad de saltar al reiniciar la posición
    }
}