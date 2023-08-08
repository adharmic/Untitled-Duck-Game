using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CannonProjectile : MonoBehaviour
{
    private Vector3 targetPosition;
    private Action onCannonBehaviorComplete;

    private void Update() {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float moveSpeed = 15f;
        transform.position += moveSpeed * Time.deltaTime * moveDirection;

        float reachedTargetDistance = .2f;
        if (Vector3.Distance(transform.position, targetPosition) < reachedTargetDistance) {
            float damageRadius = 4f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);
            foreach (Collider collider in colliderArray) {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit)) {
                    // TODO: Damage based on distance from center
                    targetUnit.Damage(30);
                }
            }
            Destroy(gameObject);
            onCannonBehaviorComplete();
        }
    }
    public void Setup(GridPosition targetGridPosition, Action onCannonBehaviorComplete) {
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        this.onCannonBehaviorComplete = onCannonBehaviorComplete;
    }
}
