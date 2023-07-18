using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

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
}
