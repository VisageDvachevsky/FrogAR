using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScene : MonoBehaviour
{
    private static string currentScene;

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        currentScene = sceneName;
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(currentScene);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}
