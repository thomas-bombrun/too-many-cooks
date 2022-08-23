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
				HUD.Singleton.AddScore();
				Destroy(gameObject);
			}
			return true;
		}
		else
		{
			HUD.Singleton.DisplayText("Wrong ingredient ! Expected " + PrefabIngredientsInOrder[currentStep].name + " but got " + ingredient.name);
			return false;
		}
	}

	private List<GameObject> editorPlacedIngredients;
	public void FillRecipe()
	{
		Clear();
		for(int i = 0; i < PrefabIngredientsInOrder.Count; i++)
		{
			editorPlacedIngredients.Add(Instantiate(PrefabIngredientsInOrder[i], SpotsForIngredients[i]));
		}
	}

	public void Clear()
	{
		if(editorPlacedIngredients != null)
		{
			foreach(GameObject placedIngredient in editorPlacedIngredients)
			{
				SafeDestroy(placedIngredient);
			}
		}
		editorPlacedIngredients = new List<GameObject>();
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
