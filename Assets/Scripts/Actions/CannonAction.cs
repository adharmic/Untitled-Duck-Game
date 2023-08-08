using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAction : BaseAction
{

    [SerializeField] private Transform cannonProjectilePrefab;
    [SerializeField] private Transform shootPoint;
    private int maxShootDistance = 4;
    private void Update() {
        if (!isActive) {
            return;
        }
    }
    public override string GetActionName()
    {
        return "Cannon";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++) {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++) {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) {
                    continue;
                }

                if (!IsInRange(testGridPosition, unitGridPosition, maxShootDistance)) {
                    continue;
                }

                // if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) {
                //     continue;
                // }

                // Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                // if (targetUnit.IsEnemy() == unit.IsEnemy()) {
                //     continue;
                // }

                // // TODO: Extract this code to Unit script to check if a unit can see another unit
                // float unitShoulderHeight = 1.7f;
                // Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                // Vector3 shootDirection = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                // if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, shootDirection, Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()), obstaclesLayerMask)) {
                //     continue;
                // }

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Transform cannonProjectileTransform = Instantiate(cannonProjectilePrefab, shootPoint.position, Quaternion.identity);
        CannonProjectile cannonProjectile = cannonProjectileTransform.GetComponent<CannonProjectile>();
        cannonProjectile.Setup(gridPosition, OnCannonBehaviorComplete);
        Debug.Log("GrenadeAction");
        ActionStart(onActionComplete);
    }

    private void OnCannonBehaviorComplete() {
        ActionComplete();
    }
}
