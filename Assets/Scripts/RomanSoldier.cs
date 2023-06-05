using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RomanSoldier : MonoBehaviour
{
    public float fuerzaSalto;
    public static int vidas = 5;

    private Rigidbody2D rigidbody2D;
    private Animator animator;

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

        if (Input.GetKeyDown(KeyCode.Space)){
            animator.SetBool("isJump", true);
            rigidbody2D.AddForce(new Vector2(0, fuerzaSalto));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Suelo" || collision.gameObject.tag == "Obstacle")
        {
            animator.SetBool("isJump", false);
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
            gameManager.IncreaseScore(20);
        }
    }

    public void ResetPosition()
    {
        Vector2 initialPosition = new Vector2(0, 0); // Cambia las coordenadas a las que desees que regrese el jugador
        rigidbody2D.velocity = Vector2.zero;
        transform.position = initialPosition;
    }
}