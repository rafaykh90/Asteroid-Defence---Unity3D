using UnityEngine;
using System.Collections;

public class MouseInteraction : MonoBehaviour {

    private AsteroidBehaviour AB;
    public Vector3 gameObjectSreenPoint;
    public Vector3 mousePreviousLocation;
    public Vector3 mouseCurLocation;
    private Rigidbody rigidbody;

    void Awake() {
        AB = GetComponent<AsteroidBehaviour>();
        rigidbody = GetComponent<Rigidbody>();
    }
    void OnMouseDown()
    {
        //This grabs the position of the object in the world and turns it into the position on the screen
        gameObjectSreenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        //Sets the mouse pointers vector3
        mousePreviousLocation = new Vector3(Input.mousePosition.x, Input.mousePosition.y, gameObjectSreenPoint.z);
    }

    public Vector3 force;
    public Vector3 objectCurrentPosition;
    public Vector3 objectTargetPosition;
    public float topSpeed = 10;

    void OnMouseDrag()
    {
        mouseCurLocation = new Vector3(Input.mousePosition.x, Input.mousePosition.y, gameObjectSreenPoint.z);
        force = (mouseCurLocation - mousePreviousLocation).normalized;//Changes the force to be applied
        mousePreviousLocation = mouseCurLocation;
    }

    public void OnMouseUp()
    {
        rigidbody.velocity = force * AB.Speed;
    }
}
