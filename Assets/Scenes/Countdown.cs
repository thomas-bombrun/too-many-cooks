using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Countdown : MonoBehaviour
{
	[SerializeField]
	private TMPro.TMP_Text countdownText;
	public float DurationInSeconds;
	private float elapsedTime;
	public static bool paused = false;
	void Update()
	{
		if(!paused)
		{
			elapsedTime += Time.deltaTime;
			countdownText.text = "" + Mathf.Round(DurationInSeconds - elapsedTime);
			if(elapsedTime > DurationInSeconds)
			{
				LevelManager.LoadGameOver();
			}
		}
	}
}
