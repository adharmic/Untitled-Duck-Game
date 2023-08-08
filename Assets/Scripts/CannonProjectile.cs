using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CannonProjectile : MonoBehaviour
{
    private Vector3 targetPosition;
    private Action onCannonBehaviorComplete;
    public static event EventHandler OnAnyCannonExploded;
    [SerializeField] private Transform cannonExplodeVFXPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve parabolaAnimationCurve;

    private float totalDistance;
    private Vector3 positionXZ;
    private void Update() {
        Vector3 moveDirection = (targetPosition - positionXZ).normalized;
        
        float moveSpeed = 15f;
        positionXZ += moveSpeed * Time.deltaTime * moveDirection;

        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - (distance / totalDistance);

        float maxHeight = totalDistance / 4f;
        float positionY = parabolaAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        float reachedTargetDistance = .2f;
        if (Vector3.Distance(transform.position, targetPosition) < reachedTargetDistance) {
            float damageRadius = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);
            foreach (Collider collider in colliderArray) {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit)) {
                    // TODO: Damage based on distance from center
                    targetUnit.Damage(30);
                }
                if (collider.TryGetComponent<DestructibleCrate>(out DestructibleCrate destructibleCrate)) {
                    destructibleCrate.Damage();
                }
            }
            OnAnyCannonExploded?.Invoke(this, EventArgs.Empty);
            trailRenderer.transform.parent = null;
            Instantiate(cannonExplodeVFXPrefab, targetPosition + Vector3.up * 1, Quaternion.identity);
            Destroy(gameObject);
            onCannonBehaviorComplete();
        }
    }
    public void Setup(GridPosition targetGridPosition, Action onCannonBehaviorComplete) {
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        this.onCannonBehaviorComplete = onCannonBehaviorComplete;

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }
}
