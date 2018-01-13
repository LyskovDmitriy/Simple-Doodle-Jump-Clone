using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour 
{

	public GameObject gameOverObject;
	public Text currentScoreText;
	public Text maxScoreText;


	public void RestartLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}


	void Start()
	{
		PlayerController.OnPlayerDeath += GameOver;
	}


	void GameOver()
	{
		gameOverObject.SetActive(true);
		if (PlayerPrefs.HasKey("Current score"))
		{
			currentScoreText.text += " " + PlayerPrefs.GetInt("Current score");
			PlayerPrefs.DeleteKey("Current score");
		}
		if (PlayerPrefs.HasKey("Max score"))
		{
			maxScoreText.text += " " + PlayerPrefs.GetInt(("Max score"));
		}
		PlayerController.OnPlayerDeath -= GameOver;
	}


	void OnDestroy()
	{
		PlayerController.OnPlayerDeath -= GameOver;
	}
}
