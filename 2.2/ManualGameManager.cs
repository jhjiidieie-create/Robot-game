using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ManualGameManager : MonoBehaviour
{
    public float timeLimit = 30f;
    public TMP_Text timerText;
    public TMP_Text resultText;
    public GameObject resultPanel;

    int totalPucks;
    int sortedPucks;
    float timeLeft;
    bool gameActive;

    void Start()
    {
        totalPucks = FindObjectsOfType<ManualPuck>().Length;
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

    public bool AddPuck()
    {
        if (!gameActive) return false;

        sortedPucks++;

        if (sortedPucks >= totalPucks)
            EndGame();

        return true;
    }

    void EndGame()
    {
        if (!gameActive) return;

        gameActive = false;
        resultText.text = $"{sortedPucks} из {totalPucks}";
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
