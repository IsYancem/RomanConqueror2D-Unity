using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RomanSoldier : MonoBehaviour
{
    private int lives = 3; 
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>(); // para encontrar una referencia al GameManager
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Warrior"))
        {
            lives -= 1;
            if (lives <= 0)
            {
                gameManager.EndGame(); // si las vidas son 0 o menos, llama al método EndGame de GameManager
            }
        }
    }
}