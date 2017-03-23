using UnityEngine;
using System.Collections;
using System;

public class AsteroidBehaviour : MonoBehaviour {

    private GameManager GM;
    private Rigidbody rigidbody;
    private Vector3 direction;
    [HideInInspector]
    public float Speed = 0.5f;

    void Awake() {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        rigidbody = gameObject.GetComponent<Rigidbody>();

        //Start moving Asteroid in Random Direction at Constant Speed
        //direction = (new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f), 0.0f)).normalized;
        //rigidbody.velocity = direction * Speed;

    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision ColObject) {
        if (ColObject.gameObject.tag.Equals("Missile")) {
            //Instantiate Explosion Particle
            Destroy(ColObject.gameObject);
            Destroy(gameObject);
        } else if (ColObject.gameObject.tag.Equals("Turret")) {
            //Asteroid has hit the Earth and the Game is Over
            Destroy(ColObject.gameObject);
            GM.GameOver();
            Destroy(gameObject);
        } else if (ColObject.gameObject.tag.Equals(gameObject.tag)) {
            //Asteroids Collides with each other
            Destroy(gameObject); //Other Asteroid will be destroyed itself since both Asteroids have same Collision handling.
        }
    }

    //Trigger Collider is used so that we wouldn't have to check every asteroid everytime if its in the SafeZone.
    //The Asteroid will itself call 
	void OnTriggerEnter(Collider ColObj){
		if (ColObj.tag.Equals ("Turret")) {
			print ("Inside Safe Zone from Asteroid");
            TurretBehaviour TB = ColObj.GetComponent<TurretBehaviour>();
            //Double Checking if the Asteroid is in the safetyZone
            if (TB.TrajectoryWithinSafetyZone(transform.position, gameObject.GetComponent<Rigidbody>().velocity)) {
                print("Can shoot returns true");
                ShootMe(TB);
            }
		}
	}

    private void ShootMe(TurretBehaviour TurretScript)
    {
        TurretScript.ShootMissile(transform);
    }

}
