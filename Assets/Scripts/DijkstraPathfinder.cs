using System.Collections.Generic;
using UnityEngine;

public class DijkstraPathfinder
{
    private Dictionary<Vector2Int, float> distances = new Dictionary<Vector2Int, float>();
    private Dictionary<Vector2Int, Vector2Int?> previousNodes = new Dictionary<Vector2Int, Vector2Int?>();

    public DijkstraPathfinder(Graph graph, Vector2Int start)
    {
        CalculateDistances(graph, start);
    }

    private void CalculateDistances(Graph graph, Vector2Int start)
    {
        PriorityQueue<Vector2Int> priorityQueue = new PriorityQueue<Vector2Int>();
        foreach (var node in graph.GetNodes())
        {
            distances[node] = float.MaxValue;
            previousNodes[node] = null;
        }
        distances[start] = 0;
        priorityQueue.Enqueue(start, 0);

        while (priorityQueue.Count > 0)
        {
            var current = priorityQueue.Dequeue();
            foreach (var neighbor in graph.GetNeighbours4Directions(current))
            {
                float alt = distances[current] + Vector2Int.Distance(current, neighbor);
                if (alt < distances[neighbor])
                {
                    distances[neighbor] = alt;
                    previousNodes[neighbor] = current;
                    priorityQueue.Enqueue(neighbor, alt);
                }
            }
        }
    }

    public Vector2Int? GetFurthestPosition()
    {
        float maxDistance = float.MinValue;
        Vector2Int? furthestPosition = null;

        foreach (var node in distances)
        {
            if (node.Value > maxDistance)
            {
                maxDistance = node.Value;
                furthestPosition = node.Key;
            }
        }

        return furthestPosition;
    }

    public class PriorityQueue<T>
    {
        private List<KeyValuePair<T, float>> elements = new List<KeyValuePair<T, float>>();

        public int Count => elements.Count;

        public void Enqueue(T item, float priority)
        {
            elements.Add(new KeyValuePair<T, float>(item, priority));
        }

        public T Dequeue()
        {
            int bestIndex = 0;

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].Value < elements[bestIndex].Value)
                {
                    bestIndex = i;
                }
            }

            T bestItem = elements[bestIndex].Key;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }
    }
}
