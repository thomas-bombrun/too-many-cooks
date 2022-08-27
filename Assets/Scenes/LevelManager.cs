using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	public static void LoadFirstLevel()
	{
		SceneManager.LoadScene("Intro - Level 0");
	}
	public static void LoadNextLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public static void LoadGameOver()
	{
		SceneManager.LoadScene("Game Over");
	}
}
