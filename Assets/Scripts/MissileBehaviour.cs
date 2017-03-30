using UnityEngine;
using System.Collections;

public class MissileBehaviour : MonoBehaviour {

    public GameObject explosionParticle;

	// Use this for initialization
	void OnEnable () {
        //If a Missile in case misses the asteroid it will be destroyed after 5 secs.
        Destroy(gameObject, 3.0f);
	}

    void OnCollisionEnter(Collision ColObject)
    {
        if (ColObject.gameObject.tag.Equals("Asteroid"))
        {
            GameObject expParticle = (GameObject)GameObject.Instantiate(explosionParticle, transform.position, Quaternion.identity);
            Destroy(expParticle, 2);
            ColObject.gameObject.GetComponent<AsteroidBehaviour>().DestroyMe(gameObject.tag);
            Destroy(gameObject);
        }
    }
}
