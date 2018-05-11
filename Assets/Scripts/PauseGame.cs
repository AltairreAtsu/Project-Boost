using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {
	[SerializeField] private GameObject PausePanel;
	public static bool GAME_PAUSED = false;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GAME_PAUSED)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}
	}

	public void Pause()
	{
		GAME_PAUSED = true;
		Time.timeScale = 0f;
		PausePanel.SetActive(true);
	}

	public void Resume()
	{
		GAME_PAUSED = false;
		Time.timeScale = 1f;
		PausePanel.SetActive(false);
	}

	private void OnDestroy()
	{
		Time.timeScale = 1f;
	}
}
