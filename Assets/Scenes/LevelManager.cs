using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	public static void LoadFirstLevel()
	{
		SceneManager.LoadScene("Level 1");
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
