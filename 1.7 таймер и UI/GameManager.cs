using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float timeLimit = 30f;
    public TMP_Text timerText;
    public TMP_Text resultText;
    public GameObject resultPanel;
    public RobotController robot;

    float timeLeft;
    bool gameActive;

    void Start()
    {
        resultPanel.SetActive(false);
        ShowTime(timeLimit);
    }

    void Update()
    {
        if (!gameActive) return;

        timeLeft -= Time.deltaTime;
        ShowTime(timeLeft);

        if (timeLeft <= 0) EndGame();
    }

    public void StartGame()
    {
        if (gameActive) return;

        timeLeft = timeLimit;
        gameActive = true;
        ShowTime(timeLeft);
    }

    public bool CheckWin()
    {
        if (!gameActive) return false;

        Puck[] pucks = FindObjectsOfType<Puck>();
        int sorted = 0;

        foreach (Puck puck in pucks)
            if (puck.isSorted) sorted++;

        if (pucks.Length == 0 || sorted < pucks.Length) return false;

        EndGame(sorted, pucks.Length);
        return true;
    }

    void EndGame()
    {
        Puck[] pucks = FindObjectsOfType<Puck>();
        int sorted = 0;

        foreach (Puck puck in pucks)
            if (puck.isSorted) sorted++;

        EndGame(sorted, pucks.Length);
    }

    void EndGame(int sorted, int total)
    {
        if (!gameActive) return;

        gameActive = false;
        if (robot != null) robot.StopRobot();

        resultText.text = $"{sorted} из {total}";
        resultPanel.SetActive(true);
    }

    void ShowTime(float time)
    {
        int seconds = Mathf.Max(0, Mathf.CeilToInt(time));
        timerText.text = $"{seconds / 60}:{seconds % 60:00}";
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}