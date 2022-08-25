using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct CuttingTransformation
{
	public GameObject fromPrefab;
	public GameObject toPrefab;
}

public class CuttingStation : Station
{

	public Transform IngredientSpot;
	public List<CuttingTransformation> possibleTransformations;
	private CuttingTransformation currentTransformation;

	protected override bool WorkCanBeStarted(CookControl cook)
	{
		if(IngredientSpot.transform.childCount == 0)
		{
			return false;
		}
		GameObject ingredient = IngredientSpot.GetChild(0).gameObject;
		foreach(var transformation in possibleTransformations)
		{
			if(ingredient.name == transformation.fromPrefab.name)
			{
				currentTransformation = transformation;
				return true;
			}
		}
		return false;
	}

	protected override void WorkDone(CookControl cook)
	{
		if(IngredientSpot.transform.childCount == 0)
		{
			//TODO handle better ?
			Debug.LogWarning("Cutting board has lost its ingredient during work time");
			return;
		}
		GameObject ingredient = IngredientSpot.GetChild(0).gameObject;
		if(ingredient.name != currentTransformation.fromPrefab.name)
		{
			//TODO handle better ?
			Debug.LogWarning("Cutting board has lost its ingredient during work time");
			return;
		}
		Destroy(ingredient);
		GameObject newIngredient = Instantiate(currentTransformation.toPrefab, IngredientSpot);
		newIngredient.name = currentTransformation.toPrefab.name;
		currentTransformation = new CuttingTransformation { };
	}

}
