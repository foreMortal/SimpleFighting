using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 1.0f;
    }

    public void StartGame()
    {
        if(PlayerPrefs.GetInt("EnterGame") != 1)
        {
            SceneManager.LoadScene(2);
            PlayerPrefs.SetInt("EnterGame", 1);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
}
