using UnityEngine;

public class SnakeGameManager : MonoBehaviour
{
    public GameObject snakePrefab;
    public GameObject coinPrefab;

    void Start()
    {
        Instantiate(snakePrefab, RandomPosition(), Quaternion.identity);
        Instantiate(coinPrefab, RandomPosition(), Quaternion.identity);
    }

    private Vector3 RandomPosition()
    {
        float ranX = Random.Range(-13f, 13f);
        float ranY = Random.Range(-4f, 4f);

        return new Vector3(ranX, ranY, 0);
    }
}