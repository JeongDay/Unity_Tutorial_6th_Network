using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;

public class FightGameManager : MonoBehaviour
{
    void Start()
    {
        float randomX = Random.Range(-10, 11);
        float randomZ = Random.Range(-10, 11);
        var spawnPos =  new Vector3(randomX, 0, randomZ);

        int randomIndex = Random.Range(0, 4);
        string characterName = $"Player_{randomIndex}";
        
        GameObject character = PhotonNetwork.Instantiate(characterName, spawnPos, Quaternion.identity);
        
        CinemachineCamera followCamera = FindFirstObjectByType<CinemachineCamera>();
        followCamera.Follow = character.transform.GetChild(0);
    }
}