using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GraphNode
{
    public GameObject Node;
    public List<int> AccessibleNodes;
    private List<int> shuffledNodes;

    public void Shuffle()
    {
        if (AccessibleNodes.Count > 0)
        {
            var list = new List<int>(AccessibleNodes);
            
            shuffledNodes = new List<int>();

            while (list.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, list.Count - 1);
                shuffledNodes.Add(list[index]);
                list.RemoveAt(index);
            }
        }
        else
        {
            shuffledNodes = AccessibleNodes;
        }
    }

    public List<int> GetAccessibleNodes() => AccessibleNodes;
    public List<int> GetShuffledNodes() => shuffledNodes;
    public GameObject GetNode() => Node;
}
