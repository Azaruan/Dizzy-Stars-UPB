using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehavior : MonoBehaviour
{
    [SerializeField] private static float _speed;

    private void Update()
    {
        transform.position -= new Vector3(0, 0, _speed * Time.deltaTime);
    }

    public static void SetSpeed(float speed)
    {
        _speed = speed;
    }
}
