using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public static ScoreManager instance;
    public Text scoreText;

    public static int totalScore;

    void Awake () {
        instance = this;
	}
	
    /// <summary>
    /// Method for adding points to the total score.
    /// </summary>
    /// <param name="points">The points that will be added to the total.</param>
	public void addScore(int points)
    {
        totalScore += points;
        scoreText.text = totalScore.ToString();
    }
}
