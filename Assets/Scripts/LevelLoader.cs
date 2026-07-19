using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Attach this to a UI Manager object (or directly on buttons via Inspector).
    // Call these methods from Button OnClick() events.

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevelByIndex(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    public void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // make sure this matches your menu scene's exact name
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit requested (won't work in Editor, only in a real build)");
    }
}