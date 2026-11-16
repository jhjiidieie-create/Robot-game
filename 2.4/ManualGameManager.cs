using UnityEngine;
using TMPro;

public class ManualGameManager : MonoBehaviour
{
    public int totalPucks;
    public TMP_Text countText;
    public TMP_Text resultText;
    public GameObject resultPanel;

    int sortedPucks;

    void Start()
    {
        resultPanel.SetActive(false);
        UpdateUI();
    }

    public void AddPuck()
    {
        sortedPucks++;
        UpdateUI();

        if (sortedPucks == totalPucks)
        {
            resultText.text = "Победа!";
            resultPanel.SetActive(true);
        }
    }

    void UpdateUI()
    {
        countText.text = sortedPucks + " / " + totalPucks;
    }
}
