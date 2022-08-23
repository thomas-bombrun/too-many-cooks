using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
	public List<GameObject> PrefabIngredientsInOrder;
	public List<Transform> SpotsForIngredients;

	[SerializeField]
	private int currentStep;


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
				HUD.Singleton.DisplayText("Yeah !");
			}
			return true;
		}
		else
		{
			HUD.Singleton.DisplayText("Wrong ingredient ! Expected " + PrefabIngredientsInOrder[currentStep].name + " but got " + ingredient.name);
			return false;
		}
	}
}
