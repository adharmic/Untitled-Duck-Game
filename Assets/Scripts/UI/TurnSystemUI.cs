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
    [SerializeField] private GameObject enemyTurnVisualGameObject;
    private void Start() {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        nextTurnButton.onClick.AddListener(() => {
            TurnSystem.Instance.NextTurn();
        });
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void UpdateTurnText() {
        turnNumberTextMeshPro.text = "Turn " + TurnSystem.Instance.GetTurnNumber();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e) {
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void UpdateEnemyTurnVisual() {
        enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnButtonVisibility() {
        nextTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
