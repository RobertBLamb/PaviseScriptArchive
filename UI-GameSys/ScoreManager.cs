using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Could make a singleton in the future, but allowing multiple instances of ScoreManager might allow for ghost players, limited multiplayer, or other scoring modes
public class ScoreManager : MonoBehaviour {

    public int score = 0;
    public Text scoreText;
    public float comboWindowTime;
    public float comboCountdown;

	// Use this for initialization
	void Start () {
        scoreText.text = score.ToString();
    }
	
	public void UpdateScore (int scoreMod) {
        score += scoreMod;
        scoreText.text = score.ToString();
	}
}
