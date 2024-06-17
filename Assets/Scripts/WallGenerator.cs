using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPos = FindWallsInDirection(floorPositions, Direction2D.cardinalDirectionList);
        foreach(var position in basicWallPos)
        {
            tilemapVisualizer.PaintSingleBasicWall(position);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirection(HashSet<Vector2Int> floorPositions, List<Vector2Int> cardinalDirectionList)
    {
        HashSet<Vector2Int> wallPos = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in cardinalDirectionList)
            {
                var neighbourPos = position + direction;
                if(floorPositions.Contains(neighbourPos)==false)
                    wallPos.Add(neighbourPos);
            }
        }
        return wallPos;
    }
}
