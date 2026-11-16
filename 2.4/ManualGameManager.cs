using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ManualGameManager : MonoBehaviour
{
    public int totalPucks;
    public float timeLimit = 30f;
    public TMP_Text timerText;
    public TMP_Text resultText;
    public GameObject resultPanel;

    int sortedPucks;
    float timeLeft;
    bool gameActive;
    bool finished;

    void Start()
    {
        resultPanel.SetActive(false);
        ShowTime(timeLimit);
    }

    void Update()
    {
        if (!gameActive || finished) return;

        timeLeft -= Time.deltaTime;
        ShowTime(timeLeft);

        if (timeLeft <= 0) EndGame();
    }

    public void StartGame()
    {
        if (gameActive || finished) return;

        timeLeft = timeLimit;
        gameActive = true;
        ShowTime(timeLeft);
    }

    public void AddPuck()
    {
        sortedPucks++;

        if (sortedPucks == totalPucks) EndGame();
    }

    void EndGame()
    {
        if (finished) return;

        finished = true;
        gameActive = false;
        resultText.text = sortedPucks + " из " + totalPucks;
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