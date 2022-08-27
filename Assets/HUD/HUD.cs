using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{

	[SerializeField] private TMPro.TMP_Text debugText;
	[SerializeField] private TMPro.TMP_Text scoreText;
	[SerializeField] private TMPro.TMP_Text order;
	[SerializeField] private TMPro.TMP_Text possibleActions;
	public static HUD Singleton;
	public bool isWorkPossible;
	public bool isGrabPossible;

	private int score = 0;

	void Awake()
	{
		if(Singleton == null)
		{
			Singleton = this;
		}
		scoreText.text = "" + score;
	}

	private void Update()
	{
		string actions = "";
		if(isWorkPossible)
		{
			actions += "Press F to work\n";
		}
		if(isGrabPossible)
		{
			actions += "Press ? to grab";
		}
		possibleActions.text = actions;
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
