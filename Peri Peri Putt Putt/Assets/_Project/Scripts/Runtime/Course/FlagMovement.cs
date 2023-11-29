using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagMovement : MonoBehaviour
{
    #region FIELDS

    private GameObject player;
    public float AvoidanceDistance = 5f;
    private float _CurrentAvoidanceHeightOffset = 0f;
    private float distToPlayer;
    public float AvoidanceHeightOffset = 1f;
    private Transform Trans { get; set; }
    public float bobSpeed = 1.5f;
    public float bobHeight = 1f;
    public bool lockZ = true;
    public bool lockY = true;
    public bool lockX = true;
    public bool lockRotation = true;
    private Vector3 _startPos;
    private Vector3 _movementEquation;
    private Vector3 _RotationEquation;

    [Header("If -1, no overide")]
    public Vector3 MovementEquationOveride = new Vector3(-1, -1, -1);

    public Vector3 RotationEquationOveride = new Vector3(-1, -1, -1);

    [Header("Movement")]
    [Range(0f, 2f)]
    public float XAmplitude = 0.15f;

    [Range(0f, 2f)]
    public float YAmplitude = 0.15f;

    [Range(0f, 2f)]
    public float ZAmplitude = 0.15f;

    [Range(0f, 2f)]
    public float XSpeed = 0.2f;

    [Range(0f, 2f)]
    public float YSpeed = 0.2f;

    [Range(0f, 2f)]
    public float ZSpeed = 0.2f;

    [Header("Rotation")]
    [Range(0f, 3f)]
    public float RotationAmplitude = 1.5f;

    [Range(0f, 2f)]
    public float RotationSpeedMultiplier = 0.25f;

    [Range(0f, 2f)]
    public float rotationSpeed = 0.2f;

    private Vector3 _progression;
    private readonly Vector3 _speedIncrement = Vector3.one;

    [Header("Rotation Corrections")]
    public Vector3 RotationCorrection = Vector3.zero;

    #endregion FIELDS

    #region UNITY METHODS

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GetStart();
        if (MovementEquationOveride.x != -1)
            _movementEquation.x = MovementEquationOveride.x;
        if (MovementEquationOveride.y != -1)
            _movementEquation.y = MovementEquationOveride.y;
        if (MovementEquationOveride.z != -1)
            _movementEquation.z = MovementEquationOveride.z;
        if (RotationEquationOveride.x != -1)
            _RotationEquation.x = RotationEquationOveride.x;
        if (RotationEquationOveride.y != -1)
            _RotationEquation.y = RotationEquationOveride.y;
        if (RotationEquationOveride.z != -1)
            _RotationEquation.z = RotationEquationOveride.z;
    }

    private void Update()
    {
        distToPlayer = Vector3.Distance(player.transform.position, transform.position);
    }

    private void FixedUpdate()
    {
        //bob();
        ApplyWiggle();
        _progression += Time.fixedDeltaTime * _speedIncrement;
    }

    #endregion UNITY METHODS

    #region METHODS

    public void bob()
    {
        float newY = _startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        float newX = _startPos.x + Mathf.Cos(Time.time * bobSpeed) * bobHeight;

        if (lockZ)
            Trans.position = new Vector3(newX, newY, _startPos.z);
        else
            Trans.position = new Vector3(newX, newY, Trans.position.z);
    }

    private void ApplyWiggle()
    {
        transform.GetPositionAndRotation(out var position, out _);
        Vector3 currentPos = position;
        Vector3 Locking = new Vector3(1, 1, 1);

        if (lockX)
            Locking.x = 0;
        if (lockY)
            Locking.y = 0;
        if (lockZ)
            Locking.z = 0;

        if (distToPlayer < AvoidanceDistance)
        {
            _CurrentAvoidanceHeightOffset = Mathf.Lerp(_CurrentAvoidanceHeightOffset, AvoidanceHeightOffset, Time.deltaTime * 2f);
            Locking.y = 1;
        }
        else
        {
            _CurrentAvoidanceHeightOffset = Mathf.Lerp(_CurrentAvoidanceHeightOffset, 0f, Time.deltaTime * 2f);
        }

        transform.position = Vector3.Lerp(currentPos,
        new Vector3(_startPos.x + TrigMotionEquations((int)_movementEquation.x, _progression.x, XSpeed, XAmplitude) * Locking.x,
            _startPos.y + TrigMotionEquations((int)_movementEquation.y, _progression.y,
                YSpeed, YAmplitude) * Locking.y + _CurrentAvoidanceHeightOffset,
            _movementEquation.z + TrigMotionEquations((int)_movementEquation.z, _progression.z,
                ZSpeed, ZAmplitude) * Locking.z), Time.deltaTime * 2f);
        if (lockRotation)
            return;

        transform.localEulerAngles = new Vector3(TrigRotationEquations((int)_RotationEquation.x, _progression.x, RotationSpeedMultiplier, RotationAmplitude) + RotationCorrection.x,
                        TrigRotationEquations((int)_RotationEquation.y, _progression.y, RotationSpeedMultiplier, RotationAmplitude) + RotationCorrection.y, TrigRotationEquations((int)_RotationEquation.z, _progression.z, RotationSpeedMultiplier, RotationAmplitude) + RotationCorrection.z);

        /*new Vector3(
        Mathf.Clamp(TrigRotationEquations((int)_RotationEquation.x, _progression.x, RotationSpeedMultiplier, RotationAmplitude), -15f, 10f),
        Mathf.Clamp(TrigRotationEquations((int)_RotationEquation.y, _progression.y, RotationSpeedMultiplier, RotationAmplitude), -3f, 3f),
        Mathf.Clamp(TrigRotationEquations((int)_RotationEquation.z, _progression.z, RotationSpeedMultiplier, RotationAmplitude), -3f, 3f));*/
    }

    private void GetStart()
    {
        _startPos = transform.position;
        Trans = GetComponent<Transform>();
        _movementEquation = new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3));
        _RotationEquation = new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3));
    }

    private float TrigMotionEquations(int equation, float progression, float frequency, float amplitude)
    {
        float result = 0f;
        switch (equation)
        {
            case 0:
                result = Mathf.Sin(1.8f * Mathf.Sin(Mathf.Cos(progression * 0.13f)) * Mathf.Cos((progression - 3f) * frequency)) * amplitude;
                break;

            case 1:
                result = Mathf.Sin((Mathf.Sin(progression * 0.8f) * 0.5f) * (Mathf.Cos(progression * 0.2f) * frequency)) * amplitude;
                break;

            case 2:
                result = (Mathf.Sin(progression * frequency)) * (Mathf.Sin((progression * 0.4f) * 0.3f)) * amplitude;
                break;
        }
        return result;
    }

    private float TrigRotationEquations(int equation, float progression, float frequency, float amplitude)
    {
        float result = 0f;

        switch (equation)
        {
            case 0:
                result = Mathf.Sin(progression * (rotationSpeed * frequency) * Mathf.Cos(progression * (rotationSpeed * frequency))) * amplitude;
                break;

            case 1:
                result = Mathf.Cos(progression * (rotationSpeed * frequency) * Mathf.Sin(progression * (rotationSpeed * frequency))) * amplitude;
                break;

            case 2:
                result = Mathf.Sin(progression * (rotationSpeed * frequency)) * Mathf.Cos(progression * (rotationSpeed * frequency)) * amplitude;
                break;
        }
        return result;
    }

    #endregion METHODS
}