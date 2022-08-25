using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeSpawner : MonoBehaviour
{

	public List<GameObject> Recipes;
	private int recipeIndex = 0;
	// Start is called before the first frame update
	void Start()
	{
		PopulateRecipe();
	}

	void Update()
	{
		//TODO smarter checks when the recipe is done (callbacks on OnDestroy ?)
		if(transform.childCount == 0)
		{
			PopulateRecipe();
		}
	}

	void PopulateRecipe()
	{
		GameObject recipePrefab = Recipes[recipeIndex];
		recipeIndex++;
		if(recipeIndex >= Recipes.Count)
		{
			recipeIndex = 0;
		}
		HUD.Singleton.SetOrder(recipePrefab);
		Instantiate(recipePrefab, transform);
	}

}
