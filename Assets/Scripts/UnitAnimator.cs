using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;
    [SerializeField] private FireProjectileEvent fireProjectileEvent;
    private Vector3 targetLocation;

    private void Awake() {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction)) {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }
        if (TryGetComponent<ShootAction>(out ShootAction shootAction)) {
            shootAction.OnShoot += ShootAction_OnShoot;
            fireProjectileEvent.OnFireProjectile += FireProjectileEvent_OnFireProjectile;
        }
        if (TryGetComponent<HealthSystem>(out HealthSystem healthSystem)) {
            healthSystem.OnDead += HealthSystem_OnDeath;
        }
        if (TryGetComponent<Unit>(out Unit unit)) {
            unit.OnDamage += Unit_OnDamage;
        }

    }
    
    private void MoveAction_OnStartMoving(object sender, EventArgs e) {
        animator.SetBool("isWalking", true);
        
    }
    private void MoveAction_OnStopMoving(object sender, EventArgs e) {
        animator.SetBool("isWalking", false);
    }

    
    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e) {
        animator.SetTrigger("Shoot");
        targetLocation = e.targetUnit.GetWorldPosition();
    }

    private void SpawnProjectile() {
        Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        targetLocation.y = shootPointTransform.position.y;
        bulletProjectile.Setup(targetLocation);
    }

    private void FireProjectileEvent_OnFireProjectile(object sender, EventArgs e) {
        SpawnProjectile();
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e) {
        animator.SetTrigger("Exit");
        animator.SetTrigger("Die");
    }
    
    private void Unit_OnDamage(object sender, EventArgs e) {
        animator.SetTrigger("Hurt");
    }
}
