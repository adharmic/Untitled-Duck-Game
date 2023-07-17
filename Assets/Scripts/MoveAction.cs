using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private int maxMoveDistance = 4;
    private Vector3 targetPosition;
    private void Awake() {
        targetPosition = transform.position;
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
    }

    public List<GridPosition> GetValidActionGridPositionList() {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        // for (int x = )
        return validGridPositionList;
    }
}
