using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TheBob : MonoBehaviour
{
    [Header("Wiggle")]
    public GameObject[] Objects;

    private Vector3[] _StartPos;
    private Vector3[] _EulerEquations;
    private Vector3[] _MovementEquation;

    [Header("Rotation")]
    [Range(0f, 3f)]
    public float RotationAmplitude = 1.5f;

    [Range(0f, 2f)]
    public float RotationSpeedMultiplier = 0.25f;

    [Header("Movement")]
    [Range(0f, 2f)]
    public float XAmplitude = 0.5f;

    [Range(0f, 2f)]
    public float YAmplitude = 0.5f;

    [Range(0f, 2f)]
    public float ZAmplitude = 0.5f;

    [Range(0f, 2f)]
    public float XSpeed = 0.35f;

    [Range(0f, 2f)]
    public float YSpeed = 0.35f;

    [Range(0f, 2f)]
    public float ZSpeed = 0.35f;

    [Header("General Wiggle Settings")]
    [Range(0f, 2f)]
    public float rotationSpeed = 0.2f;// Speed of the rotation

    private Vector3 _progression;
    private readonly Vector3 _speedIncrement = Vector3.one;

    [Header("Rotation Corrections")]
    public Vector3 RotationCorrection = Vector3.zero;

    private void Awake()
    {
        GetStart();
    }

    private void Update()
    {
        for (int i = 0; i < Objects.Length; i++)
            ApplyWiggle(i);

        _progression += Time.deltaTime * _speedIncrement;
    }

    private void ApplyWiggle(int Index)
    {
        Objects[Index].transform.GetPositionAndRotation(out var position, out _);
        Vector3 currentPos = position;

        Objects[Index].transform.position = Vector3.Lerp(currentPos,
            new Vector3(_StartPos[Index].x + TrigMotionEquations((int)_MovementEquation[Index].x, _progression.x, XAmplitude,
                    XSpeed),
                _StartPos[Index].y + TrigMotionEquations((int)_MovementEquation[Index].y, _progression.y,
                    YAmplitude, YSpeed) * -1,
                _StartPos[Index].z + TrigMotionEquations((int)_MovementEquation[Index].y, _progression.z,
                    ZAmplitude, ZSpeed)), Time.deltaTime * 2f);

        Vector3 eulerRotation =
                        new Vector3(TrigRotationEquations((int)_EulerEquations[Index].x, _progression.x, RotationSpeedMultiplier, RotationAmplitude) + RotationCorrection.x,
                        TrigRotationEquations((int)_EulerEquations[Index].y, _progression.y, RotationSpeedMultiplier, RotationAmplitude) + RotationCorrection.y, TrigRotationEquations((int)_EulerEquations[Index].z, _progression.z, RotationSpeedMultiplier, RotationAmplitude) + RotationCorrection.z);

        Objects[Index].transform.localEulerAngles = eulerRotation;
    }

    private void GetStart()
    {
        Objects = this.GetChildren();
        _StartPos = new Vector3[Objects.Length];
        _EulerEquations = new Vector3[Objects.Length];
        _MovementEquation = new Vector3[Objects.Length];
        for (int i = 0; i < Objects.Length; i++)
        {
            _StartPos[i] = Objects[i].transform.position;
            _EulerEquations[i] = new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3));
            _MovementEquation[i] = new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3));
        }
    }

    private GameObject[] GetChildren()
    {
        // Get all children of this GameObject
        Transform[] children = GetComponentsInChildren<Transform>();
        GameObject[] childObjects = new GameObject[children.Length];
        for (int i = 0; i < children.Length; i++)
        {
            childObjects[i] = children[i].gameObject;
        }
        return childObjects;
    }

    /*
         * equations to make movement look vaguely random
         *
         * A ==> sin(cos(x*S) * cos(cos(x/S)) * S / (S^A) / A)) * S                 ==> Low Volatility
         *      A1 ==> sin(A2) * S
         *      A2 ==> cos(x * S) * cos(A3)
         *      A3 ==> cos(x / S) * (S^A)/A
         *      S = 1.5
         *      A = 0.3
         *
         * B ==> sin( (cos(sin(x*S)*S)) / A^S * cos(sin(x/S) * (S^A) / A)) * S      ==> Medium Volatility
         *      B1 ==> sin(B2 * B3) * S
         *      B2 ==> (cos(sin(x*S) * S))/(A^S)
         *      B3 ==> cos(sin(x/S) * (s^A)/A)
         *      S = 1.7
         *      A = 0.6
         *
         * C ==> sin( ((cos(cos(x*S)*S)*S) / A^S + sin(sin(x^A)) *                  ==> High Volatility
         *              tan(A) * cos( sin((x/S) * tan(A)) * S^A / A)) * S
         *      C1 ==> sin(C2) * S
         *      C2 ==> (C3) * cos(sin(x/S * tan(A)) * S^A / A)
         *      C3 ==> (cos(cos(x*S)*S)*S)) / A^S + sin(sin(x^A)) * tan(A)
         *      S = 2.25
         *      A = 1
         */

    [Tooltip("Equations index starts at 0")]
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
}