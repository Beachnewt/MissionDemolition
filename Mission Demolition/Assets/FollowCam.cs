using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // Point of interest (projectile)

    [Header("Dynamic")]
    public float camZ;

    [Header("Inscribed")];
    public float easing = 0.05f;

    void Awake() 
    {
        camZ = this.transform.position.z; // current Z pos of camera
        camY = this.transform.position.y;
    }
    
    void FixedUpdate() 
    {
        if (POI == null) 
        {
            return;
        }

        // give dest coords of POI and z of camZ then move cam there
        Vector3 dest = POI.transform.position;
        dest = Vector3.Lerp(transform.position, dest, easing);
        dest.z = camZ;
        transform.position = dest;

    }
}
