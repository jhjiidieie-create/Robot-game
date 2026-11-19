using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float timeLimit = 30f;
    public TMP_Text timerText, resultText;
    public GameObject resultPanel;
    public RobotController robot;
    public ManualRobotController manualRobot;

    int totalPucks, sortedPucks;
    int shownSeconds = -1;
    float timeLeft;
    bool active;

    void Start()
    {
        totalPucks = FindObjectsOfType<Puck>().Length;
        resultPanel.SetActive(false);
        ShowTime(timeLimit);
    }

    void Update()
    {
        if (!active) return;
        ShowTime(timeLeft -= Time.deltaTime);
        if (timeLeft <= 0) EndGame();
    }

    public void StartGame()
    {
        if (active) return;
        timeLeft = timeLimit;
        active = true;
        ShowTime(timeLeft);
    }

    public bool AddPuck()
    {
        if (!active || ++sortedPucks < totalPucks) return false;
        EndGame();
        return true;
    }

    void EndGame()
    {
        active = false;
        if (robot != null) robot.StopRobot();
        if (manualRobot != null) manualRobot.StopRobot();
        resultText.text = $"{sortedPucks} из {totalPucks}";
        resultPanel.SetActive(true);
    }

    void ShowTime(float time)
    {
        int seconds = Mathf.Max(0, Mathf.CeilToInt(time));
        if (seconds == shownSeconds) return;
        shownSeconds = seconds;
        timerText.text = $"{seconds / 60}:{seconds % 60:00}";
    }

    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}