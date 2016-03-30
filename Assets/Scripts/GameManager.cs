using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	private InputGestures m_InputGesture;
	public GameObject m_GameOverPanel;

	private int m_Score;
	private int m_Difficulty;
	public Image m_QuestImage;
	public Text m_Timer;
	public Text m_ScoreText;
	public Text m_HighScoreText;
	public float m_MaxTime;
	private float m_StartTime;

	public float m_TimeValue;
	private float m_CurrentTime;

	private string[] Shapes = new string[] {
		"circle", 
		"diamond_horizontal",
		"diamond",
		"oval",
		"rectangle",
		"square",
		"trapezium_down",
		"trapezium_left",
		"trapezium_right",
		"trapezium",
		"triangle_down",
		"triangle_left",
		"triangle_right",
		"triangle",
		"vertical_oval",
		"vertical_rectangle"};

	private string m_CurrentQuestShape;
	private List<Sprite> m_SpritesList;

	void Start () {
		m_InputGesture = FindObjectOfType<InputGestures>();
		m_InputGesture.IsGameGoing = true;
		m_SpritesList = LoadSpritesData();
		GenerateNextLevel();
		m_GameOverPanel.SetActive(false);
		m_StartTime = m_MaxTime;
		m_CurrentTime = m_StartTime;
		m_Difficulty = 0;
		m_Score = 0;
		m_ScoreText.text = "Score: " + m_Score;
		if(PlayerPrefs.GetInt("HighScore") != 0){
			m_HighScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore");	
		}

	}
	

	void Update () {
		
		if(m_CurrentTime >= 0){
			m_Timer.text = string.Format("{0:0.00}", m_CurrentTime);
			m_CurrentTime -= Time.deltaTime;
		}
		if(m_CurrentTime < 0){
			m_Timer.text = "Time is up!";
			m_InputGesture.IsGameGoing = false;
			m_GameOverPanel.SetActive(true);
		}
	}

	public void ReStartGame(){
		m_InputGesture.IsGameGoing = true;
		GenerateNextLevel();
		m_StartTime = m_MaxTime;
		m_CurrentTime = m_StartTime;
		m_Difficulty = 0;
		m_Score = 0;
		m_ScoreText.text = "Score: " + m_Score;
	}

	public void GenerateNextLevel(){
		m_CurrentQuestShape = Shapes[(int)Random.Range(0, Shapes.Length)].ToString();
		SetQuestSprite(m_CurrentQuestShape);
		m_StartTime -= m_Difficulty * 0.1f;
		m_CurrentTime = m_StartTime;
	}

	private void SetQuestSprite(string shapeName){
		Sprite result = null;
		m_QuestImage.sprite = m_SpritesList.Find(x => x.name == shapeName);
	}

	public string GetCurrentQuestShape(){
		return m_CurrentQuestShape;
	}

	public void AddScorePoint(){
		m_Score++;
		m_Difficulty++;
		if(m_Score > PlayerPrefs.GetInt("HighScore"))
		{
			PlayerPrefs.SetInt("HighScore", m_Score);
		}
		m_ScoreText.text = "Score: " + m_Score;
	}

	List<Sprite> LoadSpritesData()
	{
		List<Sprite> shapeSprites = new List<Sprite>();
		UnityEngine.Sprite[] sFiles = Resources.LoadAll<Sprite>("Sprites");
		for (int i = 0; i < sFiles.Length; i++)
		{
			shapeSprites.Add(sFiles[i]);
		}
		return shapeSprites;
	}

}
