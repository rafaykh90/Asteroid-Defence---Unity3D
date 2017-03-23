using UnityEngine;
using System.Collections;

public class TurretBehaviour : MonoBehaviour {

	public GameObject MissilePrefab;
 	public float SafeDistance = 8.5f;
	public Transform shootPoint;
    private float MissileSpeed = 3;

	void Awake(){
		
	}

	
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

    }


	public bool TrajectoryWithinSafetyZone(Vector3 asteroidPosition, Vector3 asteroidVelocity){
        Vector3 difference = gameObject.transform.position - asteroidPosition;
		float dist = difference.sqrMagnitude;
        if (dist < SafeDistance) {
            return true;
		}

		return false;
	}

	public void ShootMissile(Transform asteroidTransfrom){
        Vector3 interceptPosition = CalculateMissileVelocity(asteroidTransfrom.position, asteroidTransfrom.GetComponent<Rigidbody>().velocity);

        transform.LookAt (interceptPosition); //Since there shouldn't be any targeting delay therefore I have used Unity's Built in function

        //Instantiating Missile at the Location of turret.
        Vector3 missilePosition = shootPoint.transform.position;
		Quaternion missileRotation = shootPoint.rotation;
		GameObject missile = GameObject.Instantiate (MissilePrefab, missilePosition, missileRotation) as GameObject;
		Rigidbody rigidbody = missile.GetComponent<Rigidbody> ();
        rigidbody.velocity = interceptPosition * MissileSpeed;
	}


    /*
        The method below calculates the position at which the target asteroid and the missile will collide.
        Since both objects are moving at Constant Speed therefore we can calculate the time based on the turret 
        asteroid's position and velocity. We are only calculating the direction of the Vector in which the missile
        is going to be shot.
        */
    private Vector3 CalculateMissileVelocity(Vector3 asteroidPosition, Vector3 asteroidVelocity) {
        
        //Since we have no acceleration so, I have used Law of cosines to calculate cosTheta, which will be used to calculate time.
        Vector3 TargetOffset = transform.position - asteroidPosition;
        float cosTheta = Vector3.Dot(TargetOffset.normalized, asteroidVelocity);
        float targetSpeed = asteroidVelocity.magnitude;
        float distanceBetweenObjects = Vector3.Distance(transform.position, asteroidPosition);
        float missileSpeedSqr = Mathf.Pow(MissileSpeed, 2);
        float targerSpeedSqr = Mathf.Pow(targetSpeed, 2);
        float squaredDistance = Mathf.Pow(distanceBetweenObjects, 2);
        float A = missileSpeedSqr - targerSpeedSqr;
        float B = 2 * distanceBetweenObjects * targetSpeed * cosTheta;
        float C = -(Mathf.Pow(squaredDistance, 2));

        //Applying the quadratic formula
        float time1 = (-B + Mathf.Sqrt(Mathf.Pow(B, 2) - 4 * A * C)) / (2 * A);
        float time2 = (-B - Mathf.Sqrt(Mathf.Pow(B, 2) - 4 * A * C)) / (2 * A);

        //We have two values from the Quadratic equation. We are only going to select the one with the positive value and ignore the negative one.
        //If both are positive then we consider the smaller one.
        float time = 0;
        if (time1 < 0 && time2 > 0) {
            time = time2;
        } else if (time1 > 0 && time2 < 0) {
            time = time1;
        } else if (time1 > 0 && time2 > 0){
            time = time1 - time2 > 0 ? time1 : time2;
        } else {
            print("Projectile Speed is less than the target Speed");
        }
        time /= 3;

        //Calculating the intercepting position for the Targer asteroid and the Missile.
        Vector3 interceptLocation = (asteroidPosition + asteroidVelocity * time - transform.position) / time;
        return interceptLocation.normalized;
    }
}
