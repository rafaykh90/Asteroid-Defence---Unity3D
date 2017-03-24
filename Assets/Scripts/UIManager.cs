using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    public GameObject container;
    public GameManager GM;

    public void ShowGameOverPopup()
    {
        container.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
