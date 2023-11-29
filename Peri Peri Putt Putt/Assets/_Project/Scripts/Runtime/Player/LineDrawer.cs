using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    #region FIELDS

    private LineRenderer _lineRenderer;
    public PlayerDataSO playerDataSO;

    #endregion FIELDS

    #region UNITY METHODS

    private void Awake()
    {
        _lineRenderer = this.GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
    }

    #endregion UNITY METHODS

    #region METHODS

    public void RenderLine(Vector3[] LinePositions)
    {
        _lineRenderer.SetPositions(LinePositions);
    }

    public void Update()
    {
        if (playerDataSO.GetIsAiming())
        {
            _lineRenderer.enabled = true;
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }

    #endregion METHODS
}