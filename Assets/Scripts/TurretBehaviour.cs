using UnityEngine;

public class TurretBehaviour : MonoBehaviour {

	public GameObject MissilePrefab;
 	public float SafeDistance = 8.5f;
	public Transform shootPoint;
    private float MissileSpeed = 3.0f;

    /// <summary>
    /// This method Calculates the trajectory of the asteroid with the help of its position and velocity. I have calculated 
    /// the minimum time in which the asteroid will hit the turret if it was moving in turret's direction. Then I have
    /// calculated if the asteroid is moving towards the turret or if its moving away by anticipating the position of the 
    /// asteroid after half of the minimum time. Finally, if the asteroid is moving towards the turret then we calculate the
    /// time after which the asteroid will enter the safezone i.e. timeToHit. (Missile is shot after this time)
    /// </summary>
    /// <param name="asteroidPosition"></param>
    /// <param name="asteroidVelocity"></param>
    /// <param name="timeToHit"></param>
    /// <returns></returns>
	public bool TrajectoryWithinSafetyZone(Vector3 asteroidPosition, Vector3 asteroidVelocity, ref float timeToHit)
    {
        float distance = Vector3.Distance(transform.position, asteroidPosition);
        float minTime = distance / asteroidVelocity.magnitude;
        float timeInterval = minTime / 10;
        float tempTime = 0;
        Vector3 distanceAfterHalfTime = asteroidPosition + (asteroidVelocity * (minTime / 2));
        if (Vector3.Distance(transform.position, distanceAfterHalfTime) < distance)
        {
            do
            {
                tempTime += timeInterval;
                Vector3 tempPos = asteroidPosition + (asteroidVelocity * tempTime);
                if (Vector3.Distance(transform.position, tempPos) <= SafeDistance)
                {
                    timeToHit = tempTime;
                    return true;
                }
            } while (tempTime <= minTime);
        }
        return false;
    }

    /// <summary>
    /// Shoots Missile towards the asteroid transform provided in the parameter. This method calls the CalculateMissileVelocity
    /// method to calculate the vector of the Missile where as the Speed of the missile is constant.
    /// </summary>
    /// <param name="asteroidTransfrom"></param>
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



    /// <summary>
    /// The method below calculates the position at which the target asteroid and the missile will collide.
    /// Since both objects are moving at Constant Speed therefore we can calculate the time based on the turret
    /// asteroid's position and velocity. We are only calculating the direction of the Vector in which the missile
    /// is going to be shot.
    /// </summary>
    /// <param name="asteroidPosition"></param>
    /// <param name="asteroidVelocity"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Simply calculates the distance between the turret and the asteroid and returns true if the distance is less than than
    /// the safezone distance i.e. the asteroid is in the safezone
    /// </summary>
    /// <param name="asteroidPosition"></param>
    /// <returns></returns>
    public bool distanceIsLess(Vector3 asteroidPosition) {
        float distance = Vector3.Distance(transform.position, asteroidPosition);
        if (distance <= SafeDistance)
            return true;
        return false;
    }
}
