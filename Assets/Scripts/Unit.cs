using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private const int ACTION_POINTS_MAX = 2;
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    public event EventHandler OnDamage;
    [SerializeField] private bool isEnemy;
    private Vector3 targetPosition;
    private GridPosition gridPosition;
    private BaseAction[] baseActionArray;
    [SerializeField] private int actionPoints = ACTION_POINTS_MAX;
    private HealthSystem healthSystem;
    [SerializeField] private GameObject worldUI;
    private void Awake() {
        baseActionArray = GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>();
    }
    private void Start() {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update() {
        
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition) {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitChangedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }
    
    public GridPosition GetGridPosition() {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionArray() {
        return baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction) {
        if (CanSpendActionPointsToTakeAction(baseAction)) {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        return false;
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction) {
        return actionPoints >= baseAction.GetActionPointsCost();
    }   

    public T GetAction<T>() where T : BaseAction {
        foreach(BaseAction baseAction in baseActionArray) {
            if (baseAction is T t) {
                return t;
            }
        }
        return null;
    } 

    private void SpendActionPoints(int amount) {
        actionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public int GetActionPoints() {
        return actionPoints;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e) {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn())) {
            actionPoints = ACTION_POINTS_MAX;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy() {
        return isEnemy;
    }

    public void Damage(int damageAmount) {
        healthSystem.Damage(damageAmount);
        OnDamage?.Invoke(this, EventArgs.Empty);
    }

    public Vector3  GetWorldPosition() {
        return transform.position;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e) {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        worldUI.SetActive(false);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveUnit() {
        Destroy(gameObject);
    }

    public float GetHealthNormalized() {
        return healthSystem.GetHealthNormalized();
    }
}
