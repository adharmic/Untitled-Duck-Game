using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button nextTurnButton;
    [SerializeField] private TextMeshProUGUI turnNumberTextMeshPro;
    private void Start() {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        nextTurnButton.onClick.AddListener(() => {
            TurnSystem.Instance.NextTurn();
        });
        UpdateTurnText();
    }

    private void UpdateTurnText() {
        turnNumberTextMeshPro.text = "Turn " + TurnSystem.Instance.GetTurnNumber();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e) {
        UpdateTurnText();
    }
}
