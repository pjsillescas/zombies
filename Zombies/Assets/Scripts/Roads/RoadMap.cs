using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMap : MonoBehaviour
{
    public static RoadMap Instance = null;

    [SerializeField] public List<GraphNode> Graph;

    public class Route
	{
        public List<Transform> Positions;
        public int LastPositionIndex;

        public Route(List<Transform> positions, int lastPositionIndex)
		{
            Positions = positions;
            LastPositionIndex = lastPositionIndex;
		}
	}

    public class NodePosition
	{
        public Transform Position;
        public int Index;

        public NodePosition(int index,Transform position)
		{
            Index = index;
            Position = position;
		}
	}

    // Start is called before the first frame update
    void Start()
    {
        if(Instance != null)
		{
            Debug.LogError("There is another road map");
            return;
		}

        Instance = this;

        var path = FindPath(0, 10);

        Debug.Log(string.Join(" => ",path.ConvertAll(transform => transform.name)));
        Debug.Log("length " + path.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Route GetRoute(int sourceIndex)
	{
        int lastPositionIndex = Random.Range(0, Graph.Count - 1);

        if(lastPositionIndex == sourceIndex)
		{
            lastPositionIndex = (lastPositionIndex + 1) % Graph.Count;
        }

        var positions = FindPath(sourceIndex,lastPositionIndex);
        return new Route(positions,lastPositionIndex);
	}

    public NodePosition GetRandomPosition()
	{
        int index = Random.Range(0, Graph.Count - 1);
        return new NodePosition(index, Graph[index].GetNode().transform);
	}

    public List<Transform> FindPath(int startNodeIndex, int endNodeIndex)
    {
        bool[] isVisited = new bool[Graph.Count];
        List<int> pathList = new();

        pathList.Add(startNodeIndex);
        Graph.ForEach(obj => obj.Shuffle());

        var indices = FindPathRecursive(startNodeIndex, endNodeIndex, isVisited, ref pathList);

        return GetTransformPath(indices);
    }

    public List<Transform> GetTransformPath(List<int> indicesPath)
    {
        List<Transform> path = new();

        foreach(int index in indicesPath)
		{
            path.Add(Graph[index].GetNode().transform);
		}

        return path;
    }

    private List<int> FindPathRecursive(int currentNodeIndex, int destinationNodeIndex, bool[] isVisited, ref List<int> localPathList)
    {
        if (currentNodeIndex == destinationNodeIndex)
        {
            return localPathList;
        }

        isVisited[currentNodeIndex] = true;

        var neighbours = Graph[currentNodeIndex].GetShuffledNodes();
        foreach (int neighbourNodeIndex in neighbours)
        {
            if (!isVisited[neighbourNodeIndex])
            {
                localPathList.Add(neighbourNodeIndex);
                var finalList = FindPathRecursive(neighbourNodeIndex, destinationNodeIndex, isVisited,ref localPathList);

                if(finalList != null)
				{
                    return finalList;
				}

                localPathList.Remove(neighbourNodeIndex);
            }
        }

        isVisited[currentNodeIndex] = false;
        return null;
    }
}
