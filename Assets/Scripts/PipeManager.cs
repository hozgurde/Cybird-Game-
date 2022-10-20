using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject pipe;
    public GameObject spawnPointObject;

    

    [Header("Spawn")]
    public float spawnRate = 5f;
    public float spawnRateNoise = 0.1f;

    private float dSpawn = 0f;
    private float nextSpawn;

    [Header("Speed")]
    public float speed = 1f;
    public float increaseInterval = 10f;
    public float acceleration = 0.1f;

    private float dSpeed = 0f;
    private float curSpeed;

    [Header("Holes")]
    public float holeSize = 1.5f;
    public float holeNoise = 0.5f;
    public float holeThreshold = 0.5f;

    private Camera m_camera;
    private float cameraSize;


    private Vector3 spawnPoint;

    
    

    private List<GameObject> pipes = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = spawnPointObject.transform.localPosition;
        nextSpawn = spawnRate + Random.Range(-spawnRateNoise, spawnRateNoise);
        curSpeed = speed;
        m_camera = Camera.main;
        cameraSize = m_camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        dSpawn += Time.deltaTime;
        dSpeed += Time.deltaTime;

        if(dSpawn >= nextSpawn)
        {
            dSpawn -= nextSpawn;
            nextSpawn = spawnRate + Random.Range(-0.1f, 0.1f);
            //Determine pipe hole position and size
            float curHoleSize = Random.Range(holeSize-holeNoise, holeSize+holeNoise);
            float curHolePos = Random.Range(-cameraSize + holeThreshold + curHoleSize / 2, -(-cameraSize + holeThreshold + curHoleSize / 2));
            //Spawn new pipe
            
            
            GameObject newPipe = Instantiate(pipe, spawnPoint, new Quaternion());
            if(Random.Range(0f,1f) <= 0.5f)
            {//Lower Pipe
                newPipe.transform.position = new Vector3(newPipe.transform.position.x, newPipe.transform.position.y - cameraSize + curHolePos - curHoleSize / 2, newPipe.transform.position.z);
                newPipe.GetComponent<Rigidbody2D>().velocity = new Vector2(-curSpeed, 0);
                pipes.Add(newPipe);
            }
            else
            {//Upper Pipe
                newPipe.transform.position = new Vector3(newPipe.transform.position.x, newPipe.transform.position.y + cameraSize + curHolePos + curHoleSize / 2, newPipe.transform.position.z);
                newPipe.GetComponent<Rigidbody2D>().velocity = new Vector2(-curSpeed, 0);
                pipes.Add(newPipe);
            }
            
        }
        if(dSpeed >= increaseInterval)
        {
            dSpeed -= increaseInterval;
            curSpeed += acceleration;
            foreach(GameObject iPipe in pipes)
            {
                pipe.GetComponent<Rigidbody2D>().velocity = new Vector2(-curSpeed, 0);
            }
        }
    }
}
