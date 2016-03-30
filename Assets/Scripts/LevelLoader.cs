using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

	private Starter m_Starter;
	public Text m_ScoreText;

	void Start () {
		m_Starter = FindObjectOfType<Starter>();
		int score = PlayerPrefs.GetInt("HighScore");
		if(score > 0)
			m_ScoreText.text = "Your HighScore:" + score;
	}

	public void loadLevel(string LevelName){
		m_Starter.LoadLevel(LevelName);
	}
}
