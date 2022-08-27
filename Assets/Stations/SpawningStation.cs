using UnityEngine;

public class SpawningStation : Station
{
	public GameObject prefabToSpawn;

	protected override bool WorkCanBeStarted(CookControl cook)
	{
		return cook.GetGrabbedIngredient() == null;
	}

	protected override void WorkDone(CookControl cook)
	{
		GameObject ingredient = Instantiate(prefabToSpawn);
		ingredient.name = prefabToSpawn.name;
		cook.GrabIngredient(ingredient);
	}
}
