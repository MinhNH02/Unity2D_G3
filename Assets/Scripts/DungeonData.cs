using System.Collections.Generic;
using UnityEngine;

public class DungeonData
{
    public HashSet<Vector2Int> FloorTiles { get; private set; }
    public HashSet<Vector2Int> CorridorTiles { get; private set; }
    public Dictionary<Vector2Int, RoomType> RoomPositions { get; private set; }

    public DungeonData(HashSet<Vector2Int> floorTiles, HashSet<Vector2Int> corridorTiles, Dictionary<Vector2Int, RoomType> roomPositions)
    {
        FloorTiles = floorTiles;
        CorridorTiles = corridorTiles;
        RoomPositions = roomPositions;
    }
}
