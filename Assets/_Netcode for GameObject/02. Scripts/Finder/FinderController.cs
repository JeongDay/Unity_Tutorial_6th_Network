using System;
using System.Collections;
using StarterAssets;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class FinderController : NetworkBehaviour
{
    private ThirdPersonController controller;
    private Animator anim;

    public GameObject punchHitbox;
    public GameObject kickHitbox;

    private bool isDead = false;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        anim = GetComponent<Animator>();
        controller = GetComponent<ThirdPersonController>();

        if (IsOwner)
        {
            var camera = FindFirstObjectByType<CinemachineCamera>();
            camera.Follow = transform.GetChild(0).transform; // PlayerCameraRoot

            StartCoroutine(DelayRoutine());
        }
        else
            GetComponent<PlayerInput>().enabled = false;
    }

    IEnumerator DelayRoutine()
    {
        yield return new WaitUntil(() => FinderGameManager.Instance != null && FinderGameManager.Instance.IsSpawned);

        // 플레이어의 위치 랜덤 설정
        var spawnPoints = FinderGameManager.Instance.spawnPoints;
        int randomIndex = Random.Range(0, spawnPoints.Length);
        transform.position = spawnPoints[randomIndex].transform.position;

        FinderGameManager.Instance.NpcSpawnServerRpc(); // NPC 생성
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead)
            return;

        var hitbox = other.GetComponent<Hitbox>();
        if (hitbox != null)
        {
            GetHit();
        }
    }

    void OnPunch(InputValue value)
    {
        anim.SetTrigger("Punch");
        StartCoroutine(PunchRoutine());
    }

    IEnumerator PunchRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        punchHitbox.SetActive(true);

        yield return new WaitForSeconds(0.3f);
        punchHitbox.SetActive(false);
    }

    void OnKick(InputValue value)
    {
        anim.SetTrigger("Kick");
        StartCoroutine(KickRoutine());
    }

    IEnumerator KickRoutine()
    {
        yield return new WaitForSeconds(0.6f);
        kickHitbox.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        kickHitbox.SetActive(false);
    }

    private void GetHit()
    {
        isDead = true;
        anim.SetTrigger("Death");
        controller.enabled = false;
    }
}