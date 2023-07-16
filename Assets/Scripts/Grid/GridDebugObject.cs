using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro debugTextMeshPro;
    private GridObject gridObject;
    public void SetGridObject(GridObject gridObject) {
        this.gridObject = gridObject;
    }

    private void Update() {
        debugTextMeshPro.text = gridObject.ToString();
    }
}
