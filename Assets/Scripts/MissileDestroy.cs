using UnityEngine;
using System.Collections;

public class MissileDestroy : MonoBehaviour {

	// Use this for initialization
	void OnStart () {
        //If a Missile in case misses the asteroid it will be destroyed after 5 secs.
        Destroy(gameObject, 5.0f);
	}
}
