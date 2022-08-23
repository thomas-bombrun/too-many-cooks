using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
	public GameObject cookPrefab;
	public List<Texture> cooksSkins;
	public List<string> cooksTags;

	List<CookControl> cooks;
	int activeCookIndex = 0;

	void Start()
	{
		cooks = new List<CookControl>();
		int cookPerLine = 3;
		for(int i = 0; i < cooksSkins.Count; i++)
		{
			float cookXPosition = (i / cookPerLine) * 1.0f;
			float cookZPosition = (i % cookPerLine) * 1.0f - 2.0f;
			Vector3 cookPosition = new Vector3(cookXPosition, 0.0f, cookZPosition);
			GameObject cook = Instantiate(cookPrefab, cookPosition, Quaternion.identity);
			cook.GetComponentInChildren<SkinnedMeshRenderer>().material.SetTexture("_MainTex", cooksSkins[i]);
			cook.tag = cooksTags[i];
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
		SetActiveCook((activeCookIndex + 1) % cooksSkins.Count);
	}
}
