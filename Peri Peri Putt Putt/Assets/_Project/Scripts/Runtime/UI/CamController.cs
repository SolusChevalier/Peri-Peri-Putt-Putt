using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    #region FIELDS

    [Header("Required Components")]
    public GameObject Player;

    public GameObject OrbitSpere;
    public GameObject ViewConeCamAnchor;
    public GameObject ViewConeSphere;
    public GameObject CamPivot;
    public GameObject OrbitCam;

    [Header("Cam Settings")]
    public float ViewConeAngle = 100;

    public float MouseX;
    public float MouseY;
    public float MouseScroll;
    public Vector3 _AngleToCamera = new Vector3(0.1f, 0.1f, 0.1f);

    #endregion FIELDS

    #region UNITY METHODS

    private void Start()
    {
        //Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");
        _AngleToCamera = Quaternion.AngleAxis(MouseX, Vector3.up) * _AngleToCamera;
        _AngleToCamera = Quaternion.AngleAxis(MouseY, Vector3.left) * _AngleToCamera;

        MouseScroll = Input.GetAxis("Mouse ScrollWheel");
        ExpandSphere(OrbitSpere, MouseScroll);
        ExpandSphere(ViewConeSphere, (CalcViewConeSphereRadius() / 2));
    }

    private void FixedUpdate()
    {
        OrbitSpere.transform.position = Player.transform.position;
        moveViewConeAnchor();
    }

    #endregion UNITY METHODS

    #region METHODS

    public void ExpandSphere(GameObject Sphere, float ExpantionFactor)
    {
        if (ExpantionFactor <= 0)
        {
            ExpantionFactor = 1;
        }
        Vector3 Scale = new Vector3(ExpantionFactor, ExpantionFactor, ExpantionFactor);
        Sphere.transform.localScale = Scale;
    }

    public float CalcViewConeSphereRadius()
    {
        //Cosine rule to find radius of view cone sphere to scale apropriatly
        float Radius = 0;
        float OrbitSpereRadius = OrbitSpere.transform.localScale.x / 2;
        float CenterAngleRad = (180 - ViewConeAngle) * Mathf.Deg2Rad;
        Radius = Mathf.Sqrt(2 * Mathf.Pow(OrbitSpereRadius, 2) - (2 * Mathf.Pow(OrbitSpereRadius, 2) * Mathf.Cos(CenterAngleRad)));
        return Radius;
    }

    public void moveViewConeAnchor()
    {
        Vector3 AngleFromPlayer = new Vector3(0, 0, 0);
        AngleFromPlayer = Quaternion.LookRotation(_AngleToCamera).eulerAngles;
        ViewConeCamAnchor.transform.position = Player.transform.position - AngleFromPlayer * OrbitSpere.transform.localScale.x / 2;
        //Player.transform.position +  AngleFromPlayer * OrbitSpere.transform.localScale.x / 2
        /*Ray ray = new Ray(OrbitSpere.transform.position, AngleFromPlayer);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "CameraOrbit")
            {
                ViewConeCamAnchor.transform.position = hit.point;
            }
        }*/
    }

    #endregion METHODS
}