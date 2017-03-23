using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject TurretPrefab;
    public int asteroidCount = 10;

	[HideInInspector]
	public GameObject Turret;

    void Awake()
    {
        InstantiateTurret();
    }

    public void GameOver() {

    }

	private void InstantiateTurret(){
		Turret = GameObject.Instantiate (TurretPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		Turret.name = "Turret"; 
	}

    public void CreateAsteroid() {
        GameObject obj = ObjectPooler.current.getPooledObject();
        if (obj == null) return;

        obj.SetActive(true);
    }
}
