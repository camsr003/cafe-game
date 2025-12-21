using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject player; // assign the Player GameObject in the Inspector

    private MonoBehaviour[] playerScripts;
    private bool isPaused = false;

    void Start()
    {
        // Get all scripts you want to disable
        playerScripts = player.GetComponents<MonoBehaviour>(); 
        // filter only movement/camera scripts if there are extra?
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disable player scripts
        foreach (var script in playerScripts)
        {
            script.enabled = false;
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Re-enable player scripts
        foreach (var script in playerScripts)
        {
            script.enabled = true;
        }
    }

    public void QuitToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScreen");
    }
}
