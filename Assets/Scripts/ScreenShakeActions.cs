using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    
    private void Start() {
        FireProjectileEvent.OnAnyProjectileFired += FireProjectileEvent_OnAnyProjectileFired;
        CannonProjectile.OnAnyCannonExploded += CannonProjectile_OnAnyCannonExploded;
    }

    private void FireProjectileEvent_OnAnyProjectileFired(object sender, EventArgs e) {
        ScreenShake.Instance.Shake(.5f);
    }

    private void CannonProjectile_OnAnyCannonExploded(object sender, EventArgs e) {
        ScreenShake.Instance.Shake(2f);
    }
}
