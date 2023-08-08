using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;
    private int width;
    private int height;
    private float cellSize;
    private GridSystem<PathNode> gridSystem;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There's more than one Pathfinding System! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }

    public void Setup(int width, int height, float cellSize) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSystem = new GridSystem<PathNode>(width, height, cellSize, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        // gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                GridPosition gridPosition = new(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 5f;
                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up, raycastOffsetDistance * 2, obstaclesLayerMask)) {
                    GetNode(gridPosition).SetIsWalkable(false);
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength) {
        List<PathNode> openList = new();
        List<PathNode> closedList = new();

        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);

        for (int x = 0; x < gridSystem.GetWidth(); x++) {
            for (int z = 0; z < gridSystem.GetHeight(); z++) {
                GridPosition gridPosition = new(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetPreviousPathNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0) {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            if (currentNode == endNode) {
                pathLength = endNode.GetFCost();
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighborNode in GetNeighborList(currentNode)) {
                if (closedList.Contains(neighborNode)) {
                    continue;
                }

                if (!neighborNode.IsWalkable()) {
                    closedList.Add(neighborNode);
                    continue;
                }

                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighborNode.GetGridPosition());

                if (tentativeGCost < neighborNode.GetGCost()) {
                    neighborNode.SetPreviousNode(currentNode);
                    neighborNode.SetGCost(tentativeGCost);
                    neighborNode.SetHCost(CalculateDistance(neighborNode.GetGridPosition(), endGridPosition));
                    neighborNode.CalculateFCost();

                    if (!openList.Contains(neighborNode)) {
                        openList.Add(neighborNode);
                    }
                }
            }
        }
        pathLength = 0;
        return null;
    }

    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB) {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList) {
        PathNode lowestFCostPathNode = pathNodeList[0];
        foreach (PathNode pathNode in pathNodeList) {
            if (pathNode.GetFCost() < lowestFCostPathNode.GetFCost()) {
                lowestFCostPathNode = pathNode;
            }
        }
        return lowestFCostPathNode;
    }

    private List<PathNode> GetNeighborList(PathNode currentNode) {
        List<PathNode> neighborList = new();

        GridPosition gridPosition = currentNode.GetGridPosition();

        if(gridSystem.IsValidGridPosition(gridPosition.North())) {
            neighborList.Add(gridSystem.GetGridObject(gridPosition.North()));
        }
        
        if(gridSystem.IsValidGridPosition(gridPosition.South())) {
            neighborList.Add(gridSystem.GetGridObject(gridPosition.South()));
        }
        
        if(gridSystem.IsValidGridPosition(gridPosition.East())) {
            neighborList.Add(gridSystem.GetGridObject(gridPosition.East()));
        }
        
        if(gridSystem.IsValidGridPosition(gridPosition.West())) {
            neighborList.Add(gridSystem.GetGridObject(gridPosition.West()));
        }

        return neighborList;
    }

    private PathNode GetNode(GridPosition gridPosition) {
        return gridSystem.GetGridObject(gridPosition);
    }
    
    private List<GridPosition> CalculatePath(PathNode endNode) {
        List<PathNode> pathNodeList = new() {endNode};
        PathNode currentNode = endNode;
        while (currentNode.GetPreviousNode() != null) {
            pathNodeList.Add(currentNode.GetPreviousNode());
            currentNode = currentNode.GetPreviousNode();
        }
        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new();
        foreach (PathNode pathNode in pathNodeList) {
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;
    }

    public bool IsWalkableGridPosition(GridPosition gridPosition) {
        return gridSystem.GetGridObject(gridPosition).IsWalkable();
    }
    public void SetIsWalkableGridPosition(GridPosition gridPosition, bool isWalkable) {
        gridSystem.GetGridObject(gridPosition).SetIsWalkable(isWalkable);
    }

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition) {
        return FindPath(startGridPosition, endGridPosition, out _) != null;
    }

    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition) {
        FindPath(startGridPosition, endGridPosition, out int pathLength);
        return pathLength;
    }
}
