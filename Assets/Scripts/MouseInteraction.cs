using UnityEngine;

public class MouseInteraction : MonoBehaviour {

    private AsteroidBehaviour astroBehaviour;
    private Vector3 gameObjectSreenPoint;
    private Vector3 mousePreviousLocation;
    private Vector3 mouseCurLocation;
    private Rigidbody rigidbody;
    private Vector3 directionVector;

    void Awake() {
        astroBehaviour = GetComponent<AsteroidBehaviour>();
        rigidbody = GetComponent<Rigidbody>();
    }
    void OnMouseDown()
    {
        //This grabs the position of the object in the world and turns it into the position on the screen
        gameObjectSreenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        //Sets the mouse pointers vector3
        mousePreviousLocation = new Vector3(Input.mousePosition.x, Input.mousePosition.y, gameObjectSreenPoint.z);
    }

    void OnMouseDrag()
    {
        //We calculate the mouse movement and calculate the direction of the vector which will be assigned to the asteroid.
        mouseCurLocation = new Vector3(Input.mousePosition.x * astroBehaviour.Speed, Input.mousePosition.y * astroBehaviour.Speed, gameObjectSreenPoint.z);
        directionVector = (mouseCurLocation - mousePreviousLocation).normalized;
        mousePreviousLocation = mouseCurLocation;
    }

    public void OnMouseUp()
    {
        //Asteroid's velocity is changed according to the direction vector calculated through mouse movement.
        rigidbody.velocity = directionVector;
        astroBehaviour.checkTrajectory();
    }
}
