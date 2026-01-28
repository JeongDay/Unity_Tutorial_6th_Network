using System;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CsNetworkManager : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;

    public TMP_InputField inputField;
    public Button sendButton;

    void Awake()
    {
        sendButton.onClick.AddListener(SendText);
    }
    
    void Start()
    {
        try
        {
            client = new TcpClient("127.0.0.1", 7979);
            stream = client.GetStream();
            
            Debug.Log("서버에 연결되었습니다.");
        }
        catch (Exception e)
        {
            Debug.Log($"연결 실패 : {e.Message}");
        }
    }

    private void SendText()
    {
        if (client == null || !client.Connected)
        {
            Debug.LogError("서버에 연결되어 있지 않습니다.");
            return;
        }

        string message = inputField.text;
        if (string.IsNullOrEmpty(message))
            return;

        SendMessageToServer(message);
        inputField.text = "";
    }

    private void SendMessageToServer(string message)
    {
        try
        {
            byte[] body = Encoding.UTF8.GetBytes(message);
            byte[] header = BitConverter.GetBytes((short)body.Length);

            byte[] fullPacket = new byte[header.Length + body.Length];
            Array.Copy(header, 0, fullPacket, 0, header.Length);
            Array.Copy(body, 0, fullPacket, header.Length, body.Length);

            stream.Write(fullPacket, 0, fullPacket.Length);
            Debug.Log($"전송 성공 : {message} ({body.Length} bytes)");
        }
        catch (Exception e)
        {
            Debug.Log($"전송 에러 : {e.Message}");
        }
    }

    void OnApplicationQuit()
    {
        stream?.Close();
        client?.Close();
    }
}