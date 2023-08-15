using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovementBehaviour : MonoBehaviour
{
    #region FIELDS

    public GameObject ball;
    private Collider _ballCol;
    public float shotPwr = 5f;
    private float _stopSpd = 0.2f;
    public bool _isIdle, _isAiming;
    public GameObject AimingLine;
    private LineRenderer _lineRenderer;
    private Rigidbody rigidbody;
    private Ray ray;
    private Camera mainCam;
    //public float strength;
    public PlayerDataSO playerDataSO;
    
    #endregion

    #region UNITYMETHODS

    private void Awake()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rigidbody = ball.GetComponent<Rigidbody>();
        _ballCol = ball.GetComponent<Collider>();
        _isAiming = false;
        _lineRenderer = AimingLine.GetComponentInChildren<LineRenderer>();
        _lineRenderer.enabled = false;
    }
    
    private void FixedUpdate() {
        if(rigidbody.velocity.magnitude < _stopSpd) {
            Stop();
        }

        ProcessAim();
    }
    
    private void Update() {
        
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit);
            if (_isIdle && hit.collider == _ballCol) {
                _isAiming = true;
            }
        }
        
    }
    

    #endregion

    #region METHODS
    
    private Vector3? CastMouseClickRay() {
        
        ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            return hit.point;
        }
        return null;
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

        if (!Input.GetMouseButton(0)) {
            Shoot(worldPoint.Value);
        }
    }
    
    private void Shoot(Vector3 worldPoint)
    {
        Vector3 pos = ball.transform.position;
        _isAiming = false;
        _lineRenderer.enabled = false;

        Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, pos.y, worldPoint.z);

        Vector3 direction = (pos - horizontalWorldPoint).normalized;
        //float strength = Vector3.Distance(pos, horizontalWorldPoint);

        rigidbody.AddForce(direction * (playerDataSO.GetShotStrength() * shotPwr));
        _isIdle = false;
    }

    private void DrawLine(Vector3 worldPoint)
    {
        Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, ball.transform.position.y, worldPoint.z);
        Vector3[] positions =
        {
            ball.transform.position,
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
