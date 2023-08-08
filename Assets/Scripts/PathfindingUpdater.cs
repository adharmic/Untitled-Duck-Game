using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PathfindingUpdater : MonoBehaviour
{
    private void Start() {
        DestructibleCrate.OnAnyDestroyed += DestructibleCrate_OnAnyDestroyed;
    }

    private void DestructibleCrate_OnAnyDestroyed(object sender, EventArgs e) {
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;
        GridPosition gridPosition = destructibleCrate.GetGridPosition();
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    }
}
