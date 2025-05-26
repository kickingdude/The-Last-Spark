using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using a joint (maintain a distance between A and B 
// while allowing some sort of rigidbody movement)
// if can grapple, pull player up to ceiling
public class GrapplingHook : MonoBehaviour
{
    // rope distance
    [SerializeField] private float grappleLength;

    // to know if we can actually grapple onto it
    [SerializeField] private LayerMask grappleLayer;

    [SerializeField] private LineRenderer rope;

    private Vector3 grapplePoint;
    private DistanceJoint2D joint;

    // Start is called before the first frame update
    void Start()
    {
        joint = gameObject.GetComponent<DistanceJoint2D>();
        // disable joint bc no grappling right away
        joint.enabled = false;
        rope.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // when we left click on mouse
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(
            // take coordinates of where mouse is on screen and transfer to world coordinates
            origin: Camera.main.ScreenToWorldPoint(Input.mousePosition), 
            direction: Vector2.zero, 
            distance: Mathf.Infinity,
            layerMask: grappleLayer);

            // when we click on ceiling, we can now assign our 
            // grappling point to be there bc it now exists
            if(hit.collider != null)
            {
                grapplePoint = hit.point;
                // dealing with 2D so x, y
                grapplePoint.z = 0;
                // create an anchor between our player and ceiling
                joint.connectedAnchor = grapplePoint;
                joint.enabled = true;
                joint.distance = grappleLength;
                // create a line from player to ceiling's grappling position
                rope.SetPosition(0, grapplePoint);
                rope.SetPosition(1, transform.position);
                rope.enabled = true;
            }
        }

        // release left click on mouse
        if(Input.GetMouseButtonUp(0))
        {
           joint.enabled = false;
           rope.enabled = false;
        }

        if(rope.enabled == true)
        {
            rope.SetPosition(1, transform.position);
        }
    }
}