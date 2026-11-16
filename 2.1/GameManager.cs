using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float timeLimit = 60f;
    public Text timerText;
    public Text countText;
    public GameObject resultPanel;
    public Text resultText;

    int total;
    int sorted;
    float timeLeft;
    bool active;

    void Start()
    {
        total = FindObjectsOfType<Puck>().Length;
        timeLeft = timeLimit;
        resultPanel.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        if (!active) return;

        timeLeft -= Time.deltaTime;
        UpdateUI();

        if (timeLeft <= 0)
            EndGame(false);
    }

    public void StartGame()
    {
        active = true;
    }

    public void AddPuck()
    {
        if (!active) return;

        sorted++;
        UpdateUI();

        if (sorted >= total)
            EndGame(true);
    }

    void UpdateUI()
    {
        int seconds = Mathf.CeilToInt(Mathf.Max(0, timeLeft));
        timerText.text = $"{seconds / 60}:{seconds % 60:00}";
        countText.text = $"{sorted} / {total}";
    }

    void EndGame(bool win)
    {
        if (!active) return;

        active = false;
        resultPanel.SetActive(true);
        resultText.text = win ? "Победа!" : "Время вышло!";
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}