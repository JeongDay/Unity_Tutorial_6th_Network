using System.Collections;
using UnityEngine;

public class FinderGameManager : MonoBehaviour
{
    public static FinderGameManager Instance;

    public GameObject npcPrefab;

    public Transform[] spawnPoints;
    public int spawnNpcAmount = 10;

    void Awake()
    {
        Instance = this;
    }

    IEnumerator Start()
    {
        for (int i = 0; i < spawnNpcAmount; i++)
        {
            GameObject npc = Instantiate(npcPrefab);
            npc.transform.position = RandomPosition();
            yield return null;
        }
    }

    public Vector3 RandomPosition()
    {
        int index = Random.Range(0, spawnPoints.Length);
        return spawnPoints[index].position;
    }
}