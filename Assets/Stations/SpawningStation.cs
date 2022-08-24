using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningStation : Station
{
	public GameObject prefabToSpawn;
	protected override void WorkDone(CookControl cook)
	{
		GameObject ingredient = Instantiate(prefabToSpawn);
		ingredient.name = prefabToSpawn.name;
		cook.GrabIngredient(ingredient);
	}
}
