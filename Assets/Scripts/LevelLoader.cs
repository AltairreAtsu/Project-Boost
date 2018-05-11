using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	[SerializeField] private bool autoLoadNext = false;
	[SerializeField] private float loadDelay = 1.5f;

	private void Start()
	{
		if (autoLoadNext)
		{
			Invoke("LoadNextLevel", loadDelay);
			autoLoadNext = false;
		}
	}

	public void LoadNextLevel()
	{
		LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void LoadLevel(int buildIndex)
	{
		SceneManager.LoadScene(buildIndex);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
