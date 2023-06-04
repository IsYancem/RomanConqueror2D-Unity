using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float velocidad = 2;
    public float warriorSpeed = 3;
    
    public Renderer fondo;
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

    void Start()
    {
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

        AddWarrior(new Vector2(10, -3)); // Forzamos la creación de un "Warrior" en el inicio
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
        Vector2 warriorPosition = new Vector2(position.x, position.y + 1);
        if (warriors.Count == 0) // solo para el primer "Warrior"
        {
            initialWarriorPosition = warriorPosition;
        }
        GameObject newWarrior = Instantiate(warriorPrefab, warriorPosition, Quaternion.identity);
        warriors.Add(newWarrior);
        originalPositions[newWarrior] = warriorPosition;
        warriorDirections[newWarrior] = -1; // dirección inicial hacia la izquierda

        newWarrior.transform.Rotate(0, 180, 0); // Hace que el Guerrero mire hacia la izquierda
    }

    void Update()
    {
        fondo.material.mainTextureOffset = fondo.material.mainTextureOffset + new Vector2(0.02f,0) * Time.deltaTime;

        for (int i = 0; i < suelo.Count; i++)
        {
            if (suelo[i].transform.position.x <= -10)
            {
                suelo[i].transform.position = new Vector3(10f, -3, 0);
            }
            suelo[i].transform.position = suelo[i].transform.position + new Vector3(-1, 0, 0) * velocidad * Time.deltaTime;
        }

        if (obstacleCounter == nextWarriorObstacle) 
        {
            AddWarrior(new Vector2(10, initialWarriorPosition.y));
            nextWarriorObstacle += Random.Range(5, 15);
        }

        MoveGameObjects(obstacles);
        MoveGameObjects(warriors);
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

                // Si el objeto es un guerrero, rotamos 180 grados
                if (warriors.Contains(gameObjects[i])) 
                {
                    gameObjects[i].transform.Rotate(0, 180, 0);
                    gameObjects[i].GetComponent<Animator>().Play("Attack");
                    // Cambiamos la dirección del guerrero
                    warriorDirections[gameObjects[i]] *= 1;
                }
                else
                {
                    // Incrementa el contador de obstáculos solo cuando es un obstáculo y no un Warrior
                    obstacleCounter++;
                }
            }
            // Para los guerreros, utilizamos la dirección almacenada y la velocidad del guerrero
            int direction = warriors.Contains(gameObjects[i]) ? warriorDirections[gameObjects[i]] : -1;
            float speed = warriors.Contains(gameObjects[i]) ? warriorSpeed : velocidad;
            gameObjects[i].transform.position = gameObjects[i].transform.position + new Vector3(direction, 0, 0) * speed * Time.deltaTime;
        }
    }

}

