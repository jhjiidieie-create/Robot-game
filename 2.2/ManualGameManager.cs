using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ManualGameManager : MonoBehaviour
{
    public float timeLimit = 60f;
    public TMP_Text timerText;
    public TMP_Text statisticsText;
    public GameObject victoryPanel;
    public TMP_Text victoryText;
    public GameObject defeatPanel;
    public TMP_Text defeatText;

    int totalPucks;
    int sortedPucks;
    float timeLeft;
    bool gameActive;

    void Start()
    {
        totalPucks = FindObjectsOfType<ManualPuck>().Length;
        timeLeft = timeLimit;

        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (defeatPanel != null) defeatPanel.SetActive(false);

        UpdateUI();
    }

    void Update()
    {
        if (!gameActive) return;

        timeLeft -= Time.deltaTime;
        UpdateUI();

        if (timeLeft <= 0)
            EndGame(false);
    }

    public void StartManualGame()
    {
        if (gameActive) return;

        timeLeft = timeLimit;
        gameActive = true;
        UpdateUI();
    }

    public bool RegisterSortedPuck()
    {
        if (!gameActive) return false;

        sortedPucks++;
        UpdateUI();

        if (sortedPucks >= totalPucks)
            EndGame(true);

        return true;
    }

    void UpdateUI()
    {
        int seconds = Mathf.CeilToInt(Mathf.Max(0, timeLeft));

        if (timerText != null)
            timerText.text = $"{seconds / 60}:{seconds % 60:00}";

        if (statisticsText != null)
            statisticsText.text = $"{sortedPucks} / {totalPucks}";
    }

    void EndGame(bool won)
    {
        if (!gameActive) return;

        gameActive = false;

        if (won)
        {
            if (victoryPanel != null) victoryPanel.SetActive(true);
            if (victoryText != null) victoryText.text = "Победа!";
        }
        else
        {
            if (defeatPanel != null) defeatPanel.SetActive(true);
            if (defeatText != null) defeatText.text = "Время вышло!";
        }
    }

    public void RestartManualGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
