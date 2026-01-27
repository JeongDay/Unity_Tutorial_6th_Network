using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class SnakeController : MonoBehaviour
{
    public GameObject tailPrefab;
    public List<Transform> tailPoints = new List<Transform>();
    
    public Transform coinTransform;

    private Vector3 moveInput;
    public float moveSpeed = 5f;
    public float turnSpeed = 120f;
    public float lerpSpeed = 5f;

    void Start()
    {
        coinTransform = GameObject.FindGameObjectWithTag("Coin").transform;
    }

    void Update()
    {
        MoveHead();
        MoveTail();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            MoveCoin();
            AddTail();
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void MoveHead()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime); // 머리가 향하는 방향으로 이동
        transform.Rotate(Vector3.forward * moveInput.x, -turnSpeed * Time.deltaTime); // A, D키를 눌렀을 때 회전하는 기능
    }

    private void MoveTail()
    {
        
    }

    private void AddTail()
    {
        Vector3 spawnPosition = transform.position;
        if (tailPoints.Count > 0)
            spawnPosition = tailPoints[tailPoints.Count - 1].position;

        GameObject newTail = Instantiate(tailPrefab, spawnPosition, Quaternion.identity);
        tailPoints.Add(newTail.transform);
    }

    private void MoveCoin()
    {
        float randomX = Random.Range(-13f, 13f);
        float randomY = Random.Range(-4f, 4f);

        coinTransform.position = new Vector3(randomX, randomY, 0);
    }
}