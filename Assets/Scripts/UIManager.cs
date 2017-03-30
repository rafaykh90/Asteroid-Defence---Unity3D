using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public GameObject container;
    public GameManager GM;
    public Text scoreText;

    public void ShowGameOverPopup()
    {
        container.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void increaseScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
