using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SnakeGameManager : MonoBehaviourPunCallbacks
{
    public GameObject snakePrefab;
    public GameObject coinPrefab;

    void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
        PhotonNetwork.GameVersion = "1";
    }

    void Start()
    {
        Connect();
    }

    private void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 20 }, null);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Instantiate(coinPrefab.name, RandomPosition(), Quaternion.identity);
        
        PhotonNetwork.Instantiate(snakePrefab.name, RandomPosition(), Quaternion.identity);
    }

    private Vector3 RandomPosition()
    {
        float ranX = Random.Range(-13f, 13f);
        float ranY = Random.Range(-4f, 4f);

        return new Vector3(ranX, ranY, 0);
    }
}