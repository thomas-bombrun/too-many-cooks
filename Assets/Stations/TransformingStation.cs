using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformingStation : Station
{
	public GameObject prefabToTransform;
	public GameObject prefabResult;

	public override bool WorkCanBeStarted(CookControl cook, bool dryRun = true)
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
			if(!dryRun)
			{
				cook.UnlinkGrabbedIngredient();
				Destroy(ingredient);
			}
			return true;
		}
		else
		{
			if(!dryRun)
			{
				HUD.Singleton.DisplayText("Wrong ingredient !\nExpected " + prefabToTransform.name + " but got " + ingredient.name);
			}
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
