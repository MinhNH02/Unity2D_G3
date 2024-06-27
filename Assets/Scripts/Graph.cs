using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    private List<Vector2Int> vertices;
    private Dictionary<Vector2Int, List<Vector2Int>> adjacencyList;

    private static List<Vector2Int> neighbours4directions = new List<Vector2Int>
    {
        new Vector2Int(0,1), //up
        new Vector2Int(1,0), //right
        new Vector2Int(0,-1), //down
        new Vector2Int(-1,0) //left
    };

    private static List<Vector2Int> neighbours8directions = new List<Vector2Int>
    {
        new Vector2Int(0,1), //up
        new Vector2Int(1,0), //right
        new Vector2Int(0,-1), //down
        new Vector2Int(-1,0), //left
        new Vector2Int(1,1), //diagonal
        new Vector2Int(1,-1), //diagonal
        new Vector2Int(-1,1), //diagonal
        new Vector2Int(-1,-1) //diagonal
    };

    List<Vector2Int> graph;

    public Graph(IEnumerable<Vector2Int> vertices)
    {
        if (vertices == null)
        {
            throw new ArgumentNullException(nameof(vertices), "Vertices collection cannot be null.");
        }

        graph = new List<Vector2Int>(vertices);
    }

    public List<Vector2Int> GetNeighbours4Directions(Vector2Int startPosition)
    {
        return GetNeighbours(startPosition, neighbours4directions);
    }

    public List<Vector2Int> GetNeighbours8Directions(Vector2Int startPosition)
    {
        return GetNeighbours(startPosition, neighbours8directions);
    }

    private List<Vector2Int> GetNeighbours(Vector2Int startPosition, List<Vector2Int> neighboursOffsetList)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();
        foreach (var neighbourDirection in neighboursOffsetList)
        {
            Vector2Int potentialNeighbour = startPosition + neighbourDirection;
            if (graph.Contains(potentialNeighbour))
                neighbours.Add(potentialNeighbour);
        }
        return neighbours;
    }

    public IEnumerable<Vector2Int> GetNodes()
    {
        return graph;
    }
}
