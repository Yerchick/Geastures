using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Starter : MonoBehaviour {

	


	void Start () {
		
        DontDestroyOnLoad(gameObject);
        GoToMainMenu();
	}
	
	void Update () {
	
	}

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
