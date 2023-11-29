using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class MovementBehaviour : MonoBehaviour
{
    #region FIELDS

    public GameObject ball;
    private bool SliderPower = false;
    private Transform _ballTrans;
    private SphereCollider _ballCol;
    public float shotPwr = 35f;
    private float _stopSpd = 0.1f;
    public bool _isIdle, _isAiming;
    public GameObject AimingLine;
    public GameObject Spawn;

    //private LineRenderer _lineRenderer;
    private Rigidbody rb;

    //private Ray ray;
    public Camera mainCam;

    private GameObject _TempLine;
    //private bool _AimingLineActive = false;

    public PlayerDataSO playerDataSO;

    #endregion FIELDS

    #region UNITYMETHODS

    private void Awake()
    {
        //mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = this.GetComponent<Rigidbody>();
        _ballCol = ball.GetComponent<SphereCollider>();
        _ballTrans = ball.GetComponent<Transform>();
        _isAiming = false;
        //_lineRenderer = AimingLine.GetComponentInChildren<LineRenderer>();
        //_lineRenderer = this.GetComponent<LineRenderer>();
        //_lineRenderer.enabled = false;
        SliderPower = playerDataSO.GetSliderPower();
        playerDataSO.SetIsAiming(false);
        _ballTrans.position = Spawn.transform.position;
    }

    private void FixedUpdate()
    {
        if (IsGrounded())
        {
            rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, Time.fixedDeltaTime * _stopSpd);
            if (rb.velocity.sqrMagnitude <= 0.005f)
            {
                Stop();
            }
        }
        /*        if (rb.velocity.magnitude < _stopSpd)
                {
                    Stop();
                }*/

        ProcessAim();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit);
            if (_isIdle && hit.collider == _ballCol)
            {
                _isAiming = true;
                //ProcessAim();
            }
        }
    }

    #endregion UNITYMETHODS

    #region METHODS

    private Vector3? CastMouseClickRay()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return null;
    }

    private bool IsGrounded()
    {
        return Physics.OverlapSphere(transform.position, _ballCol.radius * 1.05f,
            LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore).Length > 0;
    }

    private void ProcessAim()
    {
        if (!_isAiming || !_isIdle)
        {
            return;
        }

        Vector3? worldPoint = CastMouseClickRay();

        if (!worldPoint.HasValue)
        {
            return;
        }

        DrawLine(worldPoint.Value);

        if (!Input.GetMouseButton(0))
        {
            Vector2 input = new Vector2(
            Input.GetAxis("Mouse Y"),
            Input.GetAxis("Mouse X"));

            Shoot(worldPoint.Value);
        }
        /*        if (Input.GetMouseButtonUp(0) && _AimingLineActive)
                {
                    Destroy(_TempLine);
                    _AimingLineActive = false;
                }*/
    }

    private void Shoot(Vector3 worldPoint)
    {
        Vector3 pos = _ballTrans.position;
        _isAiming = false;
        //_lineRenderer.enabled = false;
        /*if (_AimingLineActive)
        {
            Destroy(_TempLine);
            _AimingLineActive = false;
        }*/

        Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, pos.y, worldPoint.z);

        Vector3 direction = (pos - horizontalWorldPoint).normalized;

        rb.AddForce(direction * (SliderPower ? playerDataSO.GetShotStrength() : Vector3.Distance(pos, horizontalWorldPoint) * shotPwr), ForceMode.VelocityChange);
        _isIdle = false;
        playerDataSO.SetIsAiming(false);
    }

    private void DrawLine(Vector3 worldPoint)
    {
        playerDataSO.SetIsAiming(true);
        Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, _ballTrans.position.y, worldPoint.z);
        Vector3[] positions =
        {
            _ballTrans.position,
            horizontalWorldPoint
        };
        AimingLine.GetComponent<LineDrawer>().RenderLine(positions);

        /*        _TempLine = Instantiate(AimingLine, _ballTrans.position, Quaternion.identity);
                _TempLine.GetComponentInChildren<LineRenderer>().SetPositions(positions);*//*
                _lineRenderer.SetPositions(positions);
                _lineRenderer.enabled = true;*/
    }

    private void Stop()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        _isIdle = true;
    }

    #endregion METHODS
}