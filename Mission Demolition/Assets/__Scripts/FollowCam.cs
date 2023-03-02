using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // Point of interest (projectile)

    [Header("Dynamic")]
    public float camZ;

    [Header("Inscribed")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    void Awake() 
    {
        camZ = this.transform.position.z; // current Z pos of camera
        // camY = this.transform.position.y;
    }
    
    void FixedUpdate() 
    {
        // if (POI == null) return;    

        // // give dest coords of POI and z of camZ then move cam there
        // Vector3 dest = POI.transform.position;

        Vector3 dest = Vector3.zero;

        if(POI != null) {
            // If the POI has a Rigidbody, check to see if it is sleeping
            Rigidbody poiRigid = POI.GetComponent<Rigidbody>();
            if ((poiRigid != null) && poiRigid.IsSleeping() ) {
                POI = null;
            }

            if (POI != null) {
                dest = POI.transform.position;
            }
        }
        // Limit the minimum values of dest.x & dest.y
        dest.x = Mathf.Max( minXY.x, dest.x);
        dest.y = Mathf.Max( minXY.y, dest.y);
        dest = Vector3.Lerp(transform.position, dest, easing);
        dest.z = camZ;
        transform.position = dest;
        Camera.main.orthographicSize = dest.y + 10;
    }
}
