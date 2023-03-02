using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class ProjectileLine : MonoBehaviour
{
    static List <ProjectileLine> PROJ_LINES = new List<ProjectileLine>();
    private const float DIM_MULT = 0.75f;

    private LineRenderer _line;
    private bool _drawing = true;
    private Projectile _projectile;

    // Start is called before the first frame update
    void Start()
    {
        _line = GetComponent<LineRenderer>();
        _line.positionCount = 1;
        _line.SetPosition(0, transform.position);
        _projectile = GetComponentInParent<Projectile>();
        ADD_LINE(this);
    }

    void FixedUpdate() {
        if (_drawing) {
            _line.positionCount++;
            _line.SetPosition(_line.positionCount - 1, transform.position);
            // If the Projectile Rigidbody is sleeping, stop drawiwng
            if(_projectile != null) {
                if(!_projectile.awake) {
                    _drawing = false;
                    _projectile = null;
                }
            }
        }// end if (_drawing)
    }

    private void OnDestroy() {
        PROJ_LINES.Remove(this);
    }

    static void ADD_LINE(ProjectileLine newLine) {
        Color col;

        //Iterate over all teh old lines and dim them
        foreach(ProjectileLine p1 in PROJ_LINES) {
            col = p1._line.startColor;
            col = col * DIM_MULT;
            p1._line.startColor = p1._line.endColor = col;
        }
        // Add newLine to the List
        PROJ_LINES.Add(newLine);
    }
}
