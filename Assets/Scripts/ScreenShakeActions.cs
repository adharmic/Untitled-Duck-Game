using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    
    private void Start() {
        FireProjectileEvent.OnAnyProjectileFired += FireProjectileEvent_OnAnyProjectileFired;
    }

    private void FireProjectileEvent_OnAnyProjectileFired(object sender, EventArgs e) {
        ScreenShake.Instance.Shake(.5f);
    }
}
