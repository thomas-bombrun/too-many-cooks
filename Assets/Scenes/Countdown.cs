using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Countdown : MonoBehaviour
{
	[SerializeField]
	private TMPro.TMP_Text countdownText;
	public float DurationInSeconds;
	private float elapsedTime;
	void Update()
	{
		elapsedTime += Time.deltaTime;
		countdownText.text = "" + Mathf.Round(DurationInSeconds - elapsedTime);
		if(elapsedTime > DurationInSeconds)
		{
			LevelManager.LoadGameOver();
		}
	}
}
