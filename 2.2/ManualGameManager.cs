using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManualGameManager : MonoBehaviour
{
    public float roundDuration = 60f;
    public Text timerLabel;
    public Text progressLabel;
    public GameObject finishPanel;
    public Text finishLabel;

    int allPucks;
    int sortedPucks;
    float remainingTime;
    bool gameRunning;

    void Start()
    {
        allPucks = FindObjectsOfType<ManualPuck>().Length;
        remainingTime = roundDuration;

        if (finishPanel != null)
            finishPanel.SetActive(false);

        UpdateDisplay();
    }

    void Update()
    {
        if (!gameRunning) return;

        remainingTime -= Time.deltaTime;
        UpdateDisplay();

        if (remainingTime <= 0)
            FinishManualGame(false);
    }

    public void StartManualGame()
    {
        if (gameRunning) return;

        remainingTime = roundDuration;
        gameRunning = true;
        UpdateDisplay();
    }

    public bool RegisterSortedPuck()
    {
        if (!gameRunning) return false;

        sortedPucks++;
        UpdateDisplay();

        if (sortedPucks >= allPucks)
            FinishManualGame(true);

        return true;
    }

    void UpdateDisplay()
    {
        int secondsLeft = Mathf.CeilToInt(Mathf.Max(0, remainingTime));

        if (timerLabel != null)
            timerLabel.text = $"{secondsLeft / 60}:{secondsLeft % 60:00}";

        if (progressLabel != null)
            progressLabel.text = $"{sortedPucks} / {allPucks}";
    }

    void FinishManualGame(bool won)
    {
        if (!gameRunning) return;

        gameRunning = false;

        if (finishPanel != null)
            finishPanel.SetActive(true);

        if (finishLabel != null)
            finishLabel.text = won ? "Победа!" : "Время вышло!";
    }

    public void RestartManualGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
