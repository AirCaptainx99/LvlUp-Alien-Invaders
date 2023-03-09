using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoryScript : MonoBehaviour
{
    public TextMeshProUGUI victoryText;
    public TextMeshProUGUI scoreText;

    public void SetVictoryText(string _text)
    {
        victoryText.SetText(_text);
    }

    public void SetScoreText(string _text)
    {
        scoreText.SetText(_text);
    }
}
