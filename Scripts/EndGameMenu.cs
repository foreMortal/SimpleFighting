using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour
{
    public void Restart()
    {
        Time.timeScale = 1f;
        int i = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(i, LoadSceneMode.Single);
    }

    public void BackToHub()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
