using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour 
{

	public GameObject pauseObject;


	public void Pause()
	{
		pauseObject.SetActive(true);
		Time.timeScale = 0.0f;
	}


	public void Resume()
	{
		pauseObject.SetActive(false);
		Time.timeScale = 1.0f;
	}


	public void Restart()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}


	public void Exit()
	{
		Application.Quit();
	}

	
	void Update () 
	{
		#if	UNITY_EDITOR_WIN
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (pauseObject.activeSelf)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}
		#endif
	}
	
}
