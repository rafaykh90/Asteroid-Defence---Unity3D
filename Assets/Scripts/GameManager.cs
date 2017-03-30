using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject Turret;
    public UIManager uiManager;
    private int score = 0;

    /// <summary>
    /// Calls a method in UIManager class to show game over popup.
    /// </summary>
    public void GameOver() {
        uiManager.ShowGameOverPopup();
    }



    /// <summary>
    /// When some asteroid is hit by the missile this method is called so that the GameManager knows that a new Asteroid is 
    /// required from the pool and increases the score if the asteroid is destroyed by a missile.
    /// </summary>
    /// <param name="destroyedBy"></param>
    public void AsteroidDestroyed(string destroyedBy = "")
    {
        //Little delay in re-enabling the asteroid from the pool.
        Invoke("CreateAsteroid", 3);
        if(destroyedBy.Equals("Missile"))
            uiManager.increaseScore(++score);
    }

    public void CreateAsteroid() {
        GameObject obj = ObjectPooler.current.getPooledObject();
        if (obj == null) return;

        obj.SetActive(true);
    }
}
