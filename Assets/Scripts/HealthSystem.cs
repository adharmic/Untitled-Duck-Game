using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private int healthMax;
    [SerializeField] private int health = 100;
    public event EventHandler OnDead;

    private void Awake() {
        healthMax = health;
    }

    public void Damage(int damageAmount) {
        health -= damageAmount;

        if (health < 0) {
            health = 0;
        }

        if (health == 0) {
            Die();
        }

        Debug.Log(health);
    }

    private void Die() {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized() {
        return (float) health / healthMax;
    }
}
