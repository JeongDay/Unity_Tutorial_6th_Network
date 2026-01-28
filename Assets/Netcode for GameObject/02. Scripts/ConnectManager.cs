using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectManager : MonoBehaviour
{
    public Button hostButton;
    public Button serverButton;
    public Button clientButton;
    public Button closeButton;

    void Start()
    {
        // Button에 기능 등록
        hostButton.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
        serverButton.onClick.AddListener(() => NetworkManager.Singleton.StartServer());
        clientButton.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
        closeButton.onClick.AddListener(() => NetworkManager.Singleton.Shutdown());

        NetworkManager.Singleton.OnServerStarted += ServerStarted;
        NetworkManager.Singleton.OnServerStopped += ServerStopped;
        NetworkManager.Singleton.OnClientStarted += ClientStarted;
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