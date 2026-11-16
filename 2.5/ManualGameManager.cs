using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ManualGameManager : MonoBehaviour
{
    public int totalPucks;
    public float timeLimit = 30f;
    public TMP_Text timerText, resultText;
    public GameObject resultPanel;

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

        timeLeft -= Time.deltaTime;
        ShowTime(timeLeft);
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
        sortedPucks++;
        if (sortedPucks == totalPucks) EndGame();
    }

    void EndGame()
    {
        if (finished) return;
        finished = true;
        active = false;
        resultText.text = sortedPucks + " из " + totalPucks;
        resultPanel.SetActive(true);
    }

    void ShowTime(float time)
    {
        int seconds = Mathf.Max(0, Mathf.CeilToInt(time));
        timerText.text = seconds / 60 + ":" + (seconds % 60).ToString("00");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}