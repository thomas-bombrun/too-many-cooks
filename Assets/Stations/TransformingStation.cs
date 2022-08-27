using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformingStation : Station
{
	public GameObject prefabToTransform;
	public GameObject prefabResult;

	override protected bool WorkCanBeStarted(CookControl cook)
	{
		GameObject ingredient = cook.GetGrabbedIngredient();
		if(ingredient == null)
		{
			Debug.Log("no ingredient to transform");
			return false;
		}
		if(ingredient.name == prefabToTransform.name)
		{
			Debug.Log("transforming ingredient");
			cook.UnlinkGrabbedIngredient();
			Destroy(ingredient);
			return true;
		}
		else
		{
			HUD.Singleton.DisplayText("Wrong ingredient !\nExpected " + prefabToTransform.name + " but got " + ingredient.name);
			return false;
		}
	}

	protected override void WorkDone(CookControl cook)
	{
		GameObject ingredient = Instantiate(prefabResult);
		ingredient.name = prefabResult.name;
		cook.GrabIngredient(ingredient);
	}
}
