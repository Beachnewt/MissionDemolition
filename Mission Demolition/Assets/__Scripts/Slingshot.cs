using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{

    // Header tells compiler to only set this in editor
    // However this isn't necessary since LaunchPoint is already under slingshot
    [Header("Inscribed")]
    public GameObject projectilePrefab;
    public float velocityMult = 10f;

    // Instnatiates with Awake
    [Header("Dynamic")]
    public GameObject launchPoint;
    public GameObject projectile; // singleton; only want one of at any time
    public bool aimingMode; // defining state based machine

    void Awake()
    {
        launchPoint = transform.Find("LaunchPoint").gameObject;
        launchPoint.SetActive(false);
        aimingMode = false;
    }

    void Update() 
    {
        // PRE-CONDITION: if aimingMode, then projectile is instantiated
        if (!aimingMode)
        {
            return;
        }

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - launchPoint.transform.position;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector3 projPos = launchPoint.transform.position + mouseDelta;
        // PRE-CONDITION: projectile needs to be instantiated
        projectile.transform.position = projPos;

        // POST-CONDITION: when !aimingMode, launchPoint.SetActive(false)
        if (Input.GetMouseButtonUp(0)) {
            aimingMode = false;
            launchPoint.SetActive(false);
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projRB.isKinematic = false;
            projRB.velocity = -mouseDelta * velocityMult;
            projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            FollowCam.POI = projectile;
            projectile = null;
        }

    }

    void OnMouseEnter()
    {
        // print("Slingshot::OnMouseEnter()");
        // SetActive toggles but Halo is iffy so code below doesn't work
        // launchPoint.GetComponent<Halo>.SetActive(true);
        launchPoint.SetActive(true);
    }

    void OnMouseExit() 
    {
        // print("Slingshot::OnMouseExit()");
        if(!aimingMode) {
            launchPoint.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        aimingMode = true; // POST-CONDITIONS: Projectile must be instantiated
        projectile = Instantiate<GameObject>(projectilePrefab);
        projectile.transform.position = launchPoint.transform.position;
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }
}
