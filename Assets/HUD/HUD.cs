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
	public bool isSwitchPossible;
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
			actions += "Press O to work\n";
		}
		if(isGrabPossible)
		{
			actions += "Press P to grab/drop\n";
		}
		if(isSwitchPossible)
		{
			actions += "Press A to switch\n";
		}
		possibleActions.text = actions;
	}


	public static string PrettyName(string input)
	{
		return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim().ToLower();
	}
	public static string UppercaseFirst(string s)
	{
		// Check for empty string.
		if(string.IsNullOrEmpty(s))
		{
			return string.Empty;
		}
		// Return char and concat substring.
		return char.ToUpper(s[0]) + s.Substring(1);
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
