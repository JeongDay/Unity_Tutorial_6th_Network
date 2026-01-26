using Photon.Pun;
using UnityEngine;

public class SimpleGameManager : MonoBehaviour
{
    void Start()
    {
        int randomX = Random.Range(-5, 6);
        int randomZ = Random.Range(-5, 6);
        Vector3 spawnPos = new Vector3(randomX, 1, randomZ);
        
        PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
    }
}