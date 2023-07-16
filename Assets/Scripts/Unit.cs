using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    private Vector3 targetPosition;
    private GridPosition gridPosition;

    private void Awake() {
        targetPosition = transform.position;
    }

    private void Start() {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
    }
    public void Move(Vector3 targetPosition) {
        this.targetPosition = targetPosition;
    }

    private void Update() {
        unitAnimator.SetBool("isWalking", false);

        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) >= stoppingDistance) {
            unitAnimator.SetBool("isWalking", true);
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;

            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        }
        
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition) {
            LevelGrid.Instance.UnitChangedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }
    

    
}
