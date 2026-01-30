using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class MineralEvent : NetworkBehaviour
{
    public ScoreManager scoreManager;
    private bool isDrop = false;

    IEnumerator Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();

        isDrop = false;
        yield return new WaitForSeconds(1f);

        isDrop = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isDrop)
        {
            Debug.Log("광물 획득");
            GetMineralServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void GetMineralServerRpc()
    {
        scoreManager.AddScore();
        GetComponent<NetworkObject>().Despawn(true);
    }
}