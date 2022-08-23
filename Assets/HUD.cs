using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
	[SerializeField]
	private TMPro.TMP_Text debugText;
	public static HUD Singleton;

	void Awake()
	{
		if(Singleton == null)
		{
			Singleton = this;
		}
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


}
