using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

    protected virtual void Awake() {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    // TODO: Replace parameters with an extensible parameter class that has children corresponding to different actions.
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public bool IsValidActionGridPosition(GridPosition gridPosition) {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }
    
    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointsCost() {
        return 1;
    }

    protected void ActionStart(Action onActionComplete) {
        isActive = true;
        this.onActionComplete = onActionComplete;

        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete() {
        isActive = false;
        onActionComplete();

        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }
    

    public static bool IsInRange(GridPosition point, GridPosition center, int range) {
        int distanceX = center.x - point.x;
        int distanceZ = center.z - point.z;

        float distanceSquared = distanceX * distanceX + distanceZ * distanceZ;

        float radiusOffset = range + 0.5f;
        return distanceSquared <= radiusOffset * radiusOffset;
    }

    public Unit GetUnit() {
        return unit;
    }
}
