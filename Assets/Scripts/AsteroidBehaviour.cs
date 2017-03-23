using UnityEngine;
using System.Collections;
using System;

public class AsteroidBehaviour : MonoBehaviour
{

    private GameManager GM;
    private Rigidbody rigidbody;
    private Vector3 direction;
    [HideInInspector]
    public float Speed = 0.5f;
    private TurretBehaviour turretBehaviour;

    void OnEnable()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
        StartMovement();
        if (turretBehaviour != null)
        {
            turretBehaviour = GM.Turret.GetComponent<TurretBehaviour>();
            //Check if Asteroid is instantiated with in safezone instantly shoot at it.
            if (turretBehaviour.TrajectoryWithinSafetyZone(transform.position, gameObject.GetComponent<Rigidbody>().velocity))
            {
                ShootMe();
            }
        }
    }

    void OnStart()
    {
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(gameObject.transform.position, turretBehaviour.gameObject.transform.position) > 8)
        {
            DestroyMe();
        }
    }

    // Use this for initialization
    void StartMovement()
    {
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
        direction = (new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f), 0.0f)).normalized;
        rigidbody.velocity = direction * Speed;
    }

    // Update is called once per frame
    public void DestroyMe()
    {
        //Game object is disabled, since we are using the objects from the pool and not destroying the asteroids.
        gameObject.SetActive(false);
        GM.AsteroidDestroyed();
    }

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
            //Asteroids Collides with each other
            DestroyMe(); //Other Asteroid will be destroyed itself since both Asteroids have same Collision handling.
        }
    }

    //Trigger Collider is used so that we wouldn't have to check every asteroid everytime if its in the SafeZone.
    //The Asteroid will itself call 
    void OnTriggerEnter(Collider ColObj)
    {
        if (ColObj.tag.Equals("Turret"))
        {
            print("Inside Safe Zone from Asteroid");
            //Double Checking if the Asteroid is in the safetyZone
            if (turretBehaviour.TrajectoryWithinSafetyZone(transform.position, gameObject.GetComponent<Rigidbody>().velocity))
            {
                print("Can shoot returns true");
                ShootMe();
            }
        }
    }

    private void ShootMe()
    {
        turretBehaviour.ShootMissile(transform);
    }

}
