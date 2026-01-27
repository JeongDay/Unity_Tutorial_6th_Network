using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SimpleGameManager : MonoBehaviourPunCallbacks
{
    void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
        PhotonNetwork.SendRate = 60; // 내 컴퓨터 게임 정보에 대한 전송률
        PhotonNetwork.SerializationRate = 30; // PhotonView에서 관측 중인 대상의 전송률

        PhotonNetwork.GameVersion = "1";
    }

    void Start()
    {
        Connect();
    }

    private void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("서버 접속");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 20 }, null);
        Debug.Log("서버 접속 완료");
    }

    public override void OnJoinedRoom()
    {
        int randomX = Random.Range(-5, 6);
        int randomZ = Random.Range(-5, 6);
        Vector3 spawnPos = new Vector3(randomX, 1, randomZ);
        
        PhotonNetwork.Instantiate("Player_1", spawnPos, Quaternion.identity);
    }
}