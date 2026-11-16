using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ManualGameManager : MonoBehaviour
{
    public float timeLimit = 30f;
    public TMP_Text timerText;
    public GameObject resultPanel;
    public TMP_Text resultText;

    int totalPucks;
    int sortedPucks;
    float timeLeft;
    bool gameActive;

    void Start()
    {
        totalPucks = FindObjectsOfType<ManualPuck>().Length;
        timeLeft = timeLimit;
        resultPanel.SetActive(false);
        UpdateTimer();
    }

    void Update()
    {
        if (!gameActive) return;

        timeLeft -= Time.deltaTime;
        UpdateTimer();

        if (timeLeft <= 0)
            EndGame(false);
    }

    public void StartGame()
    {
        if (gameActive) return;

        timeLeft = timeLimit;
        gameActive = true;
        UpdateTimer();
    }

    public bool AddPuck()
    {
        if (!gameActive) return false;

        sortedPucks++;

        if (sortedPucks >= totalPucks)
            EndGame(true);

        return true;
    }

    void UpdateTimer()
    {
        int seconds = Mathf.CeilToInt(Mathf.Max(0, timeLeft));

        if (timerText != null)
            timerText.text = $"{seconds / 60}:{seconds % 60:00}";
    }

    void EndGame(bool won)
    {
        if (!gameActive) return;

        gameActive = false;
        resultPanel.SetActive(true);
        resultText.text = won
            ? $"Победа! {sortedPucks} из {totalPucks}"
            : $"Время вышло! {sortedPucks} из {totalPucks}";
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
