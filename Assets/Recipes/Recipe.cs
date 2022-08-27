using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Recipe : MonoBehaviour
{
	public List<GameObject> PrefabIngredientsInOrder;
	public List<Transform> SpotsForIngredients;

	[SerializeField] private int currentStep;

	[HideInInspector] public UnityEvent recipeDone;
	private AudioSource ingredientAddedAudio;

	void Awake()
	{
		ingredientAddedAudio = GetComponent<AudioSource>();
	}

	///<returns>Wheter the ingredient was accepted or not.</returns>
	public bool AddIngredient(GameObject ingredient)
	{
		if(ingredient.name == PrefabIngredientsInOrder[currentStep].name)
		{
			ingredient.transform.SetParent(SpotsForIngredients[currentStep]);
			ingredient.transform.localPosition = Vector3.zero;
			ingredient.transform.localRotation = Quaternion.identity;
			ingredient.tag = "Untagged";
			currentStep++;
			if(currentStep == SpotsForIngredients.Count)
			{
				recipeDone.Invoke();
				HUD.Singleton.DisplayText("Yeah !");
				HUD.Singleton.AddScore();
				Destroy(gameObject);
			}
			else
			{
				ingredientAddedAudio.Play();
			}
			return true;
		}
		else
		{
			HUD.Singleton.DisplayText("Wrong ingredient ! Expected " + PrefabIngredientsInOrder[currentStep].name + " but got " + ingredient.name);
			return false;
		}
	}

	public void FillRecipe()
	{
		Clear();
		for(int i = 0; i < PrefabIngredientsInOrder.Count; i++)
		{
			Instantiate(PrefabIngredientsInOrder[i], SpotsForIngredients[i]);
		}
	}

	public void Clear()
	{
		foreach(Transform ingredientSpot in SpotsForIngredients)
		{
			for(int i = 0; i < ingredientSpot.childCount; i++)
			{
				SafeDestroy(ingredientSpot.GetChild(i).gameObject);
			}
		}
	}

	public static T SafeDestroy<T>(T obj) where T : Object
	{
		if(Application.isEditor)
			Object.DestroyImmediate(obj);
		else
			Object.Destroy(obj);

		return null;
	}

}
