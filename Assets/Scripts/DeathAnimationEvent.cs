using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimationEvent : MonoBehaviour
{
    [SerializeField] private Unit unit;

    public void RemoveUnit() {
        unit.RemoveUnit();
    }
}
