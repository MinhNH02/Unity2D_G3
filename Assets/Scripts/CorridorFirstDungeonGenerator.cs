using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CorridorFirstDungeonGenerator : DungeonGenerator
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject bossPrefab;
    [SerializeField]
    private List<GameObject> itemPrefabs;
    [SerializeField]
    private int enemyRoomCount = 2;
    [SerializeField]
    private int treasureRoomCount = 2;
    [SerializeField]
    private int itemCount = 5;

    private Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary = new Dictionary<Vector2Int, HashSet<Vector2Int>>();
    private HashSet<Vector2Int> floorPositions, corridorPositions;
    public enum RoomType
    {
        PlayerRoom,
        BossRoom,
        EnemyRoom,
        TreasureRoom,
        RegularRoom
    }
    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
        PlaceSpecialRooms();
        PlacePlayerAndItems();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPos = new HashSet<Vector2Int>();

        CreateCorridors(floorPos, potentialRoomPos);
        HashSet<Vector2Int> roomPos = CreateRooms(potentialRoomPos);

        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPos);
        CreateRoomsAtDeadEnd(deadEnds, roomPos);

        floorPos.UnionWith(roomPos);

        foreach (var corridor in corridorPositions)
        {
            var increasedCorridor = IncreaseCorridorSizeByOne(new List<Vector2Int> { corridor });
            floorPos.UnionWith(increasedCorridor);
        }

        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPos, tilemapVisualizer.GetFloorTile);
        WallGenerator.CreateWalls(floorPos, tilemapVisualizer);

        floorPositions = floorPos;
    }

    private void PlaceSpecialRooms()
    {
        Graph dungeonGraph = new Graph(floorPositions);
        DijkstraPathfinder dijkstra = new DijkstraPathfinder(dungeonGraph, startPos);

        // Place the player room at the starting position
        PlaceRoom(startPos, playerPrefab, RoomType.PlayerRoom);

        // Place the boss room at the furthest position
        Vector2Int? bossRoomPos = dijkstra.GetFurthestPosition();
        if (bossRoomPos.HasValue)
        {
            PlaceRoom(bossRoomPos.Value, bossPrefab, RoomType.BossRoom);
            dijkstra = new DijkstraPathfinder(dungeonGraph, bossRoomPos.Value);
        }

        // Place enemy rooms
        for (int i = 0; i < enemyRoomCount; i++)
        {
            Vector2Int? enemyRoomPos = dijkstra.GetFurthestPosition();
            if (enemyRoomPos.HasValue)
            {
                PlaceRoom(enemyRoomPos.Value, null, RoomType.EnemyRoom);
                dijkstra = new DijkstraPathfinder(dungeonGraph, enemyRoomPos.Value);
            }
        }

        // Place treasure rooms
        for (int i = 0; i < treasureRoomCount; i++)
        {
            Vector2Int? treasureRoomPos = dijkstra.GetFurthestPosition();
            if (treasureRoomPos.HasValue)
            {
                PlaceRoom(treasureRoomPos.Value, null, RoomType.TreasureRoom);
                dijkstra = new DijkstraPathfinder(dungeonGraph, treasureRoomPos.Value);
            }
        }
    }

    private void PlaceRoom(Vector2Int position, GameObject prefab, RoomType roomType)
    {
        if (prefab != null)
        {
            Instantiate(prefab, (Vector3Int)position, Quaternion.identity);
        }
        // Optionally update your dungeon data to mark the room type
        // Example: dungeonData.AddRoom(position, roomType);
    }

    private void PlacePlayerAndItems()
    {
        // Use ItemPlacementHelper to place items logically
        ItemPlacementHelper itemPlacementHelper = new ItemPlacementHelper(floorPositions, floorPositions);

        // Place items
        for (int i = 0; i < itemCount; i++)
        {
            Vector2? itemPosition = itemPlacementHelper.GetItemPlacementPosition(PlacementType.OpenSpace, 10, new Vector2Int(1, 1), false);
            if (itemPosition.HasValue)
            {
                int itemIndex = UnityEngine.Random.Range(0, itemPrefabs.Count);
                Instantiate(itemPrefabs[itemIndex], (Vector3)itemPosition.Value, Quaternion.identity);
            }
        }
    }

    private List<Vector2Int> IncreaseCorridorBrush3By3(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        for (int i = 1; i < corridor.Count; i++)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                }
            }
        }
        return newCorridor;
    }

    private List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        Vector2Int previousDirection = Vector2Int.zero;
        for (int i = 1; i < corridor.Count; i++)
        {
            Vector2Int directionFromCell = corridor[i] - corridor[i - 1];
            if (previousDirection != Vector2Int.zero &&
                directionFromCell != previousDirection)
            {
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                    }
                }
                previousDirection = directionFromCell;
            }
            else
            {
                Vector2Int newCorridorTileOffset = GetDirection90From(directionFromCell);
                newCorridor.Add(corridor[i - 1]);
                newCorridor.Add(corridor[i - 1] + newCorridorTileOffset);
            }
        }
        return newCorridor;
    }

    private Vector2Int GetDirection90From(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
            return Vector2Int.right;
        if (direction == Vector2Int.right)
            return Vector2Int.down;
        if (direction == Vector2Int.down)
            return Vector2Int.left;
        if (direction == Vector2Int.left)
            return Vector2Int.up;
        return Vector2Int.zero;
    }



    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if (roomFloors.Contains(position) == false)
            {
                var room = RunRandomWalk(randomWalkParameter, position);
                roomFloors.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPos)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var position in floorPos)
        {
            int neeighboursCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionList)
            {
                if (floorPos.Contains(position + direction))
                    neeighboursCount++;
            }
            if (neeighboursCount == 1)
                deadEnds.Add(position);
        }
        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPos)
    {
        HashSet<Vector2Int> roomPos = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPos.Count * roomPercent);
        List<Vector2Int> roomsToCreate = potentialRoomPos.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();
        ClearRoomData();
        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameter, roomPosition);
            SaveRoomData(roomPosition, roomFloor);
            roomPos.UnionWith(roomFloor);
        }
        return roomPos;
    }

    private void ClearRoomData()
    {
        roomsDictionary.Clear();
    }

    private void SaveRoomData(Vector2Int roomPos, HashSet<Vector2Int> roomFloor)
    {
        roomsDictionary[roomPos] = roomFloor;

    }

    private void CreateCorridors(HashSet<Vector2Int> floorPos, HashSet<Vector2Int> potentialRoomPos)
    {
        var currentPos = startPos;
        potentialRoomPos.Add(currentPos);

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPos, corridorLength);
            currentPos = corridor[corridor.Count - 1];
            potentialRoomPos.Add(currentPos);
            floorPos.UnionWith(corridor);
        }
        corridorPositions = new HashSet<Vector2Int>(floorPos);
    }
}
