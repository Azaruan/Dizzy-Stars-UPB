using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour
{
    private Vector2 realOrientation;
    private Vector2 newOrientation;

    private int state;
    private bool isDead;

    [SerializeField] private float changeTimer, speed;

    [SerializeField] private float hLimiter = 8f;
    [SerializeField] private float vLimiter = 5f;
    [SerializeField] private GameObject child;

    private float _t;
    private float timeAlive;

    private void Start()
    {
        state = 0;
        _t = 0;
        timeAlive = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        
        realOrientation.x = Input.GetAxis("Horizontal");
        realOrientation.y = Input.GetAxis("Vertical");

        ChangeStateWithTime();

        newOrientation = TransformOrientation(state);
        ApplyMovement(newOrientation);
        CheckIfDead();
    }

    private void CheckIfDead()
    {
        if (isDead)
        {
            child.SetActive(false);
        }
        else
        {
            timeAlive += Time.deltaTime;
        }
    }

    private void ThrowRay()
    {
        RaycastHit r;
        if (Physics.Raycast(transform.position, Vector3.forward, out r, 100f))
        {
            if (r.distance <= 1)
            {
                isDead = true;
                Debug.Log(isDead);
            }
        }
    }

    private void ChangeStateWithTime()
    {
        if (_t < changeTimer)
        {
            _t += Time.deltaTime;
        }
        else
        {
            _t = 0;
            if (state < 3)
            {
                state++;
            }
            else
            {
                state = 0;
            }
        }
    }

    private Vector2 TransformOrientation(int i)
    {
        Vector2 temp;
        switch (i)
        {
        case 0:                                                                                                         //no rotation
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                return realOrientation;
            }
            case 1:                                                                                                     //rotated 90 degrees clockwise
            {
                transform.eulerAngles = new Vector3(0, 0, 270);
                temp.x = realOrientation.y;
                temp.y = realOrientation.x * -1;
                return temp;
            }
            case 2:                                                                                                     //rotated 180 degrees clockwise
            {
                transform.eulerAngles = new Vector3(0, 0, 180);
                return realOrientation * -1;
            }
            case 3:                                                                                                     //rotated 270 degrees clockwise
            {
                transform.eulerAngles = new Vector3(0, 0, 90);
                temp.x = realOrientation.y * -1;
                temp.y = realOrientation.x;
                return temp;
            }
            default:
            {
                return realOrientation;
            }
        }
    }

    private void ApplyMovement(Vector2 vector2)
    {
        Vector3 temp;
        temp.x = vector2.x * speed * Time.deltaTime;
        temp.y = vector2.y * speed * Time.deltaTime;
        temp.z = 0;

        if (IsWithinBounds(transform.position.x + temp.x, hLimiter) &&
            IsWithinBounds(transform.position.y + temp.y, vLimiter))
        {
            transform.position += temp;
        }
    }

    private bool IsWithinBounds(float f, float bound)
    {
        return f > -bound && f < bound;
    }

    public void Kill()
    {
        isDead = true;
    }
}