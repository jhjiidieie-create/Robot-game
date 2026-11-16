using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ManualGameManager : MonoBehaviour
{
    public float timeLimit = 30f;
    public TMP_Text timerText;
    public TMP_Text resultText;
    public GameObject resultPanel;
    public ManualRobotController robot;

    public bool gameActive;
    float timeLeft;

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
        timeLeft = timeLimit;
        gameActive = true;
    }

    public void CheckWin()
    {
        ManualPuck[] pucks = FindObjectsOfType<ManualPuck>();

        foreach (ManualPuck puck in pucks)
            if (!puck.alreadySorted) return;

        EndGame();
    }

    void EndGame()
    {
        gameActive = false;
        if (robot != null) robot.StopRobot();

        ManualPuck[] pucks = FindObjectsOfType<ManualPuck>();
        int sorted = 0;

        foreach (ManualPuck puck in pucks)
            if (puck.alreadySorted) sorted++;

        resultText.text = sorted + " из " + pucks.Length;
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
