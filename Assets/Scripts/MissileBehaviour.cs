using UnityEngine;
using System.Collections;

public class MissileBehaviour : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
        //If a Missile in case misses the asteroid it will be destroyed after 5 secs.
        Destroy(gameObject, 3.0f);
	}

    void OnCollisionEnter(Collision ColObject)
    {
        if (ColObject.gameObject.tag.Equals("Asteroid"))
        {
            //Instantiate Explosion Particle
            ColObject.gameObject.GetComponent<AsteroidBehaviour>().DestroyMe();
            Destroy(gameObject);
        }
    }
}
