using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScrolling : MonoBehaviour
{
    Transform playerTransform;
    private Vector2Int currentTilePosition = new Vector2Int(0,0);
    [SerializeField] private Vector2Int playerTilePosition;
    private Vector2Int onTileGridPlayerPosition;
    [SerializeField] float tileSize = 20f;
    private GameObject[,] terrainTiles;

    [SerializeField] private int terrainTileHorizontalCount;
    [SerializeField] private int terrainTileVerticalCount;

    [SerializeField] private int fieldOfVisionHeight = 3;
    [SerializeField] private int fieldOfVisionWidth = 3;

    private void Awake()
    {
        terrainTiles = new GameObject[terrainTileHorizontalCount, terrainTileVerticalCount];
    }

    private void Start()
    {
        UpdateTileOnScreen(); 
        playerTransform = GameManager.instance.playerTranform;
    }

    private void Update()
    {
        playerTilePosition.x = (int)(playerTransform.position.x / tileSize);
        playerTilePosition.y = (int)(playerTransform.position.y / tileSize);

        playerTilePosition.x -= playerTransform.position.x < 0 ? 1 : 0;
        playerTilePosition.y -= playerTransform.position.y < 0 ? 1 : 0;

        if (currentTilePosition != playerTilePosition)
        {
            currentTilePosition = playerTilePosition;

            onTileGridPlayerPosition.x = CalculatePositionOnAxis(onTileGridPlayerPosition.x, true);
            onTileGridPlayerPosition.y = CalculatePositionOnAxis(onTileGridPlayerPosition.y, false);
            UpdateTileOnScreen();
        }
    }

    private void UpdateTileOnScreen()
    {
        for (int i = -(fieldOfVisionWidth / 2); i <= fieldOfVisionWidth/2; i++)
        {
            for (int j = -(fieldOfVisionHeight / 2); j <= fieldOfVisionHeight/2; j++)
            {
                int tileToUpdate_x = CalculatePositionOnAxis(playerTilePosition.x + i, true);
                int tileToUpdate_y = CalculatePositionOnAxis(playerTilePosition.y + j, false);

                GameObject tile = terrainTiles[tileToUpdate_x, tileToUpdate_y];
                Vector3 newPosition = CalculateTilePosition(playerTilePosition.x + i, playerTilePosition.y + j);
                if(newPosition != tile.transform.position)
                {
                    tile.transform.position = newPosition;
                    terrainTiles[tileToUpdate_x, tileToUpdate_y].GetComponent<TerrainTile>().Spawn();
                }
            }
        }
    }

    private Vector3 CalculateTilePosition(int v1, int v2)
    {
        return new Vector3(v1 * tileSize, v2 * tileSize);
    }

    private int CalculatePositionOnAxis(float currentValue, bool horizontal)
    {
        if (horizontal)
        {
            if (currentValue >= 0)
            {
                currentValue = currentValue % terrainTileHorizontalCount;
            }
            else
            {
                currentValue += 1;
                currentValue = terrainTileHorizontalCount - 1 + currentValue % terrainTileHorizontalCount;
            }
        }
        else
        {
            if (currentValue >= 0)
            {
                currentValue = currentValue % terrainTileVerticalCount;
            }
            else
            {
                currentValue += 1;
                currentValue = terrainTileVerticalCount - 1 + currentValue % terrainTileVerticalCount;
            }
        }
        return (int)currentValue;
    }

    public void Add(GameObject tileGameObject, Vector2Int tilePosition)
    {
        terrainTiles[tilePosition.x, tilePosition.y] = tileGameObject;
    }
}
