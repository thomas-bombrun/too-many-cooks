using UnityEngine;

public class IngredientSpawner : SpawningStation
{
	public float spawningDelay = 1.0f;

	private void Start() {
		SpawnIngredient();
	}

	void SpawnIngredient()
	{
		if(transform.childCount == 0)
		{
			GameObject ingredient = Instantiate(prefabToSpawn, transform);
			ingredient.name = prefabToSpawn.name;
		}
	}

	protected override void WorkDone(CookControl cook)
	{
		Invoke("SpawnIngredient", spawningDelay);
	}
}
