using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAction : BaseAction
{

    [SerializeField] private Transform cannonProjectilePrefab;
    [SerializeField] private Transform shootPoint;
    private GridPosition gridPosition;
    [SerializeField] private int maxShootDistance = 4;
    private float stateTimer;
    private Vector3 targetPosition;
    private bool canShootBullet;
    [SerializeField] private float shootingTime = .1f;
    [SerializeField] private float cooldownTime = .5f;
    [SerializeField] private LayerMask obstaclesLayerMask;

    private enum State {
        Aiming,
        Shooting,
        Cooldown
    }
    private State state;
    private void Update() {
        if (!isActive) {
            return;
        }
        


        stateTimer -= Time.deltaTime;
        switch (state) {
            case State.Aiming:
                Aim();
                break;
            case State.Shooting:
                if (canShootBullet) {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooldown:
                break;
        }

        if (stateTimer <= 0f) {
            NextState();
        }
    }

    private void Shoot()
    {
        UnitAnimator animator = GetComponent<UnitAnimator>();
        animator.GetAnimator().SetTrigger("Shoot");
        Transform cannonProjectileTransform = Instantiate(cannonProjectilePrefab, shootPoint.position, Quaternion.identity);
        CannonProjectile cannonProjectile = cannonProjectileTransform.GetComponent<CannonProjectile>();
        cannonProjectile.Setup(gridPosition, OnCannonBehaviorComplete);
    }

    public override string GetActionName()
    {
        return "Cannon";
    }
    private void NextState() {
        switch (state) {
            case State.Aiming:
                state = State.Shooting;
                stateTimer = shootingTime;
                break;
            case State.Shooting:
                state = State.Cooldown;
                stateTimer = cooldownTime;
                break;
            case State.Cooldown:
                ActionComplete();
                break;
        }
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

                if (unitGridPosition == testGridPosition) {
                    continue;
                }

                float unitShoulderHeight = 1.7f;
                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDirection = (LevelGrid.Instance.GetWorldPosition(testGridPosition) - unitWorldPosition).normalized;
                if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, shootDirection, Vector3.Distance(unitWorldPosition, LevelGrid.Instance.GetWorldPosition(testGridPosition)), obstaclesLayerMask)) {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        this.gridPosition = gridPosition;
        // Debug.Log("GrenadeAction");
        state = State.Aiming;
        float aimingTime = .3f;
        stateTimer = aimingTime;
        canShootBullet = true;
        ActionStart(onActionComplete);
    }

    
    private void Aim() {
        Vector3 aimDirection = (targetPosition - unit.GetWorldPosition()).normalized;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
    }

    private void OnCannonBehaviorComplete() {
        ActionComplete();
    }
}
