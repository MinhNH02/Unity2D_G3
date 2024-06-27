using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random; // Explicitly specify UnityEngine.Random

public class RoomFirstDungeonGenerator : DungeonGenerator
{
    [SerializeField] private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField] private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField] private int offset = 1;
    [SerializeField] private bool randomWalkRooms = false;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject[] propsPrefabs;
    [SerializeField] private GameObject chestPrefab;
    [SerializeField] private int corridorWidth = 2;

    private Vector2Int spawnRoomCenter;
    private Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary = new Dictionary<Vector2Int, HashSet<Vector2Int>>();
    private HashSet<Vector2Int> roomFloorNoCorridor;

    private void Start()
    {
        GenerateDungeon();
    }

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
        PlaceItemsAndEntities();
    }

    private void CreateRooms()
    {
        var roomList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(
            new BoundsInt((Vector3Int)startPos, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        // Debug: Check room list
        Debug.Log($"Generated {roomList.Count} rooms");

        HashSet<Vector2Int> floor = randomWalkRooms ? CreateRoomsRandomly(roomList) : CreateSimpleRooms(roomList);

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        if (roomCenters.Count > 0)
        {
            spawnRoomCenter = roomCenters[0];
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);


        Func<Vector2Int, TileBase> getTile = tilemapVisualizer.GetFloorTile;
        tilemapVisualizer.PaintFloorTiles(floor, getTile);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);

        // Store room data for item placement
        roomFloorNoCorridor = new HashSet<Vector2Int>();
        foreach (var room in roomList)
        {
            var roomFloor = new HashSet<Vector2Int>();
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    roomFloor.Add(position);
                    roomFloorNoCorridor.Add(position);
                }
            }
            roomsDictionary[(Vector2Int)Vector3Int.RoundToInt(room.center)] = roomFloor;
        }

        // Debug: Check floor and corridor tile counts
        Debug.Log($"Total floor tiles: {floor.Count}, Total corridor tiles: {corridors.Count}");
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomList.Count; i++)
        {
            var roomBounds = roomList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameter, roomCenter);
            foreach (var position in roomFloor)
            {
                if (position.x >= (roomBounds.xMin + offset)
                    && position.x <= (roomBounds.xMax - offset)
                    && position.y >= (roomBounds.yMin - offset)
                    && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
        }
        // Debug: Check floor tiles count
        Debug.Log($"Total floor tiles from random walk rooms: {floor.Count}");
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        // Debug: Check corridor tiles count
        Debug.Log($"Total corridor tiles: {corridors.Count}");
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);

        // Move vertically
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
                position += Vector2Int.up;
            else if (destination.y < position.y)
                position += Vector2Int.down;

            AddCorridorWidth(position, corridor);
        }

        // Move horizontally
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
                position += Vector2Int.right;
            else if (destination.x < position.x)
                position += Vector2Int.left;

            AddCorridorWidth(position, corridor);
        }

        return corridor;
    }

    private void AddCorridorWidth(Vector2Int position, HashSet<Vector2Int> corridor)
    {
        for (int i = -corridorWidth / 2; i <= corridorWidth / 2; i++)
        {
            for (int j = -corridorWidth / 2; j <= corridorWidth / 2; j++)
            {
                Vector2Int newPosition = position + new Vector2Int(i, j);
                corridor.Add(newPosition);
            }
        }
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        // Debug: Check floor tiles count for simple rooms
        Debug.Log($"Total floor tiles from simple rooms: {floor.Count}");
        return floor;
    }

    private void PlaceItemsAndEntities()
    {
        if (roomFloorNoCorridor == null || roomFloorNoCorridor.Count == 0)
        {
            Debug.LogError("roomFloorNoCorridor is null or empty.");
            return;
        }

        ItemPlacementHelper itemPlacementHelper = new ItemPlacementHelper(roomFloorNoCorridor, roomFloorNoCorridor);

        // Example: Place player in the spawn room
        PlacePlayer();

        // Example: Place enemies in enemy rooms
        PlaceEnemies(itemPlacementHelper);

        // Example: Place chests in treasure rooms
        PlaceChests(itemPlacementHelper);

        // Example: Place props in various rooms
        PlaceProps(itemPlacementHelper);
    }

    private void PlacePlayer()
    {
        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, new Vector3(spawnRoomCenter.x, spawnRoomCenter.y, 0), Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Player Prefab is not assigned.");
        }
    }

    private void PlaceEnemies(ItemPlacementHelper itemPlacementHelper)
    {
        if (enemyPrefab != null)
        {
            foreach (var roomData in roomsDictionary)
            {
                if (roomData.Key != spawnRoomCenter)  // Example condition: Place enemies in all rooms except spawn room
                {
                    Vector2? enemyPosition = itemPlacementHelper.GetItemPlacementPosition(PlacementType.NearWall, 100, new Vector2Int(1, 1), true);
                    if (enemyPosition.HasValue)
                    {
                        Instantiate(enemyPrefab, new Vector3(enemyPosition.Value.x, enemyPosition.Value.y, 0), Quaternion.identity);
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("Enemy Prefab is not assigned.");
        }
    }

    private void PlaceChests(ItemPlacementHelper itemPlacementHelper)
    {
        if (chestPrefab != null)
        {
            foreach (var roomData in roomsDictionary)
            {
                Vector2? chestPosition = itemPlacementHelper.GetItemPlacementPosition(PlacementType.OpenSpace, 100, new Vector2Int(1, 1), true);
                if (chestPosition.HasValue)
                {
                    Instantiate(chestPrefab, new Vector3(chestPosition.Value.x, chestPosition.Value.y, 0), Quaternion.identity);
                }
            }
        }
        else
        {
            Debug.LogWarning("Chest Prefab is not assigned.");
        }
    }

    private void PlaceProps(ItemPlacementHelper itemPlacementHelper)
    {
        if (propsPrefabs != null)
        {
            foreach (var propPrefab in propsPrefabs)
            {
                foreach (var roomData in roomsDictionary)
                {
                    Vector2? propPosition = itemPlacementHelper.GetItemPlacementPosition(PlacementType.OpenSpace, 100, new Vector2Int(1, 1), true);
                    if (propPosition.HasValue)
                    {
                        Instantiate(propPrefab, new Vector3(propPosition.Value.x, propPosition.Value.y, 0), Quaternion.identity);
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("Props Prefabs array is not assigned.");
        }
    }
}
