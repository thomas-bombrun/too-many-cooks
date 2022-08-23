using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
	[SerializeField]
	private TMPro.TMP_Text debugText;
	[SerializeField]
	private TMPro.TMP_Text scoreText;
	[SerializeField]
	private TMPro.TMP_Text order;
	public static HUD Singleton;

	private int score = 0;

	void Awake()
	{
		if(Singleton == null)
		{
			Singleton = this;
		}
		scoreText.text = "" + score;
	}

	public void DisplayText(string textToDisplay)
	{
		debugText.text = textToDisplay;
		CancelInvoke("ClearText");
		Invoke("ClearText", 3f);
	}

	private void ClearText()
	{
		debugText.text = "";
	}

	public void AddScore()
	{
		score++;
		scoreText.text = "" + score;
	}

	public void SetOrder(GameObject recipe)
	{
		order.text = "Next order : " + recipe.name;
	}


}
