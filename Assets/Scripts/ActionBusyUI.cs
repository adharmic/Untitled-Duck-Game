using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    // TO DO: A more elegant solution to prevent input mismanagement. Perhaps just hide the UI buttons?
    private void Start() {
        UnitActionSystem.Instance.OnBusyStatusChanged += UnitActionSystem_OnBusyStatusChanged;
        Hide();
    }

    private void UnitActionSystem_OnBusyStatusChanged(object sender, bool isBusy) {
        if(isBusy) {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
