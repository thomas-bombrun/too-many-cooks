using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class SelectWithKeyboard : MonoBehaviour
{
	public void Update()
	{
		if(Input.anyKeyDown && !(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
		{
			GetComponent<Button>().onClick.Invoke();
		}
	}
}
