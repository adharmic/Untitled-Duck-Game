using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }
    [SerializeField] private Transform gridSystemVisualCellPrefab;

    private GridSystemVisualCell[,] gridSystemVisualCellArray;
    private void Awake() {
            if (Instance != null) {
                Debug.LogError("There's more than one GridSystemVisual! " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            Instance = this;
    }

    private void Update() {
        UpdateGridVisual();
    }
    private void Start() {
        gridSystemVisualCellArray = new GridSystemVisualCell[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++) {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++) {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualCellTransform = Instantiate(gridSystemVisualCellPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                gridSystemVisualCellArray[x, z] = gridSystemVisualCellTransform.GetComponent<GridSystemVisualCell>();
            }
        }
        HideAllGridPositions();
    }

    public void HideAllGridPositions() {
        foreach (GridSystemVisualCell gridSystemVisualCell in gridSystemVisualCellArray) {
            gridSystemVisualCell.Hide();
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList) {
        foreach (GridPosition gridPosition in gridPositionList) {
            gridSystemVisualCellArray[gridPosition.x, gridPosition.z].Show();
        }
    }

    private void UpdateGridVisual() {
        HideAllGridPositions();
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();
        ShowGridPositionList(unit.GetMoveAction().GetValidActionGridPositionList());
    }
}
