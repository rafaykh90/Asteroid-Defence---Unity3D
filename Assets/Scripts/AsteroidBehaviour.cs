using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{

    private GameManager GM;
    private Rigidbody rigidbody;
    private Vector3 direction;
    [HideInInspector]
    public float Speed = 1.0f;
    private TurretBehaviour turretBehaviour;
    private float timeToHit = 0;

    void OnEnable()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
        StartMovement();
        if (GM.Turret != null)
        {
            turretBehaviour = GM.Turret.GetComponent<TurretBehaviour>();
        }

        if (turretBehaviour != null) {
            if (turretBehaviour.distanceIsLess(transform.position))
                ShootMe();
            else
                checkTrajectory();
        }

        InvokeRepeating("CheckifTooFar", 0.0f, 1.0f);
    }

    public void checkTrajectory()
    {
        CancelInvoke("ShootMe");
        if (turretBehaviour.TrajectoryWithinSafetyZone(transform.position, rigidbody.velocity, ref timeToHit))
        {
            Invoke("ShootMe", timeToHit);
            //InvokeRepeating("CheckIfInside", 0.0f, 1.0f);
        }
    }

    void OnDisable()
    {
        /*
        We have to cancel the invoke otherwise the turret will keep on shooting at the position of the asteroid even after it
        has been disabled.
        */
        CancelInvoke();
    }

    /// <summary>
    /// Checking if the asteroid is too far from the Turret (i.e. possibly out of the screen) then disable the
    /// asteroid and re-enable at a new location closer to the turret.
    /// </summary>
    void CheckifTooFar()
    {
        if (GM.Turret != null)
        {
            if (Vector3.Distance(gameObject.transform.position, GM.Turret.transform.position) > 8)
            {
                DestroyMe();
            }
        }
    }

    /// <summary>
    /// This method Checks if Asteroid is instantiated or flied into the safezone. If so, then instantly shoot at it.
    /// </summary>
    //void CheckIfInside()
    //{
    //    //Check if the turret hasn't been destroyed and the specific asteroid is still active in the hierarchy.
    //    if (gameObject.activeSelf && GM.Turret != null)
    //    {
    //        turretBehaviour = GM.Turret.GetComponent<TurretBehaviour>();
    //        if (turretBehaviour.TrajectoryWithinSafetyZone(transform.position, gameObject.GetComponent<Rigidbody>().velocity))
    //        {
    //            ShootMe();
    //        }
    //    }
    //}

    /// <summary>
    /// When an asteroid is enabled in the screen it is assigned a random position on the screen and a velocty with
    /// constant speed in random direction is applied.
    /// </summary>
    void StartMovement()
    {
        /* Dividing the screen in four quadrants and assigning a random position within the quadrant to the asteroid.
        Reason for dividing is that the asteroid should be assigned a position such that it collides with the turret instantly
        when enabled. 
        */
        Vector3 position1 = new Vector3(UnityEngine.Random.Range(-5, -1), UnityEngine.Random.Range(-5, -1), 0);
        Vector3 position2 = new Vector3(UnityEngine.Random.Range(1, 5), UnityEngine.Random.Range(1, 5), 0);
        Vector3 position3 = new Vector3(UnityEngine.Random.Range(-5, -1), UnityEngine.Random.Range(1, 5), 0);
        Vector3 position4 = new Vector3(UnityEngine.Random.Range(1, 5), UnityEngine.Random.Range(-5, -1), 0);

        int num = UnityEngine.Random.Range(1, 4);
        switch (num)
        {
            case 1:
                transform.position = position1;
                break;
            case 2:
                transform.position = position2;
                break;
            case 3:
                transform.position = position3;
                break;
            case 4:
                transform.position = position4;
                break;
            default:
                transform.position = position1;
                break;
        }
        //Start moving Asteroid in Random Direction at Constant Speed
        direction = (new Vector3(UnityEngine.Random.Range(-Speed, Speed), UnityEngine.Random.Range(-Speed, Speed), 0.0f)).normalized;
        rigidbody.velocity = direction;;
    }

    /// <summary>
    /// This method is called if the asteroid is hit by the missile or it hits the turret.
    /// </summary>
    public void DestroyMe(string destroyedBy = "")
    {
        //Game object is disabled, since we are using the objects from the pool and not destroying the asteroids.
        gameObject.SetActive(false);
        GM.AsteroidDestroyed(destroyedBy);
    }


    /// <summary>
    /// Basic collision checking.
    /// Collision with the turret.
    /// </summary>
    /// <param name="ColObject"></param>
    void OnCollisionEnter(Collision ColObject)
    {
        if (ColObject.gameObject.tag.Equals("Turret"))
        {
            //Asteroid has hit the Earth and the Game is Over
            Destroy(ColObject.gameObject);
            GM.GameOver();
            DestroyMe();
        }

        else if (ColObject.gameObject.tag.Equals(gameObject.tag))
        {
            //Asteroid has hit another asteroid
            if (turretBehaviour != null)
                checkTrajectory();
        }
    }

    /// <summary>
    /// This method calls the turret ShootMissile method and the asteroid's transform which is to be shot down is passed as the
    /// parameter.
    /// The reason of calling ShootMissile method from the asteroid is that we wouldn't have to continuously check every active asteroid
    /// in the reason if it is in the safezone or not.The one which enters the safezone or is instantiated in the safezone will call
    /// this method on its own.
    /// </summary>
    private void ShootMe()
    {
        if (turretBehaviour != null)
            turretBehaviour.ShootMissile(transform);
    }

}
