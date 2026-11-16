using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float timeLimit = 30f;
    public TMP_Text timerText;
    public TMP_Text loseResultText;
    public GameObject victoryUI;
    public GameObject loseUI;
    public RobotController robot;

    float timeLeft;
    bool gameActive;

    void Start()
    {
        victoryUI.SetActive(false);
        loseUI.SetActive(false);
        timerText.text = "0:30";
    }

    void Update()
    {
        if (!gameActive) return;

        timeLeft -= Time.deltaTime;
        timerText.text = $"{Mathf.FloorToInt(timeLeft / 60)}:{Mathf.CeilToInt(timeLeft % 60):00}";

        if (timeLeft <= 0)
            EndGame(false);
    }

    public void StartGame()
    {
        timeLeft = timeLimit;
        gameActive = true;
        timerText.text = "0:30";
    }

    public void CheckWin()
    {
        if (!gameActive) return;

        Puck[] pucks = FindObjectsOfType<Puck>();
        int sorted = 0;

        foreach (Puck puck in pucks)
            if (puck.isSorted) sorted++;

        if (pucks.Length > 0 && sorted == pucks.Length)
            EndGame(true);
    }

    void EndGame(bool win)
    {
        gameActive = false;
        robot.StopRobot();

        if (win)
            victoryUI.SetActive(true);
        else
        {
            Puck[] pucks = FindObjectsOfType<Puck>();
            int sorted = 0;

            foreach (Puck puck in pucks)
                if (puck.isSorted) sorted++;

            loseResultText.text = $"{sorted} из {pucks.Length}";
            loseUI.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
