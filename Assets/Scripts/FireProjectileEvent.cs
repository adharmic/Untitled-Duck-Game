using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectileEvent : MonoBehaviour
{
    public event EventHandler OnFireProjectile;

    public void FireProjectile() {
        OnFireProjectile?.Invoke(this, EventArgs.Empty);
    }
}
