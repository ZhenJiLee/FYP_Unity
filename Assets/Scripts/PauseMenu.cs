using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  
    private bool isPaused = false;
    public bool pauseDisabled;

    [SerializeField] AudioSource song;

    void Update()
    {
        if(!pauseDisabled)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                    Resume();
                else
                    Pause();
            }
        }

    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        song.Play();
        Time.timeScale = 1f;           
        isPaused = false;              
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        song.Pause();
        Time.timeScale = 0f;           
        isPaused = true;               
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;  
        SceneManager.LoadScene("MainMenu1");  
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }
}
