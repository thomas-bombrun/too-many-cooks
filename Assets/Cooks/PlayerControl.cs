using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CookSetup
{
	public Texture skin;
	public string tag;
	public Transform startingPlace;
}

public class PlayerControl : MonoBehaviour
{
	public GameObject cookPrefab;
	public List<CookSetup> cookSetups;

	List<CookControl> cooks;
	int activeCookIndex = 0;

	void Start()
	{
		HUD.Singleton.isSwitchPossible = cookSetups.Count > 1;
		cooks = new List<CookControl>();
		foreach(var cookSetup in cookSetups)
		{
			GameObject cook = Instantiate(cookPrefab, cookSetup.startingPlace.position, cookSetup.startingPlace.rotation);
			cook.GetComponentInChildren<SkinnedMeshRenderer>().material.SetTexture("_MainTex", cookSetup.skin);
			cook.tag = cookSetup.tag;
			cooks.Add(cook.GetComponent<CookControl>());
		}
		SetActiveCook(0);
	}

	public void SetActiveCook(int cookIndex)
	{
		cooks[activeCookIndex].SetInactive();
		cooks[cookIndex].SetActive();
		activeCookIndex = cookIndex;
	}

	private void Update()
	{
		if(Input.GetButtonDown("NextCook"))
			ActivateNextCook();
	}

	private void ActivateNextCook()
	{
		SetActiveCook((activeCookIndex + 1) % cookSetups.Count);
	}
}
