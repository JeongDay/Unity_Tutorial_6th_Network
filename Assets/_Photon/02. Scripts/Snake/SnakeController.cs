using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class SnakeController : MonoBehaviourPun
{
    public GameObject tailPrefab;
    public List<Transform> tailPoints = new List<Transform>();

    public Transform coinTransform;

    private Vector3 moveInput;
    public float moveSpeed = 5f;
    public float turnSpeed = 120f;
    public float lerpSpeed = 5f;

    private bool isCoin = false;

    void Start()
    {
        coinTransform = GameObject.FindGameObjectWithTag("Coin").transform;
    }

    void Update()
    {
        if (photonView.IsMine)
            MoveHead();
        
        MoveTail();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            if (photonView.IsMine && !isCoin)
            {
                isCoin = true;
                photonView.RPC("MoveCoin", RpcTarget.MasterClient);
            }
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
        Transform target = transform; // 처음에는 Target을 Snake로 설정

        foreach (Transform tail in tailPoints)
        {
            Vector3 pos = target.position;
            Quaternion rot = target.rotation;

            tail.position = Vector3.Lerp(tail.position, pos, lerpSpeed * Time.deltaTime);
            tail.rotation = Quaternion.Lerp(tail.rotation, rot, lerpSpeed * Time.deltaTime);

            target = tail; // 현재 꼬리를 Target 설정
        }
    }

    private void AddTail()
    {
        Vector3 spawnPosition = transform.position;
        if (tailPoints.Count > 0)
            spawnPosition = tailPoints[tailPoints.Count - 1].position;

        GameObject newTail = Instantiate(tailPrefab, spawnPosition, Quaternion.identity);
        tailPoints.Add(newTail.transform);

        newTail.GetComponent<Tail>().SetSnake(this);
    }

    [PunRPC]
    private void MoveCoin()
    {
        float randomX = Random.Range(-13f, 13f);
        float randomY = Random.Range(-4f, 4f);
        Vector3 pos = new Vector3(randomX, randomY, 0);

        photonView.RPC("SetCoinPosition", RpcTarget.AllBufferedViaServer, pos);
    }

    [PunRPC]
    private void SetCoinPosition(Vector3 newPos)
    {
        if (coinTransform == null)
            coinTransform = FindFirstObjectByType<Coin>().transform;

        coinTransform.position = newPos;
        
        if (photonView.IsMine)
            isCoin = false;

        AddTail();
    }
}