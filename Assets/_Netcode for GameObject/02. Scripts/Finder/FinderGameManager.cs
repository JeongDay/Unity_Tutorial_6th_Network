using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class FinderGameManager : NetworkBehaviour
{
    public static FinderGameManager Instance;

    public GameObject npcPrefab;

    public Transform[] spawnPoints;
    public int spawnNpcAmount = 10;
    
    public Button hostButton;
    public Button serverButton;
    public Button clientButton;
    public Button closeButton;

    void Awake()
    {
        Instance = this;
        
        hostButton.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
        serverButton.onClick.AddListener(() => NetworkManager.Singleton.StartServer());
        clientButton.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
        closeButton.onClick.AddListener(() => NetworkManager.Singleton.Shutdown());

        NetworkManager.Singleton.OnServerStarted += ServerStarted;
        NetworkManager.Singleton.OnServerStopped += ServerStopped;
        NetworkManager.Singleton.OnClientStarted += ClientStarted;
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void NpcSpawnServerRpc()
    {
        for (int i = 0; i < spawnNpcAmount; i++)
        {
            GameObject npc = Instantiate(npcPrefab); // 캐릭터 생성
            
            int index = Random.Range(0, spawnPoints.Length);
            var randomPos = spawnPoints[index].position;

            npc.transform.position = randomPos; // 랜덤 위치 적용

            npc.GetComponent<NetworkObject>().Spawn(); // 동기화
        }
    }
    
    private void ServerStarted()
    {
        Debug.Log("서버 시작");
    }

    private void ServerStopped(bool isBool)
    {
        Debug.Log("서버 종료");
    }
    
    private void ClientStarted()
    {
        Debug.Log("클라이언트 접속");
    }
}