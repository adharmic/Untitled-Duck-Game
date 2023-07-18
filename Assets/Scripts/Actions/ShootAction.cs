using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{

    private enum State {
        Aiming,
        Shooting,
        Cooldown
    }
    private State state;
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs {
        public Unit targetUnit;
        public Unit shootingUnit;
    }
    private int maxShootDistance = 4;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;
    [SerializeField] private FireProjectileEvent fireProjectileEvent;
    private void Start() {
        fireProjectileEvent.OnFireProjectile += FireProjectileEvent_OnFireProjectile;
    }
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

    private void NextState() {
        switch (state) {
            case State.Aiming:
                state = State.Shooting;
                float shootingTime = 0.1f;
                stateTimer = shootingTime;
                break;
            case State.Shooting:
                state = State.Cooldown;
                float cooldownTime = 0.5f;
                stateTimer = cooldownTime;
                break;
            case State.Cooldown:
                ActionComplete();
                break;
        }
    }
    public override string GetActionName()
    {
        return "Shoot";
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

                if (!IsInShootingRange(testGridPosition, unitGridPosition)) {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) {
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == unit.IsEnemy()) {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    private void Shoot() {
        OnShoot?.Invoke(this, new OnShootEventArgs{targetUnit = targetUnit, shootingUnit = unit});
    }

    private void Aim() {
        Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
    }

    private bool IsInShootingRange(GridPosition point, GridPosition center) {
        int distanceX = center.x - point.x;
        int distanceZ = center.z - point.z;

        float distanceSquared = distanceX * distanceX + distanceZ * distanceZ;

        float radiusOffset = maxShootDistance + 0.5f;
        return distanceSquared <= radiusOffset * radiusOffset;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float aimingTime = 1f;
        stateTimer = aimingTime;

        canShootBullet = true;
    }

    

    private void FireProjectileEvent_OnFireProjectile(object sender, EventArgs e) {
        targetUnit.Damage();
    }
}
