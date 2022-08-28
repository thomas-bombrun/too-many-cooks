using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	public static int levelIndex = 1;

	public static void LoadFirstLevel()
	{
		levelIndex = 1;
		SceneManager.LoadScene("Intro - Level 0");
	}


	public static void LoadPreviousLevel()
	{
		SceneManager.LoadScene(levelIndex - 1);
	}

	public static void LoadNextLevel()
	{
		levelIndex = SceneManager.GetActiveScene().buildIndex + 1;
		SceneManager.LoadScene(levelIndex);
	}

	public static void LoadGameOver()
	{
		SceneManager.LoadScene("Game Over");
	}
}
