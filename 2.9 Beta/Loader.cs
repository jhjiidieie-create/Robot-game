using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public void LoadMenu() => SceneManager.LoadScene("Menu");
    public void LoadManual() => SceneManager.LoadScene("ManualScene");
    public void LoadAutomatic() => SceneManager.LoadScene("AutomaticScene");
    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
    public void QuitGame() => Application.Quit();
}