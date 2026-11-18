using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool manualMode;
    public int totalPucks;
    public float timeLimit = 30f;
    public TMP_Text timerText, resultText;
    public GameObject resultPanel;
    public RobotController robot;

    int sortedPucks;
    float timeLeft;
    bool active, finished;

    void Start()
    {
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
        if (active || finished) return;
        timeLeft = timeLimit;
        active = true;
        ShowTime(timeLeft);
    }

    public void AddPuck()
    {
        if (!manualMode || !active || finished) return;
        sortedPucks++;
        if (sortedPucks >= totalPucks) EndGame();
    }

    public bool CheckWin()
    {
        if (!active || manualMode) return false;

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
        if (manualMode) EndGame(sortedPucks, totalPucks);
        else
        {
            Puck[] pucks = FindObjectsOfType<Puck>();
            int sorted = 0;
            foreach (Puck puck in pucks)
                if (puck.isSorted) sorted++;
            EndGame(sorted, pucks.Length);
        }
    }

    void EndGame(int sorted, int total)
    {
        if (finished) return;
        finished = true;
        active = false;
        if (robot != null) robot.StopRobot();
        resultText.text = $"{sorted} из {total}";
        resultPanel.SetActive(true);
    }

    void ShowTime(float time)
    {
        int seconds = Mathf.Max(0, Mathf.CeilToInt(time));
        timerText.text = $"{seconds / 60}:{seconds % 60:00}";
    }

    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}