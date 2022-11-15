using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BoxCollider))]
public class ObstacleManager : MonoBehaviour
{
    private Bounds _bounds;
    [SerializeField] private GameObject obstaclePrefab;

    private Queue<GameObject> _obstaclePool;
    [SerializeField] private int initialPoolSize = 15;
    [SerializeField] private float timeBetweenObstacleSpawn;
    [SerializeField] private float obstacleSpeed;

    private float t;
    private float extentsZ;
    private ObstacleBehavior _behavior;

    private void Awake()
    {
        _bounds = GetComponent<BoxCollider>().bounds;
        _behavior = obstaclePrefab.GetComponent<ObstacleBehavior>();
    }

    private void Start()
    {
        _obstaclePool = new Queue<GameObject>();
        InitializePool();
        t = 0f;
        extentsZ = _bounds.center.z - _bounds.extents.z;
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject x = Instantiate(obstaclePrefab, transform);
            MoveToVoid(x);
            _obstaclePool.Enqueue(x);
        }
    }

    private void MoveToVoid(GameObject o)
    {
        o.transform.position = new Vector3(-100f, -100f, -100f);
    }

    private void Update()
    {
        SpawnObstacles();
        CheckObstacle();
        ObstacleBehavior.SetSpeed(obstacleSpeed);
    }

    private void CheckObstacle()
    {
        foreach (Transform t in transform)
        {
            if (t.position.z <= extentsZ)
            {
                EnqueueAgain(t.gameObject);
            }
        }
    }

    private void SpawnObstacles()
    {
        if (t < timeBetweenObstacleSpawn)
        {
            t += Time.deltaTime;
        }
        else
        {
            float x = Random.Range(_bounds.center.x - _bounds.extents.x, _bounds.center.x + _bounds.extents.x);
            float y = Random.Range(_bounds.center.y - _bounds.extents.y, _bounds.center.y + _bounds.extents.y);
            Vector3 v = new Vector3(x, y, _bounds.center.z + _bounds.extents.z);

            DequeueFromPoolInPos(v);
            
            t = 0;
        }
    }

    private void DequeueFromPoolInPos(Vector3 vector3)
    {
        var a = _obstaclePool.Dequeue();
        a.transform.position = vector3;
    }

    private void EnqueueAgain(GameObject g)
    {
        _obstaclePool.Enqueue(g);
        g.transform.parent = transform;
        MoveToVoid(g);
    }
}