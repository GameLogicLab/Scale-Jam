using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Attach this to a UI Manager object (or directly on buttons via Inspector).
    // Call these methods from Button OnClick() events.

    public void LoadLevel(string sceneName)
    {
        Time.timeScale = 1f; // in case the game was paused (e.g. Level Complete panel)
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevelByIndex(int buildIndex)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(buildIndex);
    }

    public void ReloadCurrentLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // make sure this matches your menu scene's exact name
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit requested (won't work in Editor, only in a real build)");
    }
}