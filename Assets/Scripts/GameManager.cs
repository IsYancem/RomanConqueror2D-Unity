using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float velocidad = 2;
    public float warriorSpeed = 3;
    public TextMeshProUGUI vidaText;
    public TextMeshProUGUI scoreText;
    public GameObject playButton;
    public GameObject gameOverPanel;
    public GameObject menuInicioPanel;
    public GameObject scorePanel; // Agregamos una referencia al panel de puntuación.

    public Renderer fondo;
    public Renderer fondo2;
    private int fondoActual = 1; // 1 representa el fondo y 2 representa el fondo2
    private int puntosParaCambio = 100;

    public GameObject col1;
    public GameObject rock1;
    public GameObject rock2;
    public GameObject rock3;
    public GameObject rock4;
    public GameObject rock5;
    public GameObject warriorPrefab;

    private int obstacleCounter = 0;
    private int nextWarriorObstacle;
    private Vector2 initialWarriorPosition;

    public List<GameObject> suelo;
    public List<GameObject> obstacles;
    public List<GameObject> warriors;

    public Dictionary<GameObject, Vector2> originalPositions = new Dictionary<GameObject, Vector2>();
    public Dictionary<GameObject, int> warriorDirections = new Dictionary<GameObject, int>();

    public static bool isGameRunning = false;

    private int score = 0;

    void Start()
    {
        playButton.GetComponent<Button>().onClick.AddListener(() => StartGame());
        gameOverPanel.SetActive(false);
        menuInicioPanel.SetActive(true);
        scorePanel.SetActive(false); // Inicialmente, el panel de puntuación estará desactivado.

        for (int i = 0; i < 31; i++)
        {
            suelo.Add(Instantiate(col1, new Vector2(-10 + i, -3), Quaternion.identity));
        }

        nextWarriorObstacle = Random.Range(5, 15);

        AddObstacle(rock1, new Vector2(5, -3.25f));
        AddObstacle(rock2, new Vector2(9, -3.15f));
        AddObstacle(rock3, new Vector2(14, -3.13f));
        AddObstacle(rock4, new Vector2(17, -3.22f));
        AddObstacle(rock5, new Vector2(26, -3.25f));

        AddWarrior(new Vector2(10, -3));

        isGameRunning = false;

        fondoActual = 1; // Establecer el fondo como fondo inicial
        fondo.gameObject.SetActive(true);
        fondo2.gameObject.SetActive(false);
    }

    private void AddObstacle(GameObject obstaclePrefab, Vector2 position)
    {
        GameObject newObstacle = Instantiate(obstaclePrefab, position, Quaternion.identity);
        obstacles.Add(newObstacle);
        originalPositions[newObstacle] = position;

        obstacleCounter++;
    }

    private void AddWarrior(Vector2 position)
    {
        Vector2 warriorPosition = new Vector2(position.x, position.y + 0.5f);
        if (warriors.Count == 0)
        {
            initialWarriorPosition = warriorPosition;
        }
        
        // Genera un número aleatorio para determinar si el guerrero mirará hacia la izquierda o la derecha
        int direction = Random.Range(0, 2) == 0 ? -1 : 1;
        Vector3 scale = new Vector3(direction, 1, 1);
        
        GameObject newWarrior = Instantiate(warriorPrefab, warriorPosition, Quaternion.identity);
        warriors.Add(newWarrior);
        originalPositions[newWarrior] = warriorPosition;
        warriorDirections[newWarrior] = -1;
        
        // Aplica la escala para que el guerrero mire hacia la izquierda
        newWarrior.transform.localScale = scale;
    }

    void Update()
    {
        if (!isGameRunning)
        {
            return;
        }

        fondo.material.mainTextureOffset = fondo.material.mainTextureOffset + new Vector2(0.02f, 0) * Time.deltaTime;
        for (int i = 0; i < suelo.Count; i++)
        {
            if (suelo[i].transform.position.x <= -10)
            {
                suelo[i].transform.position = new Vector3(10f, -3, 0);
            }
            suelo[i].transform.position = suelo[i].transform.position + new Vector3(-1, 0, 0) * velocidad * Time.deltaTime;
        }

        // Generar un nuevo guerrero de forma aleatoria
        if (Random.Range(0, 1000) < 1) // Ajusta el valor 10 según la frecuencia deseada
        {
            AddWarrior(new Vector2(10, initialWarriorPosition.y));
        }

        MoveGameObjects(obstacles);
        MoveGameObjects(warriors);

        if (RomanSoldier.vidas <= 0)
        {
            gameOverPanel.SetActive(true);
            playButton.SetActive(true);
            isGameRunning = false; // Detenemos el juego cuando el jugador pierde todas las vidas.
        }
        vidaText.text = RomanSoldier.vidas.ToString();
    }

    private void MoveGameObjects(List<GameObject> gameObjects)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i].transform.position.x <= -10)
            {
                float randomObs = Random.Range(4, 30);
                Vector2 originalPos = originalPositions[gameObjects[i]];
                gameObjects[i].transform.position = new Vector3(randomObs, originalPos.y, 0);

                if (warriors.Contains(gameObjects[i]))
                {
                    gameObjects[i].transform.Rotate(0, 180, 0);
                    gameObjects[i].GetComponent<Animator>().Play("Attack");
                    warriorDirections[gameObjects[i]] *= 1;
                }
                else
                {
                    obstacleCounter++;
                }
            }

            int direction = warriors.Contains(gameObjects[i]) ? warriorDirections[gameObjects[i]] : -1;
            float speed = warriors.Contains(gameObjects[i]) ? warriorSpeed : velocidad;
            gameObjects[i].transform.position = gameObjects[i].transform.position + new Vector3(direction, 0, 0) * speed * Time.deltaTime;
        }
    }

    public void StartGame()
    {
        ResetGame(); // Agrega esta línea

        menuInicioPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        scorePanel.SetActive(true); // Activamos el panel de puntuación al iniciar el juego.
        RomanSoldier.vidas = 5;
        playButton.SetActive(false);
        isGameRunning = true;
        score = 0; // Restablecemos la puntuación a cero.
        UpdateScoreText(); // Actualizamos el texto de puntuación en el panel.



        fondoActual = 1; // Reiniciar el fondo a fondo al iniciar el juego
        fondo.gameObject.SetActive(true);
        fondo2.gameObject.SetActive(false);
    }

    private void ResetGame()
    {
        // Reinicia la posición del jugador
        RomanSoldier[] soldiers = FindObjectsOfType<RomanSoldier>();
        foreach (RomanSoldier soldier in soldiers)
        {
            soldier.ResetPosition();
        }

        // Reinicia la posición de los obstáculos
        foreach (GameObject obstacle in obstacles)
        {
            Vector2 originalPos = originalPositions[obstacle];
            obstacle.transform.position = originalPos;
        }

        // Reinicia la posición de los guerreros
        foreach (GameObject warrior in warriors)
        {
            Vector2 originalPos = originalPositions[warrior];
            warrior.transform.position = originalPos;
        }

        // Restablece las variables de control
        obstacleCounter = 0;
        nextWarriorObstacle = Random.Range(5, 15);
        score = 0;
    }

    public void IncreaseScore(int amount)
    {
        score += amount; // Incrementamos la puntuación.
        UpdateScoreText(); // Actualizamos el texto de puntuación en el panel.
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString(); // Actualizamos el texto de puntuación en el panel.

        // Comprobar si se alcanzó la cantidad de puntos para cambiar de fondo
        if (score % puntosParaCambio == 0)
        {
            if (fondoActual == 1)
            {
                // Cambiar a fondo2
                fondo.gameObject.SetActive(false);
                fondo2.gameObject.SetActive(true);
                fondoActual = 2;
            }
            else
            {
                // Cambiar a fondo
                fondo2.gameObject.SetActive(false);
                fondo.gameObject.SetActive(true);
                fondoActual = 1;
            }
        }
    }

    public void DecreaseLives()
    {
        vidaText.text = "0"; // Actualiza el texto de vidas a "0".
        gameOverPanel.SetActive(true); // Activa el panel de Game Over.
        playButton.SetActive(true);
        isGameRunning = false;
    }
}