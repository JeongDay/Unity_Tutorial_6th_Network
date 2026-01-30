using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    public TextMeshProUGUI scoreText;

    private NetworkVariable<int> networkScore = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        networkScore.OnValueChanged += SetScore;
    }

    public void AddScore()
    {
        networkScore.Value++;
    }

    private void SetScore(int prevValue, int newValue)
    {
        scoreText.text = $"현재 획득한 광물의 수 : {newValue}";
    }
}