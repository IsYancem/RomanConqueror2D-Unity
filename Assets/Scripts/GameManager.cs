using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float velocidad = 2;
    
    public Renderer fondo;
    public GameObject col1;
    public GameObject rock1;
    public GameObject rock2;
    public GameObject rock3;
    public GameObject rock4;
    public GameObject rock5;

    public List<GameObject> suelo;
    public List<GameObject> obstacles;

    // Start is called before the first frame update
    void Start()
    {
        // Crear Mapa
        for (int i = 0; i < 21; i++)
        {
            suelo.Add(Instantiate(col1, new Vector2(-10 + i, -3), Quaternion.identity));
        }

        obstacles.Add(Instantiate(rock1, new Vector2(5, -3.15f), Quaternion.identity));
        obstacles.Add(Instantiate(rock2, new Vector2(9, -3.15f), Quaternion.identity));
        obstacles.Add(Instantiate(rock3, new Vector2(14, -3.15f), Quaternion.identity));
        obstacles.Add(Instantiate(rock4, new Vector2(16, -3.15f), Quaternion.identity));
        obstacles.Add(Instantiate(rock5, new Vector2(20, -3.15f), Quaternion.identity));


    }

    // Update is called once per frame
    void Update()
    {
        fondo.material.mainTextureOffset = fondo.material.mainTextureOffset + new Vector2(0.02f,0) * Time.deltaTime;
    
        // Mover Mapa
        for (int i = 0; i < suelo.Count; i++)
        {
            if (suelo[i].transform.position.x <= -10)
            {
                suelo[i].transform.position = new Vector3(10f, -3, 0);
            }
            suelo[i].transform.position = suelo[i].transform.position + new Vector3(-1, 0, 0) * velocidad * Time.deltaTime;
        }

        //Mover Obstaculos
        for (int i = 0; i < obstacles.Count; i++)
        {
            if (obstacles[i].transform.position.x <= -10)
            {
                float randomObs = Random.Range(8, 21);
                obstacles[i].transform.position = new Vector3(randomObs, -3.1f, 0);
            }
            obstacles[i].transform.position = obstacles[i].transform.position + new Vector3(-1, 0, 0) * velocidad * Time.deltaTime;
        }

    }
}
