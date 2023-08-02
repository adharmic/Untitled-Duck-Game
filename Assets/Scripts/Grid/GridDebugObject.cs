using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro debugTextMeshPro;
    private object gridObject;
    public virtual void SetGridObject(object gridObject) {
        this.gridObject = gridObject;
    }

    protected virtual private void Update() {
        debugTextMeshPro.text = gridObject.ToString();
    }
}
