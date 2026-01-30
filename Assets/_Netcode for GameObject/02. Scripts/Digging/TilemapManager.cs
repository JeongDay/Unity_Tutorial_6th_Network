using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : NetworkBehaviour
{
    public Tilemap tilemap;

    public GameObject[] minerals;

    private NetworkList<Vector3Int> destroyedTiles = new NetworkList<Vector3Int>();
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        tilemap = GetComponent<Tilemap>();

        destroyedTiles.OnListChanged += OnTileDestroyed;

        foreach (Vector3Int pos in destroyedTiles)
            tilemap.SetTile(pos, null);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemoveTileServerRpc(Vector3Int cellPos)
    {
        RemoveTile(cellPos);
    }
    
    public void RemoveTile(Vector3Int cellPos)
    {
        int randomValue = Random.Range(0, 101);

        if (randomValue >= 70) // 드롭율 30퍼센트
        {
            int randomIndex = Random.Range(0, minerals.Length);
            GameObject mineral = Instantiate(minerals[randomIndex], cellPos, Quaternion.identity);

            mineral.GetComponent<NetworkObject>().Spawn(); // 동기화
        }

        destroyedTiles.Add(cellPos);
    }

    private void OnTileDestroyed(NetworkListEvent<Vector3Int> changedEvent)
    {
        if (changedEvent.Type == NetworkListEvent<Vector3Int>.EventType.Add)
            tilemap.SetTile(changedEvent.Value, null);
    }
}