using UnityEngine;

public class SpawningStation : Station
{
	public GameObject prefabToSpawn;

	public override bool WorkCanBeStarted(CookControl cook, bool dryRun = true)
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
