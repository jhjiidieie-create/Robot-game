using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float gameTime = 30f;
    public float checkRadius = 0.25f;

    public RobotController robotController;
    public HoleSlot[] holes;

    [Header("UI")]
    public GameObject titleCanvas;
    public TMP_Text statisticsText;
    public GameObject timerCanvas;
    public TMP_Text timerText;
    public GameObject panelCanvas;
    public GameObject victoryPanel;
    public GameObject defeatPanel;
    public TMP_Text victoryText;
    public TMP_Text defeatText;

    float time;
    bool started;
    bool finished;

    void Start()
    {
        time = gameTime;
        titleCanvas.SetActive(false);
        timerCanvas.SetActive(false);
        panelCanvas.SetActive(false);
    }

    void Update()
    {
        if (finished)
            return;

        if (!started)
        {
            if (robotController.agent.enabled &&
                robotController.agent.isOnNavMesh &&
                robotController.agent.velocity.sqrMagnitude > 0.01f)
            {
                started = true;
                titleCanvas.SetActive(true);
                timerCanvas.SetActive(true);
            }

            return;
        }

        time -= Time.deltaTime;
        int seconds = Mathf.CeilToInt(Mathf.Max(0, time));
        timerText.text = (seconds / 60).ToString("00") + ":" + (seconds % 60).ToString("00");

        int filled = 0;

        foreach (HoleSlot hole in holes)
        {
            if (hole == null || hole.placePoint == null)
                continue;

            foreach (Collider obj in Physics.OverlapSphere(hole.placePoint.position, checkRadius))
            {
                if (obj.GetComponentInParent<Puck>() == null)
                    continue;

                filled++;
                break;
            }
        }

        statisticsText.text = filled + " / " + holes.Length;

        if (holes.Length > 0 && filled == holes.Length)
        {
            Finish(true, filled);
            return;
        }

        if (time <= 0)
            Finish(false, filled);
    }

    void Finish(bool victory, int filled)
    {
        finished = true;
        robotController.StopRobot();

        titleCanvas.SetActive(false);
        timerCanvas.SetActive(false);
        panelCanvas.SetActive(true);
        victoryPanel.SetActive(victory);
        defeatPanel.SetActive(!victory);

        string result = filled + " / " + holes.Length;

        if (victory)
            victoryText.text = result;
        else
            defeatText.text = result;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
