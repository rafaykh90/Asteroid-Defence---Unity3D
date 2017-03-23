﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject Turret;

    void Awake()
    {
    }

    public void GameOver() {

    }

    public void AsteroidDestroyed()
    {
        //Little delay in re-enabling the asteroid from the pool.
        Invoke("CreateAsteroid", 3);
    }

    public void CreateAsteroid() {
        GameObject obj = ObjectPooler.current.getPooledObject();
        if (obj == null) return;

        obj.SetActive(true);
    }
}
