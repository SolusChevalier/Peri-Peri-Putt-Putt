using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    #region FIELDS
    public float shotPwr;
    public float _stopSpd;
    public bool _isIdle, _isAiming;
    public LineRenderer _lineRenderer;
    public Rigidbody rigidbody;
    public float strength;
    public PlayerDataSO playerDataSO;
    
    #endregion

    #region UNITYMETHODS

    private void Awake() {
        rigidbody = GetComponentInChildren<Rigidbody>();
        _isAiming = false;
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        _lineRenderer.enabled = false;
    }
    
    private void FixedUpdate() {
        if(rigidbody.velocity.magnitude < _stopSpd) {
            Stop();
        }

        ProcessAim();
    }
    
    /*private void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            if (_isIdle) {
                _isAiming = true;
            }
        }
        
    }
    */

    #endregion

    #region METHODS
    
    private Vector3? CastMouseClickRay() {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        if (Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, float.PositiveInfinity)) {
            return hit.point;
        } else {
            return null;
        }
    }
    
    private void ProcessAim() {
        if(!_isAiming || !_isIdle) {
            return;
        }

        Vector3? worldPoint = CastMouseClickRay();

        if (!worldPoint.HasValue) {
            return;
        }

        DrawLine(worldPoint.Value);

        if (Input.GetMouseButtonUp(0)) {
            Shoot(worldPoint.Value);
        }
    }
    
    private void Shoot(Vector3 worldPoint) {
        _isAiming = false;
        _lineRenderer.enabled = false;

        Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);

        Vector3 direction = (transform.position - horizontalWorldPoint).normalized;
        //float strength = Vector3.Distance(transform.position, horizontalWorldPoint);

        rigidbody.AddForce(direction * playerDataSO.GetShotStrength() * shotPwr);
        _isIdle = false;
    }

    private void DrawLine(Vector3 worldPoint) {
        Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);
        Vector3[] positions =
        {
            transform.position,
            horizontalWorldPoint
        };
        _lineRenderer.SetPositions(positions);
        _lineRenderer.enabled = true;
    }
    
    private void Stop() {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        _isIdle = true;
    }

    #endregion
    
    
}
