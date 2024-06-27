using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    PlayerRoom,
    BossRoom,
    EnemyRoom,
    TreasureRoom,
    RegularRoom,
    PropRoom
}


public class RoomTypeData
{
    public Vector2Int Position { get; set; }
    public RoomType RoomType { get; set; }

    public RoomTypeData(Vector2Int position, RoomType roomType)
    {
        Position = position;
        RoomType = roomType;
    }
}
